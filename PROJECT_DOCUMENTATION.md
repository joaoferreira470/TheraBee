# TheraBee Project Documentation

## Overview

TheraBee is a .NET 8 backend project currently focused on patient management. The solution is organized with a layered architecture and exposes a Patients API backed by PostgreSQL.

The current service lets the application create, update, delete, list, and search patients. Each patient belongs to a therapist through `TherapistId` and stores basic clinical and address information.

## Current Scope

The repository currently contains one bounded service:

- `Patients.API`: HTTP API built with ASP.NET Core Minimal APIs and Carter.
- `Patients.Application`: application use cases, DTOs, CQRS commands/queries, handlers, and validation.
- `Patients.Domain`: domain model, value objects, domain events, and core abstractions.
- `Patients.Infrastructure`: Entity Framework Core, PostgreSQL persistence, migrations, seed data, and database initialization.
- `BuildingBlocks`: shared CQRS, validation/logging behaviors, pagination, and common exceptions.

## Project Structure

```text
TheraBee/
  src/
    therabee.sln
    docker-compose.yml
    docker-compose.override.yml
    Clients/
      therabee-web/
    BuildingBlocks/
      BuildingBlocks/
    Services/
      Patients/
        Patients.API/
        Patients.Application/
        Patients.Domain/
        Patients.Infrastructure/
```

## Main Technologies

- .NET 8
- ASP.NET Core
- Carter
- MediatR
- Mapster
- FluentValidation
- Entity Framework Core
- PostgreSQL
- Docker Compose

## Local Requirements

- .NET SDK installed
- Docker Desktop running
- PostgreSQL container started through Docker Compose
- Optional: DBeaver or another database client
- Optional: Postman for API testing

## Build

From the repository root:

```powershell
dotnet restore src\therabee.sln
dotnet build src\therabee.sln
```

Build only the Patients API:

```powershell
dotnet build src\Services\Patients\Patients.API\Patients.API.csproj
```

Build the Angular frontend:

```powershell
cd src\Clients\therabee-web
npm.cmd run build
```

## Run With Docker Compose

From the `src` folder:

```powershell
docker compose up --build -d
```

Check container status:

```powershell
docker compose ps
```

View API logs:

```powershell
docker compose logs -f patients.api
```

Stop the stack:

```powershell
docker compose down
```

Stop the stack and remove the database volume:

```powershell
docker compose down -v
```

Use `down -v` only when you are comfortable deleting the local PostgreSQL data.

## Run The Angular Frontend

The Angular client lives in:

```text
src\Clients\therabee-web
```

Start the frontend locally:

```powershell
cd src\Clients\therabee-web
npm.cmd run start
```

The development URL is:

```text
http://localhost:4200
```

The current Angular client is still a frontend MVP with in-memory data. The next step is to connect it to the Patients API through Angular services.

## Local URLs And Ports

With the current Docker Compose configuration:

- Patients API: `http://localhost:6001`
- PostgreSQL: `localhost:5432`

## Database Connection

The Docker PostgreSQL connection uses:

```text
Host: localhost
Port: 5432
Database: therabee_patients
Username: postgres
Password: secret
```

Inside Docker, the API connects with:

```text
Host=patientsdb;Port=5432;Database=therabee_patients;Username=postgres;Password=secret
```

## Database Initialization

When the API runs in `Development`, it automatically:

- applies EF Core migrations;
- creates the database schema if needed;
- seeds initial patient data if the `Patients` table is empty.

The seed data is defined in:

```text
src\Services\Patients\Patients.Infrastructure\Data\Extensions\InitialData.cs
```

## Patient Model

A patient currently has:

- `id`: patient identifier.
- `name`: patient name.
- `dateOfBirth`: patient birth date.
- `patientAddress`: address object.
- `diagnosis`: clinical diagnosis text.
- `info`: extra information.
- `therapistId`: therapist identifier.

Address fields:

- `addressLine`
- `district`
- `location`
- `zipCode`

## API Endpoints

Base URL when running through Docker Compose:

```text
http://localhost:6001
```

PowerShell note: use `Invoke-RestMethod` or `curl.exe`. In Windows PowerShell, `curl` can resolve to `Invoke-WebRequest`, which handles headers differently from real curl.

## List Patients

```http
GET /patients
```

Optional query parameters:

- `pageIndex`: zero-based page index. Default: `0`.
- `pageSize`: number of records per page. Default: `10`.

Example:

```powershell
Invoke-RestMethod -Method GET -Uri "http://localhost:6001/patients?pageIndex=0&pageSize=10"
```

Expected response shape:

```json
{
  "patients": {
    "pageIndex": 0,
    "pageSize": 10,
    "count": 2,
    "data": [
      {
        "id": "00000000-0000-0000-0000-000000000000",
        "name": "Joao",
        "dateOfBirth": "1992-03-14T00:00:00Z",
        "patientAddress": {
          "addressLine": "Rua Agolada n50",
          "district": "Santarem",
          "location": "Vale Mansos",
          "zipCode": "2100-049"
        },
        "diagnosis": "Example diagnosis",
        "info": "Example info",
        "therapistId": "f372455a-c80c-487c-85a6-c5c6f18a83a8"
      }
    ]
  }
}
```

## Get Patient By Id

```http
GET /patients/id/{id}
```

Example:

```powershell
Invoke-RestMethod -Method GET -Uri "http://localhost:6001/patients/id/PUT_PATIENT_ID_HERE"
```

## Get Patient By Name

```http
GET /patients/name/{patientName}
```

Example:

```powershell
Invoke-RestMethod -Method GET -Uri "http://localhost:6001/patients/name/Joao"
```

Important: if more than one patient has the same name, the current handler returns the first match only.

## Get Patients By Therapist

```http
GET /patients/therapist/{therapistId}
```

Example:

```powershell
Invoke-RestMethod -Method GET -Uri "http://localhost:6001/patients/therapist/f372455a-c80c-487c-85a6-c5c6f18a83a8"
```

The response returns patients ordered by name.

## Create Patient

```http
POST /patients
```

Body:

```json
{
  "patient": {
    "name": "Maria Silva",
    "dateOfBirth": "1990-05-20T00:00:00Z",
    "patientAddress": {
      "addressLine": "Rua das Flores 10",
      "district": "Lisboa",
      "location": "Lisboa",
      "zipCode": "1000-001"
    },
    "diagnosis": "Initial diagnosis notes",
    "info": "Additional patient information",
    "therapistId": "f372455a-c80c-487c-85a6-c5c6f18a83a8"
  }
}
```

Example:

```powershell
$body = @{
  patient = @{
    name = "Maria Silva"
    dateOfBirth = "1990-05-20T00:00:00Z"
    patientAddress = @{
      addressLine = "Rua das Flores 10"
      district = "Lisboa"
      location = "Lisboa"
      zipCode = "1000-001"
    }
    diagnosis = "Initial diagnosis notes"
    info = "Additional patient information"
    therapistId = "f372455a-c80c-487c-85a6-c5c6f18a83a8"
  }
} | ConvertTo-Json -Depth 4

Invoke-RestMethod -Method POST -Uri "http://localhost:6001/patients" -ContentType "application/json" -Body $body
```

Expected response:

```json
{
  "id": "generated-patient-id"
}
```

## Update Patient

```http
PUT /patients
```

Body:

```json
{
  "patient": {
    "id": "PUT_PATIENT_ID_HERE",
    "name": "Maria Silva",
    "dateOfBirth": "1990-05-20T00:00:00Z",
    "patientAddress": {
      "addressLine": "Rua das Flores 10",
      "district": "Lisboa",
      "location": "Lisboa",
      "zipCode": "1000-001"
    },
    "diagnosis": "Updated diagnosis notes",
    "info": "Updated patient information",
    "therapistId": "f372455a-c80c-487c-85a6-c5c6f18a83a8"
  }
}
```

Example:

```powershell
$body = @{
  patient = @{
    id = "PUT_PATIENT_ID_HERE"
    name = "Maria Silva"
    dateOfBirth = "1990-05-20T00:00:00Z"
    patientAddress = @{
      addressLine = "Rua das Flores 10"
      district = "Lisboa"
      location = "Lisboa"
      zipCode = "1000-001"
    }
    diagnosis = "Updated diagnosis notes"
    info = "Updated patient information"
    therapistId = "f372455a-c80c-487c-85a6-c5c6f18a83a8"
  }
} | ConvertTo-Json -Depth 4

Invoke-RestMethod -Method PUT -Uri "http://localhost:6001/patients" -ContentType "application/json" -Body $body
```

Expected response:

```json
{
  "isSuccess": true
}
```

## Delete Patient

```http
DELETE /patients/{id}
```

Example:

```powershell
Invoke-RestMethod -Method DELETE -Uri "http://localhost:6001/patients/PUT_PATIENT_ID_HERE"
```

Expected response:

```json
{
  "isSuccess": true
}
```

## Validation Rules

Create patient currently validates:

- `name` is required.
- `dateOfBirth` is required.
- `dateOfBirth` cannot be in the future.
- `patientAddress` is required.
- `therapistId` cannot be empty.

Update patient currently validates:

- `id` is required.

The domain address value object also requires:

- `addressLine`
- `district`
- `location`
- `zipCode`

## Useful Development Commands

Build:

```powershell
dotnet build src\therabee.sln
```

Run API without Docker:

```powershell
dotnet run --project src\Services\Patients\Patients.API\Patients.API.csproj
```

Run with Docker Compose:

```powershell
cd src
docker compose up --build -d
```

View running containers:

```powershell
docker compose ps
```

View API logs:

```powershell
docker compose logs -f patients.api
```

View PostgreSQL logs:

```powershell
docker compose logs -f patientsdb
```

Stop containers:

```powershell
docker compose down
```

## Notes For Future Development

- Add automated tests when behavior becomes less experimental.
- Consider exposing Swagger/OpenAPI for easier manual API exploration.
- Keep the API host port on a browser-safe value such as `6001`.
- Avoid using `postgres:latest`; keep PostgreSQL pinned to a stable version.
- Keep the API layer thin and place business behavior in application/domain layers.
- Keep database-specific logic in `Patients.Infrastructure`.
