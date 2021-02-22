using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS5.Migrations
{
    public partial class Add_NewApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "mobile",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "userAddress",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "userAddtime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "userAge",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "userEdu",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "userEmail",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "userImg",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "userNickname",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "userPhone",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "userPwd",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "userSex",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "userTag",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "userWechatid",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "userWechatname",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "userWorkPhone",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mobile",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "userAddress",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "userAddtime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "userAge",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "userEdu",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "userEmail",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "userImg",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "userNickname",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "userPhone",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "userPwd",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "userSex",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "userTag",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "userWechatid",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "userWechatname",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "userWorkPhone",
                table: "AspNetUsers");
        }
    }
}
