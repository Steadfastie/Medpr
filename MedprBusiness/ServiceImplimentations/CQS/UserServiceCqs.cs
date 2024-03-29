﻿using AutoMapper;
using MediatR;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using MedprCQS.Commands.Users;
using MedprCQS.Queries.Users;

namespace MedprBusiness.ServiceImplimentations.Cqs;

public class UserServiceCqs : PasswordHash, IUserService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public UserServiceCqs(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<UserDTO> GetUserByIdAsync(Guid id)
    {
        return await _mediator.Send(new GetUserByIdQuery()
        {
            Id = id
        });
    }

    public async Task<UserDTO> GetUserByLoginAsync(string login)
    {
        return await _mediator.Send(new GetUserByLoginQuery()
        {
            Login = login
        });
    }

    public async Task<List<UserDTO>> GetAllUsersAsync()
    {
        return await _mediator.Send(new GetAllUsersQuery());
    }

    public async Task<int> CreateUserAsync(UserDTO dto)
    {
        return await _mediator.Send(new CreateUserCommand()
        {
            User = dto
        });
    }

    public async Task<int> PatchUserAsync(Guid id, List<PatchModel> patchList)
    {
        var user = await _mediator.Send(new GetUserByIdQuery()
        {
            Id = id
        });

        return await _mediator.Send(new PatchUserCommand()
        {
            User = user,
            PatchList = patchList
        });
    }

    public async Task<int> DeleteUserAsync(UserDTO dto)
    {
        return await _mediator.Send(new DeleteUserCommand() { User = dto });
    }
}