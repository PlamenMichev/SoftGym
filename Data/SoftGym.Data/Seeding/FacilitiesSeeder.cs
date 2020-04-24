namespace SoftGym.Data.Seeding
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using SoftGym.Data.Models;
    using SoftGym.Data.Seeding.Dtos;

    public class FacilitiesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.Facilities.Any())
            {
                var facilities = JsonConvert.DeserializeObject<FacilityDto[]>(File.ReadAllText(@"../../Data/SoftGym.Data/Seeding/Data/Facilities.json"));

                foreach (var facility in facilities)
                {
                    var newFacility = new Facility()
                    {
                        Name = facility.Name,
                        PictureUrl = facility.PictureUrl,
                        Description = facility.Description,
                        Type = facility.Type,
                    };

                    await dbContext.Facilities.AddAsync(newFacility);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
