using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recipit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QuantityDetails",
                table: "ProductsRecipies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantityDetails",
                table: "ProductsRecipies");
        }
    }
}
