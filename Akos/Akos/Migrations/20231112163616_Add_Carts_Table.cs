using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Akos.Migrations
{
    /// <inheritdoc />
    public partial class Add_Carts_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "dishes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dishes_CartId",
                table: "dishes",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_dishes_Carts_CartId",
                table: "dishes",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dishes_Carts_CartId",
                table: "dishes");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_dishes_CartId",
                table: "dishes");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "dishes");
        }
    }
}
