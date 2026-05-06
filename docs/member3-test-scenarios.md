# Member 3 Test Scenarios

## Lease Workflow Test Cases

### Application Status Transitions

| # | Scenario | Starting Status | Action | Expected Result |
|---|----------|----------------|--------|-----------------|
| 1 | Move to screening | Pending | Property Manager calls `/screening` | Status changes to Screening |
| 2 | Invalid move to screening | Screening | Property Manager calls `/screening` again | 400 Bad Request |
| 3 | Approve application | Screening | Property Manager calls `/approve` | Status changes to Approved |
| 4 | Reject application | Screening | Property Manager calls `/reject` | Status changes to Rejected |
| 5 | Approve from Pending (skip) | Pending | Property Manager calls `/approve` | 400 Bad Request |
| 6 | Activate rejected application | Rejected | Property Manager calls `/activate` | 400 Bad Request |

### Lease Activation

| # | Scenario | Condition | Expected Result |
|---|----------|-----------|-----------------|
| 7 | Activate approved application | Unit is available, valid dates | Lease created, unit marked unavailable |
| 8 | Activate on unavailable unit | Unit `IsAvailable = false` | 400 Bad Request |
| 9 | Activate with end before start | `EndDate < StartDate` | 400 Bad Request |
| 10 | Activate with missing dates | No `StartDate` or `EndDate` | 400 Bad Request |
| 11 | Activate same unit twice | First lease active | 400 Bad Request on second attempt |

## Maintenance Workflow Test Cases

### Status Transitions

| # | Scenario | Starting Status | Action | Expected Result |
|---|----------|----------------|--------|-----------------|
| 12 | Assign request | Submitted | Property Manager assigns staff | Status changes to Assigned |
| 13 | Start work | Assigned | Maintenance Staff updates status | Status changes to In Progress |
| 14 | Resolve request | In Progress | Maintenance Staff updates status | Status changes to Resolved |
| 15 | Close request | Resolved | Property Manager or Tenant closes | Status changes to Closed |
| 16 | Skip Assigned (go directly to In Progress) | Submitted | Maintenance Staff updates status | 400 Bad Request |
| 17 | Skip In Progress (go directly to Resolved) | Assigned | Maintenance Staff updates status | 400 Bad Request |
| 18 | Close unresolved request | In Progress | Any role calls close | 400 Bad Request |
| 19 | Edit closed request | Closed | Maintenance Staff updates status | 400 Bad Request |

### Staff Assignment

| # | Scenario | Condition | Expected Result |
|---|----------|-----------|-----------------|
| 20 | Assign valid staff | Request is Submitted | Status changes to Assigned |
| 21 | Assign on already assigned request | Status is Assigned | 400 Bad Request |

## Role Access Test Cases

### Lease Workflow

| # | Scenario | Role | Expected Result |
|---|----------|------|-----------------|
| 22 | Tenant tries to approve application | Tenant | 403 Forbidden |
| 23 | Tenant tries to activate lease | Tenant | 403 Forbidden |
| 24 | Maintenance Staff tries to approve application | MaintenanceStaff | 403 Forbidden |
| 25 | Property Manager approves application | PropertyManager | 200 OK |
| 26 | Unauthenticated user calls approve | None | 401 Unauthorized |

### Maintenance Workflow

| # | Scenario | Role | Expected Result |
|---|----------|------|-----------------|
| 27 | Tenant tries to assign staff | Tenant | 400 Bad Request |
| 28 | Maintenance Staff tries to assign staff | MaintenanceStaff | 400 Bad Request |
| 29 | Property Manager assigns staff | PropertyManager | Status changes to Assigned |
| 30 | Maintenance Staff moves to In Progress | MaintenanceStaff | Status changes to In Progress |
| 31 | Property Manager tries to move to In Progress | PropertyManager | 400 Bad Request |
| 32 | Tenant tries to move to In Progress | Tenant | 400 Bad Request |

## Known Blockers From Other Members

| # | Blocker | Blocked Part | Waiting On |
|---|---------|-------------|------------|
| 1 | MVC application project does not exist | MVC controller skeletons (Part 7) | Member 4 or shared setup |
| 2 | MVC authentication workflow not implemented | Controller auth and role checks (Part 8) | Member 4 or shared setup |
| 3 | Razor views not built | Returning views from MVC controllers | Member 4 |
| 4 | No MaintenanceRequest controller exists yet | Testing maintenance workflow via API | Member 2 or shared setup |
| 5 | SignalR real-time board not implemented | Live status update testing | Outside Member 3 scope |
