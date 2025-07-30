// Models/Actie.cs
namespace KlantBaseWASM.Models;

public class Actie
{
    public int FldMid { get; set; }
    public DateTime? FldMDatum { get; set; }
    public int? WerknId { get; set; }
    public int? FldMKlantId { get; set; }
    public string? FldMContactPers { get; set; }
    public int? FldMProjectId { get; set; }
    public int? FldOpdrachtId { get; set; }
    public string? FldOmschrijving { get; set; }
    public string? FldMAfspraak { get; set; }
    public DateTime? FldMActieDatum { get; set; }
    public int? FldMActieVoor { get; set; }
    public string? FldMActieVoorInitialen { get; set; } // Initialen voor de eerste werknemer
    public int? FldMActieVoor2 { get; set; }
    public string? FldMActieVoor2Initialen { get; set; } // Initialen voor de tweede werknemer
    public DateTime? FldMActieGereed { get; set; }
    public string? FldMActieSoort { get; set; }
    public int? FldMPrioriteit { get; set; }
}

