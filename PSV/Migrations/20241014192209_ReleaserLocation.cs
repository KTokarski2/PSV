using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSV.Migrations
{
    /// <inheritdoc />
    public partial class ReleaserLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdLocation",
                table: "Releaser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Releaser_IdLocation",
                table: "Releaser",
                column: "IdLocation");

            migrationBuilder.AddForeignKey(
                name: "Releaser_Location",
                table: "Releaser",
                column: "IdLocation",
                principalTable: "Location",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Releaser_Location",
                table: "Releaser");

            migrationBuilder.DropIndex(
                name: "IX_Releaser_IdLocation",
                table: "Releaser");

            migrationBuilder.DropColumn(
                name: "IdLocation",
                table: "Releaser");
        }
    }
}
