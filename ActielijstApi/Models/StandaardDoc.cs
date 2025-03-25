namespace ActielijstApi.Models
{
    public class StandaardDoc
    {
        public int DocId { get; set; } // doc_id
        public string? NaamDoc { get; set; } // fldNaamDoc
        public string? PathDoc { get; set; } // fldPathDoc
        public string? DocOmschrijving { get; set; } // fldDocOmschrijving
        public int? DocNum { get; set; } // fldDocNum
        public string? DocSavePath { get; set; } // fldDocSavePath
        public bool? ProjectMap { get; set; } // fldProjectMap
        public string? DocPrefix { get; set; } // fldDocPrefix
        public int? Soort { get; set; } // fldSoort
        public int? PrijsId { get; set; } // fldPrijsId
        public string? EmailSjabloon { get; set; } // fldEmailSjabloon
        public string? EmailAan { get; set; } // fldEmailAan
        public string? EmailSubject { get; set; } // fldEmailSubject
    }
}