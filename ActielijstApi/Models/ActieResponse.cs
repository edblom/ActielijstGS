using System.Collections.Generic;

namespace ActielijstApi.Models
{
    public class ActieResponse
    {
        public List<Actie> Items { get; set; }
        public int TotalCount { get; set; }
    }
}