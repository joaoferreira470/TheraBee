# Phase 10 - Clinical Goal Model

## Objective

Separate therapeutic definitions into two clinical layers:

- `Area` for macro clinical domains.
- `Objective` for concrete therapeutic goals.

The goal of this phase was to make areas and objectives reusable patient-owned presets that can be attached to many sessions over time, while keeping the backend clinically safe.

Areas and objectives are independent definitions. An objective does not need to belong to an area.

## What Was Implemented

### Domain And Persistence

- Added `TherapeuticGoalType` with `Area` and `Objective`.
- Extended the `TherapeuticGoal` entity with:
  - `Type`
- Removed the previous parent/child goal relationship.
- Added EF Core mapping for the independent goal type.
- Created migrations for the schema changes.

### Application Layer

- Updated goal DTOs to carry `Type`.
- Updated goal mapping extensions.
- Updated create/update goal commands and handlers.
- Added backend validation so session goal selection belongs to the same patient and therapist.
- Added backend validation so session goal selection matches the session type:
  - assessment and reassessment sessions can attach only areas
  - therapeutic sessions can attach only objectives
- Added a dedicated session-goal replacement endpoint so the Angular session page can persist goal selections.

### Angular UI

- Goal creation/editing now supports selecting:
  - areas
  - objectives
- Patient page groups clinical definitions by type.
- Session selection shows the correct preset type for the current session type.
- Session creation/editing keeps selected goals aligned with the current session type.
- Session editing persists selected goals through the replacement endpoint.

## Current Clinical Rule Set

```text
Area
  -> independent preset used in Assessment/Reassessment sessions

Objective
  -> independent preset used in Therapy sessions

Session
  -> Assessment/Reassessment can attach Areas
  -> Therapy can attach Objectives
  -> attached items can later be assessed from 0 to 10
```

## Validation

- `.NET solution build` passed.
- Angular build passed.
- The routed Angular flow was already validated in-browser earlier in the navigation phase.

## What Is Still Missing

- `SessionGoalAssessment` with numeric scores from `0` to `10`.
- Dashboard trends based on those scores.
- Report visuals that use the same score history.
- Schedule overlap validation for create/reschedule.

## Next Step

Move to `Phase 11: Clinical Session Model`.
