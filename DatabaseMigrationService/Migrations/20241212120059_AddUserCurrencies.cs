using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseMigrationService.Migrations
{
    /// <inheritdoc />
    public partial class AddUserCurrencies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserCurrencies",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CurrencyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_UserCurrencies_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCurrencies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCurrencies_CurrencyId",
                table: "UserCurrencies",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCurrencies_UserId",
                table: "UserCurrencies",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCurrencies");
        }
    }
}
