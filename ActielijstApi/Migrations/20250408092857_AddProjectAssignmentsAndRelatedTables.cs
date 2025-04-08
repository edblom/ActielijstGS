using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActielijstApi.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectAssignmentsAndRelatedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "fldMActieGereed",
                schema: "dbo",
                table: "tblMemo",
                newName: "FldMActieGereed");

            migrationBuilder.AddColumn<DateTime>(
                name: "FldDatumUitDienst",
                schema: "dbo",
                table: "Werknemers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "SSMA_TimeStamp",
                schema: "dbo",
                table: "tblMemo",
                type: "rowversion",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.CreateTable(
                name: "adres",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Klantnum = table.Column<int>(type: "int", nullable: true),
                    Zoekcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BEDRIJF = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tav = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Geachte = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VestigAdr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VestigPc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VestigPlaats = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Postadres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Wpl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Land = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelPrive = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobelTel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Categorie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Omschr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HardSoft = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EMAIL_ADR = table.Column<string>(name: "E-MAIL_ADR", type: "nvarchar(max)", nullable: true),
                    Opmerkingen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cursistnr = table.Column<int>(type: "int", nullable: true),
                    Sofinummer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GebDatumOud = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GebDatum = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GebPlaats = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Voorletters = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Roepnaam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Voorvoegsel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tussenvoegsel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Achternaam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cursist = table.Column<bool>(type: "bit", nullable: true),
                    Bedrijfsadresid = table.Column<int>(type: "int", nullable: true),
                    Bedrijskoppeling = table.Column<int>(type: "int", nullable: true),
                    Leverancier = table.Column<bool>(type: "bit", nullable: true),
                    Debiteurnummer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Esteco = table.Column<bool>(type: "bit", nullable: true),
                    FldWebSite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attentie = table.Column<bool>(type: "bit", nullable: true),
                    LoginNaam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoginPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatumCursusdoc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CursistId = table.Column<int>(type: "int", nullable: true),
                    DatumJaarMon1 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DatumJaarMon2 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DatumJaarMon3 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TekstJaarMon1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TekstJaarMon2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TekstJaarMon3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Partner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NrSgg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deelnemer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KiwaNummer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KomOhouder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldId = table.Column<int>(type: "int", nullable: true),
                    FirstContactId = table.Column<int>(type: "int", nullable: true),
                    EmailFactuur = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAanmaning = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KiwaContactId = table.Column<int>(type: "int", nullable: true),
                    MeldSoort = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SsmaTimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adres", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tblGlobals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    savepath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    designpath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sjabloonpath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    version = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    archiefpath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pdfPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScanPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KiwaPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FotoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FactuurText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FactuurAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FactuurHandtekening = table.Column<bool>(type: "bit", nullable: false),
                    DisplayMailVoorVerzenden = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblGlobals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblprojecten",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fldProjectNummer = table.Column<int>(type: "int", nullable: true),
                    fldExternNummer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldExternNummer2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldProjectNaam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldAfdeling = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldjaar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldDatum = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fldAdres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldPC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldPlaats = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldSoort = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldActie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldIntracNr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldSGG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldEPA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldOpdrachtgeverId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldOpdrachtgever = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldFolder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldArchiefMap = table.Column<bool>(type: "bit", nullable: false),
                    fldVerwerkendBedrijf = table.Column<int>(type: "int", nullable: true),
                    fldFabrikant = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldSysteem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldAantalM2 = table.Column<int>(type: "int", nullable: true),
                    fldKiWa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldKiwaCert = table.Column<bool>(type: "bit", nullable: false),
                    SSMA_TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    fldAfwerking = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldPrevProjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblprojecten", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tblStandaardDoc",
                columns: table => new
                {
                    doc_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fldNaamDoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldPathDoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldDocOmschrijving = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldDocNum = table.Column<int>(type: "int", nullable: true),
                    fldDocSavePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldProjectMap = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fldDocPrefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldSoort = table.Column<int>(type: "int", nullable: true),
                    fldPrijsId = table.Column<int>(type: "int", nullable: true),
                    fldEmailSjabloon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldEmailAan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fldEmailSubject = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblStandaardDoc", x => x.doc_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "adres");

            migrationBuilder.DropTable(
                name: "tblGlobals");

            migrationBuilder.DropTable(
                name: "tblprojecten");

            migrationBuilder.DropTable(
                name: "tblStandaardDoc");

            migrationBuilder.DropColumn(
                name: "FldDatumUitDienst",
                schema: "dbo",
                table: "Werknemers");

            migrationBuilder.RenameColumn(
                name: "FldMActieGereed",
                schema: "dbo",
                table: "tblMemo",
                newName: "fldMActieGereed");

            migrationBuilder.AlterColumn<byte[]>(
                name: "SSMA_TimeStamp",
                schema: "dbo",
                table: "tblMemo",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "rowversion",
                oldRowVersion: true,
                oldNullable: true);
        }
    }
}
