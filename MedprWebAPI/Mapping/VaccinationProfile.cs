using AutoMapper;
using MedprCore.DTO;
using MedprDB.Entities;
using MedprModels;
using MedprModels.Requests;

namespace MedprModels.Mapping;

public class VaccinationProfile : Profile
{
    public VaccinationProfile() 
    {
        CreateMap<Vaccination, VaccinationDTO>();
        CreateMap<VaccinationDTO, Vaccination>();

        CreateMap<VaccinationDTO, VaccinationModel>()
            .ForMember(model => model.Id, opt => opt.MapFrom(dto => dto.Id))
            .ForMember(model => model.Date, opt => opt.MapFrom(dto => dto.Date))
            .ForMember(model => model.DaysOfProtection, opt => opt.MapFrom(dto => dto.DaysOfProtection))
            .ForMember(model => model.Vaccine, opt => opt.Ignore())
            .ForMember(model => model.VaccineId, opt => opt.MapFrom(dto => dto.VaccineId))
            .ForMember(model => model.User, opt => opt.Ignore())
            .ForMember(model => model.UserId, opt => opt.MapFrom(dto => dto.UserId));

        CreateMap<VaccinationModel, VaccinationDTO>()
            .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.Id))
            .ForMember(dto => dto.Date, opt => opt.MapFrom(model => model.Date))
            .ForMember(dto => dto.DaysOfProtection, opt => opt.MapFrom(model => model.DaysOfProtection))
            .ForMember(dto => dto.VaccineId, opt => opt.MapFrom(model => model.VaccineId))
            .ForMember(dto => dto.UserId, opt => opt.MapFrom(model => model.UserId));
    }
}
