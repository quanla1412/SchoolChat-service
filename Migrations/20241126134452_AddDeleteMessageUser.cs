using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolChat.Service.Migrations
{
    /// <inheritdoc />
    public partial class AddDeleteMessageUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUnsent",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUnsent",
                table: "Messages");
        }
    }
}
