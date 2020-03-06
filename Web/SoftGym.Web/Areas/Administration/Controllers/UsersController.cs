namespace SoftGym.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.Administration.Users;

    public class UsersController : AdministrationController
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public async Task<IActionResult> All()
        {
            var model = new AllUsersViewModel
            {
                Users = await this.usersService.GetAllUsersAsync<UserListItemViewModel>(),
            };

            return this.View(model);
        }
    }
}
