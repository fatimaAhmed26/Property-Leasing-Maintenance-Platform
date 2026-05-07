using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PropertyLeasing.Reporting.Models;
using PropertyLeasing.Reporting.Services;

namespace PropertyLeasing.Reporting.Pages.Reports;

public class MaintenanceBacklogModel : PageModel
{
    private readonly ApiService _apiService;

    public MaintenanceBacklogModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    public List<MaintenanceBacklogItemDto> Backlog { get; set; } = new();
    public List<MaintenanceRequestDto> Requests { get; set; } = new();
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (_apiService.GetToken() == null)
            return RedirectToPage("/Login");

        var errors = new List<string>();

        try
        {
            var backlog = await _apiService.GetAsync<List<MaintenanceBacklogItemDto>>("api/maintenance/report/backlog");
            Backlog = backlog ?? new();
        }
        catch
        {
            errors.Add("Could not load backlog summary.");
        }

        try
        {
            var requests = await _apiService.GetAsync<List<MaintenanceRequestDto>>("api/maintenance");
            Requests = requests ?? new();
        }
        catch
        {
            errors.Add("Could not load maintenance requests.");
        }

        if (errors.Count > 0)
            ErrorMessage = string.Join(" | ", errors);

        return Page();
    }
}
