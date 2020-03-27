namespace SoftGym.Web.ViewModels.Exports
{
    using System.Collections.Generic;

    public class ExportDayInputModel
    {
        public string Day { get; set; }

        public IEnumerable<ExportExerciseInputModel> Exercises { get; set; }
    }
}
