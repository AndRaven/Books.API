using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Books.API.Migrations
{
    public partial class BooksSeedMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Description", "Genre", "Language", "Pages", "Title", "Url", "Year" },
                values: new object[] { 1, "Erin Morgenstern", "Written in rich, seductive prose, this spell-casting novel is a feast for the senses and the heart.", "fiction", "English", 506, "The Night Circus", null, 2011 });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Description", "Genre", "Language", "Pages", "Title", "Url", "Year" },
                values: new object[] { 2, "Anthony Doer", "When everything is lost, it’s our stories that survive.", "fiction", "English", 626, "Cloud Cuckoo Land", null, 2021 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
