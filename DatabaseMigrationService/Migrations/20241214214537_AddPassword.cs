using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseMigrationService.Migrations
{
    /// <inheritdoc />
    public partial class AddPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserCurrencies_UserId",
                table: "UserCurrencies");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCurrencies",
                table: "UserCurrencies",
                columns: new[] { "UserId", "CurrencyId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCurrencies",
                table: "UserCurrencies");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_UserCurrencies_UserId",
                table: "UserCurrencies",
                column: "UserId");
        }
    }
}
