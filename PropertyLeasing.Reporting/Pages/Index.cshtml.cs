using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PropertyLeasing.Reporting.Services;

namespace PropertyLeasing.Reporting.Pages;

public class IndexModel : PageModel
{
    private readonly ApiService _apiService;

    public IndexModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    public IActionResult OnGet()
    {
        if (_apiService.GetToken() != null)
            return RedirectToPage("/Dashboard");
        return RedirectToPage("/Login");
    }
}
