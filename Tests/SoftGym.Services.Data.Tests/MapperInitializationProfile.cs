namespace SoftGym.Services.Data.Tests
{
    using System.Reflection;

    using AutoMapper;
    using SoftGym.Services.Mapping;

    public class MapperInitializationProfile : Profile
    {
        public MapperInitializationProfile()
        {
            AutoMapperConfig.RegisterMappings(Assembly.GetCallingAssembly());
        }
    }
}
