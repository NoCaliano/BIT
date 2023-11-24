using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIT.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GrandTotal = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Назва = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Опис = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Couriers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Імя = table.Column<string>(name: "Ім'я", type: "nvarchar(max)", nullable: false),
                    Прізвище = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Телефон = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Транспорт = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Працюєзараз = table.Column<bool>(name: "Працює зараз", type: "bit", nullable: true),
                    Останнязарплата = table.Column<DateTime>(name: "Остання зарплата", type: "datetime2", nullable: true),
                    Зарплата = table.Column<int>(type: "int", nullable: true),
                    Виконанодоставок = table.Column<int>(name: "Виконано доставок", type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Couriers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Страва = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Кількість = table.Column<int>(type: "int", nullable: false),
                    Клієнт = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Номертелефону = table.Column<string>(name: "Номер телефону", type: "nvarchar(max)", nullable: true),
                    Адреса = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Категорія = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Дата = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Сума = table.Column<float>(type: "real", nullable: false),
                    Коментар = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Спосібоплати = table.Column<string>(name: "Спосіб оплати", type: "nvarchar(max)", nullable: false),
                    Курєр = table.Column<string>(name: "Кур'єр", type: "nvarchar(max)", nullable: true),
                    Статус = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Requisitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Імя = table.Column<string>(name: "Ім'я", type: "nvarchar(max)", nullable: false),
                    Прізвище = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Телефон = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Місцепроживання = table.Column<string>(name: "Місце проживання", type: "nvarchar(max)", nullable: true),
                    Типтранспорту = table.Column<string>(name: "Тип транспорту", type: "nvarchar(max)", nullable: true),
                    Заявкаприйнята = table.Column<bool>(name: "Заявка прийнята", type: "bit", nullable: true),
                    Відповідь = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requisitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Назва = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Опис = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Картинка = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ціна = table.Column<float>(type: "real", nullable: false),
                    Калорії = table.Column<int>(type: "int", nullable: true),
                    Доступно = table.Column<bool>(type: "bit", nullable: false),
                    Наголовній = table.Column<bool>(name: "На головній", type: "bit", nullable: false),
                    Категорія = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dishes_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DishId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CartId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CartItems_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_DishId",
                table: "CartItems",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_OrderId",
                table: "Dishes",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Couriers");

            migrationBuilder.DropTable(
                name: "Requisitions");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Dishes");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
