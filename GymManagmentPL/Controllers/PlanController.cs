using GymManagementBLL.BusinessServices.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Unit_Of_Work;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        public IActionResult Index()
        {
            var plans = _planService.GetAllPlans();
            return View(plans);
        }

        #region Details Action
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be negative or zero";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanDetails(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }
        #endregion

        #region Edit Action

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be negative or zero";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanToUpdate(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }

        [HttpPost]
        // Same name, so i can take first parameter from route
        public ActionResult Edit([FromRoute] int id, PlanToUpdateViewModel plan)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Data Validation");
                return View(plan);
            }

            var result = _planService.UpdatePlan(id, plan);
            if (result)
                TempData["SuccessMessage"] = "Plan Updated Successfully";
            else
                TempData["ErrorMessage"] = "Plan Falid To Update";
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Activation

        [HttpPost]
        public ActionResult Activate(int id)
        {
            var result = _planService.ToggleStatus(id);
            if (result)
                TempData["SuccessMessage"] = "Plan Toggeld Successfully";
            else
                TempData["ErrorMessage"] = "Plan Falid To Toggle";
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
