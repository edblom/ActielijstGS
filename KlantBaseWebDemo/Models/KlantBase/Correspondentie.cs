using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("correspondentie", Schema = "dbo")]
    public partial class Correspondentie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("KlantID")]
        public int? KlantId { get; set; }

        [Column("fldCorOffNum")]
        public string FldCorOffNum { get; set; }

        [Column("fldCorProjNum")]
        public int? FldCorProjNum { get; set; }

        [Column("fldCorOpdrachtNum")]
        public int? FldCorOpdrachtNum { get; set; }

        [Column("fldCorConsultancyId")]
        public int? FldCorConsultancyId { get; set; }

        [Column("fldCorTrainingId")]
        public int? FldCorTrainingId { get; set; }

        [Column("fldCorFactuurId")]
        public int? FldCorFactuurId { get; set; }

        [Column("fldCorDatum")]
        public DateTime? FldCorDatum { get; set; }

        [Column("fldCorDatum2")]
        public DateTime? FldCorDatum2 { get; set; }

        [Column("fldCorAuteur")]
        public string FldCorAuteur { get; set; }

        [Column("fldCorOmschrijving")]
        public string FldCorOmschrijving { get; set; }

        [Column("fldCorKenmerk")]
        public string FldCorKenmerk { get; set; }

        [Column("fldCorBestand")]
        public string FldCorBestand { get; set; }

        [Column("fldCorSoort")]
        public int? FldCorSoort { get; set; }

        [Column("fldCorTav")]
        public string FldCorTav { get; set; }

        [Column("fldCorGeachte")]
        public string FldCorGeachte { get; set; }

        [Column("fldCorCPersId")]
        public int? FldCorCpersId { get; set; }

        [Column("fldCorExtensie")]
        public string FldCorExtensie { get; set; }

        [Column("fldCorProgramma")]
        public string FldCorProgramma { get; set; }

        [Column("fldSjabloon")]
        public string FldSjabloon { get; set; }

        [Column("fldAan")]
        public string FldAan { get; set; }

        [Column("fldCC")]
        public string FldCc { get; set; }

        [Column("fldFrom")]
        public string FldFrom { get; set; }

        [Column("fldBijlage")]
        public string FldBijlage { get; set; }

        [Column("fldBijlage2")]
        public string FldBijlage2 { get; set; }

        [Column("fldBijlage3")]
        public string FldBijlage3 { get; set; }

        [Column("fldBody")]
        public string FldBody { get; set; }

        [Timestamp]
        [Column("SSMA_TimeStamp")]
        public byte[] SsmaTimeStamp { get; set; }
    }
}