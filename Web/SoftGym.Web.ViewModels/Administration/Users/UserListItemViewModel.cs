namespace SoftGym.Web.ViewModels.Administration.Users
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Identity;
    using SoftGym.Data.Models;
    using SoftGym.Services.Mapping;

    public class UserListItemViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfilePictureUrl { get; set; }

        public IEnumerable<IdentityUserRole<string>> Roles { get; set; }
    }
}
