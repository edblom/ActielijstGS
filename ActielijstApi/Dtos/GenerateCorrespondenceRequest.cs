namespace ActielijstApi.Dtos
{
    public class GenerateCorrespondenceRequest
    {
        public int Soort { get; set; } // Soort project/opdracht om het juiste sjabloon te kiezen
        public int KlantId { get; set; }
        public int? ProjectId { get; set; }
        public int? OpdrachtId { get; set; }
        public int? ContactpersoonId { get; set; }
        public string? Omschrijving { get; set; }
        public bool OpenDocumentInWord { get; set; } // Optie om document direct te openen
        public bool SendEmail { get; set; } // Optie om e-mail te verzenden
    }
}