namespace SoftGym.Web.Areas.Trainers.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Common;
    using SoftGym.Web.Controllers;

    [Authorize(Roles = GlobalConstants.BothRolesRoleName)]
    [Area("Trainers")]
    public class TrainersController : BaseController
    {
    }
}
