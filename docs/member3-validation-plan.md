# Member 3 Validation Logic Plan

Project: Property Leasing & Maintenance Platform

Member 3 scope: validation rules for lease lifecycle, application decisions, maintenance assignment, maintenance status changes, and workflow error handling.

## Validation Approach

Business validation should happen in lifecycle services before controllers save changes.

Controllers should handle:

- Authentication and role attributes.
- Loading the required database records.
- Calling the lifecycle service.
- Returning validation errors to the user.
- Saving changes only when validation succeeds.

Lifecycle services should handle:

- Status transition rules.
- Role-based workflow rules.
- Ownership checks when the needed ids are available.
- Business conflicts such as double leasing.
- Clear validation messages for invalid actions.

## Lease Activation Validation

Before activating a lease, validate:

- Application exists.
- Unit exists.
- Authenticated user has `PropertyManager` role.
- Application status is `Approved`.
- Unit is available.
- No other active lease exists for the same unit.
- Required lease fields are available, including tenant id, unit id, start date, end date, and status.
- Lease end date is later than lease start date.

Expected success behavior:

- Application status changes to `LeaseActive`.
- Lease status is saved as active.
- Unit availability changes to unavailable.
- Status history is recorded if the status history feature is available.

Error messages:

- `Application was not found.`
- `Unit was not found.`
- `Only a Property Manager can activate a lease.`
- `Only approved applications can become active leases.`
- `This unit is not available for leasing.`
- `This unit already has an active lease.`
- `Lease start date must be before lease end date.`

Current blockers:

- `Lease` does not currently include tenant id, unit id, or status.
- Full active lease conflict checks need those lease fields.

## Application Approval Validation

Before approving an application, validate:

- Application exists.
- Authenticated user has `PropertyManager` role.
- Current application status is `Screening`.
- Related tenant exists.
- Related unit exists.
- Unit is still available.

Expected success behavior:

- Application status changes to `Approved`.
- No lease is activated yet.
- Unit remains available until lease activation.

Error messages:

- `Application was not found.`
- `Only a Property Manager can approve applications.`
- `Only applications in screening can be approved.`
- `Tenant was not found.`
- `Unit was not found.`
- `This unit is no longer available.`

## Application Rejection Validation

Before rejecting an application, validate:

- Application exists.
- Authenticated user has `PropertyManager` role.
- Current application status is `Screening`.
- Rejection reason is provided if the UI requires one.

Expected success behavior:

- Application status changes to `Rejected`.
- No lease is created.
- Unit availability is not changed.

Error messages:

- `Application was not found.`
- `Only a Property Manager can reject applications.`
- `Only applications in screening can be rejected.`
- `Rejection reason is required.`

Current blockers:

- The application model does not currently include a rejection reason field.

## Application Screening Validation

Before moving an application to screening, validate:

- Application exists.
- Authenticated user has `PropertyManager` role.
- Current application status is `Pending`.

Expected success behavior:

- Application status changes to `Screening`.

Error messages:

- `Application was not found.`
- `Only a Property Manager can move applications to screening.`
- `Only pending applications can move to screening.`

## Lease Renewal Validation

Before renewing a lease, validate:

- Lease exists.
- Authenticated user has `PropertyManager` role.
- Lease status is `LeaseActive`.
- Renewal dates are valid.
- Renewal end date is later than renewal start date.

Expected success behavior:

- Lease status changes to `Renewed`.
- Renewal dates are saved if the model supports them.

Error messages:

- `Lease was not found.`
- `Only a Property Manager can renew leases.`
- `Only active leases can be renewed.`
- `Renewal start date must be before renewal end date.`

Current blockers:

- `Lease` does not currently include status or renewal-specific fields.

## Lease Termination Validation

Before terminating a lease, validate:

- Lease exists.
- Authenticated user has `PropertyManager` role.
- Lease status is `LeaseActive` or `Renewed`.
- Termination date is valid.
- Termination reason is provided if the UI requires one.

Expected success behavior:

- Lease status changes to `Terminated`.
- Unit availability is not automatically changed unless business approval is completed.

Error messages:

- `Lease was not found.`
- `Only a Property Manager can terminate leases.`
- `Only active or renewed leases can be terminated.`
- `Termination reason is required.`

Current blockers:

- `Lease` does not currently include status, termination date, or termination reason fields.

## Maintenance Assignment Validation

Before assigning a maintenance request, validate:

- Maintenance request exists.
- Authenticated user has `PropertyManager` role.
- Request status is `Submitted`.
- Maintenance staff member exists.
- Staff member role is `MaintenanceStaff`.

Expected success behavior:

- Request assigned staff id is saved.
- Request status changes to `Assigned`.
- Assignment note or log is saved if the model supports it.

Error messages:

- `Maintenance request was not found.`
- `Only a Property Manager can assign maintenance requests.`
- `Only submitted requests can be assigned.`
- `Maintenance staff member was not found.`
- `Selected staff member is not maintenance staff.`

Current blockers:

- `MaintenanceRequest` does not currently include assigned staff id.

## Maintenance Status Change Validation

Before changing maintenance status, validate:

- Maintenance request exists.
- Next status is not empty.
- Requested transition is valid.
- Authenticated user role is allowed for the requested transition.
- Maintenance staff can update only assigned requests.
- Tenant can close only their own resolved request if tenant closure is allowed.

Valid transitions:

| Current Status | Next Status | Allowed Role |
| --- | --- | --- |
| `Submitted` | `Assigned` | `PropertyManager` |
| `Assigned` | `InProgress` | `MaintenanceStaff` |
| `InProgress` | `Resolved` | `MaintenanceStaff` |
| `Resolved` | `Closed` | `Tenant`, `PropertyManager` |

Error messages:

- `Maintenance request was not found.`
- `Next status is required.`
- `Maintenance status cannot change from {currentStatus} to {nextStatus}.`
- `Your role is not allowed to perform this status change.`
- `Maintenance staff can only update requests assigned to them.`
- `Tenant can only close their own maintenance request.`
- `Only resolved requests can be closed.`

Current blockers:

- Assigned staff ownership needs an assigned staff id field.
- Completion details need model support if the project requires them before resolution.

## Controller Error Handling Plan

For MVC controllers:

- If model state is invalid, return the same view with validation messages.
- If a lifecycle validation fails, add the error to `ModelState` or `TempData`.
- If a record does not exist, return `NotFound`.
- If the authenticated user does not own a tenant record, return `Forbid`.
- If role authorization fails, rely on `[Authorize]` and return the framework authorization result.
- After successful workflow actions, redirect to the relevant details or list page.

For API controllers:

- Return `400 Bad Request` for lifecycle validation failures.
- Return `401 Unauthorized` when the user is not authenticated.
- Return `403 Forbidden` when the user is authenticated but not allowed.
- Return `404 Not Found` when the target record does not exist.
- Return `200 OK` or `204 No Content` after successful status changes.

## Current Implementation Notes

- `LeaseLifecycleService` already validates application transitions and unit availability for lease activation.
- `MaintenanceLifecycleService` already validates maintenance status transitions and role permissions.
- Existing services mostly validate only; they do not yet perform database updates.
- Existing API controllers do not currently call the lifecycle services.
- MVC controllers are still missing.
