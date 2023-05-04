using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBookStoreApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToProductType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ProductTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "C:\\Users\\BusapalG\\Downloads\\course_9 (2).zip\\1 5 Bulky - MVC\\images\\fortune.jpg");

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ProductTypes");
        }
    }
}
