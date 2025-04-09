using System;

namespace ActielijstApi.Models
{
    public class ProjectAssignment
    {
        public int Id { get; set; }
        public int FldOpdrachtId { get; set; }
        public int FldOpdrachtNr { get; set; }
        public string? FldOpdrachtStr { get; set; }
        public int FldProjectId { get; set; }
        public string? FldAfdeling { get; set; }
        public int? FldSoort { get; set; }
        public DateTime? FldPlanDatum { get; set; }
        public string? FldOmschrijving { get; set; }
        public int? FldPrijsId { get; set; }
        public int? FldVolgnr { get; set; }
        public decimal? FldBedrag { get; set; }
        public decimal? FldKiwabedrag { get; set; }
        public decimal? FldMaandBedrag { get; set; }
        public int? FldStatus { get; set; }
        public DateTime? FldGefactureerd { get; set; }
        public int? FldFactuurRegelId { get; set; }
        public string? FldProjectLeider { get; set; }
        public string? ExtraMedewerker { get; set; }
        public DateTime? FldDatumGereed { get; set; }
        public DateTime? FldGereedVoor { get; set; }
        public int? FldOpdrachtgeverId { get; set; }
        public int? FldContactpersoonId { get; set; }
        public double? FldAantalKms { get; set; }
        public decimal? FldKmvergoeding { get; set; }
        public int? FldFacturering { get; set; }
        public string? Fabrikant { get; set; }
        public string? Systeem { get; set; }
        public string? Gnummer { get; set; }
        public DateTime? Datum1eInspectie1 { get; set; }
        public int? VerwerkendBedrijf { get; set; }
        public byte[]? SSMA_TimeStamp { get; set; }
        public string? Contractnr { get; set; }
        public int? Looptijd { get; set; }
        public DateTime? EindDatumContract { get; set; }
        public int? Factuurmaand { get; set; }
        public int? BelStatus { get; set; }
        public string? BelNotitie { get; set; }
        public DateTime? BelDatum { get; set; }
        public string? BelStatusText { get; set; }
        public bool? FldCertKeuring { get; set; }
        public int? FldKiwaKeuringsNr { get; set; }
        public int? KortingId { get; set; }
        public string? Kortingomschrijving { get; set; }
        public decimal? Kortingbedrag { get; set; }
        public double? Kortingspercentage { get; set; }
        public DateTime? Toegekend { get; set; }
        public string? AppointmentEntryID { get; set; }
        public DateTime? AppointmentDateTime { get; set; }
        public string? OpdrachtAdres { get; set; }
        public string? OpdrachtHuisnr { get; set; }
        public string? OpdrachtPC { get; set; }
        public string? OpdrachtPlaats { get; set; }
        public decimal? ContractBedrag { get; set; }
        public double? ContractIndexering { get; set; }
        public DateTime? FldPlanPeriodeVan { get; set; }
        public DateTime? FldPlanPeriodeTot { get; set; }
        public string? FldFolder { get; set; }
        public string? SteekproefMaand { get; set; }

        // Navigatie-eigenschappen
        public Project? Project { get; set; }
        public Status? Status { get; set; }
        public Adres? Customer { get; set; }
        public ProjectType? ProjectType { get; set; }
    }
}