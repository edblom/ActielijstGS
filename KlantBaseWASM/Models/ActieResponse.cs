using System.Collections.Generic;
namespace KlantBaseWASM.Models
{
    public class ActieResponse
    {
        public List<Actie> Items { get; set; } = new List<Actie>();
        public int TotalCount { get; set; }
    }
}