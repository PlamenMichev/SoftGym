namespace SoftGym.Web.ViewModels.Exports
{
    using System.Collections.Generic;

    public class ExportInputModel
    {
        public IEnumerable<ExportDayInputModel> Days { get; set; }
    }
}
