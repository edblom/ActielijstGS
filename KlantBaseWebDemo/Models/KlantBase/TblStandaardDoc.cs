using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("tblStandaardDoc", Schema = "dbo")]
    public partial class TblStandaardDoc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("doc_id")]
        public int DocId { get; set; }

        [Column("fldNaamDoc")]
        public string FldNaamDoc { get; set; }

        [Column("fldPathDoc")]
        public string FldPathDoc { get; set; }

        [Column("fldDocOmschrijving")]
        public string FldDocOmschrijving { get; set; }

        [Column("fldDocNum")]
        public int? FldDocNum { get; set; }

        [Column("fldDocSavePath")]
        public string FldDocSavePath { get; set; }

        [Column("fldProjectMap")]
        public string FldProjectMap { get; set; }

        [Column("fldDocPrefix")]
        public string FldDocPrefix { get; set; }

        [Column("fldSoort")]
        public int? FldSoort { get; set; }

        [Column("fldPrijsId")]
        public int? FldPrijsId { get; set; }

        [Column("fldEmailSjabloon")]
        public string FldEmailSjabloon { get; set; }

        [Column("fldEmailAan")]
        public string FldEmailAan { get; set; }

        [Column("fldEmailSubject")]
        public string FldEmailSubject { get; set; }
    }
}