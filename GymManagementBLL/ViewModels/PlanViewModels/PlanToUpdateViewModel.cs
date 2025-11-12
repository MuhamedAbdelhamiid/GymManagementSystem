using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.PlanViewModels
{
    public class PlanToUpdateViewModel
    {
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(
            200,
            MinimumLength = 5,
            ErrorMessage = "Description is between 3 and 200 letters"
        )]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Duration Days is required")]
        [Range(1, 365, ErrorMessage = "Duration days must be between 1 and 365")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(250, 10000, ErrorMessage = "Price must be between 250 and 10000")]
        public decimal Price { get; set; }
    }
}
