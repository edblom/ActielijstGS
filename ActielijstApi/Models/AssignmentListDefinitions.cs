using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActielijstApi.Models
{
    [Table("AssignmentListDefinition")]
    public class AssignmentListDefinition
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("ListName")]
        public string ListName { get; set; }

        [Column("CategorieId")]
        public int? CategorieId { get; set; }

        [Column("FldAfdeling")]
        public string? FldAfdeling { get; set; }

        [Column("Description")]
        public string? Description { get; set; }

        [Column("SortOrder")]
        public int SortOrder { get; set; }
    }
}