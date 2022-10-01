using AutoMapper;
using MedprCore.DTO;
using MedprDB.Entities;
using MedprMVC.Models;

namespace MedprMVC.Mapping
{
    public class VaccinationProfile : Profile
    {
        public VaccinationProfile() 
        {
            CreateMap<Vaccination, VaccinationDTO>();
            CreateMap<VaccinationDTO, Vaccination>();

            CreateMap<VaccinationDTO, VaccinationModel>();
            CreateMap<VaccinationModel, VaccinationDTO>();
        }
    }
}
