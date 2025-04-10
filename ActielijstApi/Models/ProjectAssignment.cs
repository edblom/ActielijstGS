using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActielijstApi.Models
{
    [Table("tblProjectOnderdelen", Schema = "dbo")]
    public class ProjectAssignment
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("fldOpdrachtId")]
        public int FldOpdrachtId { get; set; }

        [Column("fldOpdrachtNr")]
        public int FldOpdrachtNr { get; set; }

        [Column("fldOpdrachtStr")]
        public string? FldOpdrachtStr { get; set; }

        [Column("fldProjectId")]
        public int FldProjectId { get; set; }

        [Column("fldAfdeling")]
        public string? FldAfdeling { get; set; }

        [Column("fldSoort")]
        public int? FldSoort { get; set; }

        [Column("fldPlanDatum")]
        public DateTime? FldPlanDatum { get; set; }

        [Column("fldOmschrijving")]
        public string? FldOmschrijving { get; set; }

        [Column("fldPrijsId")]
        public int? FldPrijsId { get; set; }

        [Column("fldVolgnr")]
        public int? FldVolgnr { get; set; }

        [Column("fldBedrag")]
        public decimal? FldBedrag { get; set; }

        [Column("fldKiwabedrag")]
        public decimal? FldKiwabedrag { get; set; }

        [Column("fldMaandBedrag")]
        public decimal? FldMaandBedrag { get; set; }

        [Column("fldStatus")]
        public int? FldStatus { get; set; }

        [Column("fldGefactureerd")]
        public DateTime? FldGefactureerd { get; set; }

        [Column("fldFactuurRegelId")]
        public int? FldFactuurRegelId { get; set; }

        [Column("fldProjectLeider")]
        public string? FldProjectLeider { get; set; }

        [Column("ExtraMedewerker")]
        public string? ExtraMedewerker { get; set; }

        [Column("fldDatumGereed")]
        public DateTime? FldDatumGereed { get; set; }

        [Column("fldGereedVoor")]
        public DateTime? FldGereedVoor { get; set; }

        [Column("fldOpdrachtgeverId")]
        public int? FldOpdrachtgeverId { get; set; }

        [Column("fldContactpersoonId")]
        public int? FldContactpersoonId { get; set; }

        [Column("fldAantalKms")]
        public double? FldAantalKms { get; set; }

        [Column("fldKmvergoeding")]
        public decimal? FldKmvergoeding { get; set; }

        [Column("fldFacturering")]
        public int? FldFacturering { get; set; }

        [Column("Fabrikant")]
        public string? Fabrikant { get; set; }

        [Column("Systeem")]
        public string? Systeem { get; set; }

        [Column("Gnummer")]
        public string? Gnummer { get; set; }

        [Column("Datum1eInspectie1")]
        public DateTime? Datum1eInspectie1 { get; set; }

        [Column("VerwerkendBedrijf")]
        public int? VerwerkendBedrijf { get; set; }

        [Column("SSMA_TimeStamp")]
        [Timestamp]
        public byte[]? SSMA_TimeStamp { get; set; }

        [Column("Contractnr")]
        public string? Contractnr { get; set; }

        [Column("Looptijd")]
        public int? Looptijd { get; set; }

        [Column("EindDatumContract")]
        public DateTime? EindDatumContract { get; set; }

        [Column("Factuurmaand")]
        public int? Factuurmaand { get; set; }

        [Column("BelStatus")]
        public int? BelStatus { get; set; }

        [Column("BelNotitie")]
        public string? BelNotitie { get; set; }

        [Column("BelDatum")]
        public DateTime? BelDatum { get; set; }

        [Column("BelStatusText")]
        public string? BelStatusText { get; set; }

        [Column("fldCertKeuring")]
        public bool? FldCertKeuring { get; set; }

        [Column("fldKiwaKeuringsNr")]
        public int? FldKiwaKeuringsNr { get; set; }

        [Column("KortingId")]
        public int? KortingId { get; set; }

        [Column("Kortingomschrijving")]
        public string? Kortingomschrijving { get; set; }

        [Column("Kortingbedrag")]
        public decimal? Kortingbedrag { get; set; }

        [Column("Kortingspercentage")]
        public double? Kortingspercentage { get; set; }

        [Column("Toegekend")]
        public DateTime? Toegekend { get; set; }

        [Column("AppointmentEntryID")]
        public string? AppointmentEntryID { get; set; }

        [Column("AppointmentDateTime")]
        public DateTime? AppointmentDateTime { get; set; }

        [Column("OpdrachtAdres")]
        public string? OpdrachtAdres { get; set; }

        [Column("OpdrachtHuisnr")]
        public string? OpdrachtHuisnr { get; set; }

        [Column("OpdrachtPC")]
        public string? OpdrachtPC { get; set; }

        [Column("OpdrachtPlaats")]
        public string? OpdrachtPlaats { get; set; }

        [Column("ContractBedrag")]
        public decimal? ContractBedrag { get; set; }

        [Column("ContractIndexering")]
        public double? ContractIndexering { get; set; }

        [Column("fldPlanPeriodeVan")]
        public DateTime? FldPlanPeriodeVan { get; set; }

        [Column("fldPlanPeriodeTot")]
        public DateTime? FldPlanPeriodeTot { get; set; }

        [Column("fldFolder")]
        public string? FldFolder { get; set; }

        [Column("SteekproefMaand")]
        public string? SteekproefMaand { get; set; }

        // Navigatie-eigenschappen
        [ForeignKey("FldProjectId")]
        public Project? Project { get; set; }

        [ForeignKey("FldStatus")]
        public Status? Status { get; set; }

        [ForeignKey("FldOpdrachtgeverId")]
        public Adres? Customer { get; set; }

        [ForeignKey("FldSoort")]
        public ProjectType? ProjectType { get; set; }
    }
}