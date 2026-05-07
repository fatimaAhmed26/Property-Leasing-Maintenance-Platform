using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PropertyLeasing.Reporting.Models;
using PropertyLeasing.Reporting.Services;

namespace PropertyLeasing.Reporting.Pages;

public class DashboardModel : PageModel
{
    private readonly ApiService _apiService;

    public DashboardModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    public OccupancyReportDto? Occupancy { get; set; }
    public List<MaintenanceBacklogItemDto> MaintenanceBacklog { get; set; } = new();
    public int OverdueCount { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (_apiService.GetToken() == null)
            return RedirectToPage("/Login");

        var errors = new List<string>();

        try
        {
            Occupancy = await _apiService.GetAsync<OccupancyReportDto>("api/leases/report/occupancy");
        }
        catch
        {
            errors.Add("Could not load occupancy data.");
        }

        try
        {
            var backlog = await _apiService.GetAsync<List<MaintenanceBacklogItemDto>>("api/maintenance/report/backlog");
            MaintenanceBacklog = backlog ?? new();
        }
        catch
        {
            errors.Add("Could not load maintenance backlog.");
        }

        try
        {
            var overdue = await _apiService.GetAsync<List<OverduePaymentDto>>("api/leases/report/overdue-payments");
            OverdueCount = overdue?.Count ?? 0;
        }
        catch
        {
            errors.Add("Could not load overdue payments.");
        }

        if (errors.Count > 0)
            ErrorMessage = string.Join(" | ", errors);

        return Page();
    }
}
