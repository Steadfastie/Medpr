using AutoMapper;
using MedprCore.DTO;
using MedprDB.Entities;
using MedprModels;
using MedprModels.Requests;

namespace MedprModels.Mapping;

public class DoctorProfile : Profile
{
    public DoctorProfile() 
    {
        CreateMap<Doctor, DoctorDTO>();
        CreateMap<DoctorDTO, Doctor>();

        CreateMap<DoctorDTO, DoctorModelResponse>()
            .ForMember(model => model.Id, opt => opt.MapFrom(dto => dto.Id))
            .ForMember(model => model.Name, opt => opt.MapFrom(dto => dto.Name))
            .ForMember(model => model.Experience, opt => opt.MapFrom(dto => dto.Experience))
            .ForMember(model => model.Links, opt => opt.Ignore());
        CreateMap<DoctorModelRequest, DoctorDTO>()
            .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.Id))
            .ForMember(dto => dto.Name, opt => opt.MapFrom(model => model.Name))
            .ForMember(dto => dto.Experience, opt => opt.MapFrom(model => model.Experience));
    }
}
