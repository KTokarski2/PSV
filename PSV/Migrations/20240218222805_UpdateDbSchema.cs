using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSV.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Wrappings",
                newName: "Wrapping");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameTable(
                name: "Millings",
                newName: "Milling");

            migrationBuilder.RenameTable(
                name: "Cuts",
                newName: "Cut");

            migrationBuilder.RenameIndex(
                name: "IX_Wrappings_IdOrder",
                table: "Wrapping",
                newName: "IX_Wrapping_IdOrder");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_IdClient",
                table: "Order",
                newName: "IX_Order_IdClient");

            migrationBuilder.RenameIndex(
                name: "IX_Millings_IdOrder",
                table: "Milling",
                newName: "IX_Milling_IdOrder");

            migrationBuilder.RenameIndex(
                name: "IX_Cuts_IdOrder",
                table: "Cut",
                newName: "IX_Cut_IdOrder");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Wrapping",
                newName: "Wrappings");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "Milling",
                newName: "Millings");

            migrationBuilder.RenameTable(
                name: "Cut",
                newName: "Cuts");

            migrationBuilder.RenameIndex(
                name: "IX_Wrapping_IdOrder",
                table: "Wrappings",
                newName: "IX_Wrappings_IdOrder");

            migrationBuilder.RenameIndex(
                name: "IX_Order_IdClient",
                table: "Orders",
                newName: "IX_Orders_IdClient");

            migrationBuilder.RenameIndex(
                name: "IX_Milling_IdOrder",
                table: "Millings",
                newName: "IX_Millings_IdOrder");

            migrationBuilder.RenameIndex(
                name: "IX_Cut_IdOrder",
                table: "Cuts",
                newName: "IX_Cuts_IdOrder");
        }
    }
}
