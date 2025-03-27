using System;

namespace ActielijstApi.Dtos
{
    public class PatchActieDto
    {
        public DateTime? fldMActieGereed { get; set; }
        // Voeg andere velden toe die je wilt ondersteunen, bijv.:
        // public string? FldOmschrijving { get; set; }
    }
}