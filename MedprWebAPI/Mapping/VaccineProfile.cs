using AutoMapper;
using MedprCore.DTO;
using MedprDB.Entities;
using MedprModels;
using MedprModels.Requests;

namespace MedprModels.Mapping;

public class VaccineProfile : Profile
{
    public VaccineProfile() 
    {
        CreateMap<Vaccine, VaccineDTO>();
        CreateMap<VaccineDTO, Vaccine>();

        CreateMap<VaccineDTO, VaccineModel>();
        CreateMap<VaccineModel, VaccineDTO>();
    }
}
