using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActielijstApi.Models
{
    [Table("tblGlobals", Schema = "dbo")]
    public class Globals
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("savepath")]
        public string? SavePath { get; set; }

        [Column("designpath")]
        public string? DesignPath { get; set; }

        [Column("sjabloonpath")]
        public string? SjabloonPath { get; set; }

        [Column("version")]
        public string? Version { get; set; }

        [Column("archiefpath")]
        public string? ArchiefPath { get; set; }

        [Column("pdfPath")]
        public string? PdfPath { get; set; }

        [Column("ScanPath")]
        public string? ScanPath { get; set; }

        [Column("KiwaPath")]
        public string? KiwaPath { get; set; }

        [Column("ProjectPath")]
        public string? ProjectPath { get; set; }

        [Column("FotoPath")]
        public string? FotoPath { get; set; }

        [Column("FactuurText")]
        public string? FactuurText { get; set; }

        [Column("FactuurAccount")]
        public string? FactuurAccount { get; set; }

        [Column("FactuurHandtekening")]
        public bool FactuurHandtekening { get; set; }

        [Column("DisplayMailVoorVerzenden")]
        public bool DisplayMailVoorVerzenden { get; set; }
    }
}