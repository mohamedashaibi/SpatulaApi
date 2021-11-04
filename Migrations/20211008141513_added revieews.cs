using Microsoft.EntityFrameworkCore.Migrations;

namespace SpatulaApi.Migrations
{
    public partial class addedrevieews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppSpeed = table.Column<short>(type: "smallint", nullable: false),
                    DownloadQuality = table.Column<short>(type: "smallint", nullable: false),
                    PaymentQuality = table.Column<short>(type: "smallint", nullable: false),
                    EaseOfUse = table.Column<short>(type: "smallint", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a6d7ae48-c34c-4224-80ef-4dc5e53267ce",
                column: "ConcurrencyStamp",
                value: "d412d53c-2112-4fa5-a3a7-fe6a82c79504");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db99e2c0-7c9e-4c1d-af45-809a2a1f5e7f",
                column: "ConcurrencyStamp",
                value: "1fb10fb2-79cf-451f-8598-87ac183c91dc");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "db99e2c0-7c9e-4c1d-af45-809a2a1f5e7f", "04be4ced-e4cb-408f-8f55-38cd0677cbec" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "04be4ced-e4cb-408f-8f55-38cd0677cbec");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a6d7ae48-c34c-4224-80ef-4dc5e53267ce",
                column: "ConcurrencyStamp",
                value: "8cdf6498-7273-4446-ba7c-7befd7ef0560");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db99e2c0-7c9e-4c1d-af45-809a2a1f5e7f",
                column: "ConcurrencyStamp",
                value: "92dbb738-cdfa-4c4a-825e-053e30155702");
        }
    }
}
