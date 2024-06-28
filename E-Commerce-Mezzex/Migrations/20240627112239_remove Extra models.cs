using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Mezzex.Migrations
{
    /// <inheritdoc />
    public partial class removeExtramodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCrossSells");

            migrationBuilder.DropTable(
                name: "ProductUpsells");

            migrationBuilder.DropColumn(
                name: "CrossSellProductIds",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UpsellProductIds",
                table: "Products");

            migrationBuilder.AddColumn<bool>(
                name: "IsCrossSellProduct",
                table: "RelatedProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRelatedProduct",
                table: "RelatedProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpSellProduct",
                table: "RelatedProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCrossSellProduct",
                table: "RelatedProducts");

            migrationBuilder.DropColumn(
                name: "IsRelatedProduct",
                table: "RelatedProducts");

            migrationBuilder.DropColumn(
                name: "IsUpSellProduct",
                table: "RelatedProducts");

            migrationBuilder.AddColumn<string>(
                name: "CrossSellProductIds",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpsellProductIds",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
    }
}
