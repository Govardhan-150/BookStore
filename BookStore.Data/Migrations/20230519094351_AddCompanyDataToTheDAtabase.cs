using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBookStoreApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyDataToTheDAtabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "City", "Name", "PhoneNumber", "PostalCode", "State", "StreetAddress" },
                values: new object[] { 1, "Banglore", "Wipro", "9505341521", "515201", "Karnataka", "#27,EC2,BAnglore" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
