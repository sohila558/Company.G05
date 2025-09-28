using AutoMapper;
using Company.G05.DAL.Models;
using Company.G05.PL.DTOs;

namespace Company.G05.PL.Mapping
{
    public class DepartmenProfile : Profile
    {
        public DepartmenProfile()
        {
            CreateMap<CreateDepartmentDTO, Department>().ReverseMap();
        }
    }
}
