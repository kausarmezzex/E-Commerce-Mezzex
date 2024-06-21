using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Mezzex.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialMigration5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RelatedProducts_Products_RelatedProductDetailsId",
                table: "RelatedProducts");

            migrationBuilder.DropIndex(
                name: "IX_RelatedProducts_RelatedProductDetailsId",
                table: "RelatedProducts");

            migrationBuilder.DropColumn(
                name: "RelatedProductDetailsId",
                table: "RelatedProducts");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedProducts_RelatedProductId",
                table: "RelatedProducts",
                column: "RelatedProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_RelatedProducts_Products_RelatedProductId",
                table: "RelatedProducts",
                column: "RelatedProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RelatedProducts_Products_RelatedProductId",
                table: "RelatedProducts");

            migrationBuilder.DropIndex(
                name: "IX_RelatedProducts_RelatedProductId",
                table: "RelatedProducts");

            migrationBuilder.AddColumn<int>(
                name: "RelatedProductDetailsId",
                table: "RelatedProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RelatedProducts_RelatedProductDetailsId",
                table: "RelatedProducts",
                column: "RelatedProductDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_RelatedProducts_Products_RelatedProductDetailsId",
                table: "RelatedProducts",
                column: "RelatedProductDetailsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
