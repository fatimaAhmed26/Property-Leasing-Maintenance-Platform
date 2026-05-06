# Member 3 Business Rules

Project: Property Leasing & Maintenance Platform

Member 3 scope: MVC business logic for lease lifecycle, maintenance request lifecycle, role-based workflow rules, validation, and error handling.

## Lease and Application Statuses

Valid application and lease lifecycle statuses:

- `Pending`
- `Screening`
- `Approved`
- `Rejected`
- `LeaseActive`
- `Renewed`
- `Terminated`

Valid transitions:

| Current Status | Allowed Next Status |
| --- | --- |
| `Pending` | `Screening` |
| `Screening` | `Approved`, `Rejected` |
| `Approved` | `LeaseActive` |
| `Rejected` | No further workflow transition |
| `LeaseActive` | `Renewed`, `Terminated` |
| `Renewed` | `Terminated` |
| `Terminated` | No further workflow transition |

## Lease and Application Rules

- A tenant can submit an application only for an available unit.
- A property manager can move an application from `Pending` to `Screening`.
- A property manager can approve or reject an application only when it is in `Screening`.
- A rejected application cannot become an active lease.
- A lease can become active only from an `Approved` application.
- A unit cannot have more than one active lease at the same time.
- A lease cannot be activated if the unit is unavailable.
- When a lease becomes active, the related unit should be marked unavailable.
- When a lease is terminated, the unit should not automatically become available unless business approval is completed.
- Renewed leases can later be terminated.
- Terminated leases cannot be renewed or reactivated without a new business process.

## Maintenance Request Statuses

Valid maintenance lifecycle statuses:

- `Submitted`
- `Assigned`
- `InProgress`
- `Resolved`
- `Closed`

Valid transitions:

| Current Status | Allowed Next Status |
| --- | --- |
| `Submitted` | `Assigned` |
| `Assigned` | `InProgress` |
| `InProgress` | `Resolved` |
| `Resolved` | `Closed` |
| `Closed` | No further workflow transition |

## Maintenance Request Rules

- A tenant can submit a maintenance request for their own unit.
- A submitted request must be assigned before work can begin.
- A property manager can assign a submitted request to maintenance staff.
- A request cannot move to `InProgress` before it is `Assigned`.
- A request cannot move to `Resolved` before it is `InProgress`.
- A request cannot move to `Closed` before it is `Resolved`.
- Maintenance staff can update only requests assigned to them.
- A tenant can view only their own maintenance requests.
- A property manager can view all maintenance requests.
- Closed requests should not be edited except by a property manager if a correction is required.

## Role Action Rules

| Action | Tenant | Property Manager | Maintenance Staff |
| --- | --- | --- | --- |
| Submit lease application | Yes, own application only | No | No |
| Move application to screening | No | Yes | No |
| Approve or reject application | No | Yes | No |
| Activate lease | No | Yes | No |
| Renew or terminate lease | No | Yes | No |
| Submit maintenance request | Yes, own unit only | Yes, if needed | No |
| Assign maintenance request | No | Yes | No |
| Start maintenance work | No | No | Yes, assigned request only |
| Resolve maintenance request | No | No | Yes, assigned request only |
| Close maintenance request | Yes, own request only | Yes | No |
| View all maintenance requests | No | Yes | Limited to assigned requests |

## Validation and Error Cases

Application and lease validation should reject:

- Missing application.
- Missing unit.
- Unknown or empty next status.
- Status transitions not listed in the valid transition table.
- Any application or lease status change attempted by a user who is not a property manager.
- Lease activation for an unavailable unit.
- Lease activation when another active lease already exists for the same unit.
- Lease activation from any status except `Approved`.
- Renewal or termination when the lease is not active or renewed.

Maintenance validation should reject:

- Missing maintenance request.
- Unknown or empty next status.
- Status transitions not listed in the valid transition table.
- Assignment without a valid maintenance staff member.
- Moving a request to `InProgress` before assignment.
- Resolving a request before work has started.
- Closing a request before it is resolved.
- Staff updates by users who are not assigned to the request.
- Tenant access to another tenant's maintenance request.

## Current Implementation Notes

- Lifecycle service skeletons already exist in `PropertyLeasingSystem/Services`.
- Lease transition validation exists, but full lease activation is blocked until the lease model supports fields such as tenant, unit, and status.
- Maintenance transition validation exists, but full assignment validation is blocked until the maintenance request model supports assigned staff and completion details.
- MVC controllers are not available yet because the MVC project has not been added.
