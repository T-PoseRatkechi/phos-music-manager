namespace Phos.MusicManager.Library.Common;

using System.Security.Cryptography;

#pragma warning disable SA1600 // Elements should be documented
public static class Checksum
{
    public static string Compute(string inputFile)
    {
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(inputFile);
        var hash = md5.ComputeHash(stream);
        return Convert.ToHexString(hash);
    }
}
