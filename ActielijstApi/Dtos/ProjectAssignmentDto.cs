using System;

namespace ActielijstApi.Dtos
{
    public class ProjectAssignmentDto
    {
        public int Id { get; set; }
        public int FldProjectId { get; set; }
        public int? FldSoort { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? AssignmentTypeName { get; set; }
        public string? Department { get; set; }
        public string? FldOmschrijving { get; set; }
        public string? FldOpdrachtStr { get; set; }
        public int? FldStatus { get; set; } // int?, consistent met ProjectAssignment
        public string? StatusName { get; set; }
        public DateTime? FldPlanDatum { get; set; }
        public decimal? FldBedrag { get; set; }
        public decimal? FldKiwabedrag { get; set; }
        public decimal? FldMaandBedrag { get; set; }
        public string? FldProjectLeider { get; set; }
        public string? ExtraMedewerker { get; set; }
        public DateTime? FldDatumGereed { get; set; }
        public int? FldOpdrachtgeverId { get; set; }
        public int? FldContactpersoonId { get; set; }
        public double? FldAantalKms { get; set; }
        public decimal? FldKmvergoeding { get; set; }
        public string? Fabrikant { get; set; }
        public string? Systeem { get; set; }
        public int? AantalM2 { get; set; } // string, consistent met Project
        public string? Gnummer { get; set; }
        public DateTime? Datum1eInspectie1 { get; set; }
        public string? Contractnr { get; set; }
        public int? Looptijd { get; set; }
        public DateTime? EindDatumContract { get; set; }
        public int? Factuurmaand { get; set; }
        public bool? FldCertKeuring { get; set; }
        public int? FldKiwaKeuringsNr { get; set; }
        public decimal? Kortingbedrag { get; set; }
        public double? Kortingspercentage { get; set; }
        public DateTime? AppointmentDateTime { get; set; }
        public string? OpdrachtAdres { get; set; }
        public string? OpdrachtHuisnr { get; set; }
        public string? OpdrachtPC { get; set; }
        public string? OpdrachtPlaats { get; set; }
        public string? OpdrachtLocatie { get; set; }
        public decimal? ContractBedrag { get; set; }
        public double? ContractIndexering { get; set; }
        public DateTime? FldPlanPeriodeVan { get; set; }
        public DateTime? FldPlanPeriodeTot { get; set; }
        public string? SteekproefMaand { get; set; }
        public string? ProjectName { get; set; }
        public string? ProjectNumber { get; set; } // string, consistent met Project
        public string? ProjectLocation { get; set; }
        public string? CustomerName { get; set; }
        public string? Applicator { get; set; }
        public string? KiwaNumber { get; set; }
        public string? BelNotitie { get; set; }
    }
}