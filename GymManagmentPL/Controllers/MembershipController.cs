using GymManagementBLL.BusinessServices.Interfaces;
using GymManagementBLL.ViewModels.MembershipViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace GymManagmentPL.Controllers
{
    public class MembershipController : Controller
    {
        private readonly IMembershipService _membershipService;

        public MembershipController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        public IActionResult Index()
        {
            var data = _membershipService.GetAllMemberships();
            return View(data);
        }

        #region Creation

        public ActionResult Create()
        {
            LoadMemebrsAndPlans();
            return View();
        }

        [HttpPost]
        public ActionResult Create(MembershipToCreateViewModel membership)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "This Is Missing Fields";
                return View(nameof(Create), membership);
            }

            var result = _membershipService.CreateMembership(membership);
            if (result)
            {
                TempData["SuccessMessage"] = "Membership Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Membership Failed To Create";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Cancelation

        [HttpPost]
        public ActionResult Cancel(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be negative or zero";
                return RedirectToAction(nameof(Index));
            }

            var result = _membershipService.CancelMembership(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Membership Canceled Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Membership Already Canceled!";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Helper Methods
        private void LoadMemebrsAndPlans()
        {
            var members = _membershipService.GetAllAvialableMembers();
            var plans = _membershipService.GetAllAvialablePlans();

            ViewBag.Members = new SelectList(members, "Id", "Name");
            ViewBag.Plans = new SelectList(plans, "Id", "Name");
        }
        #endregion
    }
}
