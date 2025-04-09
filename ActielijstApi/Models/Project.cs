using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ActielijstApi.Models
{

    [Table("tblprojecten")]
    public class Project
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("fldProjectNummer")]
        public int? FldProjectNummer { get; set; } // string
        public string? FldExternNummer { get; set; }
        public string? FldExternNummer2 { get; set; }
        public string? FldProjectNaam { get; set; }
        public string? FldAfdeling { get; set; }
        public string? FldJaar { get; set; }
        public DateTime? FldDatum { get; set; }
        public string? FldAdres { get; set; }
        public string? FldPC { get; set; }
        public string? FldPlaats { get; set; }
        public int? FldSoort { get; set; }
        public string? FldActie { get; set; }
        public string? FldIntracNr { get; set; }
        public string? FldSGG { get; set; }
        public string? FldEPA { get; set; }
        public string? FldOpdrachtgeverId { get; set; }
        public string? FldOpdrachtgever { get; set; }
        public int? FldStatus { get; set; }
        public string? FldFolder { get; set; }
        public bool? FldArchiefMap { get; set; }
        public int? FldVerwerkendBedrijf { get; set; }
        public string? FldFabrikant { get; set; }
        public string? FldSysteem { get; set; }
        public int? FldAantalM2 { get; set; } // string
        public string? FldKiWa { get; set; }
        public bool? FldKiwaCert { get; set; }
        public byte[]? SSMA_TimeStamp { get; set; }
        public string? FldAfwerking { get; set; }
        public int? FldPrevProjectId { get; set; }

        // Navigatie-eigenschap
        public Adres? VerwerkendBedrijf { get; set; }
    }
}