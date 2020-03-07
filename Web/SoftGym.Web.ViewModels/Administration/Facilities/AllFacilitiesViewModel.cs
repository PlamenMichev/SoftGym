namespace SoftGym.Web.ViewModels.Administration.Facilities
{
    using System.Collections.Generic;

    public class AllFacilitiesViewModel
    {
        public IEnumerable<FacilityListItemViewModel> Facilities { get; set; }
    }
}
