namespace SoftGym.Web.Areas.Trainers.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Services.Contracts;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.Trainers.Exercises;

    public class ExercisesController : TrainersController
    {
        private readonly IExercisesService exercisesService;
        private readonly ICloudinaryService cloudinaryService;

        public ExercisesController(
            IExercisesService exercisesService,
            ICloudinaryService cloudinaryService)
        {
            this.exercisesService = exercisesService;
            this.cloudinaryService = cloudinaryService;
        }

        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddExerciseInputModel inputModel)
        {
            if (!this.ModelState.IsValid || !this.cloudinaryService.IsVideoFileValid(inputModel.VideoFile))
            {
                if (!this.cloudinaryService.IsVideoFileValid(inputModel.VideoFile))
                {
                    this.ModelState.AddModelError("VideoFile", "Please enter a valid video format!");
                }

                return this.View(inputModel);
            }

            var exercise = await this.exercisesService.AddExerciseAsync(inputModel);
            return this.Redirect($"/Trainers/Exercises/Details/{exercise.Id}");
        }

        public async Task<IActionResult> All([FromQuery] string type = null)
        {
            AllExercisesViewModel viewModel;
            if (type == null)
            {
                viewModel = new AllExercisesViewModel
                {
                    Exercises = await this.exercisesService.GetAllExercisesAsync<ExerciseViewModel>(),
                    ExerciseType = "All",
                };
            }
            else
            {
                viewModel = new AllExercisesViewModel
                {
                    Exercises = await this.exercisesService.GetAllExercisesAsync<ExerciseViewModel>(type),
                    ExerciseType = type,
                };
            }

            return this.View(viewModel);
        }

        public async Task<IActionResult> Details(string id)
        {
            var viewModel = await this.exercisesService.GetExerciseAsync<ExerciseViewModel>(id);
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await this.exercisesService.DeleteExerciseAsync(id);
            return this.Redirect("/Trainers/Exercises/All");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var viewModel = await this.exercisesService.GetExerciseAsync<EditExerciseInputModel>(id);
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditExerciseInputModel inputModel)
        {
            if (!this.ModelState.IsValid || !this.cloudinaryService.IsVideoFileValid(inputModel.VideoFile))
            {
                if (!this.cloudinaryService.IsVideoFileValid(inputModel.VideoFile))
                {
                    this.ModelState.AddModelError("VideoFile", "Please enter a valid video format!");
                }

                return this.View(inputModel);
            }

            await this.exercisesService.EditExerciseAsync(inputModel);
            return this.Redirect($"/Trainers/Exercises/Details/{inputModel.Id}");
        }

        [HttpPost]
        public IActionResult FilterExercises(AllExercisesViewModel inputModel)
        {
            if (inputModel.ExerciseType.ToLower() == "all")
            {
                return this.Redirect("/Trainers/Exercises/All");
            }

            return this.Redirect($"/Trainers/Exercises/All?type={inputModel.ExerciseType}");
        }
    }
}
