using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingSystem.Data.Data.Migrations
{
    public partial class ChangedSeededData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6e1d737d-63e0-4ff5-b558-591c20042875", "GUEST@MAIL.COM", "GUEST@MAIL.COM", "AQAAAAEAACcQAAAAEDkH7ZdcxbvpbL3WS830xHXMAZZakT3wmWBiNu29/l+NJtwBWLOoGW07YpVISpf/xg==", "a8a26528-9fe0-40b1-abd8-1db10f37194e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "787345d0-d1a4-416f-8f8c-e6d40b96c0b3",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c2e43bd4-bad3-4870-8882-65acf7833baa", "ADMIN@MAIL.COM", "ADMIN@MAIL.COM", "AQAAAAEAACcQAAAAECuHo86Q6u01S5WK7jfW30ZbFB/n1KjtqWCKoN39uz/FWTau9ejNhKn8dDPC4mXIDg==", "2facb7d8-2a6e-47f7-a3c6-ef2aff93619d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "96e765ff-614a-4409-8b36-ac9cb0a1429e", "AGENT@MAIL.COM", "AGENT@MAIL.COM", "AQAAAAEAACcQAAAAELjZ1QqygDPQS4p4VnCvGCTl5UZYKgeckM036QM3f3ZRj7Gc7+F07KxgYTcuwHVVbg==", "1e1e3fad-0512-49de-bcd6-571f1f7150d6" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2a92be69-baa3-49e1-b2c8-a9e418891c31", "guest@mail.com", "guest@mail.com", "AQAAAAEAACcQAAAAEOXVzLKk/JfiTj7adlU3POHRN+YYLayzUFwDxbrfj661UVuGN6rauIlWbfzh7xW4QQ==", "8cf9fe53-539e-4915-b38a-bf9f8b442b06" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "787345d0-d1a4-416f-8f8c-e6d40b96c0b3",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1fd278b6-e6d0-4298-b384-804aecceb4ae", "admin@mail.com", "admin@mail.com", "AQAAAAEAACcQAAAAEBJcd0DBhvsMCcwr9Vi1gJF4rWBWKj90FYC9t3HySluvYOwPQtpquy2LF1uCwMosMg==", "93f34c9c-f5bd-4632-99bf-a289dea19e25" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7473de7c-477c-4f97-8f1e-a24f98e11ab1", "agent@mail.com", "agent@mail.com", "AQAAAAEAACcQAAAAED9QtwCHuggQinkL//67TyyjaTk1b+ZnMHZFkwxEtEnYvPyW4YuJNzyQsTFVmDYiSg==", "bb456c52-f1fc-4aa6-ae8a-728113819f35" });
        }
    }
}
