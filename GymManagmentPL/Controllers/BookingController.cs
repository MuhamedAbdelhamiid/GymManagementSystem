using GymManagementBLL.BusinessServices.Interfaces;
using GymManagementBLL.ViewModels.BookingViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagmentPL.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public IActionResult Index()
        {
            var data = _bookingService.GetAvailableSessionsForBooking();
            return View(data);
        }

        #region ViewMembers
        public ActionResult GetMembersForUpcomingSession(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be negative or zero";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SessionId = id;
            var data = _bookingService.GetUpcomingSessionDetails(id);
            return View(data);
        }

        public ActionResult GetMembersForOngoingSession(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be negative or zero";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.SessionId = id;
            var data = _bookingService.GetOngoingSessionDetails(id);
            return View(data);
        }
        #endregion

        #region Creation
        public ActionResult Create(int sessionId)
        {
            var members = _bookingService.GetMembersToDropDown();
            ViewBag.Members = new SelectList(members, "Id", "Name");
            ViewBag.SessionId = sessionId;
            return View();
        }

        [HttpPost]
        public ActionResult Create(int sessionId, CreateBookingViewModel booking)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "There is missing fields";
                return RedirectToAction(
                    actionName: nameof(GetMembersForUpcomingSession),
                    routeValues: new { id = sessionId }
                );
            }

            var result = _bookingService.CreateBooking(booking);
            if (result)
                TempData["SuccessMessage"] = "Session Booked Successfully";
            else
                TempData["ErrorMessage"] = "Session Failed To Book";
            return RedirectToAction(
                actionName: nameof(GetMembersForUpcomingSession),
                routeValues: new { id = sessionId }
            );
        }
        #endregion

        #region Attendance
        [HttpPost]
        public ActionResult MarkAsAttended(int sessionId, int memberId)
        {
            var result = _bookingService.MarkAsAttended(sessionId, memberId);
            if (result)
                TempData["SuccessMessage"] = "Member Marked As Attended Successfully";
            else
                TempData["ErrorMessage"] = "Member Failed To Marked As Attended";

            return RedirectToAction(
                actionName: nameof(GetMembersForOngoingSession),
                routeValues: new { id = sessionId }
            );
        }
        #endregion

        #region Cancelation
        [HttpPost]
        public ActionResult Cancel(int sessionId, int memberId)
        {
            var result = _bookingService.CancelBooking(sessionId, memberId);
            if (result)
                TempData["SuccessMessage"] = "Booking Canceled Successfuly";
            else
                TempData["ErrorMessage"] = "Booking Failed To Be Cancel";

            return RedirectToAction(
                actionName: nameof(GetMembersForUpcomingSession),
                routeValues: new { id = sessionId }
            );
        }
        #endregion
    }
}
