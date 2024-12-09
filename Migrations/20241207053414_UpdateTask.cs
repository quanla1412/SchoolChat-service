using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolChat.Service.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "UserTasks");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "UserTasks",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "UserTasks",
                newName: "CreatedDate");

            migrationBuilder.AlterColumn<string>(
                name: "ChatRoomId",
                table: "UserTasks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "UserTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "UserTasks");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "UserTasks",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "UserTasks",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "ChatRoomId",
                table: "UserTasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "UserTasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
