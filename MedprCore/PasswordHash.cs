namespace MedprCore;

public class PasswordHash
{
    public static string CreateMd5(string password)
    {
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hashBytes = md5.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes);
        }
    }
}