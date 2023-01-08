using MedprCore.DTO;

namespace MedprCore.Abstractions;

public interface IUserService
{
    Task<UserDTO> GetUserByIdAsync(Guid id);

    Task<UserDTO> GetUserByLoginAsync(string login);

    Task<List<UserDTO>> GetAllUsersAsync();

    Task<int> CreateUserAsync(UserDTO dto);

    Task<int> PatchUserAsync(Guid id, List<PatchModel> patchList);

    Task<int> DeleteUserAsync(UserDTO dto);
}