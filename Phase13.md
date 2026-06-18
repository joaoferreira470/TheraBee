# Phase 13 - Evidence-Based Dashboards And Reports

## Objective

Turn the reporting flow into an evidence-based clinical generator that uses session chronology, goal scoring, notes, and a fixed backend template, with AI helping to draft the narrative in Portuguese from Portugal.

## What Was Implemented

### Reporting Pipeline

- Added a fixed backend report template composed of structured sections.
- Added AI-assisted narrative generation for the report sections.
- The AI layer uses a strict JSON response contract and falls back safely to deterministic text when the API key is missing or the request fails.
- Reports now ingest the selected date interval and the sessions that fall inside it.
- Report drafts keep using the existing PDF and Word export pipeline.

### Data Flow

- The report draft builder now collects:
  - patient snapshot data
  - therapeutic goals
  - sessions in the selected interval
  - session assessments
  - current dashboard metrics
- The dashboard builder already accepts the same interval so the report and dashboard can stay aligned.

### Configuration

- Added OpenAI configuration wiring in infrastructure.
- Added API settings placeholders for the AI model and base URL.
- The API key is expected from environment configuration, not hardcoded.

## Validation

- `dotnet build src/therabee.sln` passed.

## Next Step

Move toward richer dashboard visuals and report charts based on the stored 0-10 goal assessment trends.
