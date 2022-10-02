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
            CreateMap<Vaccination, VaccinationDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(vac => vac.Id))
                .ForMember(dto => dto.Date, opt => opt.MapFrom(vac => vac.Date))
                .ForMember(dto => dto.DaysOfProtection, opt => opt.MapFrom(vac => vac.DaysOfProtection))
                .ForMember(dto => dto.VaccineId, opt => opt.MapFrom(vac => vac.VaccineId))
                .ForMember(dto => dto.UserId, opt => opt.MapFrom(vac => vac.UserId));
            CreateMap<VaccinationDTO, Vaccination>()
                .ForMember(vac => vac.Id, opt => opt.MapFrom(dto => dto.Id))
                .ForMember(vac => vac.Date, opt => opt.MapFrom(dto => dto.Date))
                .ForMember(vac => vac.DaysOfProtection, opt => opt.MapFrom(dto => dto.DaysOfProtection))
                .ForMember(vac => vac.VaccineId, opt => opt.MapFrom(dto => dto.VaccineId))
                .ForMember(vac => vac.UserId, opt => opt.MapFrom(dto => dto.UserId));

            CreateMap<VaccinationDTO, VaccinationModel>()
                .ForMember(model => model.Id, opt => opt.MapFrom(dto => dto.Id))
                .ForMember(model => model.Date, opt => opt.MapFrom(dto => dto.Date))
                .ForMember(model => model.DaysOfProtection, opt => opt.MapFrom(dto => dto.DaysOfProtection))
                .ForMember(model => model.Vaccine, opt => opt.Ignore())
                .ForMember(model => model.User, opt => opt.Ignore());
            CreateMap<VaccinationModel, VaccinationDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.Id))
                .ForMember(dto => dto.Date, opt => opt.MapFrom(model => model.Date))
                .ForMember(dto => dto.DaysOfProtection, opt => opt.MapFrom(model => model.DaysOfProtection))
                .ForMember(dto => dto.VaccineId, opt => opt.MapFrom(model => model.Vaccine.Id))
                .ForMember(dto => dto.UserId, opt => opt.MapFrom(model => model.User.Id));
        }
    }
}
