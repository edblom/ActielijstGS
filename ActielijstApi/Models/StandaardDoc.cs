using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActielijstApi.Models
{
    [Table("tblStandaardDoc", Schema = "dbo")]
    public class StandaardDoc
    {
        [Key]
        [Column("doc_id")]
        public int DocId { get; set; }

        [Column("fldNaamDoc")]
        public string? NaamDoc { get; set; }

        [Column("fldPathDoc")]
        public string? PathDoc { get; set; }

        [Column("fldDocOmschrijving")]
        public string? DocOmschrijving { get; set; }

        [Column("fldDocNum")]
        public int? DocNum { get; set; }

        [Column("fldDocSavePath")]
        public string? DocSavePath { get; set; }

        [Column("fldProjectMap")]
        public bool ProjectMap { get; set; }

        [Column("fldDocPrefix")]
        public string? DocPrefix { get; set; }

        [Column("fldSoort")]
        public int? Soort { get; set; }

        [Column("fldPrijsId")]
        public int? PrijsId { get; set; }

        [Column("fldEmailSjabloon")]
        public string? EmailSjabloon { get; set; }

        [Column("fldEmailAan")]
        public string? EmailAan { get; set; }

        [Column("fldEmailSubject")]
        public string? EmailSubject { get; set; }
    }
}