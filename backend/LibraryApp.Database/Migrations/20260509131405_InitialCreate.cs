using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Author = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Isbn = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublishedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Owner = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsAvailable = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "IsAvailable", "Isbn", "Owner", "PublishedDate", "Title" },
                values: new object[] { 1, "Robert C. Martin", true, "978-0132350884", "Alice", new DateOnly(2008, 8, 1), "Clean Code" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Isbn", "Owner", "PublishedDate", "Title" },
                values: new object[] { 2, "David Thomas", "978-0135957059", "Bob", new DateOnly(2019, 9, 23), "The Pragmatic Programmer" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "IsAvailable", "Isbn", "Owner", "PublishedDate", "Title" },
                values: new object[,]
                {
                    { 3, "Gang of Four", true, "978-0201633610", "Alice", new DateOnly(1994, 10, 31), "Design Patterns" },
                    { 4, "Martin Fowler", true, "978-0134757599", "Carol", new DateOnly(2018, 11, 20), "Refactoring" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Isbn", "Owner", "PublishedDate", "Title" },
                values: new object[] { 5, "Eric Evans", "978-0321125217", "Bob", new DateOnly(2003, 8, 22), "Domain-Driven Design" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
