using AutoMapper;
using MedprCore.DTO;
using MedprDB.Entities;
using MedprModelsAndMapping.Models;

namespace MedprModelsAndMapping.Mapping;

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
