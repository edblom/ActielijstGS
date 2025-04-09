public class AssignmentListDefinition
{
    public int Id { get; set; }
    public required string ListName { get; set; }
    public int? CategorieId { get; set; }
    public string? FldAfdeling { get; set; }
    public string? Description { get; set; }
    public int SortOrder { get; set; }
}