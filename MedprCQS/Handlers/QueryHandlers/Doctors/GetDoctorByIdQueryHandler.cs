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

public class GetDoctorByIdQueryHandler: IRequestHandler<GetDoctorByIdQuery, DoctorDTO?>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetDoctorByIdQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DoctorDTO> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Doctors
            .AsNoTracking()
            .FirstOrDefaultAsync(drug => drug.Id == request.Id, cancellationToken: cancellationToken);
        var drug = _mapper.Map<DoctorDTO>(entity);

        return drug;
    }
}
