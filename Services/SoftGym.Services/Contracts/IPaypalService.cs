﻿namespace SoftGym.Services.Contracts
{
    using System.Threading.Tasks;

    using PayPal.Api;

    public interface IPaypalService
    {
        public Task<Payment> CreatePayment(decimal value, int visits);

        public Task<Payment> ExecutePayment(string payerId, string paymentId, string token);
    }
}
