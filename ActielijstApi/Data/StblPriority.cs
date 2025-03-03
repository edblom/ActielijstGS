namespace ActielijstApi.Models
{
    public class StblPriority
    {
        public int Id { get; set; }
        public int Prioriteit { get; set; }
        public string Omschrijving { get; set; }
        public string Kleur { get; set; } // Bijv. "#FF0000" voor rood
    }
}