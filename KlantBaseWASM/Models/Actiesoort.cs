// Models/Actiesoort.cs
namespace KlantBaseWASM.Models;

public class ActieSoort
{
    public int? Id { get; set; } // Match varchar in tblMemo.fldMActieSoort
    public string Omschrijving { get; set; }
}