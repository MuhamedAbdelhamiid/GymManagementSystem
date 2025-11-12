using System.Diagnostics;
using GymManagementBLL.BusinessServices.Interfaces;
using GymManagmentPL.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnaltyicalService _analtyicalService;

        public HomeController(IAnaltyicalService analtyicalService)
        {
            _analtyicalService = analtyicalService;
        }

        public ActionResult Index()
        {
            var data = _analtyicalService.GetHomeAnaltyicalService();
            return View(data);
        }
    }
}
