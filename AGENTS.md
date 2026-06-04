# AGENTS.md

Guidance for working in this repository.

## Repository Layout

- `src/therabee.sln` is the main solution file.
- `src/Services/Patients/Patients.API` contains the HTTP API layer.
- `src/Services/Patients/Patients.Application` contains use cases, DTOs, handlers, and application-level abstractions.
- `src/Services/Patients/Patients.Domain` contains domain models, value objects, events, and domain abstractions.
- `src/Services/Patients/Patients.Infrastructure` contains EF Core, persistence, migrations, and infrastructure wiring.
- `src/BuildingBlocks/BuildingBlocks` contains shared cross-cutting helpers such as CQRS primitives, behaviors, paging, and common exceptions.
- `src/docker-compose.yml`, `src/docker-compose.override.yml`, and `src/docker-compose.dcproj` are used for container-based local setup.
- Root files such as `README.md`, `LICENSE`, and `.gitignore` describe the repo and should be kept in sync with major project changes.

## Architectural Conventions

- Keep the solution layered:
  - `Domain` should stay free of infrastructure concerns.
  - `Application` should depend on `Domain` and shared building blocks, not on API or infrastructure details.
  - `Infrastructure` may depend on `Application` for interfaces and contracts.
  - `API` should stay thin and delegate work to application handlers.
- Prefer small, focused changes that fit the existing CQRS-style structure.
- Reuse existing naming patterns for folders, commands, queries, handlers, DTOs, and endpoints.
- Keep namespaces aligned with folder structure.
- Preserve the current .NET 8 and nullable-enabled style used across the codebase.

## Build And Run

Use these commands from the repo root:

- Restore dependencies:
  - `dotnet restore src/therabee.sln`
- Build the full solution:
  - `dotnet build src/therabee.sln`
- Run the Patients API:
  - `dotnet run --project src/Services/Patients/Patients.API/Patients.API.csproj`

## Tests

- There is no dedicated test project in the repository at the moment.
- If tests are added later, the default command should be:
  - `dotnet test src/therabee.sln`

## What You Can Change

- Application use cases, handlers, DTOs, validators, and mapping code.
- Domain entities, value objects, events, and domain logic.
- Infrastructure persistence, EF Core configuration, and migrations when the data model changes.
- API endpoints and dependency-injection wiring when new use cases are added.
- Shared building blocks if a change is broadly useful and still consistent with the existing style.

## What You Should Not Change By Default

- Do not edit generated output in `bin/`, `obj/`, or `.vs/`.
- Do not rewrite migrations or the EF model snapshot unless the schema change requires it.
- Do not change public contracts, route shapes, or DTO fields without a clear reason and impact check.
- Do not move code between layers unless the architecture actually requires it.
- Do not touch `README.md`, `LICENSE`, or solution/project structure unless the task calls for it.

## Working Rules

- Before changing code, inspect the relevant project files and follow the existing pattern.
- Prefer the smallest possible change that solves the request.
- If a change affects persistence, endpoints, or contracts, update the related layer together so the repo stays consistent.
- Keep formatting and naming consistent with the existing code.
- If something is unclear, inspect the local files first instead of guessing.
- Permission to execute "gets" in order to solve an issue is always granted.
- Always ask permission before performing any commits.

