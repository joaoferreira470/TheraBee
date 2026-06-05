# TheraBee MVP Scope

This document narrows the full product requirements into a first buildable MVP.

The full product vision lives in `PRODUCT_REQUIREMENTS.md`. This MVP scope is intentionally smaller so the first version can be implemented, tested, and improved without overloading the architecture too early.

## MVP Goal

Build a usable clinical workspace for an individual therapist to manage patients, schedule and record sessions, define therapy goals, and keep basic clinical history.

The MVP should prove the core loop:

```text
Therapist -> Patient -> Therapy Plan -> Session -> Progress -> Basic Report
```

## MVP Users

### Therapist

The therapist is the only required active user in the first MVP.

The therapist can:

- register and log in;
- manage their professional profile;
- create and manage patients;
- schedule and record therapeutic sessions;
- create therapy plans and goals;
- view patient progress;
- generate a simple therapeutic report.

### Patient Or Caregiver

Patients and caregivers do not need login in the MVP.

Their information is managed by the therapist.

### Clinic Administrator

Clinic administrator support is out of scope for MVP implementation, but the data model should avoid blocking future organizations, teams, and shared patients.

## MVP Modules

## 1. Authentication And Therapist Profile

Included:

- therapist registration;
- login;
- logout;
- therapist profile;
- basic data isolation by authenticated therapist.

Initial fields:

- name;
- email;
- password;
- profession;
- professional number;
- specialties;
- phone number;
- workplace;
- report signature.

Out of scope:

- clinic admin;
- complex role management;
- patient/caregiver login;
- social login;
- multi-factor authentication.

## 2. Patients

Included:

- create patient;
- list patients;
- view patient detail;
- edit patient;
- inactivate/archive patient;
- simple duplicate warning.

Initial fields:

- full name;
- birth date;
- calculated age;
- gender;
- phone number;
- email;
- address;
- caregiver name;
- caregiver phone;
- main diagnosis;
- referral reason;
- general notes;
- status: active, inactive, discharged, suspended.

Patient deletion should not be the normal workflow. Prefer status changes so clinical history is preserved.

## 3. Sessions

Included:

- schedule session;
- list sessions;
- view sessions by patient;
- cancel session;
- reschedule session;
- register completed session.

Initial fields:

- patient;
- therapist;
- start date/time;
- end date/time;
- type;
- location;
- status;
- observations.

Session statuses:

- scheduled;
- completed;
- cancelled;
- patient no-show;
- therapist no-show;
- rescheduled.

Session types:

- assessment;
- intervention;
- reassessment;
- caregiver meeting;
- multidisciplinary meeting;
- supervision;
- other.

## 4. Therapy Plans And Goals

Included:

- create therapy plan;
- add therapeutic goals;
- update plan status;
- update goal status;
- associate goals with completed sessions.

Initial plan fields:

- patient;
- therapist;
- start date;
- expected end date;
- weekly frequency;
- general goals;
- intervention areas;
- status.

Goal fields:

- description;
- area;
- priority;
- status;
- review date.

Out of scope:

- automatic plan recommendation;
- AI-assisted goals;
- advanced plan history.

## 5. Progress Summary

Included:

- sessions completed;
- missed sessions;
- cancelled sessions;
- attendance rate;
- goals in progress;
- goals achieved;
- last session;
- next session.

Out of scope:

- advanced clinical analytics;
- automatic low-progress detection;
- charts beyond simple summary indicators.

## 6. Basic Reports

Included:

- generate simple report draft from patient, plan, goals, and sessions;
- edit report content before export;
- store report history.

Out of scope for first pass:

- polished PDF export;
- custom templates;
- digital signatures;
- external sharing.

PDF export can be added after report generation is stable.

## 7. Dashboard

Included:

- today's sessions;
- upcoming sessions;
- sessions pending registration;
- active patients;
- patients without future sessions;
- basic alerts.

## Explicitly Out Of MVP

These are important, but should wait for later phases:

- clinic administration;
- patient/caregiver login;
- document upload;
- consent management;
- patient sharing between professionals;
- Google Calendar/Outlook integration;
- email/SMS/WhatsApp notifications;
- AI-assisted recommendations;
- subscriptions and payments;
- multi-clinic management;
- advanced audit and retention workflows.

## Security Minimum For MVP

The MVP must still respect sensitive clinical data:

- no public clinical data;
- therapist authentication required;
- therapist can only access their own data;
- passwords are never stored in plain text;
- test deployments must use fake data only;
- future RGPD requirements must not be blocked by the model.

## Recommended First Implementation Order

1. Auth and therapist profile.
2. Patients v2.
3. Sessions.
4. Therapy plans and goals.
5. Dashboard.
6. Basic reports.
7. Angular API integration.

