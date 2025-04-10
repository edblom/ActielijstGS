using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActielijstApi.Models
{
    [Table("correspondentie")]
    public class Correspondentie
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [DefaultValue(0)]
        [Column("KlantID")]
        public int KlantID { get; set; }

        [StringLength(250)]
        [Column("fldCorOffNum")]
        public string? fldCorOffNum { get; set; }

        [DefaultValue(0)]
        [Column("fldCorProjNum")]
        public int? fldCorProjNum { get; set; }

        [DefaultValue(0)]
        [Column("fldCorOpdrachtNum")]
        public int? fldCorOpdrachtNum { get; set; }

        [DefaultValue(0)]
        [Column("fldCorConsultancyId")]
        public int? fldCorConsultancyId { get; set; }

        [DefaultValue(0)]
        [Column("fldCorTrainingId")]
        public int? fldCorTrainingId { get; set; }

        [Column("fldCorFactuurId")]
        public int? fldCorFactuurId { get; set; }

        [Column("fldCorDatum")]
        public DateTime? fldCorDatum { get; set; }

        [Column("fldCorDatum2")]
        public DateTime? fldCorDatum2 { get; set; }

        [StringLength(250)]
        [Column("fldCorAuteur")]
        public string? fldCorAuteur { get; set; }

        [StringLength(250)]
        [Column("fldCorOmschrijving")]
        public string? fldCorOmschrijving { get; set; }

        [StringLength(250)]
        [Column("fldCorKenmerk")]
        public string? fldCorKenmerk { get; set; }

        [StringLength(250)]
        [Column("fldCorBestand")]
        public string? fldCorBestand { get; set; }

        [DefaultValue(0)]
        [Column("fldCorSoort")]
        public int? fldCorSoort { get; set; }

        [StringLength(250)]
        [Column("fldCorTav")]
        public string? fldCorTav { get; set; }

        [StringLength(250)]
        [Column("fldCorGeachte")]
        public string? fldCorGeachte { get; set; }

        [DefaultValue(0)]
        [Column("fldCorCPersId")]
        public int? fldCorCPersId { get; set; }

        [StringLength(250)]
        [Column("fldCorExtensie")]
        public string? fldCorExtensie { get; set; }

        [StringLength(250)]
        [Column("fldCorProgramma")]
        public string? fldCorProgramma { get; set; }

        [StringLength(250)]
        [Column("fldSjabloon")]
        public string? fldSjabloon { get; set; }

        [StringLength(250)]
        [Column("fldAan")]
        public string? fldAan { get; set; }

        [StringLength(250)]
        [Column("fldCC")]
        public string? fldCC { get; set; }

        [StringLength(250)]
        [Column("fldFrom")]
        public string? fldFrom { get; set; }

        [StringLength(250)]
        [Column("fldBijlage")]
        public string? fldBijlage { get; set; }

        [StringLength(250)]
        [Column("fldBijlage2")]
        public string? fldBijlage2 { get; set; }

        [StringLength(250)]
        [Column("fldBijlage3")]
        public string? fldBijlage3 { get; set; }

        [Column("fldBody")]
        public string? fldBody { get; set; } // nvarchar(max)

        [Timestamp]
        [Column("SSMA_TimeStamp")]
        public byte[]? SSMA_TimeStamp { get; set; }
    }
}