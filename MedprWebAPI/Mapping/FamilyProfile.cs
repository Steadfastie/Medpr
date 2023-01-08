using AutoMapper;
using MedprCore.DTO;
using MedprDB.Entities;
using MedprModels.Requests;
using MedprModels.Responses;

namespace MedprModels.Mapping;

public class FamilyProfile : Profile
{
    public FamilyProfile()
    {
        CreateMap<Family, FamilyDTO>();
        CreateMap<FamilyDTO, Family>();

        CreateMap<FamilyDTO, FamilyModelResponse>()
            .ForMember(model => model.Id, opt => opt.MapFrom(dto => dto.Id))
            .ForMember(model => model.Surname, opt => opt.MapFrom(dto => dto.Surname))
            .ForMember(model => model.Creator, opt => opt.MapFrom(dto => dto.Creator))
            .ForMember(model => model.Members, opt => opt.Ignore())
            .ForMember(model => model.Links, opt => opt.Ignore());
        CreateMap<FamilyModelRequest, FamilyDTO>()
            .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.Id))
            .ForMember(dto => dto.Surname, opt => opt.MapFrom(model => model.Surname))
            .ForMember(dto => dto.Creator, opt => opt.Ignore());
    }
}