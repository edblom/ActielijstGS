// Helpers/DocumentHelper.cs
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Linq;

namespace ActielijstApi.Helpers
{
    public static class DocumentHelper
    {
        public static void UpdateFieldsInDocument(WordprocessingDocument doc)
        {
            var settingsPart = doc.MainDocumentPart.DocumentSettingsPart;
            if (settingsPart == null)
            {
                settingsPart = doc.MainDocumentPart.AddNewPart<DocumentSettingsPart>();
                settingsPart.Settings = new Settings();
            }

            var updateFields = settingsPart.Settings.Elements<UpdateFieldsOnOpen>().FirstOrDefault();
            if (updateFields == null)
            {
                updateFields = new UpdateFieldsOnOpen { Val = true };
                settingsPart.Settings.Append(updateFields);
            }
            else
            {
                updateFields.Val = true;
            }

            settingsPart.Settings.Save();
        }

        public static void UpdatePropertyValue(DocumentFormat.OpenXml.CustomProperties.CustomDocumentProperty prop, string newValue)
        {
            if (prop.VTLPWSTR != null)
            {
                prop.RemoveChild(prop.VTLPWSTR);
                prop.VTLPWSTR = new DocumentFormat.OpenXml.VariantTypes.VTLPWSTR(newValue ?? "");
            }
            else if (prop.VTInt32 != null)
            {
                int.TryParse(newValue, out int intValue);
                prop.RemoveChild(prop.VTInt32);
                prop.VTInt32 = new DocumentFormat.OpenXml.VariantTypes.VTInt32(intValue.ToString());
            }
            else if (prop.VTBool != null)
            {
                bool.TryParse(newValue, out bool boolValue);
                prop.RemoveChild(prop.VTBool);
                prop.VTBool = new DocumentFormat.OpenXml.VariantTypes.VTBool(boolValue ? "1" : "0");
            }
            else if (prop.VTDate != null)
            {
                if (DateTime.TryParse(newValue, out DateTime dateValue))
                {
                    prop.RemoveChild(prop.VTDate);
                    prop.VTDate = new DocumentFormat.OpenXml.VariantTypes.VTDate(dateValue.ToString("O"));
                }
                else
                {
                    prop.RemoveChild(prop.VTDate);
                    prop.VTDate = new DocumentFormat.OpenXml.VariantTypes.VTDate(DateTime.MinValue.ToString("O"));
                }
            }
        }
    }
}