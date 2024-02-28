using AutoMapper;
using Learning_platform.Entities;
using Learning_platform.Models;

namespace Learning_platform.Profiles
{
    public class UserTypeProfile: Profile
    {
        public UserTypeProfile() {

           /* CreateMap<User, UserDto>()
                .IncludeAllDerived()
                .ForMember(
                dest => dest.UserType,
                opt => opt.MapFrom(src => new UserTypeDto { Id = src.UserType.Id, Name = src.UserType.Name }))
                .PreserveReferences();*/
            CreateMap<User, StudentDto>().IncludeAllDerived();
            CreateMap<User, TutorDto>().IncludeAllDerived();
            CreateMap<Student, StudentDto>().IncludeAllDerived();
            CreateMap<Tutor, TutorDto>().IncludeAllDerived();
            CreateMap<UserType, UserTypeDto>();
            CreateMap<AccountBasicDetailsModel, User>();
        }
    }
}
