using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Mezzex.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialMigration7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_ProductVariants_ProductVariantId",
                table: "Pictures");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariants_Products_ProductId",
                table: "ProductVariants");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariants_Products_ProductId1",
                table: "ProductVariants");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariants_ProductId",
                table: "ProductVariants");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariants_ProductId1",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "ProductVariants");

            migrationBuilder.RenameColumn(
                name: "Size",
                table: "ProductVariants",
                newName: "Name");

            migrationBuilder.CreateTable(
                name: "VariationValue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VariationTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariationValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VariationValue_ProductVariants_VariationTypeId",
                        column: x => x.VariationTypeId,
                        principalTable: "ProductVariants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductId1 = table.Column<int>(type: "int", nullable: false),
                    VariationValueId = table.Column<int>(type: "int", nullable: false),
                    PriceAdjustment = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariant_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductVariant_Products_ProductId1",
                        column: x => x.ProductId1,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductVariant_VariationValue_VariationValueId",
                        column: x => x.VariationValueId,
                        principalTable: "VariationValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariant_ProductId",
                table: "ProductVariant",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariant_ProductId1",
                table: "ProductVariant",
                column: "ProductId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariant_VariationValueId",
                table: "ProductVariant",
                column: "VariationValueId");

            migrationBuilder.CreateIndex(
                name: "IX_VariationValue_VariationTypeId",
                table: "VariationValue",
                column: "VariationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_ProductVariant_ProductVariantId",
                table: "Pictures",
                column: "ProductVariantId",
                principalTable: "ProductVariant",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_ProductVariant_ProductVariantId",
                table: "Pictures");

            migrationBuilder.DropTable(
                name: "ProductVariant");

            migrationBuilder.DropTable(
                name: "VariationValue");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ProductVariants",
                newName: "Size");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "ProductVariants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ProductVariants",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ProductVariants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductId1",
                table: "ProductVariants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ProductId",
                table: "ProductVariants",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ProductId1",
                table: "ProductVariants",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_ProductVariants_ProductVariantId",
                table: "Pictures",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariants_Products_ProductId",
                table: "ProductVariants",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariants_Products_ProductId1",
                table: "ProductVariants",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
