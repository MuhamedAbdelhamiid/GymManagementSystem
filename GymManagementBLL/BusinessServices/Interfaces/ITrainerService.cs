using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementBLL.ViewModels.TrainerViewModels;

namespace GymManagementBLL.BusinessServices.Interfaces
{
    public interface ITrainerService
    {
        // Get All Trainers [Id, Name, Email, Specialization, Phone]
        IEnumerable<TrainerViewModel> GetAllTrainers();

        // CreateTrainer()
        bool CreateTrainer(CreateTrainerViewModel trainerCreationViewModel);

        // Get Trainer Details()
        TrainerViewModel? GetTrainerDetails(int trainerId);

        // GetTrainerToUpdate
        TrainerUpdateViewModel? GetTrainerToUpdate(int trainerId);

        // UpdateTrainer()
        bool UpdateTrainer(int trainerId, TrainerUpdateViewModel trainerToUpdate);

        // DeleteTrainer()
        bool DeleteTrainer(int trainerId);
    }
}
