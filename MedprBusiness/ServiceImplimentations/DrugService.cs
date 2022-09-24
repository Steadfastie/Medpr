using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using MedprDB;
using MedprDB.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedprBusiness.ServiceImplementations
{
    public class DrugService : IDrugService
    {
        private readonly MedprDBContext _dbContext;
        private readonly IMapper _mapper;

        public DrugService(MedprDBContext dbcontext, IMapper mapper)
        {
            _dbContext = dbcontext;
            _mapper = mapper;
        }

        public async Task<DrugDTO> GetDrugsByIdAsync(Guid id)
        {
            var entity = await _dbContext.Drugs.FirstOrDefaultAsync(drug => drug.Id.Equals(id));
            var dto = _mapper.Map<DrugDTO>(entity);

            return dto;
        }

        public Task<List<DrugDTO>> GetDrugsByPageNumberAndPageSizeAsync(int pageNumber, int pageSize)
        {
            var list = _dbContext.Drugs
                .Skip(pageSize * pageNumber)
                .Take(pageSize)
                .Select(drug => _mapper.Map<DrugDTO>(drug))
                .ToListAsync();
            return list;
        }

        public async Task<int> CreateArticleAsync(DrugDTO dto)
        {
            var entity = _mapper.Map<Drug>(dto);

            if (entity != null)
            {
                await _dbContext.Drugs.AddAsync(entity);
                var addingResult = await _dbContext.SaveChangesAsync();
                return addingResult;
            }
            else
            {
                throw new ArgumentException(nameof(dto));
            }
        }

        


        public Task<List<DrugDTO>> GetNewArticlesFromExternalSourcesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
