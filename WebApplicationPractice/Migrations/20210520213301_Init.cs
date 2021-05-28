using Microsoft.EntityFrameworkCore.Migrations;

namespace BookAPI.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookStore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookStore", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "BookStore",
                columns: new[] { "Id", "Author", "Description", "Title" },
                values: new object[] { 1, "Ghassan Kanafani", "Novel by Palestinian writer", "Men In The Sun" });

            migrationBuilder.InsertData(
                table: "BookStore",
                columns: new[] { "Id", "Author", "Description", "Title" },
                values: new object[] { 2, "Mahmoud Darwish", "Combines the complete text of Darwish's two most recent full-length volumes,", "The butterfly's burden" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookStore");
        }
    }
}
