using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core.Pipeline;

namespace GymManagementBLL.ViewModels.MemberViewModels
{

    public class MemberToUpdateViewModel
    {
        public string Name { get; set; } = null!;
        public string? Photo { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email must be between 5 and 100")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid phone format")]
        [RegularExpression(@"(010|011|012|015)\d{8}$")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Building Number is required")]
        [Range(1, 9000, ErrorMessage = "Building Number must be between 1 and 9000")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "City is between 30 and 2")]
        [RegularExpression(
            @"^[a-zA-Z\s]+$",
            ErrorMessage = "City must contain letters or spaces only"
        )]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Gender is required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street is between 30 and 2")]
        [RegularExpression(
            @"^[a-zA-Z\s]+$",
            ErrorMessage = "Street must contain letters or spaces only"
        )]
        public string Street { get; set; } = null!;
    }
}
