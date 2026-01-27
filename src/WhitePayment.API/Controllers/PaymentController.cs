using Microsoft.AspNetCore.Mvc;

namespace WhitePayment.API.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
