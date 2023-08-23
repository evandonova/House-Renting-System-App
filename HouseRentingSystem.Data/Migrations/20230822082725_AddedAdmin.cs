using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingSystem.Data.Migrations
{
    public partial class AddedAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2a92be69-baa3-49e1-b2c8-a9e418891c31", "AQAAAAEAACcQAAAAEOXVzLKk/JfiTj7adlU3POHRN+YYLayzUFwDxbrfj661UVuGN6rauIlWbfzh7xW4QQ==", "8cf9fe53-539e-4915-b38a-bf9f8b442b06" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7473de7c-477c-4f97-8f1e-a24f98e11ab1", "AQAAAAEAACcQAAAAED9QtwCHuggQinkL//67TyyjaTk1b+ZnMHZFkwxEtEnYvPyW4YuJNzyQsTFVmDYiSg==", "bb456c52-f1fc-4aa6-ae8a-728113819f35" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "787345d0-d1a4-416f-8f8c-e6d40b96c0b3", 0, "1fd278b6-e6d0-4298-b384-804aecceb4ae", "admin@mail.com", false, "Great", "Admin", false, null, "admin@mail.com", "admin@mail.com", "AQAAAAEAACcQAAAAEBJcd0DBhvsMCcwr9Vi1gJF4rWBWKj90FYC9t3HySluvYOwPQtpquy2LF1uCwMosMg==", null, false, "93f34c9c-f5bd-4632-99bf-a289dea19e25", false, "admin@mail.com" });

            migrationBuilder.InsertData(
                table: "Agents",
                columns: new[] { "Id", "PhoneNumber", "UserId" },
                values: new object[] { new Guid("2d0b01e8-07fc-4069-80c3-bae95b27ff53"), "+359123456789", "787345d0-d1a4-416f-8f8c-e6d40b96c0b3" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: new Guid("2d0b01e8-07fc-4069-80c3-bae95b27ff53"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "787345d0-d1a4-416f-8f8c-e6d40b96c0b3");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f04addd7-6133-48e2-928f-cb3ef74fd449", "AQAAAAEAACcQAAAAEEkJ8RzQVBPHr7wo9bXMQm7NPyQGjn3Z11RhTuZU2fIwbnkXHcIzyEa5adDuCbcw8Q==", "e9f54b78-7149-4362-95d6-2f817d7c0032" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c67206f2-a9d2-476a-b600-fc4cfce8cbb7", "AQAAAAEAACcQAAAAEFR+60wABHtFi4Z0Vyp/rcdhTxINrO9DZ4C9TODnAHAjcR4k70mqadvvX4r7z5IpkA==", "d8294b69-9382-49ec-b905-0bc28085d2ba" });
        }
    }
}
