using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolChat.Service.Migrations
{
    /// <inheritdoc />
    public partial class ReadMessageStatusContextMessageId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReadMessageStatuses_Messages_MessageId",
                table: "ReadMessageStatuses");

            migrationBuilder.AlterColumn<string>(
                name: "MessageId",
                table: "ReadMessageStatuses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReadMessageStatuses_Messages_MessageId",
                table: "ReadMessageStatuses",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReadMessageStatuses_Messages_MessageId",
                table: "ReadMessageStatuses");

            migrationBuilder.AlterColumn<string>(
                name: "MessageId",
                table: "ReadMessageStatuses",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_ReadMessageStatuses_Messages_MessageId",
                table: "ReadMessageStatuses",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id");
        }
    }
}
