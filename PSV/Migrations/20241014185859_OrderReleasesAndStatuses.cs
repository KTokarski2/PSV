using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PSV.Migrations
{
    /// <inheritdoc />
    public partial class OrderReleasesAndStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdOrderStatus",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdReleaser",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "OrderStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("OrderStatus_pk", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Releaser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Releaser_pk", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "OrderStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "W produkcji" },
                    { 2, "Gotowe do odbioru" },
                    { 3, "Odebrane" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_IdOrderStatus",
                table: "Order",
                column: "IdOrderStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Order_IdReleaser",
                table: "Order",
                column: "IdReleaser");

            migrationBuilder.AddForeignKey(
                name: "Order_OrderStatus",
                table: "Order",
                column: "IdOrderStatus",
                principalTable: "OrderStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "Order_Releaser",
                table: "Order",
                column: "IdReleaser",
                principalTable: "Releaser",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Order_OrderStatus",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "Order_Releaser",
                table: "Order");

            migrationBuilder.DropTable(
                name: "OrderStatuses");

            migrationBuilder.DropTable(
                name: "Releaser");

            migrationBuilder.DropIndex(
                name: "IX_Order_IdOrderStatus",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_IdReleaser",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "IdOrderStatus",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "IdReleaser",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Order");
        }
    }
}
