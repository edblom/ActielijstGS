using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("tblRelatiePartijen", Schema = "dbo")]
    public partial class TblRelatiePartijen
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public string SourceTabel { get; set; }

        public int? SourceId { get; set; }

        public int? AdresId { get; set; }

        public int? ContactId { get; set; }

        public string Relatie { get; set; }
    }
}