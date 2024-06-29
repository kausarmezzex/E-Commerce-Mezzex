using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Mezzex.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelatedProducttoproductrelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RelatedProducts");

            migrationBuilder.CreateTable(
                name: "ProductRelationships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainProductId = table.Column<int>(type: "int", nullable: false),
                    RelatedProductId = table.Column<int>(type: "int", nullable: false),
                    RelatedProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelatedIsPublish = table.Column<bool>(type: "bit", nullable: false),
                    RelatedProductPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsRelatedProduct = table.Column<bool>(type: "bit", nullable: false),
                    IsCrossSellProduct = table.Column<bool>(type: "bit", nullable: false),
                    IsUpSellProduct = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRelationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRelationships_Products_MainProductId",
                        column: x => x.MainProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductRelationships_Products_RelatedProductId",
                        column: x => x.RelatedProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductRelationships_MainProductId",
                table: "ProductRelationships",
                column: "MainProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRelationships_RelatedProductId",
                table: "ProductRelationships",
                column: "RelatedProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductRelationships");

            migrationBuilder.CreateTable(
                name: "RelatedProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainProductId = table.Column<int>(type: "int", nullable: false),
                    RelatedProductId = table.Column<int>(type: "int", nullable: false),
                    IsCrossSellProduct = table.Column<bool>(type: "bit", nullable: false),
                    IsRelatedProduct = table.Column<bool>(type: "bit", nullable: false),
                    IsUpSellProduct = table.Column<bool>(type: "bit", nullable: false),
                    RelatedIsPublish = table.Column<bool>(type: "bit", nullable: false),
                    RelatedProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelatedProductPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelatedProducts_Products_MainProductId",
                        column: x => x.MainProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RelatedProducts_Products_RelatedProductId",
                        column: x => x.RelatedProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RelatedProducts_MainProductId",
                table: "RelatedProducts",
                column: "MainProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedProducts_RelatedProductId",
                table: "RelatedProducts",
                column: "RelatedProductId");
        }
    }
}
