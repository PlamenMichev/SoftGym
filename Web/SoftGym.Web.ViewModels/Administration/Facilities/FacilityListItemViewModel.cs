namespace SoftGym.Web.ViewModels.Administration.Facilities
{
    using SoftGym.Data.Models;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Services.Mapping;

    public class FacilityListItemViewModel : IMapFrom<Facility>
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public string Name { get; set; }

        public string PictureUrl { get; set; }

        public string Description { get; set; }

        public FacilityType Type { get; set; }
    }
}
