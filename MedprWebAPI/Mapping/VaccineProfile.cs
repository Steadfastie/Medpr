using AutoMapper;
using MedprCore.DTO;
using MedprDB.Entities;
using MedprModels.Requests;
using MedprModels.Responses;

namespace MedprModels.Mapping;

public class VaccineProfile : Profile
{
    public VaccineProfile()
    {
        CreateMap<Vaccine, VaccineDTO>();
        CreateMap<VaccineDTO, Vaccine>();

        CreateMap<VaccineDTO, VaccineModelResponse>()
            .ForMember(model => model.Id, opt => opt.MapFrom(dto => dto.Id))
            .ForMember(model => model.Name, opt => opt.MapFrom(dto => dto.Name))
            .ForMember(model => model.Reason, opt => opt.MapFrom(dto => dto.Reason))
            .ForMember(model => model.Price, opt => opt.MapFrom(dto => dto.Price))
            .ForMember(model => model.Links, opt => opt.Ignore());
        CreateMap<VaccineModelRequest, VaccineDTO>()
            .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.Id))
            .ForMember(dto => dto.Name, opt => opt.MapFrom(model => model.Name))
            .ForMember(dto => dto.Reason, opt => opt.MapFrom(model => model.Reason))
            .ForMember(dto => dto.Price, opt => opt.MapFrom(model => model.Price));
    }
}