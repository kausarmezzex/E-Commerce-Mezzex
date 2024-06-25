using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Mezzex.Migrations
{
    /// <inheritdoc />
    public partial class AddVariationSuccess10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_VariationValues_VariationValueId",
                table: "Pictures");

            migrationBuilder.DropForeignKey(
                name: "FK_VariationValues_Products_ProductId",
                table: "VariationValues");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_VariationValues_VariationValueId",
                table: "Pictures",
                column: "VariationValueId",
                principalTable: "VariationValues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VariationValues_Products_ProductId",
                table: "VariationValues",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_VariationValues_VariationValueId",
                table: "Pictures");

            migrationBuilder.DropForeignKey(
                name: "FK_VariationValues_Products_ProductId",
                table: "VariationValues");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_VariationValues_VariationValueId",
                table: "Pictures",
                column: "VariationValueId",
                principalTable: "VariationValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VariationValues_Products_ProductId",
                table: "VariationValues",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
