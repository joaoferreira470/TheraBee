# Phase 6 - Progress Dashboard

## Goal

Expose a patient progress dashboard that turns sessions and therapeutic goals into a clinical summary the therapist can scan quickly and later reuse for reporting.

This phase focuses on metrics that answer:

- how many sessions have happened
- how attendance is evolving
- how goals are progressing
- what the last session was
- what the next session is
- which sessions still need registration

## Implemented Scope

- patient progress dashboard query for a single patient
- attendance metrics:
  - total sessions
  - completed sessions
  - cancelled sessions
  - no-show sessions
  - upcoming sessions
  - pending registration sessions
  - attendance rate
- goal progress metrics:
  - total goals
  - not started goals
  - in progress goals
  - achieved goals
  - suspended goals
  - completion rate
- last session indicator
- next session indicator
- dashboard payload ready to be consumed by the Angular frontend and later reused by report generation

## Validation

- `dotnet build src/therabee.sln`

The solution builds successfully after the Phase 6 changes.

## Notes

- The dashboard is computed from the existing `Sessions` and `TherapeuticGoals` data.
- No new database tables were required for this phase.
- The payload is intentionally report-friendly so that a later report module can reuse the same summary without duplicating business logic.

## Next Step

Proceed to `Phase 7: Reports And PDF`.
