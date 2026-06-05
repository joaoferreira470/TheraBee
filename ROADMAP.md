# TheraBee MVP Roadmap

## Goal

Build a patient management application for therapists, psychomotricists, psychologists, occupational therapists, and related professionals.

The first milestone should prove the core clinical workflow before the system grows into a broader microservices architecture.

## Planning Documents

- [x] `PRODUCT_REQUIREMENTS.md`: full product requirements and long-term vision.
- [x] `MVP_SCOPE.md`: reduced first-version scope to implement.
- [x] `BACKEND_IMPLEMENTATION_PLAN.md`: backend phases, modules, branches, and suggested endpoints.

## Recommended Strategy

Start with a modular backend and a simple frontend prototype. Keep the architecture microservice-ready, but avoid splitting every concept into a distributed service too early.

## MVP Domains

### Patients

- [x] Current service exists.
- [x] Basic patient CRUD exists.
- [x] Address and diagnosis fields exist.
- [ ] Therapist ownership is enforced through authenticated user.
- [ ] Patients v2 richer model is implemented.

### Therapists

- [x] Therapist profile domain now exists.
- [x] Professional details and specialties can be stored.
- [ ] Ownership of patients is enforced end-to-end.
- [ ] Ownership of sessions is enforced end-to-end.

### Sessions

- [ ] Session domain exists.
- [ ] Scheduling endpoints exist.
- [ ] Session status exists.
- [ ] Session notes exist.
- [ ] Patient progress history exists.

### Clinical Notes

- [ ] Clinical notes started inside Sessions.
- [ ] Split into dedicated module if complexity grows later.

### Identity

- [x] Authentication foundation exists.
- [x] Therapist registration/login/logout exists.
- [x] JWT-based access control exists for therapist profile endpoints.
- [ ] Broader authorization rules exist across all clinical modules.
- [ ] Production-ready auth/privacy model exists.

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
- [ ] Link patients to authenticated therapist ownership.
- [ ] Add therapist-oriented local seed data if still useful.

### Phase 3: Sessions

- [ ] Add session model.
- [ ] Add scheduling/listing endpoints.
- [ ] Add session notes.
- [ ] Link sessions to patients and therapists.

### Phase 4: Angular Frontend

- [x] Angular client created under `src/Clients/therabee-web`.
- [ ] Start with dashboard, patients, patient detail, sessions, and notes.
- [ ] Use the backend API through typed client services.

### Phase 5: Online Test Deployment

- [ ] Deploy only with fake data.
- [ ] Use a free or low-cost static host for frontend.
- [ ] Use a free or low-cost backend/database provider for temporary testing.
- [x] Add authentication before handling anything sensitive.

## Branching Rule

Before changing existing code, create or switch to a branch that matches the intent:

- [x] `planning/*` for documentation, roadmap, or prototype work.
- [x] `feature/*` for implementation work.
- [ ] `fix/*` for bug fixes.
- [ ] `chore/*` for tooling and setup.

## Current Prototype

The current prototype is in:

```text
prototype/therabee-mvp
```

It is dependency-free and can be opened directly through `index.html`.

- [x] Prototype folder exists.
- [x] Prototype can be opened directly.

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
