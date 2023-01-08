using AutoMapper;
using MedprCore.DTO;
using MedprDB.Entities;
using MedprMVC.Models;

namespace MedprMVC.Mapping
{
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
}