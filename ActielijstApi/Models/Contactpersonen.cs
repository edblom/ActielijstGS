using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ActielijstApi.Models
{
    [Table("contactpersonen")]
    public class Contactpersonen
    {
        [Key]
        [Column("ContactPersID")]
        public int Id { get; set; } // Primaire sleutel
        [Column("voorletters")]
        public string? voorletters { get; set; }
        public string? roepnaam { get; set; } 
        public string? voorvoegsel { get; set; }
        public string? tussenvoegsel { get; set; }
        public string? achternaam { get; set; }
        public string? tav { get; set; }
        public string? geachte { get; set; }
        [Column("tel_prive")]
        public string? TelefoonPrive { get; set; }
        [Column("tel_werk")]
        public string? TelefoonWerk{ get; set; }
        [Column("mobiel_tel")]
        public string? TelefoonMobiel { get; set; }
        public string? email { get; set; }
        public int? KlantId { get; set; }
        [Column ("fldAdres")]
        public string? Adres { get; set; }
        [Column("fldPC")]
        public string? Postcode { get; set; }
        [Column("fldPlaats")]
        public string? Plaats { get; set; }
        public string? functie { get; set; }
        [ForeignKey("KlantId")]
        public Project? Klant { get; set; }
    }
}