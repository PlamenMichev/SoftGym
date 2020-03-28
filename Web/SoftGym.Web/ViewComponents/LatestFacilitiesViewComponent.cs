namespace SoftGym.Web.ViewComponents
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.Administration.Facilities;

    [ViewComponent(Name = "LatestFacilities")]
    public class LatestFacilitiesViewComponent : ViewComponent
    {
        private readonly IFacilitiesService facilitiesService;

        public LatestFacilitiesViewComponent(IFacilitiesService facilitiesService)
        {
            this.facilitiesService = facilitiesService;
        }

        public IViewComponentResult Invoke()
        {
            var viewModel = new LatestFacilitiesViewModel()
            {
                Facilities = this.facilitiesService
                        .GetLatestFacilitiesAsync<FacilityListItemViewModel>().GetAwaiter().GetResult(),
            };

            return this.View(viewModel);
        }
    }
}
