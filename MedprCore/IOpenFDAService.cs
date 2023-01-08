using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCore;

public interface IOpenFDAService
{
    Task<DrugDTO> GetRandomDrug();
}
