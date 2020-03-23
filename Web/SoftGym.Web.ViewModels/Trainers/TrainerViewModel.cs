namespace SoftGym.Web.ViewModels.Trainers
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using SoftGym.Data.Models;
    using SoftGym.Services.Mapping;

    public class TrainerViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfilePictureUrl { get; set; }

        public string Description { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public ICollection<string> ClientIds { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, TrainerViewModel>().ForMember(
                m => m.ClientIds,
                opt => opt.MapFrom(x => x.Clients.Select(x => x.ClientId)));
        }
    }
}
