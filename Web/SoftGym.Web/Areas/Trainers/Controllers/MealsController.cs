namespace SoftGym.Web.Areas.Trainers.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.Trainers.Meals;

    public class MealsController : TrainersController
    {
        private readonly IMealsService mealsService;
        private readonly ICloudinaryService cloudinaryService;

        public MealsController(
            IMealsService mealsService,
            ICloudinaryService cloudinaryService)
        {
            this.mealsService = mealsService;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<IActionResult> All()
        {
            var viewModel = new AllMealsViewModel
            {
                Meals = await this.mealsService.GetMealsAsync<MealViewModel>(),
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Details(string id)
        {
            var viewModel = await this.mealsService.GetMealAsync<MealViewModel>(id);
            return this.View(viewModel);
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
                    this.ModelState.AddModelError("PictureFile", "Plese enter valid image format!");
                }

                return this.View(inputModel);
            }

            var meal = await this.mealsService.AddMealAsync(inputModel);
            return this.Redirect($"/Trainers/Meals/Details/{meal.Id}");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await this.mealsService.Delete(id);

            return this.Redirect("/Trainers/Meals/All");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var model = await this.mealsService.GetMealAsync<EditMealInputModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditMealInputModel inputModel)
        {
            if (!this.cloudinaryService.IsFileValid(inputModel.NewImageFile) || !this.ModelState.IsValid)
            {
                if (!this.cloudinaryService.IsFileValid(inputModel.NewImageFile))
                {
                    this.ModelState.AddModelError("PictureFile", "Plese enter valid file format!");
                }

                return this.View(inputModel);
            }

            var meal = await this.mealsService.EditMealAsync(inputModel);
            return this.Redirect($"/Trainers/Meals/Details/{meal.Id}");
        }
    }
}
