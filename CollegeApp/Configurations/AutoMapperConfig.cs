using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Models;

namespace CollegeApp.Configurations;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        // CreateMap<Student, StudentDTO>();
        // CreateMap<StudentDTO, Student>();
        //============= OR =============
        CreateMap<StudentDTO, Student>().ReverseMap().AddTransform<string>(n => string.IsNullOrEmpty(n) ? "No data found" : n);

        // CreateMap<Student, StudentDTO>()
        //     .ForMember(dest => dest.DOB, opt => opt.MapFrom(src => Convert.ToDateTime(src.DOB)));
    }
}
