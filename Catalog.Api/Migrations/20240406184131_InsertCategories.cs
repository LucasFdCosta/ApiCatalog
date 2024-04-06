using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Api.Migrations
{
    /// <inheritdoc />
    public partial class InsertCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Categories(Name, ImageUrl) values ('Drinks', 'drinks.jpg')");
            migrationBuilder.Sql("INSERT INTO Categories(Name, ImageUrl) values ('Food', 'food.jpg')");
            migrationBuilder.Sql("INSERT INTO Categories(Name, ImageUrl) values ('Dessert', 'dessert.jpg')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Categories");
        }
    }
}
