using System;

namespace ActielijstApi.Models
{
    public class Adres
    {
        public int Id { get; set; } // Primaire sleutel
        public int? Klantnum { get; set; }
        public string? Zoekcode { get; set; }
        public string? Bedrijf { get; set; } // Voor "Bedrijf" custom property
        public string? Tav { get; set; }
        public string? Geachte { get; set; }
        public string? VestigAdr { get; set; }
        public string? VestigPc { get; set; }
        public string? VestigPlaats { get; set; }
        public string? Postadres { get; set; }
        public string? Pc { get; set; }
        public string? Wpl { get; set; }
        public string? Land { get; set; }
        public string? Tel { get; set; }
        public string? TelPrive { get; set; }
        public string? Fax { get; set; }
        public string? MobelTel { get; set; }
        public string? Categorie { get; set; }
        public string? Omschr { get; set; }
        public string? HardSoft { get; set; }
        public string? EmailAdr { get; set; } // Voor "BedrijfEmail" custom property
        public string? Opmerkingen { get; set; }
        public int? Cursistnr { get; set; }
        public string? Sofinummer { get; set; }
        public string? GebDatumOud { get; set; }
        public DateTime? GebDatum { get; set; }
        public string? GebPlaats { get; set; }
        public string? Voorletters { get; set; }
        public string? Roepnaam { get; set; }
        public string? Voorvoegsel { get; set; }
        public string? Tussenvoegsel { get; set; }
        public string? Achternaam { get; set; }
        public bool? Cursist { get; set; }
        public int? Bedrijfsadresid { get; set; }
        public int? Bedrijskoppeling { get; set; }
        public bool? Leverancier { get; set; }
        public string? Debiteurnummer { get; set; }
        public bool? Esteco { get; set; }
        public string? FldWebSite { get; set; }
        public bool? Attentie { get; set; }
        public string? LoginNaam { get; set; }
        public string? LoginPassword { get; set; }
        public DateTime? DatumCursusdoc { get; set; }
        public int? CursistId { get; set; }
        public DateTime? DatumJaarMon1 { get; set; }
        public DateTime? DatumJaarMon2 { get; set; }
        public DateTime? DatumJaarMon3 { get; set; }
        public string? TekstJaarMon1 { get; set; }
        public string? TekstJaarMon2 { get; set; }
        public string? TekstJaarMon3 { get; set; }
        public string? Partner { get; set; }
        public string? NrSgg { get; set; }
        public string? Deelnemer { get; set; }
        public string? KiwaNummer { get; set; }
        public string? KomOhouder { get; set; }
        public int? OldId { get; set; }
        public int? FirstContactId { get; set; }
        public string? EmailFactuur { get; set; }
        public string? EmailAanmaning { get; set; }
        public int? KiwaContactId { get; set; }
        public string? MeldSoort { get; set; }
        public byte[]? SsmaTimeStamp { get; set; } // Timestamp voor concurrency
    }
}