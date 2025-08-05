namespace KlantBaseShare.Dtos
{
    public class ContactpersoonDTO
    {
        public int Id { get; set; }
        public string Voornaam { get; set; }
        public string Initialen { get; set; }
        public string Tussenvoegsel { get; set; }
        public string Achternaam { get; set; }
        public string VolledigeNaam => $"{Voornaam} {Initialen} {Tussenvoegsel} {Achternaam}".Trim();
    }
}

