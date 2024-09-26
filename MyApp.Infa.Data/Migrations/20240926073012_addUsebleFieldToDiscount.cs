using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApp.Infa.Data.Migrations
{
    /// <inheritdoc />
    public partial class addUsebleFieldToDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsableCount",
                table: "Discounts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsableCount",
                table: "Discounts");
        }
    }
}
