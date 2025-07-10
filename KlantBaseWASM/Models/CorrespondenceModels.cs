namespace KlantBaseWASM.Models
{
    public class CorrespondentieDto
    {
        public int Id { get; set; }
        public int? KlantID { get; set; }
        public int? fldCorProjNum { get; set; }
        public int? fldCorOpdrachtNum { get; set; }
        public DateTime? fldCorDatum { get; set; }
        public string? fldCorOmschrijving { get; set; }
        public string? fldCorBestand { get; set; }
        public string? fldCorKenmerk { get; set; }
    }

    public class StandaardDoc
    {
        public int DocId { get; set; }
        public string? NaamDoc { get; set; }
        public string? DocOmschrijving { get; set; }
    }

}