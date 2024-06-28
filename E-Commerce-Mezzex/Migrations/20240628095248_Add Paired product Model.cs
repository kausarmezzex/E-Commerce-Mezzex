using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Mezzex.Migrations
{
    /// <inheritdoc />
    public partial class AddPairedproductModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PairDiscount",
                table: "Products",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PairedProductId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_PairedProductId",
                table: "Products",
                column: "PairedProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Products_PairedProductId",
                table: "Products",
                column: "PairedProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Products_PairedProductId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_PairedProductId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PairDiscount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PairedProductId",
                table: "Products");
        }
    }
}
