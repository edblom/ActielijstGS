using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActielijstApi.Models
{
    [Table("tblprojecten", Schema = "dbo")]
    public class Project
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("fldProjectNummer")]
        public int? FldProjectNummer { get; set; }

        [Column("fldExternNummer")]
        public string? FldExternNummer { get; set; }

        [Column("fldExternNummer2")]
        public string? FldExternNummer2 { get; set; }

        [Column("fldProjectNaam")]
        public string? FldProjectNaam { get; set; }

        [Column("fldAfdeling")]
        public string? FldAfdeling { get; set; }

        [Column("fldJaar")]
        public string? FldJaar { get; set; }

        [Column("fldDatum")]
        public DateTime? FldDatum { get; set; }

        [Column("fldAdres")]
        public string? FldAdres { get; set; }

        [Column("fldPC")]
        public string? FldPC { get; set; }

        [Column("fldPlaats")]
        public string? FldPlaats { get; set; }

        [Column("fldSoort")]
        public int? FldSoort { get; set; }

        [Column("fldActie")]
        public string? FldActie { get; set; }

        [Column("fldIntracNr")]
        public string? FldIntracNr { get; set; }

        [Column("fldSGG")]
        public string? FldSGG { get; set; }

        [Column("fldEPA")]
        public string? FldEPA { get; set; }

        [Column("fldOpdrachtgeverId")]
        public string? FldOpdrachtgeverId { get; set; }

        [Column("fldOpdrachtgever")]
        public string? FldOpdrachtgever { get; set; }

        [Column("fldStatus")]
        public int? FldStatus { get; set; }

        [Column("fldFolder")]
        public string? FldFolder { get; set; }

        [Column("fldArchiefMap")]
        public bool? FldArchiefMap { get; set; }

        [Column("fldVerwerkendBedrijf")]
        public int? FldVerwerkendBedrijf { get; set; }

        [Column("fldFabrikant")]
        public string? FldFabrikant { get; set; }

        [Column("fldSysteem")]
        public string? FldSysteem { get; set; }

        [Column("fldAantalM2")]
        public int? FldAantalM2 { get; set; }

        [Column("fldKiWa")]
        public string? FldKiWa { get; set; }

        [Column("fldKiwaCert")]
        public bool? FldKiwaCert { get; set; }

        [Column("SSMA_TimeStamp")]
        [Timestamp]
        public byte[]? SSMA_TimeStamp { get; set; }

        [Column("fldAfwerking")]
        public string? FldAfwerking { get; set; }

        [Column("fldPrevProjectId")]
        public int? FldPrevProjectId { get; set; }

        // Navigatie-eigenschap
        [ForeignKey("FldVerwerkendBedrijf")]
        public Adres? VerwerkendBedrijf { get; set; }
    }
}