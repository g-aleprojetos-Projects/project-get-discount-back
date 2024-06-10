using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project_get_discount_back.Migrations
{
    /// <inheritdoc />
    public partial class Device_token : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Device",
                table: "RefreshToken",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Device",
                table: "RefreshToken");
        }
    }
}
