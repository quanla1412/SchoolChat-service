using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolChat.Service.Migrations
{
    /// <inheritdoc />
    public partial class UploadAvatar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomUsers_ChatRooms_ChatRoomId",
                table: "ChatRoomUsers");

            migrationBuilder.AlterColumn<string>(
                name: "ChatRoomId",
                table: "ChatRoomUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomUsers_ChatRooms_ChatRoomId",
                table: "ChatRoomUsers",
                column: "ChatRoomId",
                principalTable: "ChatRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomUsers_ChatRooms_ChatRoomId",
                table: "ChatRoomUsers");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "ChatRoomId",
                table: "ChatRoomUsers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomUsers_ChatRooms_ChatRoomId",
                table: "ChatRoomUsers",
                column: "ChatRoomId",
                principalTable: "ChatRooms",
                principalColumn: "Id");
        }
    }
}
