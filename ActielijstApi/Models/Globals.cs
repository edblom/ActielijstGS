namespace ActielijstApi.Models
{
    public class Globals
    {
        public int Id { get; set; }
        public string? SavePath { get; set; } // savepath
        public string? DesignPath { get; set; } // designpath
        public string? SjabloonPath { get; set; } // sjabloonpath
        public string? Version { get; set; } // version
        public string? ArchiefPath { get; set; } // archiefpath
        public string? PdfPath { get; set; } // pdfPath
        public string? ScanPath { get; set; } // ScanPath
        public string? KiwaPath { get; set; } // KiwaPath
        public string? ProjectPath { get; set; } // ProjectPath
        public string? FotoPath { get; set; } // FotoPath
        public string? FactuurText { get; set; } // FactuurText
        public string? FactuurAccount { get; set; } // FactuurAccount
        public bool FactuurHandtekening { get; set; } // FactuurHandtekening
        public bool DisplayMailVoorVerzenden { get; set; } // DisplayMailVoorVerzenden
    }
}
