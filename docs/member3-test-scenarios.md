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
