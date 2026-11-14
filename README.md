# Zoo Simulator
A zoo simulator built as an ASP.NET Core Web API on .NET 10.

Developed with Visual Studio 2026

## Requirements
See the brief provided via email.

## Approach
I considered a console app, a server-side web app, and a Web API with a simple front end.

I have chosen the Web API as it shows API usage and also allows for any front end choice to be implemented on top.

For this exercise the user interface is a thin client over the API.

## Assumptions/Out of scope
No database is used, so all data is held in memory and is not persistent. This could be a sensible enhancement in the future.

## Tech Stack
- .NET 10 and C#
- ASP.NET Core minimal APIs
- Front End - TBC

## Structure
A single API project with folders was chosen over multiple projects to avoid over-engineering, keeping the solution quick to navigate, build and run.

## Testing
Unit tests are written with xUnit in the `ZooSimulator.Tests` project.

The simulation relies on randomness and the passing of time, so thought beneficial to have some unit tests regarding this.

## FE?
TBC

## Running the solution
TBC