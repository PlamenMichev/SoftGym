namespace SoftGym.Web.ViewModels.Users
{ 
    using SoftGym.Data.Models;
    using SoftGym.Services.Mapping;

    public class MyCardViewModel : IMapFrom<Card>
    {
        public string Id { get; set; }

        public int Visits { get; set; }

        public string PictureUrl { get; set; }
    }
}
