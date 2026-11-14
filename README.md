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

Thought this would be a valued addition, it is good to write the unit tests as you write the code, this also helps with QA.

Also it helps to test the code before building/seeing the reults in the UI.

To execute the tests, in Visual Studio got to the top navigation, select Test, Select Test Explorer, Click Run all Tests in View

Confirm the tests have passed, if not then please investigate.

TODO: List out all tests?

## FE?
TBC

## Running the solution
TBC