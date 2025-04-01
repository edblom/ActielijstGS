using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("vw_KIWAInspecties", Schema = "dbo")]
    public partial class VwKiwainspecty
    {
        public int? CategorieId { get; set; }

        public string Categorie { get; set; }

        [Column("fldProjectId")]
        public int? FldProjectId { get; set; }

        [Column("fldProjectNaam")]
        public string FldProjectNaam { get; set; }

        [Column("fldCertKeuring")]
        public bool? FldCertKeuring { get; set; }

        [Column("fldMaandBedrag")]
        public decimal? FldMaandBedrag { get; set; }

        [Column("fldSoort")]
        public int? FldSoort { get; set; }

        [Column("fldPlanDatum")]
        public DateTime? FldPlanDatum { get; set; }

        public int? PlanJaar { get; set; }

        public string Omschrijving { get; set; }

        [Column("fldProjectNummer")]
        public int? FldProjectNummer { get; set; }

        [Column("fldKiwaCert")]
        public bool? FldKiwaCert { get; set; }

        [Column("fldVerwerkendBedrijf")]
        public int? FldVerwerkendBedrijf { get; set; }

        [Column("fldExternNummer")]
        public string FldExternNummer { get; set; }

        [Column("fldPlaats")]
        public string FldPlaats { get; set; }

        [Column("fldKiwaKeuringsNr")]
        public int? FldKiwaKeuringsNr { get; set; }

        public DateTime? PlanDatum { get; set; }

        [Column("fldStatus")]
        public int? FldStatus { get; set; }

        [Column("status")]
        public string Status { get; set; }

        public string BelNotitie { get; set; }

        [Column("fldProjectLeider")]
        public string FldProjectLeider { get; set; }

        public string ExtraMedewerker { get; set; }

        [Column("fldOpdrachtStr")]
        public string FldOpdrachtStr { get; set; }

        [Column("fldGereedVoor")]
        public DateTime? FldGereedVoor { get; set; }

        [Column("fldDatumGereed")]
        public DateTime? FldDatumGereed { get; set; }

        public string Applicateur { get; set; }

        [Column("fldOpdrachtId")]
        public int? FldOpdrachtId { get; set; }

        public int? OpdrachtId { get; set; }

        public string KiwaNummer { get; set; }

        [Required]
        public int Toegewezen { get; set; }

        [Column("fldjaar")]
        public string Fldjaar { get; set; }

        [Column("fldAantalM2")]
        public int? FldAantalM2 { get; set; }

        public string OpdrachtAdres { get; set; }

        public string OpdrachtHuisnr { get; set; }

        [Column("OpdrachtPC")]
        public string OpdrachtPc { get; set; }

        public string OpdrachtPlaats { get; set; }

        [Column("fldSGG")]
        public string FldSgg { get; set; }

        [Column("fldBedrag")]
        public decimal? FldBedrag { get; set; }
    }
}