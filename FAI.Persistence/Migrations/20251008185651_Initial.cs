using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FAI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediumTypes",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediumTypes", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false, defaultValue: 0m),
                    ReleaseDate = table.Column<DateTime>(type: "date", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    MediumTypeCode = table.Column<string>(type: "nvarchar(8)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movie_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movie_MediumTypes_MediumTypeCode",
                        column: x => x.MediumTypeCode,
                        principalTable: "MediumTypes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Action" },
                    { 2, "Comedy" },
                    { 3, "Drama" },
                    { 4, "Horror" },
                    { 5, "Science Fiction" }
                });

            migrationBuilder.InsertData(
                table: "MediumTypes",
                columns: new[] { "Code", "Name" },
                values: new object[,]
                {
                    { "4K", "4K Ultra HD Blu Ray" },
                    { "BD", "Blu-ray Disc" },
                    { "DIGI", "Digital Copy" },
                    { "DVD", "Digital Versatile Disc" },
                    { "STR", "Streaming" },
                    { "VHS", "Video Home System" }
                });

            migrationBuilder.InsertData(
                table: "Movie",
                columns: new[] { "Id", "GenreId", "MediumTypeCode", "Price", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 5, "BD", 14.99m, new DateTime(2010, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inception" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 1, "4K", 19.99m, new DateTime(2008, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Dark Knight" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), 4, "BD", 12.99m, new DateTime(1980, 5, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Shining" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), 4, "STR", 9.99m, new DateTime(2017, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Get Out" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), 3, "DVD", 11.99m, new DateTime(1994, 9, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Shawshank Redemption" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), 3, "BD", 13.99m, new DateTime(1994, 7, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Forrest Gump" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), 2, "DVD", 8.99m, new DateTime(2007, 8, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Superbad" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), 2, "DIGI", 10.99m, new DateTime(2009, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Hangover" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movie_GenreId",
                table: "Movie",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Movie_MediumTypeCode",
                table: "Movie",
                column: "MediumTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_Movie_Title",
                table: "Movie",
                column: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movie");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "MediumTypes");
        }
    }
}
