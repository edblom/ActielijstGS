namespace ActielijstApi.Data
{
    public class StblCorrespondentieField
    {
        public int Id { get; set; }
        public string? ReplaceString { get; set; } // Naam van de custom property in Word
        public string? Description { get; set; }
        public string? FieldName { get; set; }
        public string? Tabel { get; set; } // Bron-tabel
        public string? Veld { get; set; } // Veld in de tabel
        public string? Standaardwaarde { get; set; } // Formaat of default
        public string? VeldType { get; set; } // Type (bijv. "datum")
        public string? IdNaam { get; set; } // ID-veld in de bron-tabel
        public string? CorrespondentieId { get; set; } // Veld in correspondentie dat als ID dient
    }
}