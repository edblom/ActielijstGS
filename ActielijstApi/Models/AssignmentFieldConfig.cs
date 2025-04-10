using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActielijstApi.Models
{
    [Table("AssignmentFieldConfig")]
    public class AssignmentFieldConfig
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("ListDefinitionId")]
        public int ListDefinitionId { get; set; } // Foreign key naar AssignmentListDefinition

        [Column("FieldName")]
        public string FieldName { get; set; }    // Veldnaam uit ProjectAssignment, bijv. "FldOmschrijving"

        [Column("DisplayOrder")]
        public int DisplayOrder { get; set; }    // Volgorde van de kolom

        [Column("DataType")]
        public string DataType { get; set; }     // Datatype, bijv. "string", "int", "datetime"

        [Column("Prompt")]
        public string Prompt { get; set; }       // Label voor de kolom

        [Column("IsVisible")]
        public bool IsVisible { get; set; }      // Toon/verberg veld

        [Column("IsSortable")]
        public bool IsSortable { get; set; }     // Kan gesorteerd worden

        [Column("IsFilterable")]
        public bool IsFilterable { get; set; }   // Kan gefilterd worden

        [Column("FormatString")]
        public string? FormatString { get; set; } // Formaat, bijv. "d" voor datum

        [Column("Width")]
        public int? Width { get; set; }          // Kolombreedte in pixels

        [Column("BackgroundColorRule")]
        public string? BackgroundColorRule { get; set; } // Regel voor achtergrondkleur

        [Column("UserRole")]
        public string? UserRole { get; set; }    // Rol die dit veld mag zien (optioneel)

        // Navigatieproperty
        [ForeignKey("ListDefinitionId")]
        public AssignmentListDefinition? ListDefinition { get; set; }
    }
}