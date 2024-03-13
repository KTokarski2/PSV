using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSV.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Client_pk", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdClient = table.Column<int>(type: "int", nullable: false),
                    QrCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Format = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Photos = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Order_pk", x => x.Id);
                    table.ForeignKey(
                        name: "Client_Orders",
                        column: x => x.IdClient,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cuts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    To = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPresent = table.Column<bool>(type: "bit", nullable: false),
                    IdOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Cut_pk", x => x.Id);
                    table.ForeignKey(
                        name: "Order_Cut",
                        column: x => x.IdOrder,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Millings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    To = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPresent = table.Column<bool>(type: "bit", nullable: false),
                    IdOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Milling_pk", x => x.Id);
                    table.ForeignKey(
                        name: "Order_Milling",
                        column: x => x.IdOrder,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wrappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    To = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPresent = table.Column<bool>(type: "bit", nullable: false),
                    IdOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Wrapping_pk", x => x.Id);
                    table.ForeignKey(
                        name: "Order_Wrapping",
                        column: x => x.IdOrder,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cuts_IdOrder",
                table: "Cuts",
                column: "IdOrder",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Millings_IdOrder",
                table: "Millings",
                column: "IdOrder",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_IdClient",
                table: "Orders",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_Wrappings_IdOrder",
                table: "Wrappings",
                column: "IdOrder",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cuts");

            migrationBuilder.DropTable(
                name: "Millings");

            migrationBuilder.DropTable(
                name: "Wrappings");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Client");
        }
    }
}
