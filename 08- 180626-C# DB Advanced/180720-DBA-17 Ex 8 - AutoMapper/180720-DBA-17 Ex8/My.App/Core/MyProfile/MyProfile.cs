using AutoMapper;
using My.App.Core.Dtos;
using My.Models;

namespace My.App.Core.MyProfile
{
    public class MyProfile : Profile
    {
        public MyProfile()
        {
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<Employee, EmployeePersonalInfoDto>().ReverseMap();
            CreateMap<Employee, ManagerDto>()
                .ForMember(dest => dest.SubordinateDtos,
                                    opt => opt.MapFrom(src => src.Subordinates))
                .ReverseMap();
            CreateMap<Employee, EmployeeOlderThanDto>()
                .ForMember(dest => dest.ManagerFullName,
                                    opt => opt.MapFrom(src => src.Manager.FirstName + " " + src.Manager.LastName))
                .ReverseMap();
        }
    }
}
