using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("stblFactuurKorting", Schema = "dbo")]
    public partial class StblFactuurKorting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public string Omschrijving { get; set; }

        public decimal? Kortingbedrag { get; set; }

        public double? Kortingspercentage { get; set; }
    }
}