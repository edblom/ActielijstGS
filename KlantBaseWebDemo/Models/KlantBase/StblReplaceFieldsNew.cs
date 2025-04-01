using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("stblReplaceFieldsNew", Schema = "dbo")]
    public partial class StblReplaceFieldsNew
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string ReplaceString { get; set; }

        public string Description { get; set; }

        public string Fieldname { get; set; }

        public bool? Gebruikt { get; set; }
    }
}