using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("stbl Selectie", Schema = "dbo")]
    public partial class StblSelectie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SelId { get; set; }

        [Column("omschrijving")]
        public string Omschrijving { get; set; }
    }
}