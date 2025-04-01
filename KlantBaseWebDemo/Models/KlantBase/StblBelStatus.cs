using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("stblBelStatus", Schema = "dbo")]
    public partial class StblBelStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public int? Volgnr { get; set; }

        [Column("omschrijving")]
        public string Omschrijving { get; set; }

        [Column("kleur")]
        public string Kleur { get; set; }
    }
}