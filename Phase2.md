# Phase 2 - Patients V2

Status: implemented in `feature/patients-v2` as an MVP slice.

## Goal

Transform the current Patients service into an authenticated, therapist-owned clinical module that matches the MVP scope more closely.

This phase keeps the existing modular monolith approach, but makes the patient workflow safer and more useful for a real therapist.

## What This Phase Should Achieve

- Every patient belongs to the authenticated therapist that created or owns it.
- A therapist can only see and manage their own patients.
- Patient deletion is no longer the main workflow.
- Patient records become richer and closer to the product requirements.
- Duplicate detection starts to protect against accidental duplicates.

## Current Baseline

Already implemented in the repository:

- therapist registration and login;
- JWT authentication;
- therapist profile read/update;
- current Patients CRUD service;
- PostgreSQL persistence and EF Core migrations.

## Scope In

- authenticated therapist ownership for patient operations;
- richer patient model for the MVP;
- archive/inactivate flow instead of hard delete by default;
- duplicate warning on create;
- therapist-filtered patient queries;
- update existing API contracts only when necessary and with care;
- documentation updates for the new patient behavior.

## Scope Out

- sessions;
- therapy plans;
- reports;
- documents;
- consent management;
- patient portal or caregiver login;
- microservice split;
- complex duplicate merge workflow;
- advanced clinical analytics.

## Suggested Patient Model Direction

Keep the current patient entity as the foundation, but evolve it toward:

- full name;
- birth date;
- calculated age where needed;
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
- suspended.

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

1. Add therapist ownership to the patient model and queries.
2. Update create/list/get/update operations to use the authenticated therapist.
3. Add a patient status field and archive/inactivate workflow.
4. Add duplicate detection for create.
5. Expand patient DTOs and validation.
6. Update documentation and local testing examples.

## Acceptance Criteria

- A therapist only sees their own patients.
- A therapist cannot create or edit a patient for another therapist.
- A patient can be marked inactive without losing history.
- Duplicate warnings exist for obvious repeated patient entries.
- Existing API behavior remains stable where possible.

## Technical Notes

- Keep CQRS handlers per use case.
- Prefer small, focused changes in the current layered architecture.
- Do not split services yet.
- Keep migrations and model changes aligned.
- Add tests once the first slice is in place.
