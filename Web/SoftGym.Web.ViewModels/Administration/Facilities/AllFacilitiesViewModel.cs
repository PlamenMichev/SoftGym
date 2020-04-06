namespace SoftGym.Web.ViewModels.Administration.Facilities
{
    using System.Collections.Generic;

    public class AllFacilitiesViewModel
    {
        public IEnumerable<FacilityListItemViewModel> Facilities { get; set; }

        public string FacilityType { get; set; }

        public int CurrentPage { get; set; }

        public int Pages { get; set; }
    }
}
