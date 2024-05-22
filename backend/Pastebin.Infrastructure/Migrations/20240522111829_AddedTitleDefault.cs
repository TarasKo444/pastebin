using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pastebin.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedTitleDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Pastes",
                type: "text",
                nullable: false,
                defaultValue: "Untitled",
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Pastes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "Untitled");
        }
    }
}
