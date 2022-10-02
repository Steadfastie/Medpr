using AutoMapper;
using MedprCore.DTO;
using MedprDB.Entities;
using MedprMVC.Models;

namespace MedprMVC.Mapping
{
    public class UserCredentialsProfile : Profile
    {
        public UserCredentialsProfile() 
        {
            CreateMap<User, UserCredentialsDTO>();
            CreateMap<UserCredentialsDTO, User>();

            CreateMap<UserCredentialsDTO, UserCredentialsModel>();
            CreateMap<UserCredentialsModel, UserCredentialsDTO>();
        }
    }
}
