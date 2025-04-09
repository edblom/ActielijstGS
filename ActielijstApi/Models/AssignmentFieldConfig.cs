public class AssignmentFieldConfig
{
    public int Id { get; set; }
    public int ListDefinitionId { get; set; } // Foreign key naar AssignmentListDefinition
    public string FieldName { get; set; }    // Veldnaam uit ProjectAssignment, bijv. "FldOmschrijving"
    public int DisplayOrder { get; set; }    // Volgorde van de kolom
    public string DataType { get; set; }     // Datatype, bijv. "string", "int", "datetime"
    public string Prompt { get; set; }       // Label voor de kolom
    public bool IsVisible { get; set; }      // Toon/verberg veld
    public bool IsSortable { get; set; }     // Kan gesorteerd worden
    public bool IsFilterable { get; set; }   // Kan gefilterd worden
    public string? FormatString { get; set; } // Formaat, bijv. "d" voor datum
    public int? Width { get; set; }          // Kolombreedte in pixels
    public string? BackgroundColorRule { get; set; } // Regel voor achtergrondkleur
    public string? UserRole { get; set; }    // Rol die dit veld mag zien (optioneel)

    // Navigatieproperty (optioneel, maar handig)
    public AssignmentListDefinition ListDefinition { get; set; }
}
