using MedprModels.Responses;

namespace MedprCore.Abstractions;

public interface IJwtUtil
{
    TokenResponse GenerateToken(UserModelResponse dto);
}