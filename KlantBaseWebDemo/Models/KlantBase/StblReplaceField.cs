using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("stblReplaceFields", Schema = "dbo")]
    public partial class StblReplaceField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public string ReplaceString { get; set; }

        public string Description { get; set; }

        public string Fieldname { get; set; }

        public bool? Gebruikt { get; set; }
    }
}