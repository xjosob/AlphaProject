using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class OverviewController : Controller
    {
        [Authorize]
        [Route("admin/overview")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
