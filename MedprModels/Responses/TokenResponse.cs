namespace MedprModels.Responses;

public class TokenResponse
{
    public Guid UserId { get; set; }
    public string AccessToken { get; set; }
    public string Login { get; set; }
    public string Role { get; set; }
    public DateTime TokenExpiration { get; set; }
}