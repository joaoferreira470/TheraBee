# TheraBee MVP Roadmap

## Product Thesis

TheraBee is a clinical workspace for therapists who need to transform patient sessions into structured clinical evidence, measurable progress, and reports without spending hours rewriting scattered notes.

The strongest MVP promise is:

```text
Assessment -> therapeutic cycle -> reassessment -> measurable goal progress -> PDF or Word report
```

The product should not be just a patient CRUD app. It should help therapists document clinical work session by session, score patient response to goals, and generate visual evidence for families, caregivers, institutions, and clinical records.

## Core Clinical Model

The application should follow this clinical loop:

```text
Therapist
  -> Patient
    -> Areas
    -> Objectives
    -> Assessment Session
    -> Therapy Sessions
    -> Reassessment Session
    -> Progress Dashboard
    -> Report by date interval or therapeutic cycle
```

### Session Types

- [ ] `AssessmentSession`: first or initial evaluation session.
- [ ] `TherapySession`: regular therapeutic intervention session.
- [ ] `ReassessmentSession`: macro checkpoint session used to evaluate progress since the previous assessment or reassessment.

Current backend has generic session types, but the product meaning needs to be refined:

- [x] Generic `Assessment` session type exists.
- [x] Generic `Intervention` session type exists.
- [x] Generic `Reassessment` session type exists.
- [x] Rename or map `Intervention` to `TherapySession`.
- [x] Rename or map `Reassessment` to `ReassessmentSession`.
- [x] Enforce clinical behavior per session type.

### Areas And Objectives

Areas and objectives should behave like reusable definitions owned by the patient, almost like clinical "classes". A session can attach instances of those definitions over time.

Areas and objectives are independent presets. An objective does not need to be a child of an area.

- [x] `Area`: macro clinical domain that can be evaluated over time.
- [x] `Objective`: concrete therapeutic objective that can be evaluated over time.
- [x] Areas and objectives can be created independently from sessions.
- [x] Areas and objectives can be reused across multiple sessions.
- [x] Areas and objectives have type, description, priority, and status.
- [x] Areas and objectives do not require a parent/child relationship.

Recommended relationship:

```text
Area
  -> reusable preset for assessment and reassessment sessions

Objective
  -> reusable preset for therapeutic sessions

Session
  -> SessionGoal
  -> SessionGoalAssessment
```

### Selection Rules

The app should help the therapist attach the right clinical definitions to a session.

- [x] When creating/editing a session, the picker should show patient areas and objectives.
- [x] Frontend should group areas and objectives clearly.
- [x] Backend should validate selected areas/objectives belong to the same patient and therapist.
- [x] Assessment and reassessment sessions can attach only areas.
- [x] Therapeutic sessions can attach only objectives.

### Goal Assessment Scale

Each session should be able to record the patient's response to each attached goal using a numerical scale.

- [ ] Add `SessionGoalAssessment`.
- [ ] Each assessment stores `SessionId`.
- [ ] Each assessment stores `GoalId`.
- [ ] Each assessment stores score `0-10`.
- [ ] Each assessment stores optional clinical notes.
- [ ] Scores can be trended over time per goal.
- [ ] Scores can power dashboards and reports.

This enables:

```text
Objective: Maintain attention for 10 minutes
Session 1: 3/10
Session 2: 5/10
Session 3: 7/10
```

## Target Navigation

The Angular application should become a routed product rather than a single operational shell.

### Routes

- [ ] `/login`: application entry point.
- [ ] `/register`: therapist sign-up.
- [ ] `/therapist`: therapist hub after login.
- [ ] `/patients/:patientId`: patient page.
- [ ] `/patients/:patientId/sessions/:sessionId`: session page.

### Navigation Rules

- [ ] After login, redirect to `/therapist`.
- [ ] Clicking the therapist name redirects to `/therapist`.
- [ ] Clicking a patient name redirects to `/patients/:patientId`.
- [ ] Clicking a session redirects to `/patients/:patientId/sessions/:sessionId`.
- [ ] Unauthorized users are redirected to `/login`.

## MVP Domains

### Identity And Therapists

- [x] Authentication foundation exists.
- [x] Therapist registration/login/logout exists.
- [x] JWT-based access control exists.
- [x] Therapist profile domain exists.
- [x] Professional details and specialties can be stored.
- [x] Patient ownership is enforced through authenticated therapist.
- [x] Session ownership is enforced end-to-end.
- [x] Report generation is restricted to the owning therapist.
- [ ] Production-ready auth/privacy model exists.

### Therapist Hub

- [ ] Routed therapist hub exists.
- [x] Therapist-owned patients can be listed.
- [ ] Patients can be edited from the therapist hub.
- [x] Patients can be deleted or archived from the therapist hub.
- [ ] Therapist schedule is visible.
- [ ] Scheduled sessions are shown in the therapist schedule.
- [ ] Schedule prevents overlapping sessions.
- [ ] Therapist name acts as navigation back to hub.

### Patients

- [x] Current Patients service exists.
- [x] Basic patient CRUD exists.
- [x] Address and main diagnosis fields exist.
- [x] Patient status exists.
- [x] Archive/inactivate flow exists.
- [x] Duplicate detection exists for patient creation.
- [x] Therapist ownership is enforced on patient operations.
- [x] Patient model supports the current MVP shape.
- [x] Patient detail endpoint exposes current Angular patient profile data.
- [ ] Routed patient page exists.
- [ ] Patient edit flow is integrated in Angular.
- [x] Patient delete/archive flow is integrated in Angular.
- [ ] Patient page shows full patient profile.
- [ ] Patient page shows clinical definitions grouped by areas and objectives.
- [ ] Patient page shows attended sessions and their assessments.
- [ ] Patient page shows scheduled sessions that have not occurred yet.
- [ ] Patient search/filtering supports name, status, diagnosis, age, and future-session gaps.

### Therapy Goals

- [x] Therapy goal domain exists.
- [x] Goals can be created for a patient.
- [x] Goals include area, priority, status, and type.
- [x] Goals can be associated with sessions.
- [x] Goal progress can be summarized with current dashboard logic.
- [x] Goals support `Area` and `Objective` types.
- [x] Areas and objectives are independent presets.
- [x] Goal create/edit UI supports type selection.
- [x] Goal picker is filtered by session type.
- [ ] Goal progress is based on 0-10 session assessments.

### Sessions

- [x] Session domain exists.
- [x] Scheduling endpoints exist.
- [x] Session status exists.
- [x] Session type exists.
- [x] Session notes exist.
- [x] Session history can be listed by patient.
- [x] Sessions pending registration can be identified.
- [x] Session model is aligned with `AssessmentSession`, `TherapySession`, and `ReassessmentSession`.
- [x] Sessions can attach areas or objectives according to session type.
- [x] Session goal selection validates patient and therapist ownership.
- [x] Assessment and reassessment sessions accept only areas.
- [x] Therapeutic sessions accept only objectives.
- [x] Session page exists in Angular.
- [x] Session page title follows `Session X | Patient Name, Age`.
- [x] Session details can be edited and saved.
- [x] Session reschedule validates therapist schedule overlap.
- [x] Session cancel/delete behavior is clearly defined.

### Session Goal Assessments

  - [x] Session goal assessment domain exists.
  - [x] Each attached goal can be scored from `0` to `10`.
  - [x] Each attached goal can store clinical notes.
  - [x] Sessions can assess attached areas.
  - [x] Sessions can assess attached objectives.
  - [x] Assessment data is stored for dashboards.
  - [x] Assessment data is stored for reports.
  - [x] Session page includes goal assessment controls.
  - [x] Session page has a `Confirm` action to submit assessments.

### Progress Dashboards

- [x] Patient progress summary exists.
- [x] Attendance indicators exist.
- [x] Goal progress indicators exist.
- [x] Last and next session indicators exist.
- [x] Dashboard data can be consumed by Angular.
- [x] Dashboard data can be reused by reports.
- [ ] Dashboard uses 0-10 goal assessment trends.
- [ ] Dashboard separates area and objective progress.
- [ ] Dashboard shows per-goal line trends.
- [ ] Dashboard highlights stagnant goals.
- [ ] Dashboard highlights goals with strongest progress.

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
- [ ] Reports can be generated for a user-selected date interval.
- [ ] Reports can be generated for a therapeutic cycle.
- [ ] Reports include visual goal progress charts.
- [ ] Reports group evidence by areas and objectives.
- [ ] Reports use session goal assessment scores.
- [ ] Report generation can export directly to PDF or Word based on user choice.

### Angular Frontend

- [x] Angular client exists under `src/Clients/therabee-web`.
- [x] Angular client can run locally.
- [x] Angular API integration exists.
- [x] Login/register screens call the backend.
- [x] Patient list/detail/create screens call the backend.
- [x] Patient-centered detail page exists as an MVP shell.
- [x] Goal status update actions are connected.
- [x] Session checkpoint workflow exists in current simplified form.
- [x] Progress dashboard exists in current simplified form.
- [x] Report builder/export workflow exists.
- [x] UI is responsive for desktop, tablet, and mobile.
- [x] Angular uses real routes for login, therapist hub, patient page, and session page.
- [x] Patient edit screen calls the backend.
- [ ] Goal create/edit UI supports goal type.
- [ ] Session create/edit UI uses context-aware goal picker.
- [ ] Session page supports goal scoring 0-10.
- [ ] Report UI supports date interval and export format choice.
- [ ] Environment-based API URL configuration exists.

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
- [x] Finalize patient delete/archive behavior for the therapist hub.
- [ ] Finalize patient edit behavior for the therapist hub.

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

### Phase 7: Reports And PDF/Word

- [x] Add report model.
- [x] Generate report draft from patient profile, goals, sessions, checkpoints, and progress summaries.
- [x] Allow report draft review/edit before export.
- [x] Export report to PDF.
- [x] Export report to Word.
- [x] Store generated report history.
- [x] Discard report drafts.
- [x] Generate report text in Portuguese from Portugal.

### Phase 8: Angular Frontend Integration

- [x] Connect Angular auth flow to backend JWT endpoints.
- [x] Connect patient list/detail/create flows.
- [x] Build patient-centered detail page with goals, sessions, and report shortcuts.
- [x] Connect goal status update actions.
- [x] Build session checkpoint workflow.
- [x] Build progress dashboard views.
- [x] Build report generation and PDF/Word export flow.
- [x] Build report draft discard flow.
- [x] Add TheraBee logo asset to Angular UI.
- [ ] Connect patient edit flow.
- [ ] Add environment-based API URL configuration.

### Phase 9: Real App Navigation

- [x] Create Angular routes for `/login`, `/register`, `/therapist`, `/patients/:patientId`, and `/patients/:patientId/sessions/:sessionId`.
- [x] Redirect authenticated users to therapist hub after login.
- [x] Redirect unauthenticated users to login.
- [x] Build therapist hub page titled `Therapist X`.
- [x] Show therapist-owned patient list in hub.
- [x] Add delete/archive patient actions in hub.
- [ ] Add edit patient action in hub.
- [x] Show therapist schedule in hub.
- [x] Navigate from patient name to patient page.
- [x] Navigate from therapist name to therapist hub.

### Phase 10: Clinical Goal Model

- [x] Add `GoalType` enum with `Area` and `Objective`.
- [x] Keep areas and objectives as independent presets.
- [x] Update goal create/update DTOs and validators.
- [x] Update EF Core configuration and migrations.
- [x] Update goal endpoints and query responses.
- [x] Update Angular goal creation/editing UI.
- [x] Show goals grouped by type on patient page.

### Phase 11: Clinical Session Model

- [x] Align session types with `AssessmentSession`, `TherapySession`, and `ReassessmentSession`.
- [x] Add or map session type labels in backend and frontend.
- [x] Implement context-aware goal picker.
- [x] Show areas for assessment/reassessment sessions and objectives for therapeutic sessions.
- [x] Validate selected areas/objectives belong to the same patient and therapist.
- [x] Validate selected goals against session type in backend.
- [x] Add schedule overlap validation for create/reschedule.
- [x] Define whether session removal means delete or cancel.
- [x] Build routed session page.
- [x] Allow session details to be edited and saved.

### Phase 12: Session Goal Assessments

  - [x] Add `SessionGoalAssessment` entity.
  - [x] Add score `0-10` validation.
  - [x] Add optional clinical notes per goal assessment.
  - [x] Add create/update assessment commands.
  - [x] Add assessment listing by session and patient.
  - [x] Update session page with assessment controls.
  - [x] Add `Confirm` action to submit session assessments.
  - [x] Store assessment data for dashboards and reports.

### Phase 13: Evidence-Based Dashboards And Reports

- [ ] Rework dashboard metrics to use 0-10 goal assessment trends.
- [ ] Add per-goal progress charts in Angular.
- [ ] Add area versus objective progress views.
- [ ] Add report date interval selection.
- [ ] Generate reports from selected date interval.
- [ ] Generate reports from therapeutic cycle.
- [ ] Include goal progress visuals in PDF and Word.
- [ ] Group report sections by areas and objectives.

### Phase 14: Online Test Deployment

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
  - [x] Patient model supports the current MVP shape.
  - [x] Therapy goals are implemented with `Area` and `Objective` types.
  - [x] Areas and objectives are independent presets.
  - [x] Session goal selection is validated against session type.
  - [x] Sessions are implemented with the current clinical-type mapping.
  - [x] Session goal assessments are implemented.
  - [x] Progress dashboards are implemented with current simplified metrics.
  - [x] Reports are implemented with current simplified data model.
  - [x] PDF export is implemented.
  - [x] Word export is implemented.
  - [x] Report deletion/discard is implemented.
  - [x] Goal scoring 0-10 is implemented.
  - [ ] Reports by date interval are implemented.
  - [x] Schedule overlap validation is implemented.

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
- [x] Logo asset is present in the Angular public assets folder.
- [x] Angular client uses the target routed app structure.

## Next Logical Step

The next implementation step is:

```text
  Phase 13: Evidence-Based Dashboards And Reports
  ```
  
  After the session goal assessments are in place, the product should move toward:

```text
Session goal assessments -> dashboards -> visual reports
```

This sequence keeps the product usable while progressively aligning the data model with the clinical workflow:

```text
Areas -> objectives -> session selection -> session assessments -> dashboards -> visual reports
```
