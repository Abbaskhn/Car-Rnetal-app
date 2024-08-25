using CRCQRS.Common;
using Microsoft.Extensions.Options;
using Stripe;
namespace CRCQRS.Infrastructure.Services
{

  public interface IStripeService
  {
    Task<Charge> ChargeUser(ChargeRequest request);
  }
  public class StripeService : IStripeService
  {
    private readonly StripeSettings _stripeSettings;

    public StripeService(IOptions<StripeSettings> stripeSettings)
    {
      _stripeSettings = stripeSettings.Value;
      StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
    }
    public async Task<Charge> ChargeUser(ChargeRequest request)
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
      return charge;
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
