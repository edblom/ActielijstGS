using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("adres", Schema = "dbo")]
    public partial class Adre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? Klantnum { get; set; }

        [Column("ZOEKCODE")]
        public string Zoekcode { get; set; }

        public string Bedrijf { get; set; }

        [Column("TAV")]
        public string Tav { get; set; }

        [Column("GEACHTE")]
        public string Geachte { get; set; }

        [Column("VESTIG-ADR")]
        public string VestigAdr { get; set; }

        [Column("Vestig-PC")]
        public string VestigPc { get; set; }

        [Column("Vestig-Plaats")]
        public string VestigPlaats { get; set; }

        [Column("POSTADRES")]
        public string Postadres { get; set; }

        [Column("PC")]
        public string Pc { get; set; }

        [Column("WPL")]
        public string Wpl { get; set; }

        [Column("LAND")]
        public string Land { get; set; }

        [Column("TEL#")]
        public string Tel { get; set; }

        [Column("TEL_PRIVE")]
        public string TelPrive { get; set; }

        [Column("FAX")]
        public string Fax { get; set; }

        [Column("mobel tel")]
        public string MobelTel { get; set; }

        [Column("CATEGORIE")]
        public string Categorie { get; set; }

        [Column("OMSCHR")]
        public string Omschr { get; set; }

        [Column("HARD/SOFT")]
        public string HardSoft { get; set; }

        [Column("E-MAIL_ADR")]
        public string EMailAdr { get; set; }

        [Column("opmerkingen")]
        public string Opmerkingen { get; set; }

        [Column("cursistnr")]
        public int? Cursistnr { get; set; }

        [Column("sofinummer")]
        public string Sofinummer { get; set; }

        [Column("geb-datum_oud")]
        public string GebDatumOud { get; set; }

        [Column("geb-datum")]
        public DateTime? GebDatum { get; set; }

        [Column("geb-plaats")]
        public string GebPlaats { get; set; }

        [Column("voorletters")]
        public string Voorletters { get; set; }

        [Column("roepnaam")]
        public string Roepnaam { get; set; }

        [Column("voorvoegsel")]
        public string Voorvoegsel { get; set; }

        [Column("tussenvoegsel")]
        public string Tussenvoegsel { get; set; }

        [Column("achternaam")]
        public string Achternaam { get; set; }

        [Column("cursist")]
        public bool? Cursist { get; set; }

        [Column("bedrijfsadresid")]
        public int? Bedrijfsadresid { get; set; }

        [Column("bedrijskoppeling")]
        public int? Bedrijskoppeling { get; set; }

        [Column("leverancier")]
        public bool? Leverancier { get; set; }

        [Column("debiteurnummer")]
        public string Debiteurnummer { get; set; }

        public bool? Esteco { get; set; }

        [Column("fldWebSite")]
        public string FldWebSite { get; set; }

        [Column("attentie")]
        public bool? Attentie { get; set; }

        public string LoginNaam { get; set; }

        public string LoginPassword { get; set; }

        [Column("datum_cursusdoc")]
        public DateTime? DatumCursusdoc { get; set; }

        [Column("cursist_id")]
        public int? CursistId { get; set; }

        [Column("datumJaarMon1")]
        public DateTime? DatumJaarMon1 { get; set; }

        [Column("datumJaarMon2")]
        public DateTime? DatumJaarMon2 { get; set; }

        [Column("datumJaarMon3")]
        public DateTime? DatumJaarMon3 { get; set; }

        [Column("tekstJaarMon1")]
        public string TekstJaarMon1 { get; set; }

        [Column("tekstJaarMon2")]
        public string TekstJaarMon2 { get; set; }

        [Column("tekstJaarMon3")]
        public string TekstJaarMon3 { get; set; }

        [Column("partner")]
        public string Partner { get; set; }

        [Column("nrSGG")]
        public string NrSgg { get; set; }

        [Column("deelnemer")]
        public string Deelnemer { get; set; }

        public string KiwaNummer { get; set; }

        [Column("KOMOhouder")]
        public string Komohouder { get; set; }

        [Column("oldId")]
        public int? OldId { get; set; }

        public int? FirstContactId { get; set; }

        [Column("emailFactuur")]
        public string EmailFactuur { get; set; }

        [Column("emailAanmaning")]
        public string EmailAanmaning { get; set; }

        public int? KiwaContactId { get; set; }

        public string MeldSoort { get; set; }

        [Timestamp]
        [Column("SSMA_TimeStamp")]
        public byte[] SsmaTimeStamp { get; set; }

        [NotMapped]
        public string BedrijfEnVestigPlaats => $"{Bedrijf} - {VestigPlaats}";
    }
}