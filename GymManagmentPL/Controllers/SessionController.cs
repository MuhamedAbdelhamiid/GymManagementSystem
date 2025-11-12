using GymManagementBLL.BusinessServices.Implementation;
using GymManagementBLL.BusinessServices.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagmentPL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public IActionResult Index()
        {
            var sessions = _sessionService.GetAllSession();
            return View(sessions);
        }

        #region Details
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be negative or zero";
                return RedirectToAction(nameof(Index));
            }

            var sessionDetails = _sessionService.GetSessionDetails(id);
            if (sessionDetails is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(sessionDetails);
        }
        #endregion

        #region Creation
        public ActionResult Create()
        {
            LoadTrainerAndCategory();
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateSessionViewModel session)
        {
            if (!ModelState.IsValid)
            {
                LoadTrainerAndCategory();
                return RedirectToAction(nameof(Create));
            }

            var result = _sessionService.CreateSession(session);
            if (result)
                TempData["SuccessMessage"] = "Session Created Successfully";
            else
                TempData["ErrorMessage"] = "Sesson Failed To Create";
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Update
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be negative or zero";
                return RedirectToAction(nameof(Index));
            }

            var session = _sessionService.GetSessionToUpdate(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Unubdatable";
                return RedirectToAction(nameof(Index));
            }

            LoadTrainerAndCategory();
            return View(session);
        }

        [HttpPost]
        public ActionResult Edit(int id, UpdateSessionViewModel session)
        {
            if (!ModelState.IsValid)
            {
                LoadTrainerAndCategory();
                return View(nameof(Edit));
            }
            var result = _sessionService.UpdateSession(id, session);

            if (result)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Session Falid To Update";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Deletion
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be negative or zero";
                return RedirectToAction(nameof(Index));
            }

            // Check if member with this id exists
            var session = _sessionService.GetSessionDetails(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }

            // To be temporary in ram
            ViewBag.SessionId = id;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var result = _sessionService.DeleteSession(id);
            if (result)
                TempData["SuccessMessage"] = "Session Deleted Successfully";
            else
                TempData["ErrorMessage"] = "Session Faild To Delete";

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Helper Methods
        private void LoadTrainerAndCategory()
        {
            ViewBag.Trainers = new SelectList(
                _sessionService.GetTrainersWithNameAndID(),
                "Id",
                "Name"
            );
            ViewBag.Categories = new SelectList(
                _sessionService.GetCategoriesWithNameAndID(),
                "Id",
                "Name"
            );
        }

        #endregion
    }
}
