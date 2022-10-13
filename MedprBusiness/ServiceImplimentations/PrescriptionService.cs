using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MedprAbstractions;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using MedprDB;
using MedprDB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MedprBusiness.ServiceImplementations;

public class PrescriptionService : IPrescriptionService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public PrescriptionService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<PrescriptionDTO> GetPrescriptionsByIdAsync(Guid id)
    {
        var entity = await _unitOfWork.Prescriptions.GetByIdAsync(id);
        var dto = _mapper.Map<PrescriptionDTO>(entity);

        return dto;
    }

    public Task<List<PrescriptionDTO>> GetPrescriptionsByPageNumberAndPageSizeAsync(int pageNumber, int pageSize)
    {
        var list = _unitOfWork.Prescriptions
            .Get()
            .Skip(pageSize * pageNumber)
            .Take(pageSize)
            .Select(prescription => _mapper.Map<PrescriptionDTO>(prescription))
            .ToListAsync();
        return list;
    }

    public Task<List<PrescriptionDTO>> GetPrescriptionsRelevantToUser(Guid id)
    {
        var list = _unitOfWork.Prescriptions
            .FindBy(prescription => prescription.UserId == id)
            .Select(prescription => _mapper.Map<PrescriptionDTO>(prescription))
            .ToListAsync();
        return list;
    }

    public async Task<int> CreatePrescriptionAsync(PrescriptionDTO dto)
    {
        var entity = _mapper.Map<Prescription>(dto);

        if (entity != null)
        {
            await _unitOfWork.Prescriptions.AddAsync(entity);
            return await _unitOfWork.Commit();
        }
        else
        {
            throw new ArgumentException(nameof(dto));
        }
    }

    public async Task<int> PatchPrescriptionAsync(Guid id, List<PatchModel> patchList)
    {
        await _unitOfWork.Prescriptions.PatchAsync(id, patchList);
        return await _unitOfWork.Commit();
    }

    public async Task<int> DeletePrescriptionAsync(PrescriptionDTO dto)
    {
        var entity = _mapper.Map<Prescription>(dto);

        if (entity != null)
        {
            _unitOfWork.Prescriptions.Remove(entity);
            return await _unitOfWork.Commit();
        }
        else
        {
            throw new ArgumentException(nameof(dto));
        }
    }
}
