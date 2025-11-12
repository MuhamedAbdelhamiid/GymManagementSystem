using GymManagementBLL.BusinessServices.Implementation;
using GymManagementBLL.BusinessServices.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        public IActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();
            return View(trainers);
        }

        #region Details
        public ActionResult TrainerDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be negative or zero";
                return RedirectToAction(nameof(Index));
            }

            var trainer = _trainerService.GetTrainerDetails(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }
        #endregion

        #region Creation
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTrainer(CreateTrainerViewModel trainer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Data Indvalid", "There is missing fields");
                // return user to same page with details that falut
                return View(nameof(Create), trainer);
            }
            // Should return extra data to index when it' created or not [With Refreshnig The Index]
            bool result = _trainerService.CreateTrainer(trainer);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Failed To Create";
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
                return View(nameof(Index));
            }

            var trainer = _trainerService.GetTrainerDetails(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return View(nameof(Index));
            }
            ViewBag.TrainerId = id;
            ViewBag.TrainerName = trainer.Name;

            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var result = _trainerService.DeleteTrainer(id);
            if (result)
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            else
                TempData["ErrorMessage"] = "Trainer Faild To Delete";

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Updating
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be negative or zero";
                return View(nameof(Index));
            }

            var trainer = _trainerService.GetTrainerToUpdate(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return View(nameof(Index));
            }

            return View(trainer);
        }

        [HttpPost]
        public ActionResult Edit(int id, TrainerUpdateViewModel trainer)
        {
            if (!ModelState.IsValid)
            {
                return View(trainer);
            }
            var result = _trainerService.UpdateTrainer(id, trainer);

            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Falid To Update";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
