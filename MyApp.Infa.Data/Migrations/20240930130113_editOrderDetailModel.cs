using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApp.Infa.Data.Migrations
{
    /// <inheritdoc />
    public partial class editOrderDetailModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "OrderDetails",
                newName: "OriginalPrice");

            migrationBuilder.RenameColumn(
                name: "Count",
                table: "OrderDetails",
                newName: "Quantity");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPercentage",
                table: "OrderDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FinalPrice",
                table: "OrderDetails",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "FinalPrice",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "OrderDetails",
                newName: "Count");

            migrationBuilder.RenameColumn(
                name: "OriginalPrice",
                table: "OrderDetails",
                newName: "Price");
        }
    }
}
