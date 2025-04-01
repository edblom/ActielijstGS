using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("stblPriority", Schema = "dbo")]
    public partial class StblPriority
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("prioriteit")]
        public int? Prioriteit { get; set; }

        public string Omschrijving { get; set; }

        public string Kleur { get; set; }
    }
}