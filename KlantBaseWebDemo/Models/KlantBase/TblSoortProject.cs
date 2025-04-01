using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("tblSoortProject", Schema = "dbo")]
    public partial class TblSoortProject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Omschrijving { get; set; }

        public string Soort { get; set; }

        [Column("categorie")]
        public string Categorie { get; set; }

        [Column("tabel")]
        public string Tabel { get; set; }

        [Column("tabelSoort")]
        public string TabelSoort { get; set; }

        [Column("facturering")]
        public int? Facturering { get; set; }

        public bool? OpEenRegel { get; set; }

        public int? CategorieId { get; set; }

        [Timestamp]
        [Column("SSMA_TimeStamp")]
        public byte[] SsmaTimeStamp { get; set; }
    }
}