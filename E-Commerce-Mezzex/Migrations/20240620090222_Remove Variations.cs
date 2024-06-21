using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Mezzex.Migrations
{
    /// <inheritdoc />
    public partial class RemoveVariations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_ProductVariants_ProductVariantId",
                table: "Pictures");

            migrationBuilder.DropTable(
                name: "ProductVariants");

            migrationBuilder.DropTable(
                name: "VariationValue");

            migrationBuilder.DropTable(
                name: "VariationTypes");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_ProductVariantId",
                table: "Pictures");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VariationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VariationValue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VariationTypeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariationValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VariationValue_VariationTypes_VariationTypeId",
                        column: x => x.VariationTypeId,
                        principalTable: "VariationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    VariationValueId = table.Column<int>(type: "int", nullable: false),
                    PriceAdjustment = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariants_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductVariants_VariationValue_VariationValueId",
                        column: x => x.VariationValueId,
                        principalTable: "VariationValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_ProductVariantId",
                table: "Pictures",
                column: "ProductVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ProductId",
                table: "ProductVariants",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_VariationValueId",
                table: "ProductVariants",
                column: "VariationValueId");

            migrationBuilder.CreateIndex(
                name: "IX_VariationValue_VariationTypeId",
                table: "VariationValue",
                column: "VariationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_ProductVariants_ProductVariantId",
                table: "Pictures",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "Id");
        }
    }
}
