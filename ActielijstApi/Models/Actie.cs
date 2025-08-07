using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActielijstApi.Models
{
    [Table("tblMemo", Schema = "dbo")]
    public class Actie
    {
        [Key]
        [Column("fldMid")]
        public int FldMid { get; set; } // IDENTITY, altijd gevuld

        [Column("fldMDatum")]
        public DateTime? FldMDatum { get; set; } // NULL toegestaan

        [Column("WerknId")]
        public int? WerknId { get; set; } // NULL toegestaan

        [Column("fldMKlantId")]
        public int? FldMKlantId { get; set; } // NULL toegestaan

        [Column("fldMContactPers")]
        public string? FldMContactPers { get; set; } // NULL toegestaan

        [Column("fldMOfferteId")]
        public int? FldMOfferteId { get; set; } // NULL toegestaan

        [Column("fldMProjectId")]
        public int? FldMProjectId { get; set; } // NULL toegestaan

        [Column("fldOpdrachtId")]
        public int? FldOpdrachtId { get; set; } // NULL toegestaan

        [Column("fldOmschrijving")]
        public string? FldOmschrijving { get; set; } // NULL toegestaan

        [Column("fldMAfspraak")]
        public string? FldMAfspraak { get; set; } // NULL toegestaan

        [Column("fldMActieDatum")]
        public DateTime? FldMActieDatum { get; set; } // NULL toegestaan

        [Column("fldMActieVoor")]
        public int? FldMActieVoor { get; set; } // NULL toegestaan

        [Column("fldMActieVoor2")]
        public int? FldMActieVoor2 { get; set; } // NULL toegestaan

        [Column("fldMActieGereed")]
        public DateTime? FldMActieGereed { get; set; } // NULL toegestaan

        [Column("fldMActieSoort")]
        public string? FldMActieSoort { get; set; } // NULL toegestaan

        [Column("fldMPrioriteit")]
        public int? FldMPrioriteit { get; set; } // NULL toegestaan

        //[Column("SSMA_TimeStamp")]
        //[Timestamp]
        //public byte[]? SSMA_TimeStamp { get; set; } // NOT NULL, timestamp
    }
}