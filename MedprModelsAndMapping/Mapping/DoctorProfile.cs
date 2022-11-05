using AutoMapper;
using MedprCore.DTO;
using MedprDB.Entities;
using MedprModelsAndMapping.Models;

namespace MedprModelsAndMapping.Mapping;

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
