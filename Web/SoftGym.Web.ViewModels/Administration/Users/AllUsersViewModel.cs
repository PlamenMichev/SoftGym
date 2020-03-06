namespace SoftGym.Web.ViewModels.Administration.Users
{
    using System.Collections.Generic;

    public class AllUsersViewModel
    {
        public IEnumerable<UserListItemViewModel> Users { get; set; }
    }
}
