namespace SoftGym.Web.Areas.Trainers.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : TrainersController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
