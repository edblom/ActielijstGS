using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActielijstApi.Models
{
    [Table("stblOpdrachtCategorie", Schema = "dbo")]
    public class AssignmentCategory
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("Categorie")]
        public string? Categorie { get; set; }

        [Column("Volgorde")]
        public int? Volgorde { get; set; }
    }
}