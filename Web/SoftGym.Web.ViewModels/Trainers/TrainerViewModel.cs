namespace SoftGym.Web.ViewModels.Trainers
{
    using SoftGym.Data.Models;
    using SoftGym.Services.Mapping;

    public class TrainerViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfilePictureUrl { get; set; }

        public string Description { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
