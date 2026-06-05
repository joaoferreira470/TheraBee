# TheraBee MVP Roadmap

## Product Thesis

TheraBee is a clinical workspace for therapists who need to turn session-by-session work into clear patient history, progress evidence, and therapeutic reports without spending hours rewriting scattered notes.

The strongest MVP promise is:

```text
Session checkpoints -> measurable progress -> report draft -> PDF for families, caregivers, institutions, or clinical records
```

The application should still support patient management, scheduling, and therapist profiles, but the core differentiator is reducing report-writing time and making therapeutic progress easier to explain visually.

## Planning Documents

- [x] `PRODUCT_REQUIREMENTS.md`: full product requirements and long-term vision.
- [x] `MVP_SCOPE.md`: reduced first-version scope to implement.
- [x] `BACKEND_IMPLEMENTATION_PLAN.md`: backend phases, modules, branches, and suggested endpoints.
- [x] `Phase2.md`: patient ownership and relationship enforcement plan.
- [ ] Add a dedicated report/checkpoint MVP plan document if the next backend phase needs a tighter brief.

## Recommended Strategy

Start with a modular monolith backend that remains microservice-ready, but do not split Patients, Sessions, Goals, and Reports into separate deployable services yet.

The first real product loop should be:

```text
Therapist -> Patient -> Goals -> Session Checkpoints -> Progress Dashboard -> Report Draft -> PDF
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
- [ ] Session ownership is enforced end-to-end.
- [ ] Report ownership is enforced end-to-end.
- [ ] Production-ready auth/privacy model exists.

### Patients

- [x] Current Patients service exists.
- [x] Basic patient CRUD exists.
- [x] Address and diagnosis fields exist.
- [x] Patient status exists.
- [x] Archive/inactivate flow exists.
- [x] Duplicate detection exists for patient creation.
- [x] Therapist ownership is enforced on patient operations.
- [ ] Patient model supports the full MVP shape.
- [ ] Patient detail endpoint exposes all data needed by the Angular patient profile.
- [ ] Patient search/filtering supports name, status, diagnosis, age, and future-session gaps.

### Therapy Goals

- [ ] Therapy goal domain exists.
- [ ] Goals can be created for a patient or therapy plan.
- [ ] Goals include area, priority, status, and review date.
- [ ] Goals can be associated with completed sessions.
- [ ] Goal progress can be summarized over time.

### Sessions And Checkpoints

- [ ] Session domain exists.
- [ ] Scheduling endpoints exist.
- [ ] Session status exists.
- [ ] Session type exists.
- [ ] Session notes exist.
- [ ] Structured session checkpoints exist.
- [ ] Checkpoints can record objectives worked, progress rating, activities, patient response, difficulties, recommendations, and next steps.
- [ ] Session history can be listed by patient.
- [ ] Sessions pending registration can be identified.

### Progress Dashboards

- [ ] Patient progress summary exists.
- [ ] Attendance indicators exist.
- [ ] Goal progress indicators exist.
- [ ] Last and next session indicators exist.
- [ ] Dashboard data can be consumed by Angular.
- [ ] Dashboard data can be reused by reports.

### Reports

- [ ] Report domain exists.
- [ ] Report draft can be generated from patient, goals, sessions, and checkpoints.
- [ ] Report draft can include dashboard-style progress summaries.
- [ ] Report content can be edited before export.
- [ ] Report can be exported to PDF.
- [ ] Generated reports are stored in patient history.
- [ ] Report generation is restricted to the owning therapist.

### Angular Frontend

- [x] Angular client exists under `src/Clients/therabee-web`.
- [x] Angular client can run locally.
- [ ] Angular API integration exists.
- [ ] Login/register screens call the backend.
- [ ] Patient list/detail screens call the backend.
- [ ] Session checkpoint workflow exists.
- [ ] Progress dashboard exists.
- [ ] Report builder/export workflow exists.
- [ ] UI is responsive for desktop, tablet, and mobile.

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
- [ ] Expand patient model to the full MVP shape.
- [ ] Add richer filtering and ownership-aware search across all patient endpoints.

### Phase 4: Therapy Goals

- [ ] Add therapeutic goal model.
- [ ] Add goal create/list/update/status endpoints.
- [ ] Link goals to patient and therapist ownership.
- [ ] Prepare goals to be referenced by sessions and reports.

### Phase 5: Sessions And Checkpoints

- [ ] Add session model.
- [ ] Add scheduling/listing endpoints.
- [ ] Add cancel/reschedule/complete session endpoints.
- [ ] Add structured checkpoint fields for completed sessions.
- [ ] Link sessions to patients, therapists, and goals.
- [ ] Expose patient session history.

### Phase 6: Progress Dashboard

- [ ] Add patient progress summary query.
- [ ] Add attendance metrics.
- [ ] Add goal progress metrics.
- [ ] Add sessions pending registration query.
- [ ] Make dashboard data report-ready.

### Phase 7: Reports And PDF

- [ ] Add report model.
- [ ] Generate report draft from patient profile, goals, sessions, checkpoints, and progress summaries.
- [ ] Allow report draft review/edit before export.
- [ ] Export report to PDF.
- [ ] Store generated report history.

### Phase 8: Angular Frontend Integration

- [ ] Connect Angular auth flow to backend JWT endpoints.
- [ ] Connect patient list/detail/create/edit flows.
- [ ] Build session checkpoint workflow.
- [ ] Build progress dashboard views.
- [ ] Build report generation and PDF export flow.

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
- [ ] Sessions are implemented.
- [ ] Therapy goals are implemented.
- [ ] Reports are implemented.
- [ ] PDF export is implemented.

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
- [ ] Angular client is integrated with the backend API.

## Next Logical Step

The next implementation step is still:

```text
Phase 3: Patients V2 -> Expand patient model to the full MVP shape
```

After that, the roadmap should move toward:

```text
Therapy Goals -> Sessions And Checkpoints -> Progress Dashboard -> Reports And PDF
```

This sequence protects the key product idea: every session should produce structured data that later reduces report-writing effort and feeds visual progress explanations.
