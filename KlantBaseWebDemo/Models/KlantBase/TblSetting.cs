using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlantBaseWebDemo.Models.KlantBase
{
    [Table("tblSettings", Schema = "dbo")]
    public partial class TblSetting
    {
        public float? SettingApptSlotDuration { get; set; }

        public int? SettingColorBackground { get; set; }

        public int? SettingColorFontNormal { get; set; }

        public int? SettingColorGrid { get; set; }

        public int? SettingColorNotAvailable { get; set; }

        public int? SettingColorIsAvailable { get; set; }

        public int? SettingColorNotArrived { get; set; }

        public int? SettingColorArrived { get; set; }

        public int? SettingColorInSession { get; set; }

        public int? SettingColorDeparted { get; set; }

        public int? SettingColorNotPaid { get; set; }

        public string SettingTimeFormat { get; set; }

        [Timestamp]
        [Column("SSMA_TimeStamp")]
        public byte[] SsmaTimeStamp { get; set; }
    }
}