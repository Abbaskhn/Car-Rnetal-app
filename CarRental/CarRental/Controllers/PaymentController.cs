using application.model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {


        private readonly StripeSettings _stripeSettings;

        public PaymentController(IOptions<StripeSettings> stripeSettings)
        {
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        }

        [HttpPost("charge")]
        public async Task<IActionResult> Charge([FromBody] ChargeRequest request)
        {
            var options = new ChargeCreateOptions
            {
                Amount = request.Amount,
                Currency = "usd",
                Description = request.Description,
                Source = request.StripeToken,
                ReceiptEmail = request.Email,
                Metadata = new Dictionary<string, string>
                {
                    { "CustomerName", request.Name },
                    { "CustomerEmail", request.Email }
                }
            };

            var service = new ChargeService();
            Charge charge = await service.CreateAsync(options);

            return Ok(charge);
        }
    }

    public class ChargeRequest
    {
        public string StripeToken { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public long Amount { get; set; } // Amount in cents
        public string Description { get; set; }
    }
}