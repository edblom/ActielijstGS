// ActielijstApi/Models/AankomendeInspectie.cs
using System;

namespace ActielijstApi.Data
{
    public class AankomendeInspectie
    {
        public string? Project { get; set; }
        public string? ProjectNr { get; set; }
        public string? Adres { get; set; }
        public string? Applicateur { get; set; }
        public string? fldSGG { get; set; }
        public string? Omschrijving { get; set; }
        public string? InspecteurId { get; set; }
        public decimal? fldBedrag { get; set; }
        public string? ExtraMedewerker { get; set; }
        public DateTime? DatumGereed { get; set; }
        public int PSID { get; set; }
        public int? StatusId { get; set; }
        public string? Status { get; set; }
        public string? Opdracht { get; set; }
        public int SoortId { get; set; }
        public string? Soort { get; set; }
        public DateTime? Toegekend { get; set; }
        public bool Toegewezen { get; set; }
        public DateTime? AppointmentDateTime { get; set; }

        // Nieuwe velden uit de view
        public int? fldOpdrachtgeverId { get; set; }
        public int? fldContactpersoonId { get; set; }
    }
}