using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EfCoreCodeFirst.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "table_products",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "table_categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_table_categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_table_products_CategoryId",
                table: "table_products",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_table_products_table_categories_CategoryId",
                table: "table_products",
                column: "CategoryId",
                principalTable: "table_categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_table_products_table_categories_CategoryId",
                table: "table_products");

            migrationBuilder.DropTable(
                name: "table_categories");

            migrationBuilder.DropIndex(
                name: "IX_table_products_CategoryId",
                table: "table_products");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "table_products");
        }
    }
}
