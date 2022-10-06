using AutoMapper;
using MedprCore.DTO;
using MedprDB.Entities;
using MedprMVC.Models;

namespace MedprMVC.Mapping;

public class AppointmentProfile : Profile
{
    public AppointmentProfile() 
    {
        CreateMap<Appointment, AppointmentDTO>();
        CreateMap<AppointmentDTO, Appointment>();

        CreateMap<AppointmentDTO, AppointmentModel>()
            .ForMember(model => model.Id, opt => opt.MapFrom(dto => dto.Id))
            .ForMember(model => model.Date, opt => opt.MapFrom(dto => dto.Date))
            .ForMember(model => model.Place, opt => opt.MapFrom(dto => dto.Place))
            .ForMember(model => model.Doctor, opt => opt.Ignore())
            .ForMember(model => model.DoctorId, opt => opt.MapFrom(dto => dto.DoctorId))
            .ForMember(model => model.Doctors, opt => opt.Ignore())
            .ForMember(model => model.User, opt => opt.Ignore())
            .ForMember(model => model.UserId, opt => opt.MapFrom(dto => dto.UserId))
            .ForMember(model => model.Users, opt => opt.Ignore());

        CreateMap<AppointmentModel, AppointmentDTO>()
            .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.Id))
            .ForMember(dto => dto.Date, opt => opt.MapFrom(model => model.Date))
            .ForMember(dto => dto.Place, opt => opt.MapFrom(model => model.Place))
            .ForMember(dto => dto.DoctorId, opt => opt.MapFrom(model => model.DoctorId))
            .ForMember(dto => dto.UserId, opt => opt.MapFrom(model => model.UserId));
    }
}
