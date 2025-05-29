using AutoMapper;
using Hotel_Management.Models;
using Hotel_Management.Areas.Admin.ViewModels;

namespace Hotel_Management.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // CreateUserVM
            CreateMap<CreateUserVM, ApplicationUser>()
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => true));

            // EditUserVM
            CreateMap<EditUserVM, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId));
            CreateMap<ApplicationUser, EditUserVM>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

            // BookingVM
            CreateMap<Booking, BookingVM>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.BookingsRoomDetails.FirstOrDefault().Room.RoomNumber))
                .ForMember(dest => dest.Services, opt => opt.MapFrom(src => src.BookingsServiceDetails.Select(s => s.Service.Id).ToList()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.HasValue ? src.CreatedAt.Value.ToString("g") : string.Empty));
        }
    }
}
