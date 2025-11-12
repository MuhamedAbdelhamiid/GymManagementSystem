using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MembershipViewModels
{
    public class MembershipToCreateViewModel
    {
        [Required(ErrorMessage = "Plan is required")]
        public int PlanId { get; set; }

        [Required(ErrorMessage = "Member is required")]
        public int MemberId { get; set; }
    }
}
