namespace SoftGym.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.Administration.Facilities;

    [AllowAnonymousAttribute]
    public class FacilitiesController : BaseController
    {
        private readonly IFacilitiesService facilitiesService;

        public FacilitiesController(IFacilitiesService facilitiesService)
        {
            this.facilitiesService = facilitiesService;
        }

        public async Task<IActionResult> All(int page = 1, FacilityType? type = null)
        {
            var facilitiesCount = await this.facilitiesService.GetFacilitiesCountAsync(type);
            var pages = facilitiesCount / 6;
            if (facilitiesCount % 6 != 0 || pages == 0)
            {
                pages++;
            }

            if (page <= 0 || page > pages)
            {
                return this.NotFound();
            }

            if (page == 0)
            {
                page = 1;
            }

            var model = new AllFacilitiesViewModel();
            if (type == null)
            {
                model.Facilities = await this.facilitiesService.GetSomeFacilitiesAsync<FacilityListItemViewModel>(page);
            }
            else
            {
                model.Facilities = await this.facilitiesService.GetSomeFacilitiesAsync<FacilityListItemViewModel>(page, 6, type);
            }

            model.Pages = pages;
            model.CurrentPage = page;

            return this.View(model);
        }
    }
}
