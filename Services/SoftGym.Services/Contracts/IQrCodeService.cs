namespace SoftGym.Services.Contracts
{
    using System.Threading.Tasks;

    public interface IQrCodeService
    {
        Task<string> GenerateQrCodeAsync(string textValue);
    }
}
