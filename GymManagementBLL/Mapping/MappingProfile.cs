using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymManagementBLL.ViewModels.BookingViewModels;
using GymManagementBLL.ViewModels.MembershipViewModels;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;

namespace GymManagementBLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapSession();
            MapMember();
            MapPlan();
            MapTrainer();
            MapMembership();
            MapBooking();
        }

        private void MapSession()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(
                    dest => dest.TrainerName,
                    options => options.MapFrom(src => src.Trainer.Name)
                )
                .ForMember(
                    dest => dest.CategoryName,
                    options => options.MapFrom(src => src.Category.CategoryName)
                )
                .ForMember(dest => dest.AvailableSlots, options => options.Ignore());

            CreateMap<CreateSessionViewModel, Session>();

            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>()
                .ForMember(dest => dest.Name, options => options.MapFrom(src => src.CategoryName));
        }

        private void MapMember()
        {
            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(
                    dest => dest.Address,
                    opt =>
                        opt.MapFrom(src => new Address
                        {
                            BuildingNumber = src.BuildingNumber,
                            Street = src.Street,
                            City = src.City,
                        })
                )
                .ForMember(dest => dest.Photo, options => options.Ignore());

            CreateMap<HealthRecordViewModel, HealthRecord>().ReverseMap();

            CreateMap<Member, MemberViewModel>()
                .ForMember(
                    dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString())
                )
                .ForMember(
                    dest => dest.Address,
                    opt =>
                        opt.MapFrom(src =>
                            $"{src.Address.BuildingNumber}-{src.Address.Street}-{src.Address.City}"
                        )
                );

            #region Second Way
            //CreateMap<CreateMemberViewModel, Member>()
            //    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src));

            //CreateMap<CreateMemberViewModel, Address>()
            //    .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
            //    .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
            //    .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City));
            #endregion

            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(
                    dest => dest.BuildingNumber,
                    opt => opt.MapFrom(src => src.Address.BuildingNumber)
                )
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City));

            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .AfterMap(
                    (src, dest) =>
                    {
                        dest.Address.BuildingNumber = src.BuildingNumber;
                        dest.Address.Street = src.Street;
                        dest.Address.City = src.City;
                        dest.UpdatedAt = DateTime.Now;
                    }
                );
        }

        private void MapPlan()
        {
            CreateMap<Plan, PlanViewModel>();

            CreateMap<Plan, PlanToUpdateViewModel>();

            CreateMap<PlanToUpdateViewModel, Plan>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));
        }

        private void MapTrainer()
        {
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(
                    dest => dest.Address,
                    opt =>
                        opt.MapFrom(src => new Address
                        {
                            BuildingNumber = src.BuildingNumber,
                            Street = src.Street,
                            City = src.City,
                        })
                )
                .ForMember(
                    dest => dest.Specialties,
                    options => options.MapFrom(src => src.Specialization)
                );

            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(
                    dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString())
                )
                .ForMember(
                    dest => dest.Address,
                    opt =>
                        opt.MapFrom(src =>
                            $"{src.Address.BuildingNumber}-{src.Address.Street}-{src.Address.City}"
                        )
                )
                .ForMember(
                    dest => dest.Specialization,
                    options => options.MapFrom(src => src.Specialties.ToString())
                );

            CreateMap<Trainer, TrainerUpdateViewModel>()
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(
                    dest => dest.BuildingNumber,
                    opt => opt.MapFrom(src => src.Address.BuildingNumber)
                )
                .ForMember(
                    dest => dest.Specialization,
                    options => options.MapFrom(src => src.Specialties)
                );

            CreateMap<TrainerUpdateViewModel, Trainer>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .AfterMap(
                    (src, dest) =>
                    {
                        dest.Address.BuildingNumber = src.BuildingNumber;
                        dest.Address.City = src.City;
                        dest.Address.Street = src.Street;
                        dest.UpdatedAt = DateTime.Now;
                    }
                )
                .ForMember(
                    dest => dest.Specialties,
                    options => options.MapFrom(src => src.Specialization)
                );
        }

        private void MapMembership()
        {
            CreateMap<Membership, MembershipViewModel>()
                .ForMember(
                    dest => dest.MemberName,
                    options => options.MapFrom(src => src.Member.Name)
                )
                .ForMember(dest => dest.PlanName, options => options.MapFrom(src => src.Plan.Name))
                .ForMember(
                    dest => dest.StartDate,
                    options => options.MapFrom(src => src.CreatedAt)
                );

            CreateMap<MembershipToCreateViewModel, Membership>()
                .ForMember(dest => dest.EndDate, options => options.Ignore());

            CreateMap<Member, MemberSelectViewModel>();
            CreateMap<Plan, PlanSelectViewModel>();
        }

        private void MapBooking()
        {
            CreateMap<MemberSession, MemberDetailsInUpcomingSessionViewModel>()
                .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.CreatedAt));
            CreateMap<MemberSession, MemberDetailsInOngoingSessionViewModel>()
                .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name));

            CreateMap<CreateBookingViewModel, MemberSession>()
                .ForMember(dest => dest.IsAttended, opt => opt.Ignore());
        }
    }
}
