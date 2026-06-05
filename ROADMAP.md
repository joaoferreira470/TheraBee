# TheraBee MVP Roadmap

## Goal

Build a patient management application for therapists, psychomotricists, psychologists, occupational therapists, and related professionals.

The first milestone should prove the core clinical workflow before the system grows into a broader microservices architecture.

## Recommended Strategy

Start with a modular backend and a simple frontend prototype. Keep the architecture microservice-ready, but avoid splitting every concept into a distributed service too early.

## MVP Domains

### Patients

Current service. Handles patient records, address, diagnosis, therapist association, and basic CRUD.

### Therapists

Next recommended domain. Handles therapist profiles, professional details, specialties, and ownership of patients/sessions.

### Sessions

Third recommended domain. Handles scheduled sessions, session status, session notes, and patient progress history.

### Clinical Notes

Can begin inside Sessions. Split later if notes become complex, auditable, versioned, or permission-heavy.

### Identity

Needed before real users or online testing with private data. Keep test deployments with fake data only until authentication and privacy rules are in place.

## Implementation Phases

### Phase 1: Planning And Prototype

- Create a simplified UI prototype.
- Define the first workflows.
- Decide MVP entities and API contracts.
- Keep all existing backend code stable.

### Phase 2: Therapists

- Add therapist model.
- Add therapist CRUD.
- Link patients to real therapist records.
- Add seed data for local development.

### Phase 3: Sessions

- Add session model.
- Add scheduling/listing endpoints.
- Add session notes.
- Link sessions to patients and therapists.

### Phase 4: Angular Frontend

- Angular client created under `src/Clients/therabee-web`.
- Start with dashboard, patients, patient detail, sessions, and notes.
- Use the backend API through typed client services.

### Phase 5: Online Test Deployment

- Deploy only with fake data.
- Use a free or low-cost static host for frontend.
- Use a free or low-cost backend/database provider for temporary testing.
- Add authentication before handling anything sensitive.

## Branching Rule

Before changing existing code, create or switch to a branch that matches the intent:

- `planning/*` for documentation, roadmap, or prototype work.
- `feature/*` for implementation work.
- `fix/*` for bug fixes.
- `chore/*` for tooling and setup.

## Current Prototype

The current prototype is in:

```text
prototype/therabee-mvp
```

It is dependency-free and can be opened directly through `index.html`.

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
