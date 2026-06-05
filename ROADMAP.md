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
    -> Long Term Goals
    -> Short Term Goals
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

### Goal Types

Goals should behave like reusable definitions owned by the patient, almost like clinical "classes". A session can attach instances of those goals over time.

- [x] `LongTermGoal`: macro clinical direction, usually defined and evaluated in assessment/reassessment sessions.
- [x] `ShortTermGoal`: operational goal, usually worked during therapy sessions.
- [x] `ShortTermGoal` can optionally reference a parent `LongTermGoal`.
- [x] Goals can be created independently from sessions.
- [x] Goals can be reused across multiple sessions.
- [x] Goals have area, priority, status, review date, and type.

Recommended relationship:

```text
LongTermGoal
  -> ShortTermGoal
  -> ShortTermGoal

Session
  -> SessionGoal
  -> SessionGoalAssessment
```

### Goal Selection Rules

The app should help the therapist pick the right goal presets based on session type.

- [x] When creating/editing an `AssessmentSession`, the goal picker should show `LongTermGoal` presets.
- [x] When creating/editing a `ReassessmentSession`, the goal picker should show `LongTermGoal` presets.
- [x] When creating/editing a `TherapySession`, the goal picker should show `ShortTermGoal` presets.
- [x] Frontend should filter goals contextually.
- [x] Backend should validate the selected goals against the session type.

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
Goal: Maintain attention for 10 minutes
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
- [ ] Patients can be deleted or archived from the therapist hub.
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
- [ ] Patient delete/archive flow is integrated in Angular.
- [ ] Patient page shows full patient profile.
- [ ] Patient page shows goals grouped by long-term and short-term.
- [ ] Patient page shows attended sessions and their assessments.
- [ ] Patient page shows scheduled sessions that have not occurred yet.
- [ ] Patient search/filtering supports name, status, diagnosis, age, and future-session gaps.

### Therapy Goals

- [x] Therapy goal domain exists.
- [x] Goals can be created for a patient.
- [x] Goals include area, priority, status, and review date.
- [x] Goals can be associated with sessions.
- [x] Goal progress can be summarized with current dashboard logic.
- [ ] Goals support `LongTerm` and `ShortTerm` types.
- [ ] Short-term goals can optionally reference a parent long-term goal.
- [ ] Goal create/edit UI supports type selection.
- [ ] Goal picker is filtered by session type.
- [ ] Goal progress is based on 0-10 session assessments.

### Sessions

- [x] Session domain exists.
- [x] Scheduling endpoints exist.
- [x] Session status exists.
- [x] Session type exists.
- [x] Session notes exist.
- [x] Session history can be listed by patient.
- [x] Sessions pending registration can be identified.
- [ ] Session model is aligned with `AssessmentSession`, `TherapySession`, and `ReassessmentSession`.
- [ ] Therapy sessions can attach short-term goals.
- [ ] Assessment/reassessment sessions can attach long-term goals.
- [ ] Session page exists in Angular.
- [ ] Session page title follows `Session X | Patient Name, Age`.
- [ ] Session details can be edited and saved.
- [ ] Session reschedule validates therapist schedule overlap.
- [ ] Session cancel/delete behavior is clearly defined.

### Session Goal Assessments

- [ ] Session goal assessment domain exists.
- [ ] Each attached goal can be scored from `0` to `10`.
- [ ] Each attached goal can store clinical notes.
- [ ] Therapy sessions assess short-term goals.
- [ ] Assessment/reassessment sessions assess long-term goals.
- [ ] Assessment data is stored for dashboards.
- [ ] Assessment data is stored for reports.
- [ ] Session page includes goal assessment controls.
- [ ] Session page has a `Confirm` action to submit assessments.

### Progress Dashboards

- [x] Patient progress summary exists.
- [x] Attendance indicators exist.
- [x] Goal progress indicators exist.
- [x] Last and next session indicators exist.
- [x] Dashboard data can be consumed by Angular.
- [x] Dashboard data can be reused by reports.
- [ ] Dashboard uses 0-10 goal assessment trends.
- [ ] Dashboard separates long-term and short-term progress.
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
- [ ] Reports group evidence by long-term and short-term goals.
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
- [ ] Goal create/edit UI supports goal type and parent goal.
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
- [ ] Finalize patient edit/delete/archive behavior for the therapist hub.

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
- [ ] Add edit/delete/archive patient actions in hub.
- [x] Show therapist schedule in hub.
- [x] Navigate from patient name to patient page.
- [x] Navigate from therapist name to therapist hub.

### Phase 10: Clinical Goal Model

- [x] Add `GoalType` enum with `LongTerm` and `ShortTerm`.
- [x] Add optional parent goal relationship for short-term goals.
- [x] Update goal create/update DTOs and validators.
- [x] Update EF Core configuration and migrations.
- [x] Update goal endpoints and query responses.
- [x] Update Angular goal creation/editing UI.
- [x] Show goals grouped by type on patient page.

### Phase 11: Clinical Session Model

- [ ] Align session types with `AssessmentSession`, `TherapySession`, and `ReassessmentSession`.
- [x] Add or map session type labels in backend and frontend.
- [x] Implement context-aware goal picker.
- [x] Filter long-term goals for assessment/reassessment sessions.
- [x] Filter short-term goals for therapy sessions.
- [x] Validate selected goals against session type in backend.
- [ ] Add schedule overlap validation for create/reschedule.
- [ ] Define whether session removal means delete or cancel.
- [x] Build routed session page.
- [x] Allow session details to be edited and saved.

### Phase 12: Session Goal Assessments

- [ ] Add `SessionGoalAssessment` entity.
- [ ] Add score `0-10` validation.
- [ ] Add optional clinical notes per goal assessment.
- [ ] Add create/update assessment commands.
- [ ] Add assessment listing by session and patient.
- [ ] Update session page with assessment controls.
- [ ] Add `Confirm` action to submit session assessments.
- [ ] Store assessment data for dashboards and reports.

### Phase 13: Evidence-Based Dashboards And Reports

- [ ] Rework dashboard metrics to use 0-10 goal assessment trends.
- [ ] Add per-goal progress charts in Angular.
- [ ] Add long-term versus short-term progress views.
- [ ] Add report date interval selection.
- [ ] Generate reports from selected date interval.
- [ ] Generate reports from therapeutic cycle.
- [ ] Include goal progress visuals in PDF and Word.
- [ ] Group report sections by long-term and short-term goals.

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
- [x] Therapy goals are implemented with `LongTerm` and `ShortTerm` types.
- [x] Short-term goals can optionally reference a parent long-term goal.
- [x] Session goal selection is validated against session type.
- [x] Sessions are implemented with the current clinical-type mapping.
- [x] Progress dashboards are implemented with current simplified metrics.
- [x] Reports are implemented with current simplified data model.
- [x] PDF export is implemented.
- [x] Word export is implemented.
- [x] Report deletion/discard is implemented.
- [ ] Goal scoring 0-10 is implemented.
- [ ] Reports by date interval are implemented.
- [ ] Schedule overlap validation is implemented.

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
Phase 11: Clinical Session Model
```

After the goal model is in place, the product should move toward:

```text
Session model semantics -> session goal assessments -> dashboards -> visual reports
```

This sequence keeps the product usable while progressively aligning the data model with the clinical workflow:

```text
Long-term goals -> short-term goals -> contextual session goal selection -> session goal assessments -> dashboards -> visual reports
```
