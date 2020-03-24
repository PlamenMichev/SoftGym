namespace SoftGym.Web.ViewModels.Administration.Cards
{
    using SoftGym.Data.Models;
    using SoftGym.Services.Mapping;

    public class UserCardViewModel : IMapFrom<Card>
    {
        public string UserId { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string Id { get; set; }

        public int Visits { get; set; }

        public string PictureUrl { get; set; }
    }
}
