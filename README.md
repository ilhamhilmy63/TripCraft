# TripCraft — Student B Component

This branch contains only the work assigned to **Student B** for TripCraft.

## Component scope

- Guide management
- Vehicle management
- Hotel and room-type management
- Resource availability searches
- Transactional resource holds
- Resource & Action agent
- OpenRouteService distance integration
- Guide schedule and trip-day views
- GPS stop check-in
- QR voucher scanning

Trip Requests & Itinerary and Quotation, Approval & Reporting features owned by other students are intentionally excluded.

## Main features

### Resource Management

Operations managers can create, update, list and deactivate guides, vehicles and hotels. Hotels include room types with capacity, nightly rates and available room counts.

Availability searches filter resources by travel dates, passenger count, language and city. Resource holds reject overlapping guide or vehicle reservations and prevent room inventory from becoming negative.

### Resource & Action Agent

The Resource & Action agent receives the proposed itinerary and finds suitable guides, vehicles and rooms. It reports any availability gaps and proposes resources without directly confirming a booking.

Its relevant tools are:

- `check_guide_availability`
- `check_vehicle_availability`
- `check_room_availability`
- `get_rate_card`

### OpenRouteService

`DistanceService` obtains road distance and duration information from OpenRouteService. The integration uses a controlled HTTP client and falls back safely when the provider is unavailable.

### Mobile device features

- GPS-based guide check-in at itinerary stops
- QR voucher scanning
- Guide schedule and trip-day views

## Repository structure

```text
agents/
  app/nodes/resources.py
  app/tools/check_guide_availability.py
  app/tools/check_vehicle_availability.py
  app/tools/check_room_availability.py
  app/tools/get_rate_card.py
  tests/test_resources.py

backend/
  src/TripCraft.Api/Controllers/Resources/
  src/TripCraft.Application/Resources/
  src/TripCraft.Infrastructure/Resources/
  src/TripCraft.Infrastructure/External/DistanceService.cs
  tests/TripCraft.Tests/Resources/
  tests/TripCraft.Tests/Workflows/External/DistanceServiceTests.cs

web/
  src/features/resources/

mobile/
  lib/features/resources/
  test/resources/

docs/
  adr/ADR-005-cloud-deployment-platform.md
  adr/ADR-006-llm-provider.md
  report/individual-B.md
```

## Configuration

Copy the example environment file and configure the services being run. OpenRouteService uses:

```env
ORS_API_KEY=your_openrouteservice_api_key
```

Do not commit real secrets or local environment files.

## Run the Resource & Action agent tests

```bash
cd agents
python -m venv .venv
pip install -r requirements.txt
pytest tests/test_resources.py
```

## Run the web interface

```bash
cd web
npm install
npm run dev
```

Student B's React routes cover guides, vehicles, hotels and availability.

## Run the Flutter application

```bash
cd mobile
flutter pub get
flutter run
```

The Student B mobile flow includes guide schedules, GPS check-in and QR voucher scanning.

## Relevant tests

- Agent tests: `agents/tests/test_resources.py`
- Backend tests: `backend/tests/TripCraft.Tests/Resources/`
- OpenRouteService tests: `backend/tests/TripCraft.Tests/Workflows/External/DistanceServiceTests.cs`
- React tests: `web/src/features/resources/__tests__/`
- Flutter tests: `mobile/test/resources/`

## Branch

Student B work is maintained on branch `IT24103817`.
