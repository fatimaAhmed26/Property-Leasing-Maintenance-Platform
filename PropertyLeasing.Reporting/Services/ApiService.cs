using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PropertyLeasing.Reporting.Services;

public class ApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private const string TokenSessionKey = "jwt_token";
    private const string RoleSessionKey = "user_role";
    private const string NameSessionKey = "user_name";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public void SetToken(string token) =>
        _httpContextAccessor.HttpContext?.Session.SetString(TokenSessionKey, token);

    public string? GetToken() =>
        _httpContextAccessor.HttpContext?.Session.GetString(TokenSessionKey);

    public void SetRole(string role) =>
        _httpContextAccessor.HttpContext?.Session.SetString(RoleSessionKey, role);

    public string? GetRole() =>
        _httpContextAccessor.HttpContext?.Session.GetString(RoleSessionKey);

    public void SetUserName(string name) =>
        _httpContextAccessor.HttpContext?.Session.SetString(NameSessionKey, name);

    public string? GetUserName() =>
        _httpContextAccessor.HttpContext?.Session.GetString(NameSessionKey);

    public void ClearSession() =>
        _httpContextAccessor.HttpContext?.Session.Clear();

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        var client = CreateAuthorizedClient();
        var response = await client.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, JsonOptions);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest body)
    {
        var client = _httpClientFactory.CreateClient("API");
        var json = JsonSerializer.Serialize(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(endpoint, content);
        response.EnsureSuccessStatusCode();
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResponse>(responseJson, JsonOptions);
    }

    private HttpClient CreateAuthorizedClient()
    {
        var client = _httpClientFactory.CreateClient("API");
        var token = GetToken();
        if (!string.IsNullOrEmpty(token))
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }
}
