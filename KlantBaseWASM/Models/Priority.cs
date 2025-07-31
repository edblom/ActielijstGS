namespace KlantBaseWASM.Models
{
    public class Priority
    {
        public int? Id { get; set; } // Nullable voor "Alles"-optie
        public int? Prioriteit { get; set; }
        public string Omschrijving { get; set; }
        public string Kleur { get; set; } // Voeg Kleur toe
    }
}