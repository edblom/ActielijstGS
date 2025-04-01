using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("tblKeuring", Schema = "dbo")]
    public partial class TblKeuring
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("fldOpdrachtId")]
        public int? FldOpdrachtId { get; set; }

        [Column("fldKeurSoort")]
        public string FldKeurSoort { get; set; }

        [Column("fldKeurPeriodeVan")]
        public DateTime? FldKeurPeriodeVan { get; set; }

        [Column("fldKeurPeriodeTot")]
        public DateTime? FldKeurPeriodeTot { get; set; }

        [Column("fldKeurPlandatum")]
        public DateTime? FldKeurPlandatum { get; set; }

        [Column("fldKeurUitgevoerd")]
        public DateTime? FldKeurUitgevoerd { get; set; }

        [Column("fldKeurNotitie")]
        public string FldKeurNotitie { get; set; }

        [Column("fldOnderhoudInspectieId")]
        public int? FldOnderhoudInspectieId { get; set; }
    }
}