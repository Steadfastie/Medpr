using AutoMapper;
using MedprCore.DTO;
using MedprDB.Entities;
using MedprModels.Requests;

namespace MedprWebAPI.Mapping;

public class DrugProfile : Profile
{
    public DrugProfile() 
    {
        CreateMap<Drug, DrugDTO>();
        CreateMap<DrugDTO, Drug>();

        CreateMap<DrugDTO, DrugModel>()
            .ForMember(model => model.Id, opt => opt.MapFrom(dto => dto.Id))
            .ForMember(model => model.Name, opt => opt.MapFrom(dto => dto.Name))
            .ForMember(model => model.PharmGroup, opt => opt.MapFrom(dto => dto.PharmGroup))
            .ForMember(model => model.Price, opt => opt.MapFrom(dto => dto.Price))
            .ForMember(model => model.Links, opt => opt.Ignore());
        CreateMap<DrugModel, DrugDTO>()
            .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.Id))
            .ForMember(dto => dto.Name, opt => opt.MapFrom(model => model.Name))
            .ForMember(dto => dto.PharmGroup, opt => opt.MapFrom(model => model.PharmGroup))
            .ForMember(dto => dto.Price, opt => opt.MapFrom(model => model.Price));
    }
}
