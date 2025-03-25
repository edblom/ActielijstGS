// Data/CorrespondentieFields.cs
using System;

namespace ActielijstApi.Data
{
    public class CorrespondentieFields
    {
        public int CorrespondentieNr { get; set; }
        public string? Betreft { get; set; }
        public DateTime? Datum { get; set; }
        public string? Kenmerk { get; set; }
        public string? Adres { get; set; }
        public string? Bedrijf { get; set; }
        public string? BedrijfEmail { get; set; }
        public string? FactuurEmail { get; set; }
        public string? AanmaningEmail { get; set; }
        public string? PC { get; set; }
        public string? Plaats { get; set; }
        public string? OnderwerpEmail { get; set; }
        public string? SjabloonEmail { get; set; }
        public string? SjabloonNaam { get; set; }
        public string? SjabloonOmschrijving { get; set; }
        public string? Aannemer { get; set; }
        public string? AannemerAdres { get; set; }
        public string? AannemerPC { get; set; }
        public string? AannemerPlaats { get; set; }
        public string? AannemerLand { get; set; }
        public string? AannemerTelefoon { get; set; }
        public string? AannemerEmail { get; set; }
        public string? Verwerker { get; set; }
        public string? VerwerkerTAV { get; set; }
        public string? VerwerkerGeachte { get; set; }
        public string? VerwerkerAdres { get; set; }
        public string? VerwerkerPCPlaats { get; set; }
        public string? VerwerkerLand { get; set; }
        public string? VerwerkerTelefoon { get; set; }
        public string? OpdrachtAdres { get; set; }
        public decimal? OpdrachtBedrag { get; set; }
        public string? OpdrachtNaam { get; set; }
        public string? OpdrachtNummer { get; set; }
        public string? OpdrachtHuisnr { get; set; }
        public string? OpdrachtPC { get; set; }
        public string? OpdrachtPlaats { get; set; }
        public decimal? ContractBedrag { get; set; }
        public string? ContractLooptijd { get; set; }
        public string? Contractnr { get; set; }
        public DateTime? EindDatumContract { get; set; }
        public string? Factuurmaand { get; set; }
        public string? JaarIndexering { get; set; }
        public DateTime? UitvoerDatum { get; set; }
        public DateTime? UitvoerDatumKort { get; set; }
    }
}