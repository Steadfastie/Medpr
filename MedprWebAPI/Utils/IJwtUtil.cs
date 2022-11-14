using MedprModels.Responses;

namespace AspNetSample.WebAPI.Utils;

public interface IJwtUtil
{
    TokenResponse GenerateToken(UserModelResponse dto);
}