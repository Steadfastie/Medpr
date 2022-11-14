using AutoMapper;
using MedprCore.DTO;
using MedprDB.Entities;
using MedprModels;
using MedprModels.Requests;
using MedprModels.Responses;

namespace MedprModels.Mapping;

public class UserProfile : Profile
{
    public UserProfile() 
    {
        CreateMap<User, UserDTO>();
        CreateMap<UserDTO, User>();

        CreateMap<UserDTO, UserModelResponse>()
            .ForMember(model => model.Id, opt => opt.MapFrom(dto => dto.Id))
            .ForMember(model => model.Name, opt => opt.MapFrom(dto => dto.Name))
            .ForMember(model => model.PharmGroup, opt => opt.MapFrom(dto => dto.PharmGroup))
            .ForMember(model => model.Price, opt => opt.MapFrom(dto => dto.Price))
            .ForMember(model => model.Links, opt => opt.Ignore());
        CreateMap<UserModelRequest, UserDTO>()
            .ForMember(dto => dto.Id, opt => opt.Ignore())
            .ForMember(dto => dto.Login, opt => opt.MapFrom(model => model.Login))
            .ForMember(dto => dto.FullName, opt => opt.Ignore())
            .ForMember(dto => dto.DateOfBirth, opt => opt.Ignore());
    }
}
