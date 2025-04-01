using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("contactpersonen", Schema = "dbo")]
    public partial class Contactpersonen
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ContactPersID")]
        public int ContactPersId { get; set; }

        [Column("voorletters")]
        public string Voorletters { get; set; }

        [Column("roepnaam")]
        public string Roepnaam { get; set; }

        [Column("voorvoegsel")]
        public string Voorvoegsel { get; set; }

        [Column("tussenvoegsel")]
        public string Tussenvoegsel { get; set; }

        [Column("achternaam")]
        public string Achternaam { get; set; }

        [Column("tav")]
        public string Tav { get; set; }

        [Column("geachte")]
        public string Geachte { get; set; }

        [Column("tel_prive")]
        public string TelPrive { get; set; }

        [Column("tel_werk")]
        public string TelWerk { get; set; }

        [Column("mobiel_tel")]
        public string MobielTel { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("klantId")]
        public int? KlantId { get; set; }

        [Column("fldAdres")]
        public string FldAdres { get; set; }

        [Column("fldPC")]
        public string FldPc { get; set; }

        [Column("fldPlaats")]
        public string FldPlaats { get; set; }

        [Column("oldid")]
        public int? Oldid { get; set; }

        [Column("functie")]
        public string Functie { get; set; }
    }
}