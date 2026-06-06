# Phase 12 - Session Goal Assessments

## Objective

Add structured clinical scoring to the attached goals of a session so the product can keep a real progress record for later dashboards and reports.

## What Was Implemented

### Domain And Persistence

- Added `SessionGoalAssessment` as a first-class domain entity.
- Each assessment stores:
  - session id
  - goal id
  - therapist id
  - score from `0` to `10`
  - optional clinical notes
- Added EF Core configuration and a new database table.
- Added the entity to the current application DbContext.

### Application Layer

- Added create/update assessment command support.
- Added validation for the `0` to `10` score range.
- Added assessment listing by session.
- Added assessment listing by patient.
- Session DTOs now include the stored assessment data.

### Angular Frontend

- Added goal assessment controls to the routed session page.
- Added score inputs and clinical note fields per attached goal.
- Updated the `Confirmar` action to submit the assessments and then complete the session.
- Added a patient-level assessment summary panel.

## Validation

- `dotnet build src/therabee.sln` passed.
- `npm.cmd run build` passed.
- EF migration `AddSessionGoalAssessments` was created and applied locally.

## Next Step

Move to `Phase 13: Evidence-Based Dashboards And Reports`.
