using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("stblBooks", Schema = "dbo")]
    public partial class StblBook
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("fldTitle")]
        public string FldTitle { get; set; }

        [Column("fldAdviePrijs")]
        public decimal? FldAdviePrijs { get; set; }
    }
}