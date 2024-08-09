using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSV.Migrations
{
    /// <inheritdoc />
    public partial class OrderFileField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderFile",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderFile",
                table: "Order");
        }
    }
}
