using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("Werknemers", Schema = "dbo")]
    public partial class Werknemer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WerknId { get; set; }

        public string Titel { get; set; }

        public string Voornaam { get; set; }

        public string Voorletters { get; set; }

        public string Tussenvoegsel { get; set; }

        public string Achternaam { get; set; }

        public string Functie { get; set; }

        [Column("geslacht")]
        public string Geslacht { get; set; }

        public string EmailNaam { get; set; }

        public string Initialen { get; set; }

        public string CalcOmschrijving { get; set; }

        [Column("fldAdres")]
        public string FldAdres { get; set; }

        [Column("fldPC")]
        public string FldPc { get; set; }

        [Column("fldPlaats")]
        public string FldPlaats { get; set; }

        [Column("fldTelefoon")]
        public string FldTelefoon { get; set; }

        [Column("fldMobiel")]
        public string FldMobiel { get; set; }

        [Column("fldEmailPrive")]
        public string FldEmailPrive { get; set; }

        [Column("fldDatumIndienst")]
        public DateTime? FldDatumIndienst { get; set; }

        [Column("fldDatumUitDienst")]
        public DateTime? FldDatumUitDienst { get; set; }

        [Column("fldToegangsCode")]
        public string FldToegangsCode { get; set; }

        [Column("fldWachtwoord")]
        public string FldWachtwoord { get; set; }

        [Column("fldLoginNaam")]
        public string FldLoginNaam { get; set; }

        [Column("fldGebDatum")]
        public DateTime? FldGebDatum { get; set; }

        [Column("fldGebPlaats")]
        public string FldGebPlaats { get; set; }

        [Column("fldBurgStaat")]
        public string FldBurgStaat { get; set; }

        [Column("fldDatumBurgStaat")]
        public DateTime? FldDatumBurgStaat { get; set; }

        [Column("fldVoornaamPartner")]
        public string FldVoornaamPartner { get; set; }

        [Column("fldVoorlettersPartner")]
        public string FldVoorlettersPartner { get; set; }

        [Column("fldTussenvoegselPartner")]
        public string FldTussenvoegselPartner { get; set; }

        [Column("fldAchternaamPartner")]
        public string FldAchternaamPartner { get; set; }

        [Column("fldGebDatumPartner")]
        public string FldGebDatumPartner { get; set; }

        [Column("fldPensioenverz")]
        public bool? FldPensioenverz { get; set; }

        [Column("fldPensioenPolisnr")]
        public string FldPensioenPolisnr { get; set; }

        [Column("Aantal Kinderen")]
        public int? AantalKinderen { get; set; }

        [Column("fldNrVeiligheidsPaspoort")]
        public string FldNrVeiligheidsPaspoort { get; set; }

        [Column("fldAdministrator")]
        public bool? FldAdministrator { get; set; }

        [Timestamp]
        [Column("SSMA_TimeStamp")]
        public byte[] SsmaTimeStamp { get; set; }

        [Column("fldFacturering")]
        public int? FldFacturering { get; set; }

        [Column("fldPlanningAlles")]
        public int? FldPlanningAlles { get; set; }

        [Column("fldPlanningKiwa")]
        public int? FldPlanningKiwa { get; set; }

        [Column("fldPlanningSGG")]
        public int? FldPlanningSgg { get; set; }

        [Column("fldPlanningSteekproeven")]
        public int? FldPlanningSteekproeven { get; set; }

        [Column("fldPlanningOverig")]
        public int? FldPlanningOverig { get; set; }

        public string Color { get; set; }

        public int? DashBoardId { get; set; }

        // Toegevoegde property voor dropdown
        public string DisplayName => $"{Voornaam} {(!string.IsNullOrEmpty(Tussenvoegsel) ? Tussenvoegsel + " " : "")}{Achternaam} ({Initialen})";
    }
}