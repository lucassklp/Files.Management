﻿// <auto-generated />
using System;
using Backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Files.Management.Migrations
{
    [DbContext(typeof(DaoContext))]
    partial class DaoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.3");

            modelBuilder.Entity("Backend.Domain.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasColumnName("email");

                    b.Property<string>("Password")
                        .HasMaxLength(130)
                        .HasColumnType("varchar(130) CHARACTER SET utf8mb4")
                        .HasColumnName("password");

                    b.Property<Guid>("Token")
                        .HasColumnType("char(36)")
                        .HasColumnName("token");

                    b.HasKey("Id")
                        .HasName("pk_user");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Email", "Password");

                    b.ToTable("user");
                });

            modelBuilder.Entity("Files.Management.Domain.EditedImage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<long>("Height")
                        .HasColumnType("bigint")
                        .HasColumnName("height");

                    b.Property<long>("ImageId")
                        .HasColumnType("bigint")
                        .HasColumnName("image_id");

                    b.Property<int>("Operation")
                        .HasColumnType("int")
                        .HasColumnName("operation");

                    b.Property<string>("Path")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("path");

                    b.Property<long>("Width")
                        .HasColumnType("bigint")
                        .HasColumnName("width");

                    b.HasKey("Id")
                        .HasName("pk_edited_image");

                    b.HasIndex("ImageId");

                    b.ToTable("edited_image");
                });

            modelBuilder.Entity("Files.Management.Domain.File", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150) CHARACTER SET utf8mb4")
                        .HasColumnName("description");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("discriminator");

                    b.Property<string>("Extension")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("extension");

                    b.Property<string>("Filename")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150) CHARACTER SET utf8mb4")
                        .HasColumnName("filename");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_public");

                    b.Property<string>("MimeType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("mime_type");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("path");

                    b.Property<long>("Size")
                        .HasColumnType("bigint")
                        .HasColumnName("size");

                    b.Property<Guid>("UID")
                        .HasColumnType("char(36)")
                        .HasColumnName("uid");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_file");

                    b.HasIndex("Date");

                    b.HasIndex("UID");

                    b.HasIndex("UserId");

                    b.ToTable("file");

                    b.HasDiscriminator<string>("Discriminator").HasValue("File");
                });

            modelBuilder.Entity("Files.Management.Domain.Image", b =>
                {
                    b.HasBaseType("Files.Management.Domain.File");

                    b.Property<long>("Height")
                        .HasColumnType("bigint")
                        .HasColumnName("height");

                    b.Property<long>("Width")
                        .HasColumnType("bigint")
                        .HasColumnName("width");

                    b.ToTable("file");

                    b.HasDiscriminator().HasValue("Image");
                });

            modelBuilder.Entity("Files.Management.Domain.EditedImage", b =>
                {
                    b.HasOne("Files.Management.Domain.Image", "Image")
                        .WithMany("EditedImages")
                        .HasForeignKey("ImageId")
                        .HasConstraintName("fk_edited_image_file_image_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");
                });

            modelBuilder.Entity("Files.Management.Domain.File", b =>
                {
                    b.HasOne("Backend.Domain.User", "User")
                        .WithMany("Files")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_file_user_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Backend.Domain.User", b =>
                {
                    b.Navigation("Files");
                });

            modelBuilder.Entity("Files.Management.Domain.Image", b =>
                {
                    b.Navigation("EditedImages");
                });
#pragma warning restore 612, 618
        }
    }
}
