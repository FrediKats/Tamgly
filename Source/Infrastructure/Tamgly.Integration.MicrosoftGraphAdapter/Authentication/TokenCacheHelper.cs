using Microsoft.Identity.Client;
using System.IO;
using System.Security.Cryptography;

namespace Tamgly.Integration.MicrosoftGraphAdapter.Authentication;

internal static class TokenCacheHelper
{
    private static readonly object FileLock = new object();
    private static string CacheFilePath { get; }

    static TokenCacheHelper()
    {
        CacheFilePath = "token.msalcache.bin3";
    }

    public static void BeforeAccessNotification(TokenCacheNotificationArgs args)
    {
        lock (FileLock)
        {
            byte[]? msalV3State = null;

            if (File.Exists(CacheFilePath))
                msalV3State = ProtectedData.Unprotect(File.ReadAllBytes(CacheFilePath), null, DataProtectionScope.CurrentUser);

            args.TokenCache.DeserializeMsalV3(msalV3State);
        }
    }

    public static void AfterAccessNotification(TokenCacheNotificationArgs args)
    {
        // if the access operation resulted in a cache update
        if (!args.HasStateChanged)
            return;

        lock (FileLock)
        {
            // reflect changes in the persistent store
            byte[] data = ProtectedData.Protect(args.TokenCache.SerializeMsalV3(), null, DataProtectionScope.CurrentUser);
            File.WriteAllBytes(CacheFilePath, data);
        }
    }

    internal static void EnableSerialization(ITokenCache tokenCache)
    {
        tokenCache.SetBeforeAccess(BeforeAccessNotification);
        tokenCache.SetAfterAccess(AfterAccessNotification);
    }
}