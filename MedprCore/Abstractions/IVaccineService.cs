﻿using AspNetSample.Core;
using MedprCore.DTO;

namespace MedprCore.Abstractions
{
    public interface IVaccineService
    {
        Task<List<VaccineDTO>> GetVaccinesByPageNumberAndPageSizeAsync
            (int pageNumber, int pageSize);

        Task<VaccineDTO> GetVaccinesByIdAsync(Guid id);

        Task<int> CreateVaccineAsync(VaccineDTO dto);
        Task<int> PatchVaccineAsync(Guid id, List<PatchModel> patchList);
        Task<int> DeleteVaccineAsync(VaccineDTO dto);
    }
}