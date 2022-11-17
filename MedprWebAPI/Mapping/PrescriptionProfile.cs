using AutoMapper;
using MedprCore.DTO;
using MedprDB.Entities;
using MedprModels;
using MedprModels.Requests;
using MedprModels.Responses;

namespace MedprModels.Mapping;

public class PrescriptionProfile : Profile
{
    public PrescriptionProfile() 
    {
        CreateMap<Prescription, PrescriptionDTO>();
        CreateMap<PrescriptionDTO, Prescription>();

        CreateMap<PrescriptionDTO, PrescriptionModelResponse>()
            .ForMember(model => model.Id, opt => opt.MapFrom(dto => dto.Id))
            .ForMember(model => model.StartDate, opt => opt.MapFrom(dto => dto.StartDate))
            .ForMember(model => model.EndDate, opt => opt.MapFrom(dto => dto.EndDate))
            .ForMember(model => model.Dose, opt => opt.MapFrom(dto => dto.Dose))
            .ForMember(model => model.Doctor, opt => opt.Ignore())
            .ForMember(model => model.User, opt => opt.Ignore())
            .ForMember(model => model.Drug, opt => opt.Ignore())
            .ForMember(model => model.Links, opt => opt.Ignore());

        CreateMap<PrescriptionModelRequest, PrescriptionDTO>()
            .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.Id))
            .ForMember(dto => dto.StartDate, opt => opt.MapFrom(model => model.StartDate))
            .ForMember(dto => dto.EndDate, opt => opt.MapFrom(model => model.EndDate))
            .ForMember(dto => dto.Dose, opt => opt.MapFrom(model => model.Dose))
            .ForMember(dto => dto.DoctorId, opt => opt.MapFrom(model => model.DoctorId))
            .ForMember(dto => dto.UserId, opt => opt.MapFrom(model => model.UserId))
            .ForMember(dto => dto.DrugId, opt => opt.MapFrom(model => model.DrugId));
    }
}
