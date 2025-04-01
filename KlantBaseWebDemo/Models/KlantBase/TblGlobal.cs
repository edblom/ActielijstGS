using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("tblGlobals", Schema = "dbo")]
    public partial class TblGlobal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("savepath")]
        public string Savepath { get; set; }

        [Column("designpath")]
        public string Designpath { get; set; }

        [Column("sjabloonpath")]
        public string Sjabloonpath { get; set; }

        [Column("version")]
        public string Version { get; set; }

        [Column("archiefpath")]
        public string Archiefpath { get; set; }

        [Column("pdfPath")]
        public string PdfPath { get; set; }

        public string ScanPath { get; set; }

        public string KiwaPath { get; set; }

        public string ProjectPath { get; set; }

        public string FotoPath { get; set; }

        public string FactuurText { get; set; }

        public string FactuurAccount { get; set; }

        public bool? FactuurHandtekening { get; set; }

        public bool? DisplayMailVoorVerzenden { get; set; }
    }
}