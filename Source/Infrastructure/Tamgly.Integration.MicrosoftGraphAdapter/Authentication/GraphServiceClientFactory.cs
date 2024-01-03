using Microsoft.Graph;
using Microsoft.Kiota.Abstractions.Authentication;
using System.Threading.Tasks;

namespace Tamgly.Integration.MicrosoftGraphAdapter.Authentication;

public class GraphServiceClientFactory
{
    private readonly PublicClientApplicationHolder _publicClientApplicationHolder;
    private readonly string[] _scopes;

    public GraphServiceClientFactory(PublicClientApplicationHolder publicClientApplicationHolder)
    {
        _publicClientApplicationHolder = publicClientApplicationHolder;
        _scopes = new[] { "User.Read", "Calendars.Read" };
    }

    public GraphServiceClientFactory(PublicClientApplicationHolder publicClientApplicationHolder, string[] scopes)
    {
        _publicClientApplicationHolder = publicClientApplicationHolder;
        _scopes = scopes;
    }

    public Task<GraphServiceClient> CreateIntegratedWindowsAuth()
    {
        var tokenProvider = new TokenProvider(_publicClientApplicationHolder, _scopes);
        var authenticationProvider = new BaseBearerTokenAuthenticationProvider(tokenProvider);
        var graphServiceClient = new GraphServiceClient(authenticationProvider);
        return Task.FromResult(graphServiceClient);
    }
}
