namespace SoftGym.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Common;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.Administration.Facilities;

    public class FacilitiesController : AdministrationController
    {
        private readonly IFacilitiesService facilitiesService;
        private readonly ICloudinaryService cloudinaryService;

        public FacilitiesController(
            IFacilitiesService facilitiesService,
            ICloudinaryService cloudinaryService)
        {
            this.facilitiesService = facilitiesService;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<IActionResult> All()
        {
            var model = new AllFacilitiesViewModel
            {
                Facilities = await this.facilitiesService.GetAllFacilitiesAsync<FacilitiListItemViewModel>(),
            };

            return this.View(model);
        }

        public async Task<IActionResult> Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddFacilityInputModel inputModel)
        {
            if (!this.cloudinaryService.IsFileValid(inputModel?.PictureFile) || !this.ModelState.IsValid)
            {
                if (!this.cloudinaryService.IsFileValid(inputModel?.PictureFile))
                {
                    this.ModelState.AddModelError("PictureFile", "Plese enter valid file format!");
                }

                return this.View(inputModel);
            }

            await this.facilitiesService.AddFacilityAsync(inputModel);
            return this.Redirect("/Administration/Facilities/All");
        }

        public async Task<IActionResult> Delete(int facilityId)
        {
            await this.facilitiesService.DeleteFacilityAsync(facilityId);
            return this.Redirect("/Administration/Facilities/All");
        }

        public async Task<IActionResult> Edit(int facilityId)
        {
            var model = await this.facilitiesService.GetFacilityAsync<EditViewModel>(facilityId);
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel inputModel)
        {
            if (!this.cloudinaryService.IsFileValid(inputModel?.NewPictureFile) || !this.ModelState.IsValid)
            {
                if (!this.cloudinaryService.IsFileValid(inputModel?.NewPictureFile))
                {
                    this.ModelState.AddModelError("PictureFile", "Plese enter valid file format!");
                }

                return this.View(inputModel);
            }

            var model = await this.facilitiesService.EditFacilityAsync(inputModel);
            return this.Redirect("/Administration/Facilities/All");
        }
    }
}
