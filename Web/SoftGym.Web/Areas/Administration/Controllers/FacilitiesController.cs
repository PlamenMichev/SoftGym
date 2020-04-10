namespace SoftGym.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Data.Models.Enums;
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

        public async Task<IActionResult> All([FromQuery] string type = null)
        {
            AllFacilitiesViewModel model;
            if (type == null || Enum.TryParse(type, out FacilityType enumType) == false)
            {
                model = new AllFacilitiesViewModel
                {
                    Facilities = await this.facilitiesService.GetAllFacilitiesAsync<FacilityListItemViewModel>(),
                    FacilityType = "all",
                };
            }
            else
            {
                model = new AllFacilitiesViewModel
                {
                    Facilities = await this.facilitiesService.GetAllFacilitiesAsync<FacilityListItemViewModel>(enumType),
                    FacilityType = type,
                };
            }

            return this.View(model);
        }

        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddFacilityInputModel inputModel)
        {
            if (!this.cloudinaryService.IsFileValid(inputModel.PictureFile) || !this.ModelState.IsValid)
            {
                if (!this.cloudinaryService.IsFileValid(inputModel.PictureFile))
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
            if (!this.cloudinaryService.IsFileValid(inputModel.NewPictureFile) || !this.ModelState.IsValid)
            {
                if (!this.cloudinaryService.IsFileValid(inputModel.NewPictureFile))
                {
                    this.ModelState.AddModelError("NewPictureFile", "Plese enter valid file format!");
                }

                return this.View(inputModel);
            }

            var model = await this.facilitiesService.EditFacilityAsync(inputModel);
            return this.Redirect("/Administration/Facilities/All");
        }

        public async Task<IActionResult> Deleted()
        {
            var model = new AllFacilitiesViewModel
            {
                Facilities = await this.facilitiesService.GetDeletedFacilitiesAsync<FacilityListItemViewModel>(),
            };

            return this.View("All", model);
        }

        public async Task<IActionResult> Restore(int facilityId)
        {
            await this.facilitiesService.RestoreFacilityAsync(facilityId);

            return this.Redirect("/Administration/Facilities/Deleted");
        }

        [HttpPost]
        public async Task<IActionResult> HardDelete(int facilityId)
        {
            await this.facilitiesService.HardDeleteFacility(facilityId);

            return this.Redirect("/Administration/Facilities/Deleted");
        }

        public IActionResult FilterFacilities(string facilityType)
        {
            if (facilityType == "all")
            {
                return this.Redirect("/Administration/Facilities/All");
            }

            return this.Redirect($"/Administration/Facilities/All?type={facilityType}");
        }
    }
}
