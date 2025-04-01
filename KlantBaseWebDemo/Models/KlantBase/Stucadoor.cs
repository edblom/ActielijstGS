using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("Stucadoors", Schema = "dbo")]
    public partial class Stucadoor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Bedrijf { get; set; }

        [Column("adresregel")]
        public string Adresregel { get; set; }

        public string Adres { get; set; }

        [Column("plaats")]
        public string Plaats { get; set; }

        [Column("postcode")]
        public string Postcode { get; set; }

        [Column("huisnummer")]
        public string Huisnummer { get; set; }

        [Column("partner")]
        public double? Partner { get; set; }

        [Column("Nr SGG")]
        public string NrSgg { get; set; }

        [Column("deelnemer")]
        public string Deelnemer { get; set; }

        [Column("contactpers")]
        public string Contactpers { get; set; }

        [Column("KOMO houder")]
        public string KomoHouder { get; set; }

        [Column("Stand van zaken / Actie")]
        public string StandVanZakenActie { get; set; }

        public string Contactpersoon { get; set; }

        [Column("E-mail")]
        public string EMail { get; set; }

        public string Tel { get; set; }

        public string Fax { get; set; }

        public string Mobiel { get; set; }

        [Timestamp]
        [Column("SSMA_TimeStamp")]
        public byte[] SsmaTimeStamp { get; set; }
    }
}