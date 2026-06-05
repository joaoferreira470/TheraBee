# TheraBee MVP Prototype

This is a lightweight, dependency-free prototype for the first version of TheraBee.

It is intentionally not wired to the backend yet. The goal is to validate the product flow before adding more services or creating the Angular client.

## What It Covers

- Therapist workspace overview.
- Patient list and patient detail summary.
- Session list.
- Simple clinical note capture.
- MVP navigation structure for a future Angular frontend.

## How To Open

Open `index.html` in a browser.

No build step is required.

## Why This Exists

Before adding Angular, authentication, and new backend services, this prototype helps validate:

- whether the screen structure makes sense;
- what entities need to exist first;
- which API endpoints will be needed;
- what can stay in the Patients service and what should move to Therapists or Sessions.

## Future Direction

Once the screens and flows feel right, port this prototype to an Angular app under `src/Clients/therabee-web` or similar.

