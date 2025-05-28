using AutoMapper;
using Hotel_Management.Models;
using Hotel_Management.Areas.Admin.ViewModels;

namespace Hotel_Management.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUserVM, ApplicationUser>()
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => true));

            CreateMap<EditUserVM, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId));
            CreateMap<ApplicationUser, EditUserVM>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
