namespace ActielijstApi.Dtos
{
    public class GenerateCorrespondenceResponse
    {
        public int CorrespondentieId { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public bool EmailSent { get; set; }
    }
}