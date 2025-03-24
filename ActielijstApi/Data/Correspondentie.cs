using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActielijstApi.Data
{
    public class Correspondentie
    {
        [Key]
        public int Id { get; set; }

        [DefaultValue(0)]
        public int KlantID { get; set; }

        [StringLength(250)]
        public string? fldCorOffNum { get; set; }

        [DefaultValue(0)]
        public int? fldCorProjNum { get; set; }

        [DefaultValue(0)]
        public int? fldCorOpdrachtNum { get; set; }

        [DefaultValue(0)]
        public int? fldCorConsultancyId { get; set; }

        [DefaultValue(0)]
        public int? fldCorTrainingId { get; set; }

        public int? fldCorFactuurId { get; set; }

        public DateTime? fldCorDatum { get; set; }

        public DateTime? fldCorDatum2 { get; set; }

        [StringLength(250)]
        public string? fldCorAuteur { get; set; }

        [StringLength(250)]
        public string? fldCorOmschrijving { get; set; }

        [StringLength(250)]
        public string? fldCorKenmerk { get; set; }

        [StringLength(250)]
        public string? fldCorBestand { get; set; }

        [DefaultValue(0)]
        public int? fldCorSoort { get; set; }

        [StringLength(250)]
        public string? fldCorTav { get; set; }

        [StringLength(250)]
        public string? fldCorGeachte { get; set; }

        [DefaultValue(0)]
        public int? fldCorCPersId { get; set; }

        [StringLength(250)]
        public string? fldCorExtensie { get; set; }

        [StringLength(250)]
        public string? fldCorProgramma { get; set; }

        [StringLength(250)]
        public string? fldSjabloon { get; set; }

        [StringLength(250)]
        public string? fldAan { get; set; }

        [StringLength(250)]
        public string? fldCC { get; set; }

        [StringLength(250)]
        public string? fldFrom { get; set; }

        [StringLength(250)]
        public string? fldBijlage { get; set; }

        [StringLength(250)]
        public string? fldBijlage2 { get; set; }

        [StringLength(250)]
        public string? fldBijlage3 { get; set; }

        public string? fldBody { get; set; } // nvarchar(max)

        [Timestamp]
        public byte[]? SSMA_TimeStamp { get; set; }
    }
}
