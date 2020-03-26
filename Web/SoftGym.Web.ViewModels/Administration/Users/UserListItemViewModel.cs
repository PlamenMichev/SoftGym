namespace SoftGym.Web.ViewModels.Administration.Users
{
    using SoftGym.Data.Models;
    using SoftGym.Services.Mapping;

    public class UserListItemViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfilePictureUrl { get; set; }

        public string CardId { get; set; }
    }
}
