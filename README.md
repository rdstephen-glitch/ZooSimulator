# Zoo Simulator
A zoo simulator built as an ASP.NET Core Web API on .NET 10, developed with Visual Studio 2026.

## Requirements
See the brief provided via email

## Running the solution
Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

**Visual Studio 2026:** open `ZooSimulator.slnx`, make sure `ZooSimulator.Api` is the startup project and choose Start Without Debugging. The app opens in a new browser tab.

**Any other editor:** from the repository root run:

```bash
dotnet run --project ZooSimulator.Api
```

Then browse to https://localhost:7055 (or http://localhost:5056). If the browser warns about the HTTPS development certificate, run `dotnet dev-certs https --trust` once.

## Approach
I considered a console app, a server-side web app, and a Web API with a simple front end.

I chose the Web API as it shows API usage and allows any front end to be placed on top. For this exercise the user interface is a thin web client over the API.

### Use of AI
For transparency I used Claude for some parts of this exercise, as I know after my discussion with Matt that the company is proactively using AI.

I used it mostly as a sounding board for my ideas and to help with repetitive code and unfamiliar parts of the stack, especially the front end code. 

I ran, debugged and committed everything myself and made the design decisions, such as Web API over a console or server-rendered app, minimal APIs,
a single project organised by folders, and the extra features (reset, per-type feeding, random names, at-risk warning and game-over effect).

### Design Notes
- Health is a `double` percentage. Damage and feeding are both a percentage of the animal's *current* health, and feeding is capped at 100%.
- Each animal type owns its own health rules, so the zoo needs no type checks for them.
- Randomness (`IRandomSource`) and time (`TimeProvider`) are injected so the simulation can be tested deterministically.
- A `BackgroundService` with a `PeriodicTimer` advances the simulation by one hour every 20 seconds. A lock protects the zoo state because the timer and web requests share it.
- Health rules are checked immediately after each hour's damage is applied.
- An elephant below 70% health cannot walk. If it is still below 70% after the following hour's damage, it dies. Feeding it back to 70% or above cancels that countdown.
- Dead animals are not damaged or fed, and stay in the zoo marked as dead.
- Simulated time starts at the real start-up time.

## Solution Structure

    A single API project organised by folder was chosen over multiple projects to avoid over-engineering.

- **Solution Items:** README and .gitignore
- **ZooSimulator.Api**
  - `Program.cs`: registers services, serves static files and maps the endpoints
  - `Properties`: launch settings
  - `wwwroot`: the front end (HTML, CSS and JavaScript)
  - `Models`: animals and their rules
  - `Services`: the zoo, the random source and the background simulation
  - `Contracts`: API response shapes
  - `Endpoints`: mapped API routes, all under `https://localhost:7055/api/zoo`:
    - `GET /api/zoo`: status
    - `POST /api/zoo/feed`: feed all animals
    - `POST /api/zoo/feed/{type}`: feed one animal type
    - `POST /api/zoo/reset`: reset the zoo
- **ZooSimulator.Tests:** xUnit tests (see Testing)

## Assumptions, out of scope and extras
- No database is used, so all data is held in memory. Stopping the app, or pressing the reset button, returns the zoo to its opening state. Because state is in memory, the app is designed to run as a single instance.
- Animal names are picked at random from a fixed list for each type, and are unique within the zoo.
- The main feed button feeds every type at once, using one random value per type. Per-type buttons are an extra, each using one random value for that type.
- When all animals have died, an animation and a synthesised sound (Web Audio API, no audio files) play. If audio is blocked or unavailable, the app carries on silently.

## Tech stack
- .NET 10 and C#
- ASP.NET Core minimal APIs
- `BackgroundService` with `PeriodicTimer`
- Plain HTML, CSS and JavaScript front end (no framework), served as static files by the API
- xUnit

## Testing
A Unit tests project was added and written with xUnit in the `ZooSimulator.Tests` project.

Thought this would be a valued addition and good future enhancement, this also helps with QA.

Sometimes I use the logger for QA first or even the UI and add the tests later but do think it will be best to write these as code is written.

Also it helps to test the code before building/seeing the results in the UI.

To execute the tests, in Visual Studio go to the top navigation, select Test, Select Test Explorer, Click Run all Tests in View

Just added 2 samples that checks for random value generation used to return the health damage values and also reducing health as a percentage
