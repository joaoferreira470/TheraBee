# Phase 11 - Clinical Session Model

## Objective

Refine the session model so the product behaves like a real clinical schedule:

- assessment and reassessment sessions work as macro checkpoints;
- therapeutic sessions work as the regular intervention loop;
- the UI only offers the right preset type for each session context;
- therapists cannot create or reschedule overlapping sessions.

This phase keeps the current session contract stable, while tightening the clinical workflow around it.

## What Was Implemented

### Session Semantics

- Session types are still stored through the current backend enum contract.
- The product UI now maps those types to the clinical language:
  - `Assessment`
  - `Intervention` as therapy sessions
  - `Reassessment`
- Assessment and reassessment sessions attach only `Areas`.
- Therapy sessions attach only `Objectives`.

### Scheduling Safety

- Added overlap validation when creating sessions.
- Added overlap validation when rescheduling sessions.
- Conflicting slots now return a `409 Conflict`.

### Angular Flow

- The routed session page remains in place.
- Session type labels in the UI are aligned with the clinical wording.
- The session picker continues to filter goals by session type.
- The session page can still edit, save, and cancel a session.

## Validation

- `dotnet build src/therabee.sln` passed.
- `npm.cmd run build` passed.

## What Is Still Missing

- `SessionGoalAssessment` with numeric scoring.
- Session-level clinical notes per area/objective.
- Dashboard trends based on those scores.
- Report visuals based on the same score history.

## Next Step

Move to `Phase 12: Session Goal Assessments`.
