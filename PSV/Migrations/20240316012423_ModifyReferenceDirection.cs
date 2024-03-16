using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSV.Migrations
{
    /// <inheritdoc />
    public partial class ModifyReferenceDirection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Order_Cut",
                table: "Cut");

            migrationBuilder.DropForeignKey(
                name: "Order_Milling",
                table: "Milling");

            migrationBuilder.DropForeignKey(
                name: "Order_Wrapping",
                table: "Wrapping");

            migrationBuilder.DropIndex(
                name: "IX_Wrapping_IdOrder",
                table: "Wrapping");

            migrationBuilder.DropIndex(
                name: "IX_Milling_IdOrder",
                table: "Milling");

            migrationBuilder.DropIndex(
                name: "IX_Cut_IdOrder",
                table: "Cut");

            migrationBuilder.DropColumn(
                name: "IdOrder",
                table: "Wrapping");

            migrationBuilder.DropColumn(
                name: "IdOrder",
                table: "Milling");

            migrationBuilder.DropColumn(
                name: "IdOrder",
                table: "Cut");

            migrationBuilder.AddColumn<int>(
                name: "IdCut",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdMilling",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdWrapping",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Order_IdCut",
                table: "Order",
                column: "IdCut",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_IdMilling",
                table: "Order",
                column: "IdMilling",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_IdWrapping",
                table: "Order",
                column: "IdWrapping",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "Order_Cut",
                table: "Order",
                column: "IdCut",
                principalTable: "Cut",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "Order_Milling",
                table: "Order",
                column: "IdMilling",
                principalTable: "Milling",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "Order_Wrapping",
                table: "Order",
                column: "IdWrapping",
                principalTable: "Wrapping",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Order_Cut",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "Order_Milling",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "Order_Wrapping",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_IdCut",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_IdMilling",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_IdWrapping",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "IdCut",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "IdMilling",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "IdWrapping",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "IdOrder",
                table: "Wrapping",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdOrder",
                table: "Milling",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdOrder",
                table: "Cut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Wrapping_IdOrder",
                table: "Wrapping",
                column: "IdOrder",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Milling_IdOrder",
                table: "Milling",
                column: "IdOrder",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cut_IdOrder",
                table: "Cut",
                column: "IdOrder",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "Order_Cut",
                table: "Cut",
                column: "IdOrder",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "Order_Milling",
                table: "Milling",
                column: "IdOrder",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "Order_Wrapping",
                table: "Wrapping",
                column: "IdOrder",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
