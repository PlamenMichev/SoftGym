namespace SoftGym.Web.ViewModels.Users
{
    using SoftGym.Data.Models;
    using SoftGym.Services.Mapping;

    public class ChatDataViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfilePictureUrl { get; set; }
    }
}
