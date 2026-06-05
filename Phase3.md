# Phase 3 - Patients V2 Richer Model

Status: implemented in `feature/patients-v2` as an MVP slice.

## Goal

Expand the current Patients module into a richer clinical record that better supports the MVP workflow around session checkpoints, progress summaries, and report drafts.

This phase keeps the same authenticated therapist-owned model, but makes the patient record materially more useful for real clinical work.

## What This Phase Should Achieve

- Patient records contain the core data needed by the MVP.
- The patient detail view becomes a stronger foundation for later goals, sessions, dashboards, and reports.
- Contact details and caregiver information are available when needed.
- The backend can surface a calculated age without storing redundant data.
- Duplicate detection considers the most obvious repeated patient details.

## Current Baseline

Already implemented in the repository:

- therapist registration and login;
- JWT authentication;
- therapist profile read/update;
- therapist-owned Patients CRUD service;
- patient status and archive/inactivate flow;
- duplicate warning on create;
- PostgreSQL persistence and EF Core migrations.

## Scope In

- richer patient model for the MVP;
- calculated age in patient read models;
- contact and caregiver fields on patient records;
- main diagnosis, referral reason, and general notes;
- duplicate detection that can consider name/date of birth and contact details;
- update existing API contracts only where necessary and with care;
- documentation updates for the richer patient behavior.

## Scope Out

- sessions;
- therapy goals;
- reports;
- dashboards;
- documents;
- consent management;
- patient portal or caregiver login;
- microservice split;
- advanced duplicate merge workflow;
- automatic clinical recommendations.

## Suggested Patient Model Direction

Keep the current patient entity as the foundation, but evolve it toward:

- full name;
- birth date;
- calculated age in DTOs;
- gender;
- phone number;
- email;
- address;
- caregiver name;
- caregiver phone;
- main diagnosis;
- referral reason;
- general notes;
- status.

Suggested patient statuses:

- active;
- inactive;
- discharged;
- suspended;

## Suggested API Direction

Likely endpoints to keep or evolve:

- `POST /patients`
- `GET /patients`
- `GET /patients/{id}`
- `PUT /patients/{id}`
- `PATCH /patients/{id}/status`
- `GET /patients/duplicates`

Important rule:

- every read/write operation must respect the authenticated therapist owner.

## Suggested Implementation Order

1. Expand the patient domain model.
2. Update create and update commands, handlers, and validators.
3. Update patient read DTOs and mapping helpers.
4. Adjust duplicate detection to use the richer shape.
5. Add and apply the EF Core migration.
6. Update local testing examples and roadmap documentation.

## Acceptance Criteria

- A therapist can create and edit patients with the richer MVP fields.
- The patient detail payload contains the data needed for the future Angular profile screen.
- Calculated age is available without storing duplicate derived data.
- A patient can still be marked inactive without losing history.
- Existing ownership checks remain intact.

## Technical Notes

- Keep CQRS handlers per use case.
- Prefer small, focused changes in the current layered architecture.
- Do not split services yet.
- Keep migrations and model changes aligned.
- Add tests once the first slice is in place.
