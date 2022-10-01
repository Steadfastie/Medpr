using AutoMapper;
using MedprCore.DTO;
using MedprDB.Entities;
using MedprMVC.Models;

namespace MedprMVC.Mapping
{
    public class DoctorProfile : Profile
    {
        public DoctorProfile() 
        {
            CreateMap<Doctor, DoctorDTO>();
            CreateMap<DoctorDTO, Doctor>();

            CreateMap<DoctorDTO, DoctorModel>();
            CreateMap<DoctorModel, DoctorDTO>();
        }
    }
}
