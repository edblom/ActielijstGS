namespace ActielijstApi.Dtos
{
    public class SendEmailRequest
    {
        public int CorrespondentieId { get; set; }
        public string EmailAan { get; set; } = string.Empty;
    }
}