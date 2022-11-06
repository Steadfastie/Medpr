using AutoMapper;
using MedprCore.DTO;
using MedprDB.Entities;
using MedprModels;
using MedprModels.Requests;

namespace MedprModels.Mapping;

public class FamilyMemberProfile : Profile
{
    public FamilyMemberProfile() 
    {
        CreateMap<FamilyMember, FamilyMemberDTO>();
        CreateMap<FamilyMemberDTO, FamilyMember>();

        CreateMap<FamilyMemberDTO, FamilyMemberModel>()
            .ForMember(model => model.Id, opt => opt.MapFrom(dto => dto.Id))
            .ForMember(model => model.IsAdmin, opt => opt.MapFrom(dto => dto.IsAdmin))
            .ForMember(model => model.Family, opt => opt.Ignore())
            .ForMember(model => model.FamilyId, opt => opt.MapFrom(dto => dto.FamilyId))
            .ForMember(model => model.User, opt => opt.Ignore())
            .ForMember(model => model.UserId, opt => opt.MapFrom(dto => dto.UserId));

        CreateMap<FamilyMemberModel, FamilyMemberDTO>()
            .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.Id))
            .ForMember(dto => dto.IsAdmin, opt => opt.MapFrom(model => model.IsAdmin))
            .ForMember(dto => dto.FamilyId, opt => opt.MapFrom(model => model.FamilyId))
            .ForMember(dto => dto.UserId, opt => opt.MapFrom(model => model.UserId));
    }
}
