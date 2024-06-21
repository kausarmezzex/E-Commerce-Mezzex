using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Mezzex.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialMigration8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_ProductVariant_ProductVariantId",
                table: "Pictures");

            migrationBuilder.DropForeignKey(
                name: "FK_VariationValue_ProductVariants_VariationTypeId",
                table: "VariationValue");

            migrationBuilder.DropTable(
                name: "ProductVariant");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ProductVariants");

            migrationBuilder.AddColumn<decimal>(
                name: "PriceAdjustment",
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
                name: "VariationValueId",
                table: "ProductVariants",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ProductId",
                table: "ProductVariants",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_VariationValueId",
                table: "ProductVariants",
                column: "VariationValueId");

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
                name: "FK_ProductVariants_VariationValue_VariationValueId",
                table: "ProductVariants",
                column: "VariationValueId",
                principalTable: "VariationValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VariationValue_VariationTypes_VariationTypeId",
                table: "VariationValue",
                column: "VariationTypeId",
                principalTable: "VariationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_ProductVariants_ProductVariantId",
                table: "Pictures");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariants_Products_ProductId",
                table: "ProductVariants");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariants_VariationValue_VariationValueId",
                table: "ProductVariants");

            migrationBuilder.DropForeignKey(
                name: "FK_VariationValue_VariationTypes_VariationTypeId",
                table: "VariationValue");

            migrationBuilder.DropTable(
                name: "VariationTypes");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariants_ProductId",
                table: "ProductVariants");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariants_VariationValueId",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "PriceAdjustment",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "VariationValueId",
                table: "ProductVariants");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ProductVariants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ProductVariant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId1 = table.Column<int>(type: "int", nullable: false),
                    VariationValueId = table.Column<int>(type: "int", nullable: false),
                    PriceAdjustment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_ProductVariant_ProductVariantId",
                table: "Pictures",
                column: "ProductVariantId",
                principalTable: "ProductVariant",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VariationValue_ProductVariants_VariationTypeId",
                table: "VariationValue",
                column: "VariationTypeId",
                principalTable: "ProductVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
