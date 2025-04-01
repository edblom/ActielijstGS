using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("stblRelatieSoort", Schema = "dbo")]
    public partial class StblRelatieSoort
    {
        [Key]
        [Required]
        public string Omschrijving { get; set; }
    }
}