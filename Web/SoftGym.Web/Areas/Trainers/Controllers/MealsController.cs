namespace SoftGym.Web.Areas.Trainers.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.Trainers.Meals;
    using System.Threading.Tasks;

    public class MealsController : TrainersController
    {
        private readonly IMealsService mealsService;
        private readonly ICloudinaryService cloudinaryService;

        public MealsController(IMealsService mealsService,
            ICloudinaryService cloudinaryService)
        {
            this.mealsService = mealsService;
            this.cloudinaryService = cloudinaryService;
        }

        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddMealInputModel inputModel)
        {
            if (!this.cloudinaryService.IsFileValid(inputModel.PictureFile) || !this.ModelState.IsValid)
            {
                if (!this.cloudinaryService.IsFileValid(inputModel.PictureFile))
                {
                    this.ModelState.AddModelError("PictureFile", "Plese enter valid file format!");
                }
                return this.View(inputModel);
            }

            await this.mealsService.AddMealAsync(inputModel);
            return this.Redirect("/Trainers/Dashboard/Index");
        }
    }
}
