using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PSV.Migrations
{
    /// <inheritdoc />
    public partial class Initialize : Migration
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
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NIP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Client_pk", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Location_pk", x => x.Id);
                });

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
                name: "Operator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdLocation = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Operator_pk", x => x.Id);
                    table.ForeignKey(
                        name: "Operator_Locations",
                        column: x => x.IdLocation,
                        principalTable: "Location",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Releaser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdLocation = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Releaser_pk", x => x.Id);
                    table.ForeignKey(
                        name: "Releaser_Location",
                        column: x => x.IdLocation,
                        principalTable: "Location",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Cut",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<DateTime>(type: "datetime2", nullable: true),
                    To = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPresent = table.Column<bool>(type: "bit", nullable: false),
                    ClientNotified = table.Column<bool>(type: "bit", nullable: false),
                    IdOperator = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Cut_pk", x => x.Id);
                    table.ForeignKey(
                        name: "Operator_Cut",
                        column: x => x.IdOperator,
                        principalTable: "Operator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Milling",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<DateTime>(type: "datetime2", nullable: true),
                    To = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPresent = table.Column<bool>(type: "bit", nullable: false),
                    ClientNotified = table.Column<bool>(type: "bit", nullable: false),
                    IdOperator = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Milling_pk", x => x.Id);
                    table.ForeignKey(
                        name: "Operator_Milling",
                        column: x => x.IdOperator,
                        principalTable: "Operator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Wrapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<DateTime>(type: "datetime2", nullable: true),
                    To = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPresent = table.Column<bool>(type: "bit", nullable: false),
                    ClientNotified = table.Column<bool>(type: "bit", nullable: false),
                    IdOperator = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Wrapping_pk", x => x.Id);
                    table.ForeignKey(
                        name: "Operator_Wrapping",
                        column: x => x.IdOperator,
                        principalTable: "Operator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdClient = table.Column<int>(type: "int", nullable: true),
                    QrCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BarCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StagesCompleted = table.Column<int>(type: "int", nullable: false),
                    StagesTotal = table.Column<int>(type: "int", nullable: false),
                    EdgeCodeProvided = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EdgeCodeUsed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Photos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCut = table.Column<int>(type: "int", nullable: false),
                    IdMilling = table.Column<int>(type: "int", nullable: false),
                    IdWrapping = table.Column<int>(type: "int", nullable: false),
                    IdLocation = table.Column<int>(type: "int", nullable: false),
                    IdReleaser = table.Column<int>(type: "int", nullable: true),
                    IdOrderStatus = table.Column<int>(type: "int", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                    table.ForeignKey(
                        name: "Order_Cut",
                        column: x => x.IdCut,
                        principalTable: "Cut",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Order_Location",
                        column: x => x.IdLocation,
                        principalTable: "Location",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Order_Milling",
                        column: x => x.IdMilling,
                        principalTable: "Milling",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Order_OrderStatus",
                        column: x => x.IdOrderStatus,
                        principalTable: "OrderStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Order_Releaser",
                        column: x => x.IdReleaser,
                        principalTable: "Releaser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Order_Wrapping",
                        column: x => x.IdWrapping,
                        principalTable: "Wrapping",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdOperator = table.Column<int>(type: "int", nullable: true),
                    IdOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Comment_pk", x => x.Id);
                    table.ForeignKey(
                        name: "Operator_Comment",
                        column: x => x.IdOperator,
                        principalTable: "Operator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "Order_Comment",
                        column: x => x.IdOrder,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Location",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Przasnysz" },
                    { 2, "Jednorożec" }
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
                name: "IX_Comment_IdOperator",
                table: "Comment",
                column: "IdOperator");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_IdOrder",
                table: "Comment",
                column: "IdOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Cut_IdOperator",
                table: "Cut",
                column: "IdOperator");

            migrationBuilder.CreateIndex(
                name: "IX_Milling_IdOperator",
                table: "Milling",
                column: "IdOperator");

            migrationBuilder.CreateIndex(
                name: "IX_Operator_IdLocation",
                table: "Operator",
                column: "IdLocation");

            migrationBuilder.CreateIndex(
                name: "IX_Order_IdClient",
                table: "Order",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_Order_IdCut",
                table: "Order",
                column: "IdCut",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_IdLocation",
                table: "Order",
                column: "IdLocation");

            migrationBuilder.CreateIndex(
                name: "IX_Order_IdMilling",
                table: "Order",
                column: "IdMilling",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_IdOrderStatus",
                table: "Order",
                column: "IdOrderStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Order_IdReleaser",
                table: "Order",
                column: "IdReleaser");

            migrationBuilder.CreateIndex(
                name: "IX_Order_IdWrapping",
                table: "Order",
                column: "IdWrapping",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Releaser_IdLocation",
                table: "Releaser",
                column: "IdLocation");

            migrationBuilder.CreateIndex(
                name: "IX_Wrapping_IdOperator",
                table: "Wrapping",
                column: "IdOperator");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Cut");

            migrationBuilder.DropTable(
                name: "Milling");

            migrationBuilder.DropTable(
                name: "OrderStatuses");

            migrationBuilder.DropTable(
                name: "Releaser");

            migrationBuilder.DropTable(
                name: "Wrapping");

            migrationBuilder.DropTable(
                name: "Operator");

            migrationBuilder.DropTable(
                name: "Location");
        }
    }
}
