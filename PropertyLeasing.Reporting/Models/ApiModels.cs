namespace PropertyLeasing.Reporting.Models;

// Auth
public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiry { get; set; }
    public string Role { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}

// Properties
public class PropertyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int TotalUnits { get; set; }
    public int OccupiedUnits { get; set; }
    public int AvailableUnits { get; set; }
    public string PropertyType { get; set; } = string.Empty;
}

// Leases
public class LeaseDto
{
    public int Id { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public string PropertyName { get; set; } = string.Empty;
    public string UnitNumber { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal MonthlyRent { get; set; }
    public string Status { get; set; } = string.Empty;
}

// Occupancy Report
public class OccupancyReportDto
{
    public int TotalUnits { get; set; }
    public int OccupiedUnits { get; set; }
    public int AvailableUnits { get; set; }
    public double OccupancyRate { get; set; }
}

// Overdue Payments
public class OverduePaymentDto
{
    public int LeaseId { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public string PropertyName { get; set; } = string.Empty;
    public string UnitNumber { get; set; } = string.Empty;
    public decimal AmountDue { get; set; }
    public DateTime DueDate { get; set; }
    public int DaysOverdue { get; set; }
}

// Maintenance
public class MaintenanceRequestDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string TenantName { get; set; } = string.Empty;
    public string PropertyName { get; set; } = string.Empty;
    public string UnitNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
}

// Maintenance Backlog
public class MaintenanceBacklogItemDto
{
    public string Status { get; set; } = string.Empty;
    public int Count { get; set; }
}
