namespace ActielijstApi.Dtos
{
    public class CorrespondenceResponse
    {
        public int CorrespondentieId { get; set; }
        public string FilePath { get; set; } // Pad naar het gegenereerde document
        public bool EmailSent { get; set; } // Indien e-mail is verzonden
    }
}