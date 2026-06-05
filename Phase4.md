# Phase 4 - Therapy Goals

Status: implemented in `feature/patients-v2` as an MVP slice.

## Goal

Add a first therapeutic goals module that lets the therapist define structured goals per patient, track their status, and prepare the data that later feeds sessions, progress dashboards, and reports.

This phase keeps the system therapist-owned and patient-centric, without introducing therapy plans yet.

## What This Phase Should Achieve

- A therapist can create therapeutic goals for a patient.
- Goals have a clear structure that supports later progress tracking.
- A therapist can list, update, and change the status of their own goals.
- Goal data is ready to be reused by sessions and reports in later phases.

## Current Baseline

Already implemented in the repository:

- therapist registration and login;
- JWT authentication;
- therapist profile read/update;
- therapist-owned Patients CRUD service;
- richer patient model;
- patient status and archive/inactivate flow;
- duplicate warning on create;
- PostgreSQL persistence and EF Core migrations.

## Scope In

- therapeutic goal domain;
- goal create/list/update/status endpoints;
- therapist-owned access control for goals;
- goals linked directly to a patient;
- goal area, priority, status, and review date;
- API and documentation updates for goal workflows.

## Scope Out

- therapy plans;
- session-to-goal linking;
- progress dashboard metrics;
- report generation;
- PDF export;
- documents and consent workflows;
- patient portal or caregiver login;
- microservice split.

## Suggested Goal Model Direction

Keep the first version simple and useful:

- patient identifier;
- therapist identifier;
- description;
- area;
- priority;
- status;
- review date;
- created-at timestamp from the shared entity base.

Suggested goal statuses:

- not started;
- in progress;
- achieved;
- suspended;

Suggested priorities:

- low;
- medium;
- high;

## Suggested API Direction

Likely endpoints to keep:

- `POST /patients/{patientId}/therapy-goals`
- `GET /patients/{patientId}/therapy-goals`
- `GET /therapy-goals/{id}`
- `PUT /therapy-goals/{id}`
- `PATCH /therapy-goals/{id}/status`

Important rule:

- every read/write operation must respect the authenticated therapist owner.

## Suggested Implementation Order

1. Add the therapeutic goal domain model and enums.
2. Add create/list/update/status use cases.
3. Add API endpoints for patient-scoped creation and reading.
4. Add EF Core configuration and migration.
5. Update roadmap and product documentation.

## Acceptance Criteria

- A therapist can create goals for their own patients.
- A therapist can list and edit only their own goals.
- Goal status can be updated independently.
- The data model is ready for future session linkage.

## Technical Notes

- Keep CQRS handlers per use case.
- Keep the module therapist-owned end-to-end.
- Do not add therapy plans yet unless the next scope explicitly needs them.
- Keep migrations and model changes aligned.
