# Zoo Simulator
A zoo simulator built as an ASP.NET Core Web API on .NET 10.

Developed with Visual Studio 2026

## Requirements
See the brief provided via email.

## Approach
I considered a console app, a server-side web app, and a Web API with a simple front end.

I have chosen the Web API as it shows API usage and also allows for any front end choice to be implemented on top.

For this exercise the user interface is a thin client over the API.

Health rules are checked immediately after each hour's damage is applied.

An elephant below 70% health cannot walk. If it is still below 70% after the following hour's damage, it dies. Feeding it back to 70% or above cancels that countdown.

Dead animals are not damaged or fed, and stay in the zoo marked as terminated.

*Document Zoo End Points

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

Thought this would be a valued addition, this also helps with QA.

Sometimes I use the logger for QA first or even the UI and add the tests later but may be better to write them as you code each section.

Also it helps to test the code before building/seeing the reults in the UI.

To execute the tests, in Visual Studio got to the top navigation, select Test, Select Test Explorer, Click Run all Tests in View

Confirm the tests have passed, if not then please investigate.

TODO: List out all tests?

## FE?
TBC

## Running the solution
TBC