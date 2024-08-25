using System.Text;
using System.Security.Cryptography;

namespace Cybersec.Service.Helpers;
public class HashPasswordHelper
{
    public static string PasswordHasher(string password)
    {
        password = password.Trim();
        using var sha256 = SHA256.Create();
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var saltBytes = Encoding.UTF8.GetBytes(Constants.PASSWORD_SALT);
        var bytes = passwordBytes.Concat(saltBytes).ToArray();
        var hashedBytes = sha256.ComputeHash(bytes);
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }
    public static bool IsEqual(string curPass, string oldPass)
        => PasswordHasher(curPass) == oldPass;
}
