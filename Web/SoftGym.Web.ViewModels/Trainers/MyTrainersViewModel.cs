namespace SoftGym.Web.ViewModels.Trainers
{
    using System.Collections.Generic;

    public class MyTrainersViewModel
    {
        public IEnumerable<MyTrainerViewModel> Trainers { get; set; }

        public IEnumerable<MyClientViewModel> Clients { get; set; }
    }
}
