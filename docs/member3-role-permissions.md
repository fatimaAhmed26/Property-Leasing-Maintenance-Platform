# Member 3 Role and Permission Plan

Project: Property Leasing & Maintenance Platform

Member 3 scope: role-based access rules for MVC business workflows. These rules should be enforced in controllers and services when the MVC workflow is added.

## Roles

The workflow uses three main roles:

- `Tenant`
- `PropertyManager`
- `MaintenanceStaff`

## Tenant Permissions

Allowed actions:

- View own lease and application information.
- Submit a lease application for an available unit.
- Submit a maintenance request for their own unit.
- View status of their own maintenance requests.
- Close their own resolved maintenance request if tenant confirmation is required.

Not allowed:

- View other tenants' applications, leases, or maintenance requests.
- Move applications to screening.
- Approve or reject applications.
- Activate, renew, or terminate leases.
- Assign maintenance requests.
- Update maintenance work status unless closing their own resolved request is allowed.

Authorization requirements:

- Tenant pages and actions require `[Authorize(Roles = "Tenant")]`.
- Any tenant-specific record access must include an ownership check using the authenticated tenant id.

Ownership checks:

- Tenant can access an application only when `Application.TenantId` matches the authenticated tenant id.
- Tenant can access a maintenance request only when `MaintenanceRequest.TenantId` matches the authenticated tenant id.
- Tenant can submit a maintenance request only for a unit connected to their active lease or approved tenancy.

## Property Manager Permissions

Allowed actions:

- View all applications.
- Move applications from `Pending` to `Screening`.
- Approve or reject applications in `Screening`.
- Activate approved leases.
- Renew or terminate active leases.
- View all maintenance requests.
- Assign submitted maintenance requests to maintenance staff.
- Close resolved maintenance requests.
- Correct closed maintenance requests if business approval is required.

Not allowed:

- Bypass valid application or lease status transitions.
- Activate a lease for an unavailable unit.
- Create multiple active leases for the same unit.
- Assign maintenance work to an invalid or missing staff member.

Authorization requirements:

- Manager workflow pages and actions require `[Authorize(Roles = "PropertyManager")]`.
- Manager actions should still call lifecycle validation services before saving status changes.

Ownership checks:

- Property managers can access records across tenants, but controller actions should still verify that the target application, unit, lease, request, or staff member exists.

## Maintenance Staff Permissions

Allowed actions:

- View maintenance requests assigned to them.
- Move assigned requests from `Assigned` to `InProgress`.
- Move assigned requests from `InProgress` to `Resolved`.
- Add maintenance notes or action details when the model supports it.

Not allowed:

- View unassigned maintenance requests unless explicitly allowed by the project.
- Assign requests to themselves or other staff.
- Approve, reject, or activate lease applications.
- Renew or terminate leases.
- Close requests unless the project later allows this role to close resolved work.

Authorization requirements:

- Staff workflow pages and actions require `[Authorize(Roles = "MaintenanceStaff")]`.
- Staff update actions must check that the request is assigned to the authenticated staff member.

Ownership checks:

- Maintenance staff can access a maintenance request only when `MaintenanceRequest.AssignedStaffId` matches the authenticated staff id.
- This check is blocked until the maintenance request model includes an assigned staff field.

## Action Permission Matrix

| Workflow Action | Required Role | Ownership or Extra Check |
| --- | --- | --- |
| Submit application | `Tenant` | Unit must be available |
| View own application | `Tenant` | Application tenant id must match current tenant |
| View all applications | `PropertyManager` | Target records must exist |
| Move application to screening | `PropertyManager` | Current status must be `Pending` |
| Approve application | `PropertyManager` | Current status must be `Screening` |
| Reject application | `PropertyManager` | Current status must be `Screening` |
| Activate lease | `PropertyManager` | Application must be `Approved`; unit must be available; no active lease can exist for unit |
| Renew lease | `PropertyManager` | Lease must be active |
| Terminate lease | `PropertyManager` | Lease must be active or renewed |
| Submit maintenance request | `Tenant` | Unit must belong to tenant |
| View own maintenance request | `Tenant` | Request tenant id must match current tenant |
| View all maintenance requests | `PropertyManager` | Target records must exist |
| View assigned maintenance requests | `MaintenanceStaff` | Request assigned staff id must match current staff |
| Assign maintenance request | `PropertyManager` | Request must be `Submitted`; staff member must exist |
| Start maintenance work | `MaintenanceStaff` | Request must be `Assigned` and assigned to current staff |
| Resolve maintenance request | `MaintenanceStaff` | Request must be `InProgress` and assigned to current staff |
| Close maintenance request | `Tenant` or `PropertyManager` | Request must be `Resolved`; tenant can close only own request |

## Controller Authorization Plan

Lease and application MVC controller actions:

- `Index` for managers: `[Authorize(Roles = "PropertyManager")]`
- `MyApplications` for tenants: `[Authorize(Roles = "Tenant")]`
- `SubmitApplication` for tenants: `[Authorize(Roles = "Tenant")]`
- `MoveToScreening` for managers: `[Authorize(Roles = "PropertyManager")]`
- `Approve` for managers: `[Authorize(Roles = "PropertyManager")]`
- `Reject` for managers: `[Authorize(Roles = "PropertyManager")]`
- `ActivateLease` for managers: `[Authorize(Roles = "PropertyManager")]`
- `RenewLease` for managers: `[Authorize(Roles = "PropertyManager")]`
- `TerminateLease` for managers: `[Authorize(Roles = "PropertyManager")]`

Maintenance MVC controller actions:

- `Index` for managers: `[Authorize(Roles = "PropertyManager")]`
- `MyRequests` for tenants: `[Authorize(Roles = "Tenant")]`
- `AssignedToMe` for staff: `[Authorize(Roles = "MaintenanceStaff")]`
- `Create` for tenants: `[Authorize(Roles = "Tenant")]`
- `Assign` for managers: `[Authorize(Roles = "PropertyManager")]`
- `StartWork` for staff: `[Authorize(Roles = "MaintenanceStaff")]`
- `Resolve` for staff: `[Authorize(Roles = "MaintenanceStaff")]`
- `Close` for tenants or managers: `[Authorize(Roles = "Tenant,PropertyManager")]`

## Service Validation Plan

Controllers should not trust role attributes alone. They should also call lifecycle services to validate:

- Current status.
- Requested next status.
- Authenticated user role.
- Tenant ownership.
- Staff assignment ownership.
- Unit availability.
- Existing active lease conflicts.

## Current Implementation Notes

- JWT role claims already exist in the API login workflow.
- Some API controllers already use `[Authorize]`.
- MVC controllers do not exist yet.
- Staff assignment ownership is blocked until `MaintenanceRequest` includes an assigned staff field.
- Full active lease checks are blocked until `Lease` includes tenant, unit, and status fields.
