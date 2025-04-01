using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("tblProjectOnderdelen", Schema = "dbo")]
    public partial class TblProjectOnderdelen
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("fldOpdrachtId")]
        public int? FldOpdrachtId { get; set; }

        [Column("fldOpdrachtNr")]
        public int? FldOpdrachtNr { get; set; }

        [Column("fldOpdrachtStr")]
        public string FldOpdrachtStr { get; set; }

        [Column("fldProjectId")]
        public int? FldProjectId { get; set; }

        public TblProjecten TblProjecten { get; set; }

        [Column("fldAfdeling")]
        public string FldAfdeling { get; set; }

        [Column("fldSoort")]
        public int? FldSoort { get; set; }

        [Column("fldPlanDatum")]
        public DateTime? FldPlanDatum { get; set; }

        [Column("fldOmschrijving")]
        public string FldOmschrijving { get; set; }

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
        public string FldProjectLeider { get; set; }

        public string ExtraMedewerker { get; set; }

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

        public string Fabrikant { get; set; }

        public string Systeem { get; set; }

        public string AantalM2 { get; set; }

        public string Gnummer { get; set; }

        public DateTime? Datum1eInspectie1 { get; set; }

        public int? VerwerkendBedrijf { get; set; }

        [Timestamp]
        [Column("SSMA_TimeStamp")]
        public byte[] SsmaTimeStamp { get; set; }

        public string Contractnr { get; set; }

        public int? Looptijd { get; set; }

        public DateTime? EindDatumContract { get; set; }

        public int? Factuurmaand { get; set; }

        public int? BelStatus { get; set; }

        public string BelNotitie { get; set; }

        public DateTime? BelDatum { get; set; }

        public string BelStatusText { get; set; }

        [Column("fldCertKeuring")]
        public bool? FldCertKeuring { get; set; }

        [Column("fldKiwaKeuringsNr")]
        public int? FldKiwaKeuringsNr { get; set; }

        public int? KortingId { get; set; }

        public string Kortingomschrijving { get; set; }

        public decimal? Kortingbedrag { get; set; }

        public double? Kortingspercentage { get; set; }

        public DateTime? Toegekend { get; set; }

        [Column("AppointmentEntryID")]
        public string AppointmentEntryId { get; set; }

        public DateTime? AppointmentDateTime { get; set; }

        public string OpdrachtAdres { get; set; }

        public string OpdrachtHuisnr { get; set; }

        [Column("OpdrachtPC")]
        public string OpdrachtPc { get; set; }

        public string OpdrachtPlaats { get; set; }

        public decimal? ContractBedrag { get; set; }

        public double? ContractIndexering { get; set; }

        [Column("fldPlanPeriodeVan")]
        public DateTime? FldPlanPeriodeVan { get; set; }

        [Column("fldPlanPeriodeTot")]
        public DateTime? FldPlanPeriodeTot { get; set; }

        [Column("fldFolder")]
        public string FldFolder { get; set; }

        public string SteekproefMaand { get; set; }

        public ICollection<TblFactRegel> TblFactRegels { get; set; }
    }
}