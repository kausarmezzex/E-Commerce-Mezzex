using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Mezzex.Migrations
{
    /// <inheritdoc />
    public partial class AddCrossSellAndUpSell : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductCrossSells",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CrossSellProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCrossSells", x => new { x.ProductId, x.CrossSellProductId });
                    table.ForeignKey(
                        name: "FK_ProductCrossSells_Products_CrossSellProductId",
                        column: x => x.CrossSellProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductCrossSells_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductUpsells",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UpsellProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductUpsells", x => new { x.ProductId, x.UpsellProductId });
                    table.ForeignKey(
                        name: "FK_ProductUpsells_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductUpsells_Products_UpsellProductId",
                        column: x => x.UpsellProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCrossSells_CrossSellProductId",
                table: "ProductCrossSells",
                column: "CrossSellProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductUpsells_UpsellProductId",
                table: "ProductUpsells",
                column: "UpsellProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCrossSells");

            migrationBuilder.DropTable(
                name: "ProductUpsells");
        }
    }
}
