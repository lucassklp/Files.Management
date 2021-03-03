using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Files.Management.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    email = table.Column<string>(type: "varchar(30) CHARACTER SET utf8mb4", maxLength: 30, nullable: true),
                    token = table.Column<Guid>(type: "char(36)", nullable: false),
                    password = table.Column<string>(type: "varchar(130) CHARACTER SET utf8mb4", maxLength: 130, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "file",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    path = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    description = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: false),
                    filename = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: false),
                    date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    is_public = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    uid = table.Column<Guid>(type: "char(36)", nullable: false),
                    extension = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    mime_type = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    discriminator = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    width = table.Column<long>(type: "bigint", nullable: true),
                    height = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_file", x => x.id);
                    table.ForeignKey(
                        name: "fk_file_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "edited_image",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    width = table.Column<long>(type: "bigint", nullable: false),
                    height = table.Column<long>(type: "bigint", nullable: false),
                    image_id = table.Column<long>(type: "bigint", nullable: false),
                    path = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    operation = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_edited_image", x => x.id);
                    table.ForeignKey(
                        name: "fk_edited_image_file_image_id",
                        column: x => x.image_id,
                        principalTable: "file",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_edited_image_image_id",
                table: "edited_image",
                column: "image_id");

            migrationBuilder.CreateIndex(
                name: "IX_file_date",
                table: "file",
                column: "date");

            migrationBuilder.CreateIndex(
                name: "IX_file_uid",
                table: "file",
                column: "uid");

            migrationBuilder.CreateIndex(
                name: "IX_file_user_id",
                table: "file",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_email",
                table: "user",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_email_password",
                table: "user",
                columns: new[] { "email", "password" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "edited_image");

            migrationBuilder.DropTable(
                name: "file");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
