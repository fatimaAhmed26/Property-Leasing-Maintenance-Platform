using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PropertyLeasing.Reporting.Services;

namespace PropertyLeasing.Reporting.Pages;

public class LogoutModel : PageModel
{
    private readonly ApiService _apiService;

    public LogoutModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    public IActionResult OnGet()
    {
        _apiService.ClearSession();
        return RedirectToPage("/Login");
    }

    public IActionResult OnPost()
    {
        _apiService.ClearSession();
        return RedirectToPage("/Login");
    }
}
