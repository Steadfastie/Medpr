using MedprCore.DTO;

namespace MedprCore.Abstractions
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetUsersByPageNumberAndPageSizeAsync
            (int pageNumber, int pageSize);

        Task<UserDTO> GetUsersByIdAsync(Guid id);
        Task<List<UserDTO>> GetAllUsersAsync();

        Task<int> CreateUserAsync(UserDTO dto);
        Task<int> PatchUserAsync(Guid id, List<PatchModel> patchList);
        Task<int> DeleteUserAsync(UserDTO dto);
    }
}