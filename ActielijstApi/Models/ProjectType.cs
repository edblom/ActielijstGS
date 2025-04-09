using ActielijstApi.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class ProjectType
{
    public int Id { get; set; }
    public string? Omschrijving { get; set; }
    public string? Soort { get; set; }
    [Column("categorie")]
    public string? CategorieName { get; set; }
    public string? Tabel { get; set; }
    public string? TabelSoort { get; set; }
    public int? Facturering { get; set; }
    public bool? OpEenRegel { get; set; }
    public int CategorieId { get; set; }
    public byte[]? SSMA_TimeStamp { get; set; }

    // Hernoemd naar een unieke naam
    public AssignmentCategory? RelatedCategory { get; set; }
}