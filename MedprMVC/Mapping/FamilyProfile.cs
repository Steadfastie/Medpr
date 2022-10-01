using AutoMapper;
using MedprCore.DTO;
using MedprDB.Entities;
using MedprMVC.Models;

namespace MedprMVC.Mapping
{
    public class FamilyProfile : Profile
    {
        public FamilyProfile() 
        {
            CreateMap<Family, FamilyDTO>();
            CreateMap<FamilyDTO, Family>();

            CreateMap<FamilyDTO, FamilyModel>();
            CreateMap<FamilyModel, FamilyDTO>();
        }
    }
}
