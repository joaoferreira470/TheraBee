# TheraBee Backend Implementation Plan

This plan translates `MVP_SCOPE.md` into implementation branches and backend modules.

The current backend already has a `Patients` service using a layered structure:

```text
API -> Application -> Domain -> Infrastructure
```

The recommended approach is to keep this style and add new modules gradually.

## Architecture Direction

Keep the system microservice-ready, but do not split every module into a separately deployed service immediately.

Recommended first approach:

- preserve DDD-style boundaries;
- keep CQRS handlers per use case;
- use modules/services with clear ownership;
- share only generic building blocks;
- avoid cross-module domain leakage.

## Branch Strategy

Use one branch per meaningful implementation phase:

- `feature/auth-therapist-profile`
- `feature/patients-v2`
- `feature/sessions`
- `feature/therapy-plans`
- `feature/dashboard`
- `feature/reports`
- `feature/angular-api-integration`

Before starting each branch:

- pull latest `main`;
- create the branch from the correct base;
- confirm whether planning changes should be merged first;
- avoid mixing unrelated modules in one branch.

## Phase 1: Auth And Therapist Profile

Goal:

Allow a therapist to register, log in, and own their data.

Recommended modules:

- `Auth`
- `Users`
- `Therapists`

Core entities:

- `User`
- `TherapistProfile`

Suggested endpoints:

```http
POST /auth/register
POST /auth/login
POST /auth/logout
GET /therapists/me
PUT /therapists/me
```

Implementation notes:

- use hashed passwords;
- return JWT or cookie-based authentication after deciding auth style;
- add therapist ownership to future queries;
- keep clinic/admin roles out of the first implementation.

Key decision before coding:

- JWT tokens vs secure cookies.

## Phase 2: Patients v2

Goal:

Expand the current Patients service to match the MVP patient model.

Current service already supports:

- create patient;
- list patients;
- get by id;
- get by name;
- get by therapist;
- update patient;
- delete patient.

Recommended changes:

- replace delete-first workflow with inactivate/archive;
- add richer patient fields;
- add patient status;
- add caregiver fields;
- add duplicate detection;
- enforce therapist ownership.

Suggested endpoints:

```http
POST /patients
GET /patients
GET /patients/{id}
PUT /patients/{id}
PATCH /patients/{id}/status
GET /patients/duplicates
```

Filters for `GET /patients`:

- name;
- status;
- diagnosis;
- age range;
- without future session.

Key decision before coding:

- whether to keep current endpoint shapes or introduce cleaner v2 routes.

## Phase 3: Sessions

Goal:

Allow therapists to schedule, update, cancel, and complete sessions.

Recommended module:

- `Sessions`

Core entities:

- `Session`
- `SessionNote`

Suggested endpoints:

```http
POST /sessions
GET /sessions
GET /sessions/{id}
GET /patients/{patientId}/sessions
PUT /sessions/{id}
PATCH /sessions/{id}/cancel
PATCH /sessions/{id}/reschedule
POST /sessions/{id}/complete
```

Important rules:

- only the owner therapist can access the session;
- completed sessions should store clinical summary;
- cancelled/no-show sessions should preserve reason/status;
- session history should never disappear because a patient becomes inactive.

## Phase 4: Therapy Plans And Goals

Goal:

Track active intervention plans and therapeutic goals.

Recommended module:

- `TherapyPlans`

Core entities:

- `TherapyPlan`
- `TherapeuticGoal`
- `SessionGoal`

Suggested endpoints:

```http
POST /patients/{patientId}/therapy-plans
GET /patients/{patientId}/therapy-plans
GET /therapy-plans/{id}
PUT /therapy-plans/{id}
PATCH /therapy-plans/{id}/status
POST /therapy-plans/{id}/goals
PUT /therapy-goals/{id}
PATCH /therapy-goals/{id}/status
POST /sessions/{sessionId}/goals
```

Important rules:

- one patient can have multiple plans over time;
- only one active plan should normally exist per patient;
- goals can be associated with multiple sessions;
- goals should support progress tracking.

## Phase 5: Dashboard

Goal:

Give the therapist a daily operational overview.

Suggested endpoint:

```http
GET /dashboard/therapist
```

Response should include:

- today's sessions;
- upcoming sessions;
- sessions pending registration;
- active patient count;
- patients without future sessions;
- basic alerts.

Implementation note:

This can be an application query that aggregates Patients, Sessions, and TherapyPlans data.

## Phase 6: Basic Reports

Goal:

Generate editable report drafts from existing clinical data.

Recommended module:

- `Reports`

Core entity:

- `Report`

Suggested endpoints:

```http
POST /patients/{patientId}/reports/draft
GET /patients/{patientId}/reports
GET /reports/{id}
PUT /reports/{id}
PATCH /reports/{id}/finalize
```

Out of first pass:

- PDF export;
- external sharing;
- custom templates.

## Phase 7: Angular API Integration

Goal:

Replace in-memory frontend data with backend calls.

Recommended frontend services:

- `AuthService`
- `PatientsService`
- `SessionsService`
- `TherapyPlansService`
- `DashboardService`
- `ReportsService`

Recommended first integration:

1. Connect patient list to `GET /patients`.
2. Connect patient detail to `GET /patients/{id}`.
3. Connect create/edit patient forms.
4. Connect sessions after backend exists.

## Database Notes

Use migrations per module/phase.

Keep PostgreSQL pinned in Docker Compose instead of using `postgres:latest`.

Current local database:

```text
therabee_patients
```

This may remain acceptable in early MVP, but later we should decide between:

- one shared database with module schemas;
- separate databases per deployed service.

For the MVP, a shared database is simpler.

## Testing Plan

Add tests gradually:

- unit tests for domain rules;
- handler tests for CQRS use cases;
- API integration tests for key workflows;
- frontend tests after Angular forms and API integration stabilize.

Minimum test workflows:

- therapist registers and logs in;
- therapist creates patient;
- therapist cannot see another therapist's patient;
- therapist schedules session;
- therapist completes session;
- dashboard reflects session and patient status.

## Important Security Rules

- Never expose clinical data publicly.
- Never use real patient data in free-tier deployments.
- Enforce therapist ownership in queries and commands.
- Prefer archive/status changes over hard deletion for clinical records.
- Log critical actions once audit support is added.

