using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementBLL.ViewModels.PlanViewModels;

namespace GymManagementBLL.BusinessServices.Interfaces
{
    public interface IPlanService
    {
        public IEnumerable<PlanViewModel> GetAllPlans();
        public PlanViewModel? GetPlanDetails(int planId);
        PlanToUpdateViewModel? GetPlanToUpdate(int planId);
        bool UpdatePlan(int planId, PlanToUpdateViewModel planToUpdate);
        bool ToggleStatus(int planId);
    }
}
