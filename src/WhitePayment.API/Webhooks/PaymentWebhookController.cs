using Microsoft.AspNetCore.Mvc;

namespace WhitePayment.API.Webhooks
{
    public class PaymentWebhookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
