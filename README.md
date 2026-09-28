# TripCraft — Student A Component

This branch contains only the work assigned to **Student A** for TripCraft.

## Component scope

- Trip request creation, listing, editing and cancellation
- Attraction management
- Day-by-day itinerary generation and display
- Planner / Coordinator agent
- Itinerary Analysis agent
- OpenWeatherMap forecast integration
- Passport-photo capture using the device camera or gallery
- Date-range picker for trip dates
- Itinerary map with location markers

The Resource Management and Quotation, Approval & Reporting components owned by other students are intentionally excluded from this branch.

## Main features

### Trip Requests & Itinerary

Tourists can submit a trip request with an objective, travel dates, passenger count, budget, preferences and passport photo. Operations managers can browse trip requests, manage attractions and inspect or edit itinerary details.

The backend validates dates, passenger counts, trip ownership and uploaded images. It stores trip requests, attractions, itineraries, itinerary days and itinerary stops.

### Planner / Coordinator Agent

The Planner converts the tourist's request into an ordered plan with constraints such as cities, dates, passenger count, preferences and budget. It uses the allowed `parse_dates` and `list_agents` tools.

### Itinerary Analysis Agent

The Itinerary Analysis agent builds the day-by-day travel plan. It selects attractions, orders stops, chooses transport information and attaches available weather information. Its relevant tools are:

- `get_attractions`
- `get_distance`
- `get_weather`

### OpenWeatherMap

`WeatherService` calls the OpenWeatherMap five-day forecast API. Weather is advisory: when the API key is missing, the requested date is outside the forecast window, or the provider fails, itinerary planning continues without weather.

### Mobile device features

- Camera/gallery passport-photo selection
- Date-range picker
- Interactive itinerary map

## Repository structure

```text
agents/
  app/nodes/planner.py
  app/nodes/itinerary.py
  app/tools/
  tests/test_planner.py
  tests/test_itinerary.py

backend/
  src/TripCraft.Api/Controllers/Trips/
  src/TripCraft.Application/Trips/
  src/TripCraft.Infrastructure/Trips/
  src/TripCraft.Infrastructure/External/WeatherService.cs
  tests/TripCraft.Tests/Trips/
  tests/TripCraft.Tests/Workflows/External/WeatherServiceTests.cs

web/
  src/features/trips/

mobile/
  lib/features/trips/
  test/trips/

docs/
  adr/ADR-002-flutter-state-management.md
  adr/ADR-003-agentic-ai-framework.md
  report/individual-A.md
```

## Configuration

Copy the example environment file and provide values required by the service being run. The weather integration uses:

```env
OWM_API_KEY=your_openweathermap_api_key
```

Do not commit real secrets or local environment files.

## Run the Planner and Itinerary agents

```bash
cd agents
python -m venv .venv
pip install -r requirements.txt
pytest tests/test_planner.py tests/test_itinerary.py
```

## Run the web interface

```bash
cd web
npm install
npm run dev
```

The included React routes are limited to:

- `/trips`
- `/trips/:id`
- `/attractions`

## Run the Flutter application

```bash
cd mobile
flutter pub get
flutter run
```

The Flutter application includes the Student A tourist flow: registration/login, trip request submission, passport-photo capture, trip history, itinerary details and map display.

## Relevant tests

- Agent tests: `agents/tests/test_planner.py`, `agents/tests/test_itinerary.py`
- Backend tests: `backend/tests/TripCraft.Tests/Trips/`
- Weather tests: `backend/tests/TripCraft.Tests/Workflows/External/WeatherServiceTests.cs`
- React tests: `web/src/features/trips/__tests__/`
- Flutter tests: `mobile/test/trips/`

## Branch

Student A work is maintained on branch `IT24103652`.
