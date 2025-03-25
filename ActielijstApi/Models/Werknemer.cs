namespace ActielijstApi.Models
{
    public class Werknemer
    {
        public int WerknId { get; set; }
        public string? Voornaam { get; set; }
        public string? FldLoginNaam { get; set; }
        public string? Initialen { get; set; } // Toegevoegd voor fldProjectLeider
    }
}