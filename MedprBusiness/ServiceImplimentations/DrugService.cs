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

        public Task<int> CreateArticleAsync(DrugDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task<DrugDTO> GetDrugsByIdAsync(Guid id)
        {
            var entity = await _dbContext.Drugs.FirstOrDefaultAsync(drug => drug.Id.Equals(id));
            var dto = _mapper.Map<DrugDTO>(entity);

            return dto;
        }

        public Task<List<DrugDTO>> GetDrugsByPageNumberAndPageSizeAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<List<DrugDTO>> GetNewArticlesFromExternalSourcesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
