using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PropertyLeasing.Reporting.Models;
using PropertyLeasing.Reporting.Services;

namespace PropertyLeasing.Reporting.Pages.Reports;

public class OccupancyModel : PageModel
{
    private readonly ApiService _apiService;

    public OccupancyModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    public OccupancyReportDto? Summary { get; set; }
    public List<PropertyDto> Properties { get; set; } = new();
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (_apiService.GetToken() == null)
            return RedirectToPage("/Login");

        var errors = new List<string>();

        try
        {
            Summary = await _apiService.GetAsync<OccupancyReportDto>("api/leases/report/occupancy");
        }
        catch
        {
            errors.Add("Could not load occupancy summary.");
        }

        try
        {
            var props = await _apiService.GetAsync<List<PropertyDto>>("api/properties");
            Properties = props ?? new();
        }
        catch
        {
            errors.Add("Could not load property list.");
        }

        if (errors.Count > 0)
            ErrorMessage = string.Join(" | ", errors);

        return Page();
    }
}
