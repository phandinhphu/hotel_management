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

            // HotelServicesVM
            CreateMap<HotelServicesVM, Service>();
            CreateMap<Service, HotelServicesVM>();

            // ReviewVM
            CreateMap<ReviewVM, Review>();
            CreateMap<Review, ReviewVM>();

            // PasswordVM
            CreateMap<ApplicationUser, PasswordVM>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
