using AspNetSample.Core;
using MedprCore.DTO;

namespace MedprCore.Abstractions
{
    public interface IUserService
    {
        Task<UserCredentialsDTO> GetUserByIdAsync(Guid id);
        Task<List<UserCredentialsDTO>> GetAllUsersAsync(Guid id);
        Task<List<UserCredentialsDTO>> GetUsersByPageNumberAndPageSizeAsync
            (int pageNumber, int pageSize);
    }
}