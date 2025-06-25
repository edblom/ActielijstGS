// Models/Actiesoort.cs
namespace KlantBaseWASM.Models;

public class Actiesoort
{
    public string Id { get; set; } // Match varchar in tblMemo.fldMActieSoort
    public string Omschrijving { get; set; }
}