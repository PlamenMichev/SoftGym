namespace SoftGym.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using PayPal.Api;
    using SoftGym.Services.Contracts;

    public class PaypalService : IPaypalService
    {
        private readonly IConfiguration configuration;

        public PaypalService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<Payment> CreatePayment(decimal value, int visits)
        {
            var config = new Dictionary<string, string>();
            config.Add("mode", this.configuration["Logging:Paypal:Mode"]);
            config.Add("clientId", this.configuration["Logging:Paypal:ClientId"]);
            config.Add("clientSecret", this.configuration["Logging:Paypal:ClientSecret"]);

            var accessToken = new OAuthTokenCredential(config).GetAccessToken();

            var apiContext = new APIContext(accessToken)
            {
                Config = config,
            };

            try
            {
                Payment payment = new Payment
                {
                    intent = "sale",
                    payer = new Payer { payment_method = "paypal" },
                    transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            payee = new Payee
                            {
                                email = "sb-aubpt1189410@business.example.com",
                            },
                            amount = new Amount
                            {
                                currency = "USD",
                                total = value.ToString(),
                            },
                            description = "Buying card for fitness entrance in softgym.",
                        },
                    },
                    redirect_urls = new RedirectUrls
                    {
                        cancel_url = @"https://localhost:44319/Paypal/FailedPayment",
                        return_url = $@"https://localhost:44319/Paypal/SuccessedPayment?visits={visits}",
                    },
                };

                var createdPayment = await Task.Run(() => payment.Create(apiContext));
                return createdPayment;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Payment> ExecutePayment(string payerId, string paymentId, string token)
        {
            var config = new Dictionary<string, string>();
            config.Add("mode", this.configuration["Logging:Paypal:Mode"]);
            config.Add("clientId", this.configuration["Logging:Paypal:ClientId"]);
            config.Add("clientSecret", this.configuration["Logging:Paypal:ClientSecret"]);

            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            var apiContext = new APIContext(accessToken)
            {
                Config = config,
            };

            PaymentExecution paymentExecution = new PaymentExecution() { payer_id = payerId };
            Payment payment = new Payment() { id = paymentId };
            Payment executedPayment = await Task.Run(() => payment.Execute(apiContext, paymentExecution));

            return executedPayment;
        }
    }
}
