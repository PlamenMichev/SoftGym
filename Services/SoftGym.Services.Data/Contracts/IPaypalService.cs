namespace SoftGym.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using PayPal.Api;

    public interface IPaypalService
    {
        public Task<Payment> CreatePayment();

        public Task<Payment> ExecutePayment(string payerId, string paymentId, string token);
    }
}
