using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("stblGlobals", Schema = "dbo")]
    public partial class StblGlobal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("fldArchiefPath")]
        public string FldArchiefPath { get; set; }

        [Column("fldDocumentPath")]
        public string FldDocumentPath { get; set; }

        [Column("fldSjablonenPath")]
        public string FldSjablonenPath { get; set; }

        [Column("fldWerkenPath")]
        public string FldWerkenPath { get; set; }

        [Column("fldOffertePath")]
        public string FldOffertePath { get; set; }
    }
}