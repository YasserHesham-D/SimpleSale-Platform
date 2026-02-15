using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WoodSalesPlatform.Migrations
{
    /// <inheritdoc />
    public partial class adddonetoroder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Done",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Done",
                table: "Orders");
        }
    }
}
