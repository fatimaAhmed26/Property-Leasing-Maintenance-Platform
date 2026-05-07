using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PropertyLeasing.Reporting.Models;
using PropertyLeasing.Reporting.Services;

namespace PropertyLeasing.Reporting.Pages.Reports;

public class OverduePaymentsModel : PageModel
{
    private readonly ApiService _apiService;

    public OverduePaymentsModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    public List<OverduePaymentDto> OverduePayments { get; set; } = new();
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (_apiService.GetToken() == null)
            return RedirectToPage("/Login");

        try
        {
            var payments = await _apiService.GetAsync<List<OverduePaymentDto>>("api/leases/report/overdue-payments");
            OverduePayments = payments ?? new();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Could not load overdue payment data: {ex.Message}";
        }

        return Page();
    }
}
