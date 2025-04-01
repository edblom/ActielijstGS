using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("vw_AankomendeInspecties", Schema = "dbo")]
    public partial class VwAankomendeInspecty
    {
        public string Project { get; set; }

        public string ProjectNr { get; set; }

        public string Adres { get; set; }

        public string Applicateur { get; set; }

        public string Omschrijving { get; set; }

        public string InspecteurId { get; set; }

        public string ExtraMedewerker { get; set; }

        public DateTime? DatumGereed { get; set; }

        [Column("PSID")]
        [Required]
        public int Psid { get; set; }

        public int? StatusId { get; set; }

        [Column("status")]
        public string Status { get; set; }

        public string Opdracht { get; set; }

        public int? SoortId { get; set; }

        public string Soort { get; set; }

        public DateTime? Toegekend { get; set; }

        public bool? Toegewezen { get; set; }

        public DateTime? AppointmentDateTime { get; set; }

        [Column("fldSGG")]
        public string FldSgg { get; set; }

        [Column("fldBedrag")]
        public decimal? FldBedrag { get; set; }
    }
}