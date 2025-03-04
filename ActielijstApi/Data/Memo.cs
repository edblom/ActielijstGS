using System;

namespace ActielijstApi.Data
{
    public class Memo
    {
        public int FldMid { get; set; } // IDENTITY, altijd gevuld
        public DateTime? FldMDatum { get; set; } // NULL toegestaan
        public int? WerknId { get; set; } // NULL toegestaan
        public int? FldMKlantId { get; set; } // NULL toegestaan
        public string FldMContactPers { get; set; } // NULL toegestaan
        public int? FldMOfferteId { get; set; } // NULL toegestaan
        public int? FldMProjectId { get; set; } // NULL toegestaan
        public int? FldOpdrachtId { get; set; } // NULL toegestaan
        public string FldOmschrijving { get; set; } // NULL toegestaan
        public string FldMAfspraak { get; set; } // NULL toegestaan
        public DateTime? FldMActieDatum { get; set; } // NULL toegestaan
        public int? FldMActieVoor { get; set; } // NULL toegestaan
        public int? FldMActieVoor2 { get; set; } // NULL toegestaan
        public DateTime? fldMActieGereed { get; set; } // NULL toegestaan
        public string FldMActieSoort { get; set; } // NULL toegestaan
        public int? FldMPrioriteit { get; set; } // NULL toegestaan
        public byte[] SSMA_TimeStamp { get; set; } // NOT NULL, timestamp
    }
}