using Microsoft.AspNetCore.Mvc;
using SoftGym.Services.Data.Contracts;
using SoftGym.Web.ViewModels.Administration.Roles;

using System.Threading.Tasks;

namespace SoftGym.Web.Areas.Administration.Controllers
{
    public class RolesController : AdministrationController
    {
        private readonly IRolesService rolesService;

        public RolesController(IRolesService rolesService)
        {
            this.rolesService = rolesService;
        }

        public async Task<IActionResult> Add(ChangeRoleInputModel inputModel)
        {
            await this.rolesService.AddRoleAsync(inputModel.UserId, inputModel.RoleName);
            return this.Redirect("/Administration/Users/All");
        }

        public async Task<IActionResult> Remove(ChangeRoleInputModel inputModel)
        {
            await this.rolesService.RemoveRoleAsync(inputModel.UserId, inputModel.RoleName);
            return this.Redirect("/Administration/Users/All");
        }
    }
}
