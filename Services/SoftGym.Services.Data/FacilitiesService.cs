namespace SoftGym.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SoftGym.Data.Common.Repositories;
    using SoftGym.Data.Models;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Services.Mapping;
    using SoftGym.Web.ViewModels.Administration.Facilities;

    public class FacilitiesService : IFacilitiesService
    {
        private readonly IDeletableEntityRepository<Facility> facilityRepository;
        private readonly ICloudinaryService cloudinaryService;

        public FacilitiesService(
            IDeletableEntityRepository<Facility> facilityRepository,
            ICloudinaryService cloudinaryService)
        {
            this.facilityRepository = facilityRepository;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<Facility> AddFacilityAsync(AddFacilityInputModel inputModel)
        {
            var facility = new Facility()
            {
                Name = inputModel.Name,
                Description = inputModel.Description,
                PictureUrl = await this.cloudinaryService.UploudAsync(inputModel.PictureFile),
                Type = inputModel.Type,
            };

            await this.facilityRepository.AddAsync(facility);
            await this.facilityRepository.SaveChangesAsync();

            return facility;
        }

        public async Task<Facility> DeleteFacilityAsync(int facilityId)
        {
            Facility facility = await this.facilityRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == facilityId);
            this.facilityRepository.Delete(facility);
            await this.facilityRepository.SaveChangesAsync();

            return facility;
        }

        public async Task<Facility> EditFacilityAsync(EditViewModel inputModel)
        {
            var currentFacility = await this.facilityRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == inputModel.Id);
            currentFacility.Name = inputModel.Name;
            if (inputModel.NewPictureFile != null)
            {
                currentFacility.PictureUrl = await this.cloudinaryService.UploudAsync(inputModel.NewPictureFile);
            }

            currentFacility.Type = inputModel.Type;
            currentFacility.Description = inputModel.Description;
            await this.facilityRepository.SaveChangesAsync();

            return currentFacility;
        }

        public async Task<IEnumerable<T>> GetAllFacilitiesAsync<T>(FacilityType? type = null)
        {
            if (type == null)
            {
                return await this.facilityRepository
                                .All()
                                .To<T>()
                                .ToListAsync();
            }
            else
            {
                return await this.facilityRepository
                                .All()
                                .Where(x => x.Type == type)
                                .To<T>()
                                .ToListAsync();
            }
        }

        public async Task<IEnumerable<T>> GetDeletedFacilitiesAsync<T>()
        {
            return await this.facilityRepository
                .AllWithDeleted()
                .Where(x => x.IsDeleted == true)
                .To<T>()
                .ToListAsync();
        }

        public async Task<int> GetFacilitiesCountAsync()
        {
            return await this.facilityRepository
                .All()
                .CountAsync();
        }

        public async Task<T> GetFacilityAsync<T>(int id)
        {
            return await this.facilityRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstAsync();
        }

        public async Task HardDeleteFacility(int facilityId)
        {
            var currentFacility = await this.facilityRepository
                .AllWithDeleted()
                .FirstOrDefaultAsync(x => x.Id == facilityId);

            this.facilityRepository.HardDelete(currentFacility);
            await this.facilityRepository.SaveChangesAsync();
        }

        public async Task<Facility> RestoreFacilityAsync(int facilityId)
        {
            var currentFacility = await this.facilityRepository
                .AllWithDeleted()
                .FirstOrDefaultAsync(x => x.Id == facilityId);

            currentFacility.IsDeleted = false;
            await this.facilityRepository.SaveChangesAsync();

            return currentFacility;
        }
    }
}
