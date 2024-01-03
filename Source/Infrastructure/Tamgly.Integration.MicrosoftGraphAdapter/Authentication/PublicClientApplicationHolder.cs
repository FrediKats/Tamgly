using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Broker;
using Microsoft.Kiota.Abstractions.Authentication;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Tamgly.Integration.MicrosoftGraphAdapter.Authentication;


public class TokenProvider : IAccessTokenProvider
{
    private readonly PublicClientApplicationHolder _applicationHolder;
    private readonly IReadOnlyCollection<string> _scopes;

    public TokenProvider(PublicClientApplicationHolder applicationHolder, IReadOnlyCollection<string> scopes)
    {
        _applicationHolder = applicationHolder;
        _scopes = scopes;
    }

    public async Task<string> GetAuthorizationTokenAsync(Uri uri, Dictionary<string, object>? additionalAuthenticationContext = null, CancellationToken cancellationToken = new CancellationToken())
    {
        AuthenticationResult authenticationResult = await _applicationHolder.Authenticate(_scopes);
        return authenticationResult.AccessToken;
    }

    public AllowedHostsValidator AllowedHostsValidator { get; } = new AllowedHostsValidator();
}

public class PublicClientApplicationHolder
{
    private const string Tenant = "common";

    private readonly IPublicClientApplication _clientApp;

    public PublicClientApplicationHolder(string clientId)
    {
        _clientApp = CreateApplication(clientId);
    }

    public async Task<AuthenticationResult> Authenticate(IReadOnlyCollection<string> scopes)
    {
        //IEnumerable<IAccount> accountsAsync = await _clientApp.GetAccountsAsync();

        AuthenticationResult authResult = await _clientApp.AcquireTokenInteractive(scopes)
            .WithAccount(null)
            .WithParentActivityOrWindow(ConsoleWindowCreator.GetConsoleOrTerminalWindow())
            .WithPrompt(Prompt.SelectAccount)
            .ExecuteAsync();

        return authResult;
    }

    private static IPublicClientApplication CreateApplication(string clientId)
    {
        PublicClientApplicationBuilder builder = PublicClientApplicationBuilder.Create(clientId)
            .WithAuthority($"{MicrosoftStaticLinks.MicrosoftAuthUrl}{Tenant}")
            .WithDefaultRedirectUri()
            .WithBrokerPreview();

        IPublicClientApplication? clientApp = builder.Build();
        TokenCacheHelper.EnableSerialization(clientApp.UserTokenCache);

        return clientApp;
    }
}