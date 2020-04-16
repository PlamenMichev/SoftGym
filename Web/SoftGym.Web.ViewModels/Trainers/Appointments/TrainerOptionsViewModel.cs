namespace SoftGym.Web.ViewModels.Trainers.Appointments
{
    using SoftGym.Data.Models;
    using SoftGym.Services.Mapping;

    public class TrainerOptionsViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
