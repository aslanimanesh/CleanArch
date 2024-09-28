using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApp.Infa.Data.Migrations
{
    /// <inheritdoc />
    public partial class editDiscountModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGeneralForProducts",
                table: "Discounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsGeneralForUsers",
                table: "Discounts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGeneralForProducts",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "IsGeneralForUsers",
                table: "Discounts");
        }
    }
}
