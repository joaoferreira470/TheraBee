# Phase 9 - Real App Navigation

## Goal

Turn the Angular frontend into a routed product instead of a single operational shell.

## Implemented

- `/login` and `/register` entry routes.
- `/therapist` hub for the authenticated therapist.
- `/patients/:patientId` patient page.
- `/patients/:patientId/sessions/:sessionId` session page.
- Redirects for authenticated and unauthenticated users.
- Therapist name navigation back to the hub.
- Patient name navigation into the patient page.
- Patient detail editing connected to the backend.
- Therapist schedule visible from the hub.

## Current Flow

```text
Login / Register
  -> Therapist Hub
  -> Patient Page
  -> Session Page
```

## Notes

- The current Angular frontend still reuses the existing clinical API and local token storage.
- The next phase should focus on the clinical model:
  - goal types
  - context-aware goal selection
  - session goal assessments
  - score-driven dashboards

