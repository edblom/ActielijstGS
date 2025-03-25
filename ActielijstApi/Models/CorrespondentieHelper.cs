using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.RegularExpressions;
using ActielijstApi.Data;

namespace ActielijstApi.Models
{
    public static class CorrespondentieHelper
    {
        public static async Task<string> GetCorrespondentieVeldValueAsync(
            ApplicationDbContext context,
            StblCorrespondentieField field,
            Correspondentie correspondentie)
        {
            // Stap 1: Valideer de invoerparameters
            Console.WriteLine("Stap 1: Valideer invoerparameters");
            if (string.IsNullOrEmpty(field.Tabel))
            {
                Console.WriteLine("Tabel is leeg of null. Retourneer lege string.");
                return "";
            }
            if (string.IsNullOrEmpty(field.Veld))
            {
                Console.WriteLine("Veld is leeg of null. Retourneer lege string.");
                return "";
            }
            if (string.IsNullOrEmpty(field.IdNaam))
            {
                Console.WriteLine("IdNaam is leeg of null. Retourneer lege string.");
                return "";
            }
            if (string.IsNullOrEmpty(field.CorrespondentieId))
            {
                Console.WriteLine("CorrespondentieId is leeg of null. Retourneer lege string.");
                return "";
            }
            if (correspondentie == null)
            {
                Console.WriteLine("Correspondentie is null. Gooi uitzondering.");
                throw new ArgumentNullException(nameof(correspondentie));
            }

            // Log de waarden van de field-parameter
            Console.WriteLine($"Field waarden: Tabel={field.Tabel}, Veld={field.Veld}, IdNaam={field.IdNaam}, CorrespondentieId={field.CorrespondentieId}, ReplaceString={field.ReplaceString}");

            // Stap 2: Controleer of de tabel wordt ondersteund (voorlopig alleen "adres")
            Console.WriteLine("Stap 2: Controleer ondersteunde tabel");
            if (field.Tabel.ToLower() != "adres")
            {
                Console.WriteLine($"Tabel '{field.Tabel}' wordt niet ondersteund (alleen 'adres' wordt ondersteund). Retourneer lege string.");
                return "";
            }

            // Stap 3: Haal het ID uit correspondentie gebaseerd op CorrespondentieId
            Console.WriteLine("Stap 3: Haal ID uit Correspondentie gebaseerd op CorrespondentieId");
            int? idValue = null;
            string correspondentieIdLower = field.CorrespondentieId.ToLower();
            Console.WriteLine($"CorrespondentieId (na ToLower): {correspondentieIdLower}");

            switch (correspondentieIdLower)
            {
                case "id":
                    idValue = correspondentie.Id;
                    Console.WriteLine($"CorrespondentieId='id', idValue=correspondentie.Id={idValue}");
                    break;
                case "klantid":
                    idValue = correspondentie.KlantID;
                    Console.WriteLine($"CorrespondentieId='klantid', idValue=correspondentie.KlantID={idValue}");
                    break;
                case "fldcoropdrachtnum":
                    idValue = correspondentie.fldCorOpdrachtNum;
                    Console.WriteLine($"CorrespondentieId='fldcoropdrachtnum', idValue=correspondentie.fldCorOpdrachtNum={idValue}");
                    break;
                case "fldcorprojnum":
                    idValue = correspondentie.fldCorProjNum;
                    Console.WriteLine($"CorrespondentieId='fldcorprojnum', idValue=correspondentie.fldCorProjNum={idValue}");
                    break;
                case "fldcorcpersid":
                    idValue = correspondentie.fldCorCPersId;
                    Console.WriteLine($"CorrespondentieId='fldcorcpersid', idValue=correspondentie.fldCorCPersId={idValue}");
                    break;
                case "fldcorauteur":
                    bool parseSuccess = int.TryParse(correspondentie.fldCorAuteur, out int auteurId);
                    idValue = parseSuccess ? auteurId : null;
                    Console.WriteLine($"CorrespondentieId='fldcorauteur', fldCorAuteur='{correspondentie.fldCorAuteur}', parseSuccess={parseSuccess}, idValue={idValue}");
                    break;
                default:
                    Console.WriteLine($"CorrespondentieId '{field.CorrespondentieId}' wordt niet ondersteund. Gooi uitzondering.");
                    throw new NotSupportedException($"CorrespondentieId {field.CorrespondentieId} wordt niet ondersteund.");
            }

            // Stap 4: Controleer of idValue een waarde heeft
            Console.WriteLine("Stap 4: Controleer idValue");
            if (!idValue.HasValue)
            {
                Console.WriteLine("idValue is null. Retourneer lege string.");
                return "";
            }
            Console.WriteLine($"idValue heeft waarde: {idValue}");

            // Stap 5: Voer de query uit voor de adres-tabel
            Console.WriteLine("Stap 5: Voer query uit voor adres-tabel");
            object? value = null;
            if (field.Tabel.ToLower() == "adres")
            {
                // Controleer of IdNaam overeenkomt met "id" of "Id" (hoofdletterongevoelig)
                Console.WriteLine($"Controleer IdNaam: {field.IdNaam}");
                if (!string.Equals(field.IdNaam, "id", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"IdNaam '{field.IdNaam}' komt niet overeen met 'id' of 'Id'. Gooi uitzondering.");
                    throw new InvalidOperationException($"Voor tabel 'adres' moet IdNaam 'id' of 'Id' zijn, maar was '{field.IdNaam}'.");
                }

                // Normaliseer field.Veld om speciale tekens te verwijderen en hoofdletterongevoelig te maken
                string fieldVeldNormalized = field.Veld;
                if (fieldVeldNormalized.StartsWith("[") && fieldVeldNormalized.EndsWith("]"))
                {
                    fieldVeldNormalized = fieldVeldNormalized.Substring(1, fieldVeldNormalized.Length - 2);
                    Console.WriteLine($"Veld na verwijderen van vierkante haken: {fieldVeldNormalized}");
                }

                // Normaliseer de kolomnaam door speciale tekens te verwijderen (nabootsen van EF Core-conventie)
                string fieldVeldNormalizedForComparison = Regex.Replace(fieldVeldNormalized, "[^a-zA-Z0-9]", "").ToLower();
                Console.WriteLine($"Genormaliseerde Veld voor vergelijking: {fieldVeldNormalizedForComparison}");

                // Zoek de eigenschap in de Adres-klasse (hoofdletterongevoelig)
                Console.WriteLine($"Zoek eigenschap voor Veld: {field.Veld}");
                var propertyInfo = typeof(Adres).GetProperties()
                    .FirstOrDefault(p => p.Name.Equals(fieldVeldNormalizedForComparison, StringComparison.OrdinalIgnoreCase));

                if (propertyInfo == null)
                {
                    Console.WriteLine($"Geen eigenschap gevonden voor Veld '{field.Veld}' in Adres-klasse. Gooi uitzondering.");
                    throw new InvalidOperationException($"Eigenschap '{field.Veld}' bestaat niet op type Adres.");
                }

                string veldPropertyName = propertyInfo.Name; // Bijv. "EmailAdr"
                Console.WriteLine($"Gevonden eigenschap voor Veld: {veldPropertyName}");

                // Voer de query uit met de eigenschapnaam
                Console.WriteLine($"Voer query uit: WHERE Id = {idValue}, SELECT {veldPropertyName}");
                value = await context.Adresses
                    .Where(a => a.Id == idValue)
                    .Select(a => EF.Property<object>(a, veldPropertyName)) // Gebruik de eigenschapnaam
                    .FirstOrDefaultAsync();

                Console.WriteLine($"Query resultaat: value={value ?? "null"}");
            }

            // Stap 6: Formatteer de waarde en retourneer
            Console.WriteLine("Stap 6: Formatteer de waarde");
            string result = FormatValue(value, field.VeldType, field.Standaardwaarde);
            Console.WriteLine($"Geformatteerde waarde: {result}");
            return result;
        }

        private static string FormatValue(object? value, string? fieldType, string? defaultFormat)
        {
            Console.WriteLine($"FormatValue: value={value ?? "null"}, fieldType={fieldType}, defaultFormat={defaultFormat}");
            if (value == null) return "";

            if (fieldType?.ToLower() == "datum" && value is DateTime date)
            {
                string formattedDate = date.ToString(defaultFormat ?? "yyyy-MM-dd");
                Console.WriteLine($"Datum geformatteerd: {formattedDate}");
                return formattedDate;
            }

            string result = value.ToString() ?? "";
            Console.WriteLine($"Waarde als string: {result}");
            return result;
        }
    }
}