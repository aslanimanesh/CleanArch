using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApp.Infa.Data.Migrations
{
    /// <inheritdoc />
    public partial class addUsableUserDiscountModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsableUserDiscounts",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DiscountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsableUserDiscounts", x => new { x.UserId, x.DiscountId });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsableUserDiscounts");
        }
    }
}
