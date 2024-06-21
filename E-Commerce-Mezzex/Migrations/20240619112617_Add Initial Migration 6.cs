using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Mezzex.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialMigration6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId1",
                table: "QuestionsAnswers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsAnswers_ProductId1",
                table: "QuestionsAnswers",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionsAnswers_Products_ProductId1",
                table: "QuestionsAnswers",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionsAnswers_Products_ProductId1",
                table: "QuestionsAnswers");

            migrationBuilder.DropIndex(
                name: "IX_QuestionsAnswers_ProductId1",
                table: "QuestionsAnswers");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "QuestionsAnswers");
        }
    }
}
