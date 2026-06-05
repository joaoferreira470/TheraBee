# Phase 7 - Reports And Export Pipeline

## Goal

Turn patient, goal, session, checkpoint, and dashboard data into a clinical report draft that therapists can review, edit, and export in both PDF and Word formats.

This phase is the output layer of the MVP:

- it captures a snapshot of the patient's progress
- it keeps a report history attached to the patient
- it lets the therapist refine the content before export
- it supports both PDF and Word export from the same report model

## Implemented Scope

- report domain model with patient and therapist ownership
- report draft generation from:
  - patient profile
  - therapeutic goals
  - session history
  - checkpoint notes
  - progress dashboard data
- editable report sections for:
  - patient snapshot
  - executive summary
  - attendance summary
  - goal progress summary
  - session summary
  - recommendations
  - additional notes
- report history listing by patient
- single report retrieval by id
- report update before export
- PDF export
- Word export
- exported documents stored back on the report history entry

## Validation

- `dotnet build src/therabee.sln`

The solution builds successfully after the Phase 7 changes.

## Notes

- No extra package dependency was required for the first implementation of PDF/Word export.
- The report export pipeline is intentionally lightweight so it can be extended later if richer formatting is needed.
- The report draft is reusable for future Angular views and for eventual PDF/Word download buttons.

## Next Step

Proceed to `Phase 8: Angular Frontend Integration`.
