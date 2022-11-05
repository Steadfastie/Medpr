using AutoMapper;
using MedprCore.DTO;
using MedprDB.Entities;
using MedprModelsAndMapping.Models;

namespace MedprModelsAndMapping.Mapping;

public class UserProfile : Profile
{
    public UserProfile() 
    {
        CreateMap<User, UserDTO>();
        CreateMap<UserDTO, User>();

        CreateMap<UserDTO, UserModel>();
        CreateMap<UserModel, UserDTO>()
            .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.Id))
            .ForMember(dto => dto.Login, opt => opt.MapFrom(model => model.Login))
            .ForMember(dto => dto.FullName, opt => opt.MapFrom(model => model.FullName))
            .ForMember(dto => dto.DateOfBirth, opt => opt.MapFrom(model => model.DateOfBirth));
    }
}
