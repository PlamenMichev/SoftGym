namespace SoftGym.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using SoftGym.Data.Models;

    public interface ICardService
    {
        Task<Card> GenerateCardAsync(ApplicationUser user);
    }
}
