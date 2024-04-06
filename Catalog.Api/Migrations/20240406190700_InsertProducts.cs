using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Api.Migrations
{
    /// <inheritdoc />
    public partial class InsertProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Products(Name, Description, Price, ImageUrl, Stock, CreatedAt, CategoryId)" +
                " values ('Diet Coke', 'Diet Coke 350ml', 5.45, 'coke.jpg', 50, now(), 1)");
            
            migrationBuilder.Sql("INSERT INTO Products(Name, Description, Price, ImageUrl, Stock, CreatedAt, CategoryId)" +
                " values ('Fries', 'Medium Fries', 4.00, 'fries.jpg', 10, now(), 2)");

            migrationBuilder.Sql("INSERT INTO Products(Name, Description, Price, ImageUrl, Stock, CreatedAt, CategoryId)" +
                " values ('Ice Cream', 'Chocolate Ice Cream', 3.00, 'ice-cream.jpg', 15, now(), 3)");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Products");
        }
    }
}
