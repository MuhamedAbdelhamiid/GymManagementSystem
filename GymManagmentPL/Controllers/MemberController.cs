using System.ComponentModel.Design;
using GymManagementBLL.BusinessServices.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public ActionResult Index()
        {
            var data = _memberService.GetAllMembers();
            return View(data);
        }

        #region Details Of Member Or HealthRecord
        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be negative or zero";
                return RedirectToAction(nameof(Index));
            }

            var member = _memberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be negative or zero";
                return RedirectToAction(nameof(Index));
            }

            var healthRecord = _memberService.GetMemberHealthRecord(id);
            if (healthRecord is null)
            {
                TempData["ErrorMessage"] = "No Health Record Found";
                return RedirectToAction(nameof(Index));
            }

            return View(healthRecord);
        }
        #endregion

        #region Creation
        // HTTP Get
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel member)
        {
            // If it not passed all application validations
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Data Indvalid", "There is missing fields");
                // return user to same page with details that falut
                return View(nameof(Create), member);
            }
            // Should return extra data to index when it' created or not [With Refreshnig The Index]
            bool result = _memberService.CreateMember(member);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed To Create";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit
        public ActionResult MemberEdit(int id)
        {
            if (id <= 0)
            {
                TempData["Error Message"] = "Id cannot be negative or zero";
                return RedirectToAction(nameof(Index));
            }

            var member = _memberService.GetMemberDetailsToUpdate(id);
            if (member is null)
            {
                TempData["Error Message"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        [HttpPost]
        // It will take the id from route when it passed to MemberUpdate(int id)
        // And because they are with same name there will be no problem [Because they same URL => EX: Member/MemberEdit/5]
        public ActionResult MemberEdit([FromRoute] int id, MemberToUpdateViewModel member)
        {
            // Validate that no one will change the data from inspect
            if (!ModelState.IsValid)
            {
                return View(member);
            }
            var result = _memberService.UpdateMember(id, member);

            if (result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Falid To Update";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be negative or zero";
                return RedirectToAction(nameof(Index));
            }

            // Check if member with this id exists
            var member = _memberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            // To be temporary in ram
            ViewBag.MemberId = id;
            ViewBag.MemberName = member.Name;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var result = _memberService.DeleteMember(id);
            if (result)
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            else
                TempData["ErrorMessage"] = "Member Faild To Delete";

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
