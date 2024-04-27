using System.Security.Cryptography;

namespace Pastebin.Common;

public static class Hasher
{
    public static string GenerateHash(int length)
    {
        var randomNumber = new byte[length];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public static string GenerateAlphabeticString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var charLen = chars.Length;
        var stringChars = new char[length];
        var random = new Random();

        for (int i = 0; i < length; i++)
        {
            stringChars[i] = chars[random.Next(charLen)];
        }

        return new string(stringChars);
    }
}
