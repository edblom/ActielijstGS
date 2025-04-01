using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("tblProjecten", Schema = "dbo")]
    public partial class TblProjecten
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("fldProjectNummer")]
        public int? FldProjectNummer { get; set; }

        [Column("fldExternNummer")]
        public string FldExternNummer { get; set; }

        [Column("fldExternNummer2")]
        public string FldExternNummer2 { get; set; }

        [Column("fldProjectNaam")]
        public string FldProjectNaam { get; set; }

        [Column("fldAfdeling")]
        public string FldAfdeling { get; set; }

        [Column("fldjaar")]
        public string Fldjaar { get; set; }

        [Column("fldDatum")]
        public DateTime? FldDatum { get; set; }

        [Column("fldAdres")]
        public string FldAdres { get; set; }

        [Column("fldPC")]
        public string FldPc { get; set; }

        [Column("fldPlaats")]
        public string FldPlaats { get; set; }

        [Column("fldSoort")]
        public string FldSoort { get; set; }

        [Column("fldActie")]
        public string FldActie { get; set; }

        [Column("fldIntracNr")]
        public string FldIntracNr { get; set; }

        [Column("fldSGG")]
        public string FldSgg { get; set; }

        [Column("fldEPA")]
        public string FldEpa { get; set; }

        [Column("fldOpdrachtgeverId")]
        public string FldOpdrachtgeverId { get; set; }

        [Column("fldOpdrachtgever")]
        public string FldOpdrachtgever { get; set; }

        [Column("fldStatus")]
        public string FldStatus { get; set; }

        [Column("fldFolder")]
        public string FldFolder { get; set; }

        [Column("fldArchiefMap")]
        public bool? FldArchiefMap { get; set; }

        [Column("fldVerwerkendBedrijf")]
        public int? FldVerwerkendBedrijf { get; set; }

        [Column("fldFabrikant")]
        public string FldFabrikant { get; set; }

        [Column("fldSysteem")]
        public string FldSysteem { get; set; }

        [Column("fldAantalM2")]
        public int? FldAantalM2 { get; set; }

        [Column("fldKiWa")]
        public string FldKiWa { get; set; }

        [Column("fldKiwaCert")]
        public bool? FldKiwaCert { get; set; }

        [Timestamp]
        [Column("SSMA_TimeStamp")]
        public byte[] SsmaTimeStamp { get; set; }

        [Column("fldAfwerking")]
        public string FldAfwerking { get; set; }

        [Column("fldPrevProjectId")]
        public int? FldPrevProjectId { get; set; }

        public ICollection<TblProjectOnderdelen> TblProjectOnderdelens { get; set; }
    }
}