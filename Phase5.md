# Phase 5 - Sessions And Checkpoints

## Goal

Add a session workflow that lets therapists schedule, reschedule, cancel, complete, and annotate patient sessions with structured checkpoints and linked therapeutic goals.

This phase is the bridge between goal setting and reporting:

- sessions can be tracked over time
- completed sessions can store structured clinical notes
- pending sessions can be identified for follow-up
- session goals can be linked to patient goals

## Implemented Scope

- `Session` domain model with:
  - `StartDateTime`
  - `EndDateTime`
  - `Type`
  - `Location`
  - `Status`
  - `CancellationReason`
  - `ClinicalSummary`
  - `ObjectivesWorked`
  - `ProgressRating`
  - `Activities`
  - `PatientResponse`
  - `Difficulties`
  - `Recommendations`
  - `NextSteps`
- `SessionGoal` linking entity for associating therapeutic goals with a session
- session DTOs for create, reschedule, cancel, complete, and goal linking flows
- application use cases for:
  - creating sessions
  - listing all sessions for the current therapist
  - listing sessions by patient
  - fetching a session by id
  - listing pending-registration sessions
  - rescheduling sessions
  - cancelling sessions
  - completing sessions
  - attaching therapeutic goals to a session
- Carter endpoints for the full session workflow
- persistence mappings and relationship configuration in EF Core
- session not-found error handling

## Validation

- `dotnet build src/therabee.sln`

The solution builds successfully after the Phase 5 changes.

## Notes

- Session ownership is enforced through the authenticated therapist across all endpoints and handlers.
- Pending-registration sessions are currently identified as sessions that are still scheduled and have already reached their end time.
- The model is intentionally structured so that later reporting can reuse the same session data without rewriting it from scratch.

## Next Step

Proceed to `Phase 6: Progress Dashboard`.
