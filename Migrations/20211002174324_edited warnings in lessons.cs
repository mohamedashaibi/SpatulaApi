using Microsoft.EntityFrameworkCore.Migrations;

namespace SpatulaApi.Migrations
{
    public partial class editedwarningsinlessons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f063bc0a-a963-4340-8126-53f80c342e48");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f1a48243-153c-46bd-be8d-d08e2e309af9");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a6d7ae48-c34c-4224-80ef-4dc5e53267ce", "8cdf6498-7273-4446-ba7c-7befd7ef0560", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "db99e2c0-7c9e-4c1d-af45-809a2a1f5e7f", "92dbb738-cdfa-4c4a-825e-053e30155702", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a6d7ae48-c34c-4224-80ef-4dc5e53267ce");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db99e2c0-7c9e-4c1d-af45-809a2a1f5e7f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f1a48243-153c-46bd-be8d-d08e2e309af9", "506058ec-cb64-4229-b512-bb5bee2c78aa", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f063bc0a-a963-4340-8126-53f80c342e48", "c8c38cba-770f-44c6-90a1-af7cc07a3532", "Administrator", "ADMINISTRATOR" });
        }
    }
}
