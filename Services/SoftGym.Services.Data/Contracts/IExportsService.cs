namespace SoftGym.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using SoftGym.Web.ViewModels.Exports;

    public interface IExportsService
    {
        public byte[] ProcessWordDocument(ExportInputModel inputModel);

        public Task<ExportInputModel> GetExportModel(string id);
    }
}
