using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApp.Infa.Data.Migrations
{
    /// <inheritdoc />
    public partial class editModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsedUserDiscounts",
                table: "UsedUserDiscounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsedProductDiscounts",
                table: "UsedProductDiscounts");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UsedProductDiscounts",
                newName: "OrderId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UsedUserDiscounts",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "UsedUserDiscounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UsedDate",
                table: "UsedUserDiscounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UsedProductDiscounts",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "UsedDate",
                table: "UsedProductDiscounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsedUserDiscounts",
                table: "UsedUserDiscounts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsedProductDiscounts",
                table: "UsedProductDiscounts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UsedUserDiscounts_DiscountId",
                table: "UsedUserDiscounts",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_UsedUserDiscounts_OrderId",
                table: "UsedUserDiscounts",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_UsedUserDiscounts_UserId",
                table: "UsedUserDiscounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsedProductDiscounts_DiscountId",
                table: "UsedProductDiscounts",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_UsedProductDiscounts_OrderId",
                table: "UsedProductDiscounts",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_UsedProductDiscounts_ProductId",
                table: "UsedProductDiscounts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsedProductDiscounts_Discounts_DiscountId",
                table: "UsedProductDiscounts",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsedProductDiscounts_Orders_OrderId",
                table: "UsedProductDiscounts",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsedProductDiscounts_Products_ProductId",
                table: "UsedProductDiscounts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsedUserDiscounts_Discounts_DiscountId",
                table: "UsedUserDiscounts",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsedUserDiscounts_Orders_OrderId",
                table: "UsedUserDiscounts",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsedUserDiscounts_Users_UserId",
                table: "UsedUserDiscounts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsedProductDiscounts_Discounts_DiscountId",
                table: "UsedProductDiscounts");

            migrationBuilder.DropForeignKey(
                name: "FK_UsedProductDiscounts_Orders_OrderId",
                table: "UsedProductDiscounts");

            migrationBuilder.DropForeignKey(
                name: "FK_UsedProductDiscounts_Products_ProductId",
                table: "UsedProductDiscounts");

            migrationBuilder.DropForeignKey(
                name: "FK_UsedUserDiscounts_Discounts_DiscountId",
                table: "UsedUserDiscounts");

            migrationBuilder.DropForeignKey(
                name: "FK_UsedUserDiscounts_Orders_OrderId",
                table: "UsedUserDiscounts");

            migrationBuilder.DropForeignKey(
                name: "FK_UsedUserDiscounts_Users_UserId",
                table: "UsedUserDiscounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsedUserDiscounts",
                table: "UsedUserDiscounts");

            migrationBuilder.DropIndex(
                name: "IX_UsedUserDiscounts_DiscountId",
                table: "UsedUserDiscounts");

            migrationBuilder.DropIndex(
                name: "IX_UsedUserDiscounts_OrderId",
                table: "UsedUserDiscounts");

            migrationBuilder.DropIndex(
                name: "IX_UsedUserDiscounts_UserId",
                table: "UsedUserDiscounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsedProductDiscounts",
                table: "UsedProductDiscounts");

            migrationBuilder.DropIndex(
                name: "IX_UsedProductDiscounts_DiscountId",
                table: "UsedProductDiscounts");

            migrationBuilder.DropIndex(
                name: "IX_UsedProductDiscounts_OrderId",
                table: "UsedProductDiscounts");

            migrationBuilder.DropIndex(
                name: "IX_UsedProductDiscounts_ProductId",
                table: "UsedProductDiscounts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UsedUserDiscounts");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "UsedUserDiscounts");

            migrationBuilder.DropColumn(
                name: "UsedDate",
                table: "UsedUserDiscounts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UsedProductDiscounts");

            migrationBuilder.DropColumn(
                name: "UsedDate",
                table: "UsedProductDiscounts");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "UsedProductDiscounts",
                newName: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsedUserDiscounts",
                table: "UsedUserDiscounts",
                columns: new[] { "UserId", "DiscountId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsedProductDiscounts",
                table: "UsedProductDiscounts",
                columns: new[] { "UserId", "DiscountId" });
        }
    }
}
