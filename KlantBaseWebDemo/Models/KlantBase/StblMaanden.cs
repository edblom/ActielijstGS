using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("stbl Maanden", Schema = "dbo")]
    public partial class StblMaanden
    {
        [Column("mnd_id")]
        public int? MndId { get; set; }

        [Column("mnd_omschrijving")]
        public string MndOmschrijving { get; set; }
    }
}