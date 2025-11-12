using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GymManagementBLL.ViewModels.SessionViewModels;

namespace GymManagementBLL.BusinessServices.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSession();

        SessionViewModel? GetSessionDetails(int sessionId);

        bool CreateSession(CreateSessionViewModel session);

        UpdateSessionViewModel? GetSessionToUpdate(int sessionId);

        bool UpdateSession(int sessionId, UpdateSessionViewModel sessionToUpdate);

        bool DeleteSession(int sessionId);

        IEnumerable<TrainerSelectViewModel> GetTrainersWithNameAndID();
        IEnumerable<CategorySelectViewModel> GetCategoriesWithNameAndID();
    }
}
