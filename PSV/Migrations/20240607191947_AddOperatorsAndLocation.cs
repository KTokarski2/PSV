using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PSV.Migrations
{
    /// <inheritdoc />
    public partial class AddOperatorsAndLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdOperator",
                table: "Wrapping",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EdgeCodeProvided",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EdgeCodeUsed",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdLocation",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdOperator",
                table: "Milling",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdOperator",
                table: "Cut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Client",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NIP",
                table: "Client",
                type: "nvarchar(max)",
                nullable: true);

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
                name: "Operator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Operator_pk", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Location",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Przasnysz" },
                    { 2, "Jednorożec" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wrapping_IdOperator",
                table: "Wrapping",
                column: "IdOperator");

            migrationBuilder.CreateIndex(
                name: "IX_Order_IdLocation",
                table: "Order",
                column: "IdLocation");

            migrationBuilder.CreateIndex(
                name: "IX_Milling_IdOperator",
                table: "Milling",
                column: "IdOperator");

            migrationBuilder.CreateIndex(
                name: "IX_Cut_IdOperator",
                table: "Cut",
                column: "IdOperator");

            migrationBuilder.AddForeignKey(
                name: "Operator_Cut",
                table: "Cut",
                column: "IdOperator",
                principalTable: "Operator",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "Operator_Milling",
                table: "Milling",
                column: "IdOperator",
                principalTable: "Operator",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "Order_Location",
                table: "Order",
                column: "IdLocation",
                principalTable: "Location",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "Operator_Wrapping",
                table: "Wrapping",
                column: "IdOperator",
                principalTable: "Operator",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Operator_Cut",
                table: "Cut");

            migrationBuilder.DropForeignKey(
                name: "Operator_Milling",
                table: "Milling");

            migrationBuilder.DropForeignKey(
                name: "Order_Location",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "Operator_Wrapping",
                table: "Wrapping");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Operator");

            migrationBuilder.DropIndex(
                name: "IX_Wrapping_IdOperator",
                table: "Wrapping");

            migrationBuilder.DropIndex(
                name: "IX_Order_IdLocation",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Milling_IdOperator",
                table: "Milling");

            migrationBuilder.DropIndex(
                name: "IX_Cut_IdOperator",
                table: "Cut");

            migrationBuilder.DropColumn(
                name: "IdOperator",
                table: "Wrapping");

            migrationBuilder.DropColumn(
                name: "EdgeCodeProvided",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "EdgeCodeUsed",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "IdLocation",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "IdOperator",
                table: "Milling");

            migrationBuilder.DropColumn(
                name: "IdOperator",
                table: "Cut");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "NIP",
                table: "Client");
        }
    }
}
