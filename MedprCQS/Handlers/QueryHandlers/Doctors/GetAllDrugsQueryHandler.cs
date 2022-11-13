using AutoMapper;
using MediatR;
using MedprCore.DTO;
using MedprCQS.Queries.Doctors;
using MedprDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Handlers.QueryHandlers.Doctors;

public class GetAllDoctorsQueryHandler: IRequestHandler<GetAllDoctorsQuery, List<DoctorDTO>>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetAllDoctorsQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<DoctorDTO>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _context.Doctors
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
        var drugs = _mapper.Map<List<DoctorDTO>>(entities);

        return drugs;
    }
}
