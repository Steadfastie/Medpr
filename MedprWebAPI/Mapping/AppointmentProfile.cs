using AutoMapper;
using MedprCore.DTO;
using MedprDB.Entities;
using MedprModels.Requests;
using MedprModels.Responses;

namespace MedprWebAPI.Mapping;

public class AppointmentProfile : Profile
{
    public AppointmentProfile()
    {
        CreateMap<Appointment, AppointmentDTO>();
        CreateMap<AppointmentDTO, Appointment>();

        CreateMap<AppointmentDTO, AppointmentModelResponse>()
            .ForMember(model => model.Id, opt => opt.MapFrom(dto => dto.Id))
            .ForMember(model => model.Date, opt => opt.MapFrom(dto => dto.Date))
            .ForMember(model => model.Place, opt => opt.MapFrom(dto => dto.Place))
            .ForMember(model => model.Doctor, opt => opt.Ignore())
            .ForMember(model => model.User, opt => opt.Ignore())
            .ForMember(model => model.Links, opt => opt.Ignore());

        CreateMap<AppointmentModelRequest, AppointmentDTO>()
            .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.Id))
            .ForMember(dto => dto.Date, opt => opt.MapFrom(model => model.Date))
            .ForMember(dto => dto.Place, opt => opt.MapFrom(model => model.Place))
            .ForMember(dto => dto.DoctorId, opt => opt.MapFrom(model => model.DoctorId))
            .ForMember(dto => dto.UserId, opt => opt.MapFrom(model => model.UserId));
    }
}