using System;

namespace ActielijstApi.Models
{
    public class Inspectie
    {
        public int? CategorieId { get; set; }
        public string? Categorie { get; set; }
        public int? fldProjectId { get; set; }
        public string? fldProjectNaam { get; set; }
        public bool fldCertKeuring { get; set; }
        public decimal? fldMaandBedrag { get; set; }
        public int? fldSoort { get; set; }
        public DateTime? fldPlanDatum { get; set; }
        public int? PlanJaar { get; set; }
        public string? Omschrijving { get; set; }
        public int? fldProjectNummer { get; set; }
        public bool fldKiwaCert { get; set; }
        public int? fldVerwerkendBedrijf { get; set; }
        public string? fldExternNummer { get; set; }
        public string? fldPlaats { get; set; }
        public int? fldKiwaKeuringsNr { get; set; } // Eerste instantie
        public DateTime? PlanDatum { get; set; }
        public int? fldStatus { get; set; }
        public string? status { get; set; }
        public string? BelNotitie { get; set; }
        public string? fldProjectLeider { get; set; }
        public string? ExtraMedewerker { get; set; }
        public string? fldOpdrachtStr { get; set; }
        public DateTime? fldGereedVoor { get; set; }
        public DateTime? fldDatumGereed { get; set; }
        //public string Locatie { get; set; }
        public string? Applicateur { get; set; }
        public int? fldOpdrachtId { get; set; }
        public int? OpdrachtId { get; set; }
        public string? KiwaNummer { get; set; }
        public int? Toegewezen { get; set; }
        public string? fldjaar { get; set; }
        public int? fldAantalM2 { get; set; }
        //public string OpdrachtLocatie { get; set; }
        public string? OpdrachtAdres { get; set; }
        public string? OpdrachtHuisnr { get; set; }
        public string? OpdrachtPC { get; set; }
        public string? OpdrachtPlaats { get; set; }
        public string? fldSGG { get; set; }
        public decimal? fldBedrag { get; set; }

        // Nieuwe velden uit de view
        public int? fldOpdrachtgeverId { get; set; }
        public int? fldContactpersoonId { get; set; }
    }
}
