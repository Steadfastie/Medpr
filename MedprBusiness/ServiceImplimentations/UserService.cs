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

public class UserService : PasswordHash, IUserService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDTO> GetUsersByIdAsync(Guid id)
    {
        var entity = await _unitOfWork.Users.GetByIdAsync(id);
        var dto = _mapper.Map<UserDTO>(entity);

        return dto;
    }

    public async Task<List<UserDTO>> GetAllUsersAsync()
    {
        var entities = await _unitOfWork.Users.GetAllAsync();
        var dto = _mapper.Map<List<UserDTO>>(entities);

        return dto;
    }

    public Task<List<UserDTO>> GetUsersByPageNumberAndPageSizeAsync(int pageNumber, int pageSize)
    {
        var list = _unitOfWork.Users
            .Get()
            .Skip(pageSize * pageNumber)
            .Take(pageSize)
            .OrderBy(user => user.FullName)
            .Select(user => _mapper.Map<UserDTO>(user))
            .ToListAsync();
        return list;
    }

    public async Task<int> CreateUserAsync(UserDTO dto)
    {
        var entity = _mapper.Map<User>(dto);

        if (entity != null)
        {
            await _unitOfWork.Users.AddAsync(entity);
            return await _unitOfWork.Commit();
        }
        else
        {
            throw new ArgumentException(nameof(dto));
        }
    }

    public async Task<int> PatchUserAsync(Guid id, List<PatchModel> patchList)
    {
        await _unitOfWork.Users.PatchAsync(id, patchList);
        return await _unitOfWork.Commit();
    }

    public async Task<int> DeleteUserAsync(UserDTO dto)
    {
        var entity = _mapper.Map<User>(dto);

        if (entity != null)
        {
            _unitOfWork.Users.Remove(entity);
            return await _unitOfWork.Commit();
        }
        else
        {
            throw new ArgumentException(nameof(dto));
        }
    }
}
