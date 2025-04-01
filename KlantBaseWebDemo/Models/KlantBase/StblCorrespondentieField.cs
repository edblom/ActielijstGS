using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("stblCorrespondentieFields", Schema = "dbo")]
    public partial class StblCorrespondentieField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public string ReplaceString { get; set; }

        public string Description { get; set; }

        public string FieldName { get; set; }

        public bool? Gebruikt { get; set; }

        public string Tabel { get; set; }

        public string Veld { get; set; }

        public string Standaardwaarde { get; set; }

        [Column("veldType")]
        public string VeldType { get; set; }

        [Column("idNaam")]
        public string IdNaam { get; set; }

        public string CorrespondentieId { get; set; }
    }
}