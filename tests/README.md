# Portable regression checks

These tests execute the selected production source using synthetic inputs and explicit platform stubs. They do not start the app, access a sensor, location, network, audio device or real game service.

Run `dotnet run --project tests/Portable.Tests/Portable.Tests.csproj --configuration Release` with .NET SDK 10.0.401. The harness links the actual application files and compiles in C# 5 mode, with no NuGet package sources. The historic Phone project and its dependencies remain unchanged.

Coverage: repeated Record, one callback per synthetic event, Stop/repeated Stop, restart, leaving the page and opening a replacement page. Sensor/event, dispatcher, UI, AR and media APIs are inert stubs; no sensor hardware or service is started. Native dispatch races, real sensor acquisition, UI/audio/AR and full application execution remain unverified.

Full native validation still requires the matching historical Windows Phone SDK and original third-party assemblies. This harness is not a platform migration or a native build certification.
