namespace SoftGym.Data.Seeding.Dtos
{
    using SoftGym.Data.Models.Enums;

    public class FacilityDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string PictureUrl { get; set; }

        public FacilityType Type { get; set; }
    }
}
