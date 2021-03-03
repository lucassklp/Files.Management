using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Backend.Persistence;
using Files.Management.Domain;
using Files.Management.Dtos;
using Files.Management.Extensions;
using Files.Management.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeTypes;
using File = Files.Management.Domain.File;

namespace Files.Management.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class FileController : ControllerBase
    {
        protected readonly JwtUserDto user;
        protected readonly DaoContext context;
        protected readonly ImageComponent typeProvider;

        public FileController(JwtUserDto user, DaoContext context, ImageComponent typeProvider)
        {
            this.user = user;
            this.context = context;
            this.typeProvider = typeProvider;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(List<PublishFileDto<IFormFile>> files)
        {
            var store = files.Select(x =>
            {
                var stream = new MemoryStream();
                x.File.CopyTo(stream);
                return new StoreFileDto
                {
                    Description = x.Description,
                    File = stream,
                    IsPublic = x.IsPublic,
                    Filename = x.File.FileName
                };
            })
            .ToArray();

            var fileDtos = await Store(store);

            return Ok(fileDtos);
        }


        [HttpGet("{uid}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid uid, Guid? token)
        {
            var file = await GetFileAndValidate<File>(uid, token);
            using var fileStream = System.IO.File.Open(file.Path, FileMode.Open);
            return File(fileStream, file.MimeType, file.Filename);
        }


        [HttpDelete("{uid}")]
        public async Task<IActionResult> Delete(Guid uid)
        {
            var file = context.Set<File>()
                .Single(x => x.UserId == user.Id && x.UID == uid);

            System.IO.File.Delete(file.Path);
            context.Remove(file);
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("List")]
        public async Task<PagedResultDto<FileDto>> List(int offset, int size)
        {
            var count = context.Set<File>()
                .Where(x => x.UserId == user.Id)
                .Count();

            var content = await context.Set<File>()
                .Where(x => x.UserId == user.Id)
                .OrderByDescending(x => x.Id)
                .Skip(offset)
                .Take(size)
                .Select(x => new File
                {
                    UID = x.UID,
                    Date = x.Date,
                    Description = x.Description,
                    Filename = x.Filename,
                    IsPublic = x.IsPublic,
                    Size = x.Size
                })
                .ToListAsync();

            return new PagedResultDto<FileDto>
            {
                Content = content.Select(x => new FileDto
                {
                    Filename = x.Filename,
                    IsImage = x is Image,
                    IsPublic = x.IsPublic,
                    UID = x.UID,
                    UploadAt = x.Date,
                    Description = x.Description,
                    Size = x.Size
                }).ToList(),
                Total = count
            };

        }

        private async Task<List<FileDto>> Store(params StoreFileDto[] inputs)
        {
            var filesDto = new List<FileDto>();

            foreach (var input in inputs)
            {
                var physicalFile = await input.CreatePhysicalFile();
                if (typeProvider.IsValidImageFile(input.File, physicalFile.Extension))
                {
                    var img = SixLabors.ImageSharp.Image.Load(input.File);
                    var image = new Domain.Image
                    {
                        Date = DateTime.Now,
                        Path = physicalFile.Path,
                        UID = physicalFile.UID,
                        UserId = user.Id,
                        Description = input.Description,
                        IsPublic = input.IsPublic,
                        Filename = input.Filename,
                        Extension = physicalFile.Extension,
                        Size = input.File.Length,
                        MimeType = MimeTypeMap.GetMimeType(physicalFile.Extension),
                        Width = img.Width,
                        Height = img.Height
                    };

                    filesDto.Add(new FileDto
                    {
                        Filename = image.Filename,
                        UID = image.UID,
                        IsPublic = image.IsPublic,
                        IsImage = true
                    });

                    context.Add(image);
                }
                else
                {
                    var file = new File()
                    {
                        Date = DateTime.Now,
                        Path = physicalFile.Path,
                        UID = physicalFile.UID,
                        UserId = user.Id,
                        Description = input.Description,
                        IsPublic = input.IsPublic,
                        Filename = input.Filename,
                        Extension = physicalFile.Extension,
                        Size = input.File.Length,
                        MimeType = MimeTypeMap.GetMimeType(physicalFile.Extension),
                    };


                    filesDto.Add(new FileDto
                    {
                        Filename = file.Filename,
                        UID = file.UID,
                        IsPublic = file.IsPublic,
                        IsImage = false
                    });

                    context.Add(file);
                }
            }

            await context.SaveChangesAsync();

            return filesDto;
        }


        private async Task<T> GetFileAndValidate<T>(Guid uid, Guid? token)
            where T: File
        {
            T file = null;

            try
            {
                file = await context.Set<T>()
                    .Include(x => x.User.Token)
                    .SingleAsync(x => x.IsPublic == !token.HasValue && x.UID == uid);
            }
            catch (Exception ex)
            {
                throw new Exceptions.FileNotFoundException(ex);
            }

            if (token.HasValue && file.User.Token != token.Value)
            {
                throw new Exceptions.FileNotFoundException(null);
            }

            return file;
        }
    }
}
