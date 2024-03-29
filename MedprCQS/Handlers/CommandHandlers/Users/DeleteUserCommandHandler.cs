﻿using AutoMapper;
using MediatR;
using MedprCQS.Commands.Users;
using MedprDB;
using MedprDB.Entities;

namespace MedprCQS.Handlers.CommandHandlers.Users;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, int>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public DeleteUserCommandHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<User>(request.User);

        if (entity != null)
        {
            _context.Users.Remove(entity);
            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException(nameof(request.User));
        }
    }
}