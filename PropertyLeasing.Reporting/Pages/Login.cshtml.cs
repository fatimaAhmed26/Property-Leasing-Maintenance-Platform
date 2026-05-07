using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PropertyLeasing.Reporting.Models;
using PropertyLeasing.Reporting.Services;
using System.ComponentModel.DataAnnotations;

namespace PropertyLeasing.Reporting.Pages;

public class LoginModel : PageModel
{
    private readonly ApiService _apiService;

    public LoginModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    [BindProperty]
    public LoginInputModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public IActionResult OnGet()
    {
        // Already logged in — go to Dashboard
        if (_apiService.GetToken() != null)
            return RedirectToPage("/Dashboard");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            var loginRequest = new LoginRequest
            {
                Email = Input.Email,
                Password = Input.Password
            };

            var response = await _apiService.PostAsync<LoginRequest, LoginResponse>(
                "api/auth/login", loginRequest);

            if (response == null || string.IsNullOrEmpty(response.Token))
            {
                ErrorMessage = "Invalid credentials. Please try again.";
                return Page();
            }

            if (!string.Equals(response.Role, "Property Manager", StringComparison.OrdinalIgnoreCase))
            {
                ErrorMessage = "Access denied. This portal is restricted to Property Managers.";
                return Page();
            }

            _apiService.SetToken(response.Token);
            _apiService.SetRole(response.Role);
            _apiService.SetUserName(response.FullName);

            return RedirectToPage("/Dashboard");
        }
        catch (HttpRequestException ex)
        {
            ErrorMessage = $"Unable to connect to the API server. {ex.Message}";
            return Page();
        }
        catch (Exception)
        {
            ErrorMessage = "Login failed. Please check your credentials and try again.";
            return Page();
        }
    }

    public class LoginInputModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
