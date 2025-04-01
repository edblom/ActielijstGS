using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("stblFactureermethode", Schema = "dbo")]
    public partial class StblFactureermethode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public string Omschrijving { get; set; }

        public string AantalMaal { get; set; }

        public string Periode { get; set; }
    }
}