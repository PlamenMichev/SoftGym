﻿namespace SoftGym.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SoftGym.Data.Models;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Web.ViewModels.Administration.Facilities;

    public interface IFacilitiesService
    {
        public Task<IEnumerable<T>> GetAllFacilitiesAsync<T>(FacilityType? type = null);

        public Task<Facility> AddFacilityAsync(AddFacilityInputModel inputModel);

        public Task<Facility> DeleteFacilityAsync(int facilityId);

        public Task<T> GetFacilityAsync<T>(int id);

        public Task<Facility> EditFacilityAsync(EditViewModel inputModel);

        public Task<IEnumerable<T>> GetDeletedFacilitiesAsync<T>();

        public Task<Facility> RestoreFacilityAsync(int facilityId);

        public Task HardDeleteFacility(int facilityId);

        public Task<int> GetFacilitiesCountAsync();
    }
}
