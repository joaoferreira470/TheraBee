# Phase 8: Angular Frontend Integration

## Goal

Connect the Angular client to the backend capabilities already available in the Patients service, turning the previous prototype into an operational MVP interface.

The frontend should support the first real product loop:

```text
Therapist login -> Patient -> Goals -> Sessions -> Checkpoints -> Progress Dashboard -> Report Draft -> PDF or Word export
```

## Scope

- Add backend API integration through Angular `HttpClient`.
- Support JWT login and registration against the backend auth endpoints.
- Store the access token locally for development convenience.
- Load therapist-owned patients from the backend.
- Create patients using the full MVP patient shape.
- Select a patient and load related goals, sessions, dashboard data, and reports.
- Create therapeutic goals for the selected patient.
- Schedule sessions for the selected patient.
- Complete sessions with structured checkpoint fields.
- Display progress dashboard metrics for attendance and goal completion.
- Generate report drafts from backend data.
- Export reports to PDF and Word through backend endpoints.
- Discard report drafts that should not remain in patient history.
- Apply a responsive visual design suitable for clinical daily use.

## Out Of Scope

- Production-grade auth hardening.
- Full patient edit screen.
- Goal status management from the UI.
- Session cancel/reschedule UI actions.
- Report rich-text editor.
- Offline mode.
- Automated frontend tests.

## Implemented UI Areas

### Authentication

- Login screen.
- Registration screen.
- JWT token storage in `localStorage`.
- Logout action.

### Patients

- Patient selector.
- Patient list.
- Patient creation form.
- Patient detail summary through loaded patient data.
- Patient-centered page with clinical data, goals, sessions, and report shortcuts.

### Goals

- Goal creation form.
- Goal list for the selected patient.
- Goal status actions for in-progress, achieved, and suspended states.

### Sessions And Checkpoints

- Session scheduling form.
- Session list for the selected patient.
- Session checkpoint completion form.

### Progress Dashboard

- Attendance metrics.
- Goal completion metrics.
- Last session indicator.
- Next session indicator.
- Pending registration count.

### Reports

- Report draft generation.
- Patient report history.
- PDF export action.
- Word export action.
- Draft discard action.
- Report draft content generated in Portuguese from Portugal.

## API Base URL

The Angular client currently expects the backend API at:

```text
http://localhost:6001
```

This should become environment-based before deployment.

## Manual Run

From the repository root:

```powershell
cd src\Clients\therabee-web
npm.cmd install
npm.cmd run start
```

Default local URL:

```text
http://localhost:4200
```

The Patients API and PostgreSQL container must also be running for real data.

## Validation Checklist

- [x] Angular build succeeds.
- [x] Auth flow calls backend endpoints.
- [x] Patient list/detail/create flow calls backend endpoints.
- [x] Goal creation and listing call backend endpoints.
- [x] Session scheduling and checkpoint completion call backend endpoints.
- [x] Progress dashboard consumes backend endpoint.
- [x] Report draft and PDF/Word export call backend endpoints.
- [x] Report drafts can be discarded from the frontend.
- [x] UI adapts to desktop, tablet, and mobile layouts.

## Follow-Up

- Add environment files for API base URL.
- Add patient update UI.
- Add goal status update UI.
- Add session cancel/reschedule UI.
- Add report edit UI.
- Add frontend tests once the UX stabilizes.
