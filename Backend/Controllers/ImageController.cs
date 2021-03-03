using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Backend.Persistence;
using Files.Management.Domain;
using Files.Management.Dtos;
using Files.Management.Extensions;
using Files.Management.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;

namespace Files.Management.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ImageController : ControllerBase
    {
        private readonly JwtUserDto user;
        private readonly DaoContext context;
        private readonly ImageComponent imageComponent;
        public ImageController(JwtUserDto user, DaoContext context, ImageComponent imageComponent)
        {
            this.user = user;
            this.context = context;
            this.imageComponent = imageComponent;
        }

        [HttpGet("{uid}/Resize")]
        [AllowAnonymous]
        public async Task<IActionResult> Resize(Guid uid, Guid? token, int? width, int? height)
        {
            var file = await GetImageWitEditedAndValidate(uid, token);

            if (!width.HasValue && !height.HasValue)
            {
                var originalPhysicalFile = System.IO.File.Open(file.Path, FileMode.Open);
                return File(originalPhysicalFile, file.MimeType, file.Filename);
            }

            int actualWidth = width.HasValue ? width.Value :
                (int)(file.Width * height.Value / file.Height);

            int actualHeight = height.HasValue ? height.Value :
                (int)(file.Height * width.Value / file.Width);

            var editedImage = file.EditedImages.FirstOrDefault(x =>
                x.Operation == ImageOperation.Resize &&
                x.Width == actualWidth &&
                x.Height == actualHeight
            );

            if (editedImage == null)
            {
                var ret = await ResizeImage(file, actualWidth, actualHeight);
                return ret;
            }

            var physicalFile = System.IO.File.Open(editedImage.Path, FileMode.Open);
            return File(physicalFile, file.MimeType, file.Filename);

        }


        private async Task<IActionResult> ResizeImage(Domain.Image file, int width, int height)
        {
            var image = Image.Load(file.Path, out IImageFormat format);
            image.Mutate(x => x.Resize(width, height));

            var ms = new MemoryStream();
            image.Save(ms, format);

            await AttachEditedImage(ms, file, ImageOperation.Resize, width, height);
            return File(ms, file.MimeType, file.Filename);
        }


        private async Task<EditedImage> AttachEditedImage(MemoryStream stream, Domain.Image image, ImageOperation operation, int width, int height)
        {

            var physicalFile = await new StoreFileDto
            {
                File = stream,
                Filename = image.Filename
            }.CreatePhysicalFile();

            var editedImage = new EditedImage
            {
                Width = width,
                Height = height,
                ImageId = image.Id,
                Operation = operation,
                Path = physicalFile.Path,
            };

            context.Add(editedImage);
            await context.SaveChangesAsync();

            return editedImage;
        }


        private async Task<Domain.Image> GetImageWitEditedAndValidate(Guid uid, Guid? token)
        {
            Domain.Image image = null;

            try
            {
                image = await context.Set<Domain.Image>()
                    .Where(x => x.IsPublic == !token.HasValue && x.UID == uid)
                    .Select(x => new Domain.Image
                    {
                        User = new Backend.Domain.User
                        {
                            Token = x.User.Token
                        },
                        Path = x.Path,
                        Width = x.Width,
                        Height = x.Height,
                        EditedImages = x.EditedImages,
                        MimeType = x.MimeType,
                        Filename = x.Filename,
                        Id = x.Id
                    })
                    .SingleAsync();
            }
            catch (Exception ex)
            {
                throw new Exceptions.FileNotFoundException(ex);
            }

            if (token.HasValue && image.User.Token != token.Value)
            {
                throw new Exceptions.FileNotFoundException(null);
            }

            return image;
        }
    }
}
