using AutoMapper;
using MedprCore.DTO;
using MedprDB.Entities;
using MedprMVC.Models;

namespace MedprMVC.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<UserDTO, UserModel>();
            CreateMap<UserModel, UserDTO>();
        }
    }
}
