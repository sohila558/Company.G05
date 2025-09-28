using AutoMapper;
using Company.G05.DAL.Models;
using Company.G05.PL.DTOs;

namespace Company.G05.PL.Mapping
{
    // CLR
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeDTO, Employee>().ReverseMap();
        }
    }
}
