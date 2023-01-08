using AutoMapper;
using MedprCore.DTO;
using MedprDB.Entities;
using MedprMVC.Models;

namespace MedprMVC.Mapping;

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
            .ForMember(model => model.Families, opt => opt.Ignore())
            .ForMember(model => model.User, opt => opt.Ignore())
            .ForMember(model => model.UserId, opt => opt.MapFrom(dto => dto.UserId))
            .ForMember(model => model.Users, opt => opt.Ignore());

        CreateMap<FamilyMemberModel, FamilyMemberDTO>()
            .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.Id))
            .ForMember(dto => dto.IsAdmin, opt => opt.MapFrom(model => model.IsAdmin))
            .ForMember(dto => dto.FamilyId, opt => opt.MapFrom(model => model.FamilyId))
            .ForMember(dto => dto.UserId, opt => opt.MapFrom(model => model.UserId));
    }
}