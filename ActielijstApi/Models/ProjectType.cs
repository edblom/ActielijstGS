using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActielijstApi.Models
{
    [Table("tblSoortProject", Schema = "dbo")]
    public class ProjectType
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Omschrijving")]
        public string? Omschrijving { get; set; }

        [Column("Soort")]
        public string? Soort { get; set; }

        [Column("categorie")]
        public string? CategorieName { get; set; }

        [Column("tabel")]
        public string? Tabel { get; set; }

        [Column("tabelSoort")]
        public string? TabelSoort { get; set; }

        [Column("facturering")]
        public int? Facturering { get; set; }

        [Column("OpEenRegel")]
        public bool? OpEenRegel { get; set; }

        [Column("CategorieId")]
        public int CategorieId { get; set; }

        [Column("SSMA_TimeStamp")]
        [Timestamp]
        public byte[]? SSMA_TimeStamp { get; set; }

        // Navigatie-eigenschap
        [ForeignKey("CategorieId")]
        public AssignmentCategory? RelatedCategory { get; set; }
    }
}