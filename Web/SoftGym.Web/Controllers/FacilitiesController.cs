namespace SoftGym.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.Administration.Facilities;

    public class FacilitiesController : BaseController
    {
        private readonly IFacilitiesService facilitiesService;

        public FacilitiesController(IFacilitiesService facilitiesService)
        {
            this.facilitiesService = facilitiesService;
        }

        public async Task<IActionResult> All()
        {
            var model = new AllFacilitiesViewModel
            {
                Facilities = await this.facilitiesService.GetAllFacilitiesAsync<FacilityListItemViewModel>(),
            };

            return this.View(model);
        }

        public async Task<IActionResult> Equipments()
        {
            var model = new AllFacilitiesViewModel
            {
                Facilities = await this.facilitiesService.GetAllFacilitiesAsync<FacilityListItemViewModel>(FacilityType.Equipment),
            };

            return this.View("All", model);
        }

        public async Task<IActionResult> Spa()
        {
            var model = new AllFacilitiesViewModel
            {
                Facilities = await this.facilitiesService.GetAllFacilitiesAsync<FacilityListItemViewModel>(FacilityType.Spa),
            };

            return this.View("All", model);
        }

        public async Task<IActionResult> Rooms()
        {
            var model = new AllFacilitiesViewModel
            {
                Facilities = await this.facilitiesService.GetAllFacilitiesAsync<FacilityListItemViewModel>(FacilityType.Room),
            };

            return this.View("All", model);
        }
    }
}
