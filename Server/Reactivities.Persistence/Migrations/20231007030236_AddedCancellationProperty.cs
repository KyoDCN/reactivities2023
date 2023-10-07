using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reactivities.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedCancellationProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "Activities",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "Activities");
        }
    }
}
