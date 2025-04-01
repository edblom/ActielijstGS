using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("tblMemo", Schema = "dbo")]
    public partial class TblMemo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("fldMid")]
        public int FldMid { get; set; }

        [Column("fldMDatum")]
        public DateTime? FldMdatum { get; set; }

        public int? WerknId { get; set; }

        [Column("fldMKlantId")]
        public int? FldMklantId { get; set; }

        [Column("fldMContactPers")]
        public string FldMcontactPers { get; set; }

        [Column("fldMOfferteId")]
        public int? FldMofferteId { get; set; }

        [Column("fldMProjectId")]
        public int? FldMprojectId { get; set; }

        [Column("fldOpdrachtId")]
        public int? FldOpdrachtId { get; set; }

        [Column("fldOmschrijving")]
        public string FldOmschrijving { get; set; }

        [Column("fldMAfspraak")]
        public string FldMafspraak { get; set; }

        [Column("fldMActieDatum")]
        public DateTime? FldMactieDatum { get; set; }

        [Column("fldMActieVoor")]
        public int? FldMactieVoor { get; set; }

        [Column("fldMActieVoor2")]
        public int? FldMactieVoor2 { get; set; }

        [Column("fldMActieGereed")]
        public DateTime? FldMactieGereed { get; set; }

        [Column("fldMActieSoort")]
        public string FldMactieSoort { get; set; }

        [Column("fldMPrioriteit")]
        public int? FldMprioriteit { get; set; }

        [Timestamp]
        [Column("SSMA_TimeStamp")]
        public byte[] SsmaTimeStamp { get; set; }
    }
}