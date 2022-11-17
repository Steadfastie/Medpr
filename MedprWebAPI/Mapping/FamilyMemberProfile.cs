using AutoMapper;
using MedprCore.DTO;
using MedprDB.Entities;
using MedprModels;
using MedprModels.Requests;
using MedprModels.Responses;

namespace MedprModels.Mapping;

public class FamilyMemberProfile : Profile
{
    public FamilyMemberProfile() 
    {
        CreateMap<FamilyMember, FamilyMemberDTO>();
        CreateMap<FamilyMemberDTO, FamilyMember>();

        CreateMap<FamilyMemberDTO, FamilyMemberModelResponse>()
            .ForMember(model => model.Id, opt => opt.MapFrom(dto => dto.Id))
            .ForMember(model => model.IsAdmin, opt => opt.MapFrom(dto => dto.IsAdmin))
            .ForMember(model => model.User, opt => opt.Ignore())
            .ForMember(model => model.Links, opt => opt.Ignore());

        CreateMap<FamilyMemberModelRequest, FamilyMemberDTO>()
            .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.Id))
            .ForMember(dto => dto.IsAdmin, opt => opt.MapFrom(model => model.IsAdmin))
            .ForMember(dto => dto.FamilyId, opt => opt.MapFrom(model => model.FamilyId))
            .ForMember(dto => dto.UserId, opt => opt.MapFrom(model => model.UserId));
    }
}
