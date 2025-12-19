using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaneenStore2.DAL.Migrations
{
    /// <inheritdoc />
    public partial class resetPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodeResetPassword",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CodeResetPasswordExpired",
                table: "User",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeResetPassword",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CodeResetPasswordExpired",
                table: "User");
        }
    }
}
