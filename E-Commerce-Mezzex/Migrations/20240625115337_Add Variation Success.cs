using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Mezzex.Migrations
{
    /// <inheritdoc />
    public partial class AddVariationSuccess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VariationValues_Products_ProductId",
                table: "VariationValues");

            migrationBuilder.AddForeignKey(
                name: "FK_VariationValues_Products_ProductId",
                table: "VariationValues",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VariationValues_Products_ProductId",
                table: "VariationValues");

            migrationBuilder.AddForeignKey(
                name: "FK_VariationValues_Products_ProductId",
                table: "VariationValues",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
