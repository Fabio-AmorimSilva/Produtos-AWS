using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProdutosAws.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingImageValueObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Image",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "Image");
        }
    }
}
