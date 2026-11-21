using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace books.Migrations;

/// <inheritdoc />
public partial class _20261119161803_AddFavoriteBooks : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
                             CREATE TABLE IF NOT EXISTS FavoriteBooks (
                                 Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                 UserId TEXT NOT NULL,
                                 BookId TEXT NOT NULL,
                                 FOREIGN KEY (UserId)
                                     REFERENCES AspNetUsers(Id)
                                     ON DELETE CASCADE
                             );

                             CREATE UNIQUE INDEX IF NOT EXISTS
                             IX_FavoriteBooks_UserId_BookId
                             ON FavoriteBooks(UserId, BookId);
                             """);
    }
    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "FavoriteBooks");
    }
}
