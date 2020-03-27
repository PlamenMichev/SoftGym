namespace SoftGym.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Services.Data.Contracts;

    public class ExportsController : BaseController
    {
        private readonly IExportsService exportsService;

        public ExportsController(
            IExportsService exportsService)
        {
            this.exportsService = exportsService;
        }

        public async Task<IActionResult> ExportToWord(string id)
        {
            var model = await this.exportsService.GetExportModel(id);

            var document = this.exportsService.Process(model);

            return this.File(document, "application/.docx", "Workout Plan.docx");
        }
    }
}
