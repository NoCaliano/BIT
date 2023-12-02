using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIT.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class AddcourIdToOrderModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourId",
                table: "Orders");
        }
    }
}
