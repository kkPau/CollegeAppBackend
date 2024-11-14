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
        CreateMap<StudentDTO, Student>().ReverseMap();
    }
}
