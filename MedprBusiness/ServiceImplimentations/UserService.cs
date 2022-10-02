using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetSample.Core;
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

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<UserCredentialsDTO>> GetAllUsersAsync(Guid id)
    {
        List<User> users = _unitOfWork.Users.Get().ToList();
        List<UserCredentialsDTO> userCredentials = _mapper.Map<List<UserCredentialsDTO>>(users);

        return userCredentials;
    }

    public async Task<UserCredentialsDTO> GetUserByIdAsync(Guid id)
    {
        var entity = await _unitOfWork.Users.GetByIdAsync(id);
        var dto = _mapper.Map<UserCredentialsDTO>(entity);

        return dto;
    }

    public Task<List<UserCredentialsDTO>> GetUsersByPageNumberAndPageSizeAsync(int pageNumber, int pageSize)
    {
        var list = _unitOfWork.Users
            .Get()
            .Skip(pageSize * pageNumber)
            .Take(pageSize)
            .OrderBy(user => user.Name)
            .Select(user => _mapper.Map<UserCredentialsDTO>(user))
            .ToListAsync();
        return list;
    }
}
