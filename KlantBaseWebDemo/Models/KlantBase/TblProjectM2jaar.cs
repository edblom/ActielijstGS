using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("tblProjectM2Jaar", Schema = "dbo")]
    public partial class TblProjectM2jaar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public int? ProjectId { get; set; }

        public int? AantalM2 { get; set; }

        public int? Jaar { get; set; }
    }
}