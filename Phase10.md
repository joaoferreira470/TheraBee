# Phase 10 - Clinical Goal Model

## Objective

Separate therapeutic goals into two clinical layers:

- `LongTerm` goals for macro clinical direction and reassessment checkpoints.
- `ShortTerm` goals for operational work inside therapy sessions.

The goal of this phase was to make goals reusable definitions that can be attached to many sessions over time, while keeping the UI context-aware and the backend clinically safe.

## What Was Implemented

### Domain And Persistence

- Added `TherapeuticGoalType` with `LongTerm` and `ShortTerm`.
- Extended the `TherapeuticGoal` entity with:
  - `Type`
  - `ParentGoalId`
  - parent/child navigation properties
- Added EF Core mapping for the self-referencing goal hierarchy.
- Created a migration for the new schema.

### Application Layer

- Updated goal DTOs to carry `Type` and `ParentGoalId`.
- Updated goal mapping extensions.
- Updated create/update goal commands and handlers.
- Added validation so only short-term goals can reference a parent long-term goal.
- Added backend validation so session goal selection must match session type.
- Added a dedicated session-goal replacement endpoint so the Angular session page can persist goal selections.

### Angular UI

- Goal creation/editing now supports selecting:
  - long-term goals
  - short-term goals
  - parent long-term goal for short-term goals
- Patient page groups goals by type.
- Session goal selection is contextual:
  - therapy sessions show short-term goals
  - assessment/reassessment sessions show long-term goals
- Session creation/editing keeps selected goals aligned with the current session type.
- Session editing persists selected goals through the replacement endpoint.

## Current Clinical Rule Set

```text
LongTermGoal
  -> may parent ShortTermGoal

TherapySession
  -> should use ShortTermGoal presets

AssessmentSession / ReassessmentSession
  -> should use LongTermGoal presets
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
