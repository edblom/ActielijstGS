using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("tblFactRegel", Schema = "dbo")]
    public partial class TblFactRegel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("fldFDId")]
        public int FldFdid { get; set; }

        [Column("fldFDProjOndId")]
        public int? FldFdprojOndId { get; set; }

        public TblProjectOnderdelen TblProjectOnderdelen { get; set; }

        [Column("fldFDFactId")]
        public int? FldFdfactId { get; set; }

        [Column("fldFDPrijsId")]
        public int? FldFdprijsId { get; set; }

        [Column("fldFDDatum")]
        public DateTime? FldFddatum { get; set; }

        [Column("fldFDPercentage")]
        public double? FldFdpercentage { get; set; }

        [Column("fldFDAantal")]
        public double? FldFdaantal { get; set; }

        [Column("fldFDOmschrijving")]
        public string FldFdomschrijving { get; set; }

        [Column("lfdFDEenheid")]
        public string LfdFdeenheid { get; set; }

        [Column("fldFDPrijsStukEur")]
        public decimal? FldFdprijsStukEur { get; set; }

        [Column("fldFDTotPrijsEur")]
        public decimal? FldFdtotPrijsEur { get; set; }

        [Column("fldFDBTWPerc")]
        public double? FldFdbtwperc { get; set; }

        [Column("fldFDOpdrachtId")]
        public int? FldFdopdrachtId { get; set; }

        [Column("fldFDAfgerond")]
        public bool? FldFdafgerond { get; set; }

        [Column("fldFDFacturerenAanId")]
        public int? FldFdfacturerenAanId { get; set; }

        [Column("fldFDRefnrKlant")]
        public string FldFdrefnrKlant { get; set; }

        [Timestamp]
        [Column("SSMA_TimeStamp")]
        public byte[] SsmaTimeStamp { get; set; }

        [Column("test")]
        public string Test { get; set; }

        [Column("fldFDJaar")]
        public int? FldFdjaar { get; set; }

        [Column("fldFDKorting")]
        public bool? FldFdkorting { get; set; }

        [Column("fldFDOnderhoudscontractId")]
        public int? FldFdonderhoudscontractId { get; set; }
    }
}