# TheraBee MVP Roadmap

## Product Thesis

TheraBee is a clinical workspace for therapists who need to turn session-by-session work into clear patient history, progress evidence, and therapeutic reports without spending hours rewriting scattered notes.

The strongest MVP promise is:

```text
Session checkpoints -> measurable progress -> report draft -> PDF or Word for families, caregivers, institutions, or clinical records
```

The application should still support patient management, scheduling, and therapist profiles, but the core differentiator is reducing report-writing time and making therapeutic progress easier to explain visually.

## Planning Documents

- [x] `PRODUCT_REQUIREMENTS.md`: full product requirements and long-term vision.
- [x] `MVP_SCOPE.md`: reduced first-version scope to implement.
- [x] `BACKEND_IMPLEMENTATION_PLAN.md`: backend phases, modules, branches, and suggested endpoints.
- [x] `Phase2.md`: patient ownership and relationship enforcement plan.
- [x] `Phase8.md`: Angular frontend integration plan and validation checklist.
- [ ] Add a dedicated report/checkpoint MVP plan document if the next backend phase needs a tighter brief.

## Recommended Strategy

Start with a modular monolith backend that remains microservice-ready, but do not split Patients, Sessions, Goals, and Reports into separate deployable services yet.

The first real product loop should be:

```text
Therapist -> Patient -> Goals -> Session Checkpoints -> Progress Dashboard -> Report Draft -> PDF or Word
```

This keeps the architecture practical while focusing development on the painful workflow: clinical documentation and report generation.

## MVP Domains

### Identity And Therapists

- [x] Authentication foundation exists.
- [x] Therapist registration/login/logout exists.
- [x] JWT-based access control exists.
- [x] Therapist profile domain exists.
- [x] Professional details and specialties can be stored.
- [x] Patient ownership is enforced through authenticated therapist.
- [x] Session ownership is enforced end-to-end.
- [ ] Report ownership is enforced end-to-end.
- [ ] Production-ready auth/privacy model exists.

### Patients

- [x] Current Patients service exists.
- [x] Basic patient CRUD exists.
- [x] Address and main diagnosis fields exist.
- [x] Patient status exists.
- [x] Archive/inactivate flow exists.
- [x] Duplicate detection exists for patient creation.
- [x] Therapist ownership is enforced on patient operations.
- [x] Patient model supports the full MVP shape.
- [x] Patient detail endpoint exposes all data needed by the Angular patient profile.
- [ ] Patient search/filtering supports name, status, diagnosis, age, and future-session gaps.

### Therapy Goals

- [x] Therapy goal domain exists.
- [x] Goals can be created for a patient.
- [x] Goals include area, priority, status, and review date.
- [x] Goals can be associated with completed sessions.
- [x] Goal progress can be summarized over time.

### Sessions And Checkpoints

- [x] Session domain exists.
- [x] Scheduling endpoints exist.
- [x] Session status exists.
- [x] Session type exists.
- [x] Session notes exist.
- [x] Structured session checkpoints exist.
- [x] Checkpoints can record objectives worked, progress rating, activities, patient response, difficulties, recommendations, and next steps.
- [x] Session history can be listed by patient.
- [x] Sessions pending registration can be identified.

### Progress Dashboards

- [x] Patient progress summary exists.
- [x] Attendance indicators exist.
- [x] Goal progress indicators exist.
- [x] Last and next session indicators exist.
- [x] Dashboard data can be consumed by Angular.
- [x] Dashboard data can be reused by reports.

### Reports

- [x] Report domain exists.
- [x] Report draft can be generated from patient, goals, sessions, and checkpoints.
- [x] Report draft can include dashboard-style progress summaries.
- [x] Report content can be edited before export.
- [x] Report can be exported to PDF.
- [x] Report can be exported to Word.
- [x] Report drafts can be discarded.
- [x] Generated reports are stored in patient history.
- [x] Report generation is restricted to the owning therapist.
- [x] Report draft content is generated in Portuguese from Portugal.

### Angular Frontend

- [x] Angular client exists under `src/Clients/therabee-web`.
- [x] Angular client can run locally.
- [x] Angular API integration exists.
- [x] Login/register screens call the backend.
- [x] Patient list/detail/create screens call the backend.
- [ ] Patient edit screen calls the backend.
- [x] Session checkpoint workflow exists.
- [x] Progress dashboard exists.
- [x] Report builder/export workflow exists.
- [x] UI is responsive for desktop, tablet, and mobile.

## Implementation Phases

### Phase 1: Planning And Prototype

- [x] Create a simplified UI prototype.
- [x] Define the first workflows.
- [x] Decide MVP entities and API contracts.
- [x] Keep all existing backend code stable.

### Phase 2: Auth And Therapists

- [x] Add therapist model.
- [x] Add therapist registration and login.
- [x] Add therapist profile read/update endpoints.
- [x] Add password hashing and JWT authentication.
- [x] Link patients to authenticated therapist ownership.
- [ ] Add therapist-oriented local seed data if still useful.

### Phase 3: Patients V2

- [x] Enforce authenticated therapist ownership on patient operations.
- [x] Add patient status and archive/inactivate flow.
- [x] Add duplicate detection for patient creation.
- [x] Expand patient model to the full MVP shape.
- [ ] Add richer filtering and ownership-aware search across all patient endpoints.

### Phase 4: Therapy Goals

- [x] Add therapeutic goal model.
- [x] Add goal create/list/update/status endpoints.
- [x] Link goals to patient and therapist ownership.
- [x] Prepare goals to be referenced by sessions and reports.

### Phase 5: Sessions And Checkpoints

- [x] Add session model.
- [x] Add scheduling/listing endpoints.
- [x] Add cancel/reschedule/complete session endpoints.
- [x] Add structured checkpoint fields for completed sessions.
- [x] Link sessions to patients, therapists, and goals.
- [x] Expose patient session history.

### Phase 6: Progress Dashboard

- [x] Add patient progress summary query.
- [x] Add attendance metrics.
- [x] Add goal progress metrics.
- [x] Add sessions pending registration query.
- [x] Make dashboard data report-ready.

### Phase 7: Reports And PDF

- [x] Add report model.
- [x] Generate report draft from patient profile, goals, sessions, checkpoints, and progress summaries.
- [x] Allow report draft review/edit before export.
- [x] Export report to PDF.
- [x] Export report to Word.
- [x] Store generated report history.

### Phase 8: Angular Frontend Integration

- [x] Connect Angular auth flow to backend JWT endpoints.
- [x] Connect patient list/detail/create flows.
- [ ] Connect patient edit flow.
- [x] Build patient-centered detail page with goals, sessions, and report shortcuts.
- [x] Connect goal status update actions.
- [x] Build session checkpoint workflow.
- [x] Build progress dashboard views.
- [x] Build report generation and PDF/Word export flow.
- [x] Build report draft discard flow.
- [ ] Add environment-based API URL configuration.

### Phase 9: Online Test Deployment

- [ ] Deploy only with fake data.
- [ ] Use a free or low-cost static host for frontend.
- [ ] Use a free or low-cost backend/database provider for temporary testing.
- [x] Add authentication before handling anything sensitive.
- [ ] Document deployment environment variables and seed strategy.

## Branching Rule

Before changing existing code, create or switch to a branch that matches the intent:

- [x] `planning/*` for documentation, roadmap, or prototype work.
- [x] `feature/*` for implementation work.
- [ ] `fix/*` for bug fixes.
- [ ] `chore/*` for tooling and setup.

## Current Backend State

- [x] Current branch is `feature/patients-v2`.
- [x] `feature/patients-v2` includes the previous auth/therapist profile work.
- [x] Patients API can build and run locally with Docker/PostgreSQL.
- [x] Patient endpoints require authentication.
- [x] Patients are associated with the authenticated therapist.
- [x] Patient model supports the full MVP shape.
- [x] Therapy goals are implemented.
- [x] Sessions are implemented.
- [x] Progress dashboards are implemented.
- [x] Reports are implemented.
- [x] PDF export is implemented.
- [x] Word export is implemented.

## Current Prototype

The dependency-free prototype is in:

```text
prototype/therabee-mvp
```

- [x] Prototype folder exists.
- [x] Prototype can be opened directly through `index.html`.

## Current Angular Client

The Angular frontend is in:

```text
src/Clients/therabee-web
```

Run it locally with:

```powershell
cd src\Clients\therabee-web
npm.cmd install
npm.cmd run start
```

The default development URL is:

```text
http://localhost:4200
```

- [x] Angular client exists locally.
- [x] Angular client can run locally.
- [x] Angular client is integrated with the backend API.

## Next Logical Step

The next implementation step is now:

```text
Phase 8 follow-up -> Add environment-based API URL and patient edit UI
```

After that, the roadmap should move toward:

```text
Angular Frontend Integration -> Online Test Deployment
```

This sequence protects the key product idea: every session should produce structured data that later reduces report-writing effort and feeds visual progress explanations.
