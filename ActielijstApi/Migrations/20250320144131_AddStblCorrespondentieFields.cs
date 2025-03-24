using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActielijstApi.Migrations
{
    /// <inheritdoc />
    public partial class AddStblCorrespondentieFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "correspondentie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KlantID = table.Column<int>(type: "int", nullable: false),
                    fldCorOffNum = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    fldCorProjNum = table.Column<int>(type: "int", nullable: true),
                    fldCorOpdrachtNum = table.Column<int>(type: "int", nullable: true),
                    fldCorConsultancyId = table.Column<int>(type: "int", nullable: true),
                    fldCorTrainingId = table.Column<int>(type: "int", nullable: true),
                    fldCorFactuurId = table.Column<int>(type: "int", nullable: true),
                    fldCorDatum = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fldCorDatum2 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fldCorAuteur = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    fldCorOmschrijving = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    fldCorKenmerk = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    fldCorBestand = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    fldCorSoort = table.Column<int>(type: "int", nullable: true),
                    fldCorTav = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    fldCorGeachte = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    fldCorCPersId = table.Column<int>(type: "int", nullable: true),
                    fldCorExtensie = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    fldCorProgramma = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    fldSjabloon = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    fldAan = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    fldCC = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    fldFrom = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    fldBijlage = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    fldBijlage2 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    fldBijlage3 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    fldBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SSMA_TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_correspondentie", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "stblActieSoort",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Omschrijving = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stblActieSoort", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "stblCorrespondentieFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReplaceString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FieldName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Veld = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Standaardwaarde = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VeldType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdNaam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrespondentieId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stblCorrespondentieFields", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "stblPriority",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Prioriteit = table.Column<int>(type: "int", nullable: false),
                    Omschrijving = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kleur = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stblPriority", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblMemo",
                schema: "dbo",
                columns: table => new
                {
                    FldMid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FldMDatum = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WerknId = table.Column<int>(type: "int", nullable: true),
                    FldMKlantId = table.Column<int>(type: "int", nullable: true),
                    FldMContactPers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FldMOfferteId = table.Column<int>(type: "int", nullable: true),
                    FldMProjectId = table.Column<int>(type: "int", nullable: true),
                    FldOpdrachtId = table.Column<int>(type: "int", nullable: true),
                    FldOmschrijving = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FldMAfspraak = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FldMActieDatum = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FldMActieVoor = table.Column<int>(type: "int", nullable: true),
                    FldMActieVoor2 = table.Column<int>(type: "int", nullable: true),
                    fldMActieGereed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FldMActieSoort = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FldMPrioriteit = table.Column<int>(type: "int", nullable: true),
                    SSMA_TimeStamp = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblMemo", x => x.FldMid);
                });

            migrationBuilder.CreateTable(
                name: "tblProjectOnderdelen",
                columns: table => new
                {
                    fldProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblProjectOnderdelen", x => x.fldProjectId);
                });

            migrationBuilder.CreateTable(
                name: "Werknemers",
                schema: "dbo",
                columns: table => new
                {
                    WerknId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Voornaam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FldLoginNaam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Initialen = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Werknemers", x => x.WerknId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "correspondentie");

            migrationBuilder.DropTable(
                name: "stblActieSoort",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "stblCorrespondentieFields");

            migrationBuilder.DropTable(
                name: "stblPriority",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tblMemo",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tblProjectOnderdelen");

            migrationBuilder.DropTable(
                name: "Werknemers",
                schema: "dbo");
        }
    }
}
