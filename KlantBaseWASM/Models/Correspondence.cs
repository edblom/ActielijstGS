namespace KlantBaseWASM.Models
{
    // Models/Correspondence.cs
    public class GenerateCorrespondenceRequest
    {
        public int Soort { get; set; }
        public int KlantId { get; set; }
        public int? ProjectId { get; set; }
        public int? OpdrachtId { get; set; }
        public int? ContactpersoonId { get; set; }
        public string? Omschrijving { get; set; }
        public bool OpenDocumentInWord { get; set; }
        public bool SendEmail { get; set; }
    }

    public class GenerateCorrespondenceResponse
    {
        public int CorrespondentieId { get; set; }
        public string? FilePath { get; set; }
        public bool EmailSent { get; set; }
    }

    public class SendEmailRequest
    {
        public int CorrespondentieId { get; set; }
        public string? EmailAan { get; set; }
    }

    public class SendEmailResponse
    {
        public int CorrespondentieId { get; set; }
        public bool EmailSent { get; set; }
    }
}
