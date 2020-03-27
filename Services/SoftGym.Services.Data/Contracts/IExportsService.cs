namespace SoftGym.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using DocumentFormat.OpenXml.Packaging;
    using SoftGym.Web.ViewModels.Exports;

    public interface IExportsService
    {
        public byte[] Process(ExportInputModel inputModel);

        public Task<ExportInputModel> GetExportModel(string id);

        public byte[] GetBytes();
    }
}
