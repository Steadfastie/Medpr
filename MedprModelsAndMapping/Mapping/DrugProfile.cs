using AutoMapper;
using MedprCore.DTO;
using MedprDB.Entities;
using MedprModelsAndMapping.Models;

namespace MedprModelsAndMapping.Mapping;

public class DrugProfile : Profile
{
    public DrugProfile() 
    {
        CreateMap<Drug, DrugDTO>();
        CreateMap<DrugDTO, Drug>();

        CreateMap<DrugDTO, DrugModel>();
        CreateMap<DrugModel, DrugDTO>();
    }
}
