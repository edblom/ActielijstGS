using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("tblFaktuur", Schema = "dbo")]
    public partial class TblFaktuur
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("KLNTNR")]
        public int? Klntnr { get; set; }

        public int? FacturerenAan { get; set; }

        public int? FactContPers { get; set; }

        public string FactBedrijf { get; set; }

        public string FactNaam { get; set; }

        public string FactAdres { get; set; }

        [Column("FactPC")]
        public string FactPc { get; set; }

        public string FactPlaats { get; set; }

        public string FactDebNr { get; set; }

        [Column("faktnr")]
        public string Faktnr { get; set; }

        public double? Faktuur { get; set; }

        public decimal? FaktuurEur { get; set; }

        public int? Opdracht { get; set; }

        public int? Project { get; set; }

        public DateTime? Datum2 { get; set; }

        [Column("BTW")]
        public double? Btw { get; set; }

        [Column("BTWEur")]
        public decimal? Btweur { get; set; }

        public DateTime? Betaald { get; set; }

        [Column("termijn")]
        public string Termijn { get; set; }

        [Column("tarief")]
        public double? Tarief { get; set; }

        public double? Totaal { get; set; }

        public decimal? TotaalEur { get; set; }

        public string Werk { get; set; }

        [Column("omschr")]
        public string Omschr { get; set; }

        [Column("omschr2")]
        public string Omschr2 { get; set; }

        [Column("omschr22")]
        public string Omschr22 { get; set; }

        [Column("omschr222")]
        public string Omschr222 { get; set; }

        [Column("Datum_1eHerrin")]
        public DateTime? Datum1eHerrin { get; set; }

        [Column("Datum_2eHerrin")]
        public DateTime? Datum2eHerrin { get; set; }

        [Column("Datum_LAanman")]
        public DateTime? DatumLaanman { get; set; }

        public string Kortingomschrijving { get; set; }

        public decimal? Kortingbedrag { get; set; }

        public double? Kortingspercentage { get; set; }

        [Timestamp]
        [Column("SSMA_TimeStamp")]
        public byte[] SsmaTimeStamp { get; set; }
    }
}