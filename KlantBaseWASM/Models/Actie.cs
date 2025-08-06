namespace KlantBaseWASM.Models
{
    public class Actie
    {
        public int FldMid { get; set; }
        public DateTime? FldMDatum { get; set; }
        public int? WerknId { get; set; }
        public int? FldMKlantId { get; set; }
        public string? FldMContactPers { get; set; }
        public int? FldMOfferteId { get; set; }
        public int? FldMProjectId { get; set; }
        public int? FldOpdrachtId { get; set; }
        public string? FldOmschrijving { get; set; }
        public string? FldMAfspraak { get; set; }
        public DateTime? FldMActieDatum { get; set; }
        public int? FldMActieVoor { get; set; }
        public int? FldMActieVoor2 { get; set; }
        public DateTime? FldMActieGereed { get; set; }
        public string? FldMActieSoort { get; set; } // String, matches database
        public int? FldMPrioriteit { get; set; }
        public byte[]? SsmA_TimeStamp { get; set; }
        public string? FldMActieVoorInitialen { get; set; }
        public string? FldMActieVoor2Initialen { get; set; }
    }
}