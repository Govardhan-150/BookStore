using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBookStoreApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeyToProductRelatedToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "ProductTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CategoryId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CategoryId",
                value: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypes_CategoryId",
                table: "ProductTypes",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTypes_Categories_CategoryId",
                table: "ProductTypes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductTypes_Categories_CategoryId",
                table: "ProductTypes");

            migrationBuilder.DropIndex(
                name: "IX_ProductTypes_CategoryId",
                table: "ProductTypes");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ProductTypes");
        }
    }
}
