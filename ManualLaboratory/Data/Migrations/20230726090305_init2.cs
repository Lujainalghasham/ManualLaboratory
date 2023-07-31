using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManualLaboratory.Data.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Manage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Request",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NationalId = table.Column<int>(type: "int", nullable: false),
                    UniversityNo = table.Column<int>(type: "int", nullable: false),
                    StudentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    College = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstNameEn = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FatherNameEn = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GrandfatherNameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FamilyNameEn = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FirstNameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FatherNameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GrandfatherNameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FamilyNameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNo = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MidecalfileNo = table.Column<int>(type: "int", nullable: true),
                    DateSelected = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Request", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Manage");

            migrationBuilder.DropTable(
                name: "Request");
        }
    }
}
