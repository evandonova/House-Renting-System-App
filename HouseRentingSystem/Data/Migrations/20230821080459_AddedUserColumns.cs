using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingSystem.Data.Migrations
{
    public partial class AddedUserColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "FirstName", "LastName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f04addd7-6133-48e2-928f-cb3ef74fd449", "Teodor", "Lesly", "AQAAAAEAACcQAAAAEEkJ8RzQVBPHr7wo9bXMQm7NPyQGjn3Z11RhTuZU2fIwbnkXHcIzyEa5adDuCbcw8Q==", "e9f54b78-7149-4362-95d6-2f817d7c0032" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "FirstName", "LastName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c67206f2-a9d2-476a-b600-fc4cfce8cbb7", "Linda", "Michaels", "AQAAAAEAACcQAAAAEFR+60wABHtFi4Z0Vyp/rcdhTxINrO9DZ4C9TODnAHAjcR4k70mqadvvX4r7z5IpkA==", "d8294b69-9382-49ec-b905-0bc28085d2ba" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1242fc49-8f1d-45af-bd67-137c7f754359", "AQAAAAEAACcQAAAAEG8zdajiTT6GlsV+SciN3/q08lhC29pPpmCFd5ek1sOYQEJSrVImFqECGzVdIZ+e3A==", "80031e1c-9d17-4c49-a36c-4462e9e97bea" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a974c936-5df8-498d-b3de-da5f5295b46d", "AQAAAAEAACcQAAAAEDuAUoZznCDwJz8HRICHfiGpz17G8omV/jxO8Wwi1akMxq+hfciqlojsq2ji40TXNg==", "83523421-56e1-4649-b660-f5f0f4e79222" });
        }
    }
}
