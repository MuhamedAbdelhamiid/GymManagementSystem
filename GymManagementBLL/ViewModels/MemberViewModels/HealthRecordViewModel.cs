using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    public class HealthRecordViewModel
    {
        [Required(ErrorMessage = "Height is required")]
        [Range(0.1, 300, ErrorMessage = "Height between 0.1 and 300")]
        public decimal Height { get; set; }

        [Required(ErrorMessage = "Weight is required")]
        [Range(1, 350, ErrorMessage = "Weight between 1 and 350 kg")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "Blood Type is required")]
        [StringLength(3, MinimumLength = 1, ErrorMessage = "Blood Type must be max 3 and min 1")]
        public string BloodType { get; set; } = null!;

        public string? Note { get; set; }
    }
}
