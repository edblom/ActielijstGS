using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("stblTrainingsSoort", Schema = "dbo")]
    public partial class StblTrainingsSoort
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public string Code { get; set; }

        public string Omschrijving { get; set; }

        public decimal? Standaardprijs { get; set; }

        [Column("extra")]
        public string Extra { get; set; }
    }
}