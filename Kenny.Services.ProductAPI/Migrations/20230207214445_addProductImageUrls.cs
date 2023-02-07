using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kenny.Services.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class addProductImageUrls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://assets.unileversolutions.com/recipes-v2/54349.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://s2.glbimg.com/jJirZVMNK5ZsZ9UDLKQBqPu3iXk=/620x455/e.glbimg.com/og/ed/f/original/2020/10/20/hamburgueria_bob_beef_-_dia_das_criancas_-_foto_pfz_studio__norma_lima.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://img.itdg.com.br/tdg/images/recipes/000/000/114/201447/201447_original.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://marettimo.com.br/blog/wp-content/uploads/2022/10/Como-identificar-a-carne-que-parece-picanha.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4,
                column: "ImageUrl",
                value: "");
        }
    }
}
