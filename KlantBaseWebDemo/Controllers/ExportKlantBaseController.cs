using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using KlantBaseWebDemo.Data;

namespace KlantBaseWebDemo.Controllers
{
    public partial class ExportKlantBaseController : ExportController
    {
        private readonly KlantBaseContext context;
        private readonly KlantBaseService service;

        public ExportKlantBaseController(KlantBaseContext context, KlantBaseService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/KlantBase/adres/csv")]
        [HttpGet("/export/KlantBase/adres/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdresToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAdres(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/adres/excel")]
        [HttpGet("/export/KlantBase/adres/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdresToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAdres(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/contactpersonens/csv")]
        [HttpGet("/export/KlantBase/contactpersonens/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportContactpersonensToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetContactpersonens(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/contactpersonens/excel")]
        [HttpGet("/export/KlantBase/contactpersonens/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportContactpersonensToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetContactpersonens(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/correspondenties/csv")]
        [HttpGet("/export/KlantBase/correspondenties/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCorrespondentiesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCorrespondenties(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/correspondenties/excel")]
        [HttpGet("/export/KlantBase/correspondenties/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCorrespondentiesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCorrespondenties(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblmaandens/csv")]
        [HttpGet("/export/KlantBase/stblmaandens/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblMaandensToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStblMaandens(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblmaandens/excel")]
        [HttpGet("/export/KlantBase/stblmaandens/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblMaandensToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStblMaandens(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblselecties/csv")]
        [HttpGet("/export/KlantBase/stblselecties/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblSelectiesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStblSelecties(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblselecties/excel")]
        [HttpGet("/export/KlantBase/stblselecties/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblSelectiesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStblSelecties(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblactiesoorts/csv")]
        [HttpGet("/export/KlantBase/stblactiesoorts/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblActieSoortsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStblActieSoorts(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblactiesoorts/excel")]
        [HttpGet("/export/KlantBase/stblactiesoorts/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblActieSoortsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStblActieSoorts(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblafwerkings/csv")]
        [HttpGet("/export/KlantBase/stblafwerkings/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblAfwerkingsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStblAfwerkings(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblafwerkings/excel")]
        [HttpGet("/export/KlantBase/stblafwerkings/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblAfwerkingsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStblAfwerkings(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblbelstatuses/csv")]
        [HttpGet("/export/KlantBase/stblbelstatuses/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblBelStatusesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStblBelStatuses(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblbelstatuses/excel")]
        [HttpGet("/export/KlantBase/stblbelstatuses/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblBelStatusesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStblBelStatuses(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblbooks/csv")]
        [HttpGet("/export/KlantBase/stblbooks/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblBooksToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStblBooks(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblbooks/excel")]
        [HttpGet("/export/KlantBase/stblbooks/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblBooksToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStblBooks(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblcorrespondentiefields/csv")]
        [HttpGet("/export/KlantBase/stblcorrespondentiefields/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblCorrespondentieFieldsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStblCorrespondentieFields(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblcorrespondentiefields/excel")]
        [HttpGet("/export/KlantBase/stblcorrespondentiefields/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblCorrespondentieFieldsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStblCorrespondentieFields(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stbldocumentsoorts/csv")]
        [HttpGet("/export/KlantBase/stbldocumentsoorts/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblDocumentSoortsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStblDocumentSoorts(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stbldocumentsoorts/excel")]
        [HttpGet("/export/KlantBase/stbldocumentsoorts/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblDocumentSoortsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStblDocumentSoorts(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblfabrikants/csv")]
        [HttpGet("/export/KlantBase/stblfabrikants/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblFabrikantsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStblFabrikants(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblfabrikants/excel")]
        [HttpGet("/export/KlantBase/stblfabrikants/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblFabrikantsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStblFabrikants(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblfactureermethodes/csv")]
        [HttpGet("/export/KlantBase/stblfactureermethodes/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblFactureermethodesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStblFactureermethodes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblfactureermethodes/excel")]
        [HttpGet("/export/KlantBase/stblfactureermethodes/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblFactureermethodesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStblFactureermethodes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblfactuurkortings/csv")]
        [HttpGet("/export/KlantBase/stblfactuurkortings/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblFactuurKortingsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStblFactuurKortings(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblfactuurkortings/excel")]
        [HttpGet("/export/KlantBase/stblfactuurkortings/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblFactuurKortingsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStblFactuurKortings(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblglobals/csv")]
        [HttpGet("/export/KlantBase/stblglobals/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblGlobalsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStblGlobals(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblglobals/excel")]
        [HttpGet("/export/KlantBase/stblglobals/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblGlobalsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStblGlobals(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblmeldsoorts/csv")]
        [HttpGet("/export/KlantBase/stblmeldsoorts/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblMeldsoortsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStblMeldsoorts(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblmeldsoorts/excel")]
        [HttpGet("/export/KlantBase/stblmeldsoorts/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblMeldsoortsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStblMeldsoorts(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblopdrachtcategories/csv")]
        [HttpGet("/export/KlantBase/stblopdrachtcategories/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblOpdrachtCategoriesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStblOpdrachtCategories(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblopdrachtcategories/excel")]
        [HttpGet("/export/KlantBase/stblopdrachtcategories/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblOpdrachtCategoriesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStblOpdrachtCategories(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblpriorities/csv")]
        [HttpGet("/export/KlantBase/stblpriorities/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblPrioritiesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStblPriorities(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblpriorities/excel")]
        [HttpGet("/export/KlantBase/stblpriorities/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblPrioritiesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStblPriorities(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblrelatiesoorts/csv")]
        [HttpGet("/export/KlantBase/stblrelatiesoorts/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblRelatieSoortsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStblRelatieSoorts(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblrelatiesoorts/excel")]
        [HttpGet("/export/KlantBase/stblrelatiesoorts/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblRelatieSoortsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStblRelatieSoorts(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblreplacefields/csv")]
        [HttpGet("/export/KlantBase/stblreplacefields/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblReplaceFieldsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStblReplaceFields(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblreplacefields/excel")]
        [HttpGet("/export/KlantBase/stblreplacefields/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblReplaceFieldsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStblReplaceFields(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblreplacefieldsnews/csv")]
        [HttpGet("/export/KlantBase/stblreplacefieldsnews/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblReplaceFieldsNewsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStblReplaceFieldsNews(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblreplacefieldsnews/excel")]
        [HttpGet("/export/KlantBase/stblreplacefieldsnews/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblReplaceFieldsNewsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStblReplaceFieldsNews(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblselectiecodes/csv")]
        [HttpGet("/export/KlantBase/stblselectiecodes/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblSelectieCodesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStblSelectieCodes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblselectiecodes/excel")]
        [HttpGet("/export/KlantBase/stblselectiecodes/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblSelectieCodesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStblSelectieCodes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblstatuses/csv")]
        [HttpGet("/export/KlantBase/stblstatuses/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblStatusesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStblStatuses(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblstatuses/excel")]
        [HttpGet("/export/KlantBase/stblstatuses/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblStatusesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStblStatuses(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblsysteems/csv")]
        [HttpGet("/export/KlantBase/stblsysteems/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblSysteemsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStblSysteems(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stblsysteems/excel")]
        [HttpGet("/export/KlantBase/stblsysteems/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblSysteemsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStblSysteems(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stbltrainingssoorts/csv")]
        [HttpGet("/export/KlantBase/stbltrainingssoorts/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblTrainingsSoortsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStblTrainingsSoorts(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stbltrainingssoorts/excel")]
        [HttpGet("/export/KlantBase/stbltrainingssoorts/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStblTrainingsSoortsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStblTrainingsSoorts(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stucadoors/csv")]
        [HttpGet("/export/KlantBase/stucadoors/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStucadoorsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStucadoors(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/stucadoors/excel")]
        [HttpGet("/export/KlantBase/stucadoors/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStucadoorsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStucadoors(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblfactregels/csv")]
        [HttpGet("/export/KlantBase/tblfactregels/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblFactRegelsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTblFactRegels(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblfactregels/excel")]
        [HttpGet("/export/KlantBase/tblfactregels/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblFactRegelsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTblFactRegels(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblfaktuurs/csv")]
        [HttpGet("/export/KlantBase/tblfaktuurs/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblFaktuursToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTblFaktuurs(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblfaktuurs/excel")]
        [HttpGet("/export/KlantBase/tblfaktuurs/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblFaktuursToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTblFaktuurs(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblglobals/csv")]
        [HttpGet("/export/KlantBase/tblglobals/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblGlobalsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTblGlobals(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblglobals/excel")]
        [HttpGet("/export/KlantBase/tblglobals/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblGlobalsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTblGlobals(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblkeurings/csv")]
        [HttpGet("/export/KlantBase/tblkeurings/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblKeuringsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTblKeurings(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblkeurings/excel")]
        [HttpGet("/export/KlantBase/tblkeurings/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblKeuringsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTblKeurings(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblmemos/csv")]
        [HttpGet("/export/KlantBase/tblmemos/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblMemosToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTblMemos(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblmemos/excel")]
        [HttpGet("/export/KlantBase/tblmemos/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblMemosToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTblMemos(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblprojectens/csv")]
        [HttpGet("/export/KlantBase/tblprojectens/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblProjectensToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTblProjectens(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblprojectens/excel")]
        [HttpGet("/export/KlantBase/tblprojectens/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblProjectensToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTblProjectens(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblprojectm2jaars/csv")]
        [HttpGet("/export/KlantBase/tblprojectm2jaars/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblProjectM2jaarsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTblProjectM2jaars(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblprojectm2jaars/excel")]
        [HttpGet("/export/KlantBase/tblprojectm2jaars/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblProjectM2jaarsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTblProjectM2jaars(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblprojectnrs/csv")]
        [HttpGet("/export/KlantBase/tblprojectnrs/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblProjectNrsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTblProjectNrs(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblprojectnrs/excel")]
        [HttpGet("/export/KlantBase/tblprojectnrs/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblProjectNrsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTblProjectNrs(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblprojectonderdelens/csv")]
        [HttpGet("/export/KlantBase/tblprojectonderdelens/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblProjectOnderdelensToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTblProjectOnderdelens(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblprojectonderdelens/excel")]
        [HttpGet("/export/KlantBase/tblprojectonderdelens/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblProjectOnderdelensToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTblProjectOnderdelens(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblrelatiepartijens/csv")]
        [HttpGet("/export/KlantBase/tblrelatiepartijens/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblRelatiePartijensToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTblRelatiePartijens(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblrelatiepartijens/excel")]
        [HttpGet("/export/KlantBase/tblrelatiepartijens/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblRelatiePartijensToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTblRelatiePartijens(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblsettings/csv")]
        [HttpGet("/export/KlantBase/tblsettings/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblSettingsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTblSettings(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblsettings/excel")]
        [HttpGet("/export/KlantBase/tblsettings/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblSettingsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTblSettings(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblsoortprojects/csv")]
        [HttpGet("/export/KlantBase/tblsoortprojects/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblSoortProjectsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTblSoortProjects(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblsoortprojects/excel")]
        [HttpGet("/export/KlantBase/tblsoortprojects/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblSoortProjectsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTblSoortProjects(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblstandaarddocs/csv")]
        [HttpGet("/export/KlantBase/tblstandaarddocs/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblStandaardDocsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTblStandaardDocs(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/tblstandaarddocs/excel")]
        [HttpGet("/export/KlantBase/tblstandaarddocs/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblStandaardDocsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTblStandaardDocs(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/werknemers/csv")]
        [HttpGet("/export/KlantBase/werknemers/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportWerknemersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetWerknemers(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/werknemers/excel")]
        [HttpGet("/export/KlantBase/werknemers/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportWerknemersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetWerknemers(), Request.Query, false), fileName);
        }

        [HttpGet("/export/KlantBase/vwaankomendeinspecties/csv")]
        [HttpGet("/export/KlantBase/vwaankomendeinspecties/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportVwAankomendeInspectiesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetVwAankomendeInspecties(), Request.Query, true), fileName);
        }

        [HttpGet("/export/KlantBase/vwaankomendeinspecties/excel")]
        [HttpGet("/export/KlantBase/vwaankomendeinspecties/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportVwAankomendeInspectiesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetVwAankomendeInspecties(), Request.Query, true), fileName);
        }

        [HttpGet("/export/KlantBase/vwkiwainspecties/csv")]
        [HttpGet("/export/KlantBase/vwkiwainspecties/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportVwKiwainspectiesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetVwKiwainspecties(), Request.Query, true), fileName);
        }

        [HttpGet("/export/KlantBase/vwkiwainspecties/excel")]
        [HttpGet("/export/KlantBase/vwkiwainspecties/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportVwKiwainspectiesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetVwKiwainspecties(), Request.Query, true), fileName);
        }
    }
}
