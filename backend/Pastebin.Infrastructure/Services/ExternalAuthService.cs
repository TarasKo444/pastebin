using System.Net.Http.Json;
using System.Text.Json.Serialization;
using ErrorOr;
using Flurl;
using Microsoft.Extensions.Options;
using Pastebin.Common.Options;

namespace Pastebin.Infrastructure.Services;

public record TokenResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}

public record UserInfoResponse
{
    [JsonPropertyName("id")]
    public required string Sub { get; set; }
    [JsonPropertyName("name")]
    public required string Username { get; set; } = null!;
    public required string Email { get; set; } = null!;
    public required string Picture { get; set; } = null!;
}

public class ExternalAuthService(IOptions<GoogleOAuthOptions> googleOAuthOptions, IHttpClientFactory httpClientFactory)
{
    private readonly IOptions<GoogleOAuthOptions> _googleOAuthOptions = googleOAuthOptions;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    
    private const string AuthUrl = "https://accounts.google.com/o/oauth2/v2/auth";
    private const string TokenUrl = "https://oauth2.googleapis.com/token";
    private const string UserInfoUrl = "https://www.googleapis.com/oauth2/v1/userinfo";

    public string GetGoogleAuthUrl(string callback)
    {
        var url = AuthUrl.SetQueryParams(new
        {
            client_id = _googleOAuthOptions.Value.ClientId,
            redirect_uri = callback,
            response_type = "code",
            scope = "email profile openid",
            access_type = "offline"
        });

        return url;
    }

    public async Task<ErrorOr<TokenResponse>> GetCredentials(string code, string redirectUrl)
    {
        var uri = TokenUrl.SetQueryParams(new
        {
            client_id = _googleOAuthOptions.Value.ClientId,
            client_secret = _googleOAuthOptions.Value.ClientSecret,
            grant_type = "authorization_code",
            redirect_uri = redirectUrl,
            code,
        }).ToUri();

        var client = _httpClientFactory.CreateClient();

        var response = await client.PostAsync(uri, null);

        if (!response.IsSuccessStatusCode)
        {
            return Error.Failure(description: "wrong code");
        }
        
        var json = (await response.Content.ReadFromJsonAsync<Dictionary<string, object>>())!;

        return new TokenResponse
        {
            AccessToken = json["access_token"].ToString()!,
            RefreshToken = json["refresh_token"].ToString()!
        };
    }

    public async Task<UserInfoResponse> GetUserInfo(string accessToken)
    {
        var uri = UserInfoUrl.SetQueryParams(new { access_token = accessToken }).ToUri();

        var client = _httpClientFactory.CreateClient();

        var response = await client.GetAsync(uri);

        return (await response.Content.ReadFromJsonAsync<UserInfoResponse>())!;
    }
}