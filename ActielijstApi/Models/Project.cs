using System;

namespace ActielijstApi.Models
{
    public class Project
    {
        public int Id { get; set; } // id
        public int? FldProjectNummer { get; set; } // fldProjectNummer (int, nullable vanwege DEFAULT 0)
        public string? FldExternNummer { get; set; } // fldExternNummer (nvarchar(50))
        public string? FldExternNummer2 { get; set; } // fldExternNummer2 (nvarchar(50))
        public string? FldProjectNaam { get; set; } // fldProjectNaam (nvarchar(50))
        public string? FldAfdeling { get; set; } // fldAfdeling (nchar(10))
        public string? FldJaar { get; set; } // fldjaar (nvarchar(50))
        public DateTime? FldDatum { get; set; } // fldDatum (datetime)
        public string? FldAdres { get; set; } // fldAdres (nvarchar(50))
        public string? FldPC { get; set; } // fldPC (nvarchar(50))
        public string? FldPlaats { get; set; } // fldPlaats (nvarchar(50))
        public string? FldSoort { get; set; } // fldSoort (nvarchar(50))
        public string? FldActie { get; set; } // fldActie (nvarchar(50))
        public string? FldIntracNr { get; set; } // fldIntracNr (nvarchar(50))
        public string? FldSGG { get; set; } // fldSGG (nvarchar(50))
        public string? FldEPA { get; set; } // fldEPA (nvarchar(50))
        public string? FldOpdrachtgeverId { get; set; } // fldOpdrachtgeverId (nvarchar(50), DEFAULT 0)
        public string? FldOpdrachtgever { get; set; } // fldOpdrachtgever (nvarchar(50))
        public string? FldStatus { get; set; } // fldStatus (nvarchar(50))
        public string? FldFolder { get; set; } // fldFolder (nvarchar(max))
        public bool FldArchiefMap { get; set; } // fldArchiefMap (bit, DEFAULT 0)
        public int? FldVerwerkendBedrijf { get; set; } // fldVerwerkendBedrijf (int)
        public string? FldFabrikant { get; set; } // fldFabrikant (nvarchar(50))
        public string? FldSysteem { get; set; } // fldSysteem (nvarchar(50))
        public int? FldAantalM2 { get; set; } // fldAantalM2 (int)
        public string? FldKiWa { get; set; } // fldKiWa (nvarchar(50))
        public bool FldKiwaCert { get; set; } // fldKiwaCert (bit, DEFAULT 0)
        public byte[]? SSMA_TimeStamp { get; set; } // SSMA_TimeStamp (timestamp)
        public string? FldAfwerking { get; set; } // fldAfwerking (nvarchar(50))
        public int? FldPrevProjectId { get; set; } // fldPrevProjectId (int)
    }
}