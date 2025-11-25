using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities.Enums;
using Microsoft.AspNetCore.Http;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    public class CreateMemberViewModel
    {
        [Required(ErrorMessage = "Photo is required")]
        [Display(Name = "Profile Photo")]
        public IFormFile Photo { get; set; } = null!;

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50")]
        [RegularExpression(
            @"^[a-zA-Z\s]+$",
            ErrorMessage = "Name must contain letters or spaces only"
        )]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email must be between 5 and 100")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid phone format")]
        [RegularExpression(
            @"(010|011|012|015)\d{8}$",
            ErrorMessage = "Must Be Valid Egyption Phone Number"
        )]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Date Of Birth is required")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }

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

        [Required(ErrorMessage = "Street is required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street is between 30 and 2")]
        [RegularExpression(
            @"^[a-zA-Z\s]+$",
            ErrorMessage = "Street must contain letters or spaces only"
        )]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "Health Record is required")]
        public HealthRecordViewModel HealthRecord { get; set; } = null!;
    }
}
