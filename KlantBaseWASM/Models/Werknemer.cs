//Models/Werknemer.cs
namespace KlantBaseWASM.Models;

public class Werknemer
{
    public int WerknId { get; set; }
    public string? Voornaam { get; set; }
    public string? FldLoginNaam { get; set; }
    public string? Initialen { get; set; }
    public DateTime? FldDatumUitDienst { get; set; }
}