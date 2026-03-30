---
name: GenerateCQRSFeature
description: Guide to generating a CQRS-based feature using MediatR (including Entity, Command/Query, Handler, and Controller).
---

# GenerateCQRSFeature Skill

When a user requests to create a feature using the **CQRS** pattern, providing the **Feature Name** (e.g., `Order`) and **Action** (e.g., `Create`), you **MUST** follow these steps to generate the corresponding code and files:

## 1. Directory and File Outputs

Based on the {FeatureName} and {Action}, create the following files (if the action is Read/Get, change 'Commands' to 'Queries'):

1. **Domain Entity**
   - **Path**: `Domain/Entities/{FeatureName}.cs`
   - **Content**: A basic Entity class defining the properties.

2. **Command / Query**
   - **Path**: `Application/Features/{FeatureName}s/Commands/{Action}{FeatureName}/{Action}{FeatureName}Command.cs` 
   - **Content**: An input model that implements MediatR's `IRequest<TResponse>`.

3. **Command / Query Handler**
   - **Path**: `Application/Features/{FeatureName}s/Commands/{Action}{FeatureName}/{Action}{FeatureName}CommandHandler.cs`
   - **Content**: Business logic processor that implements `IRequestHandler<{Action}{FeatureName}Command, TResponse>`. Inject any needed Repository or service here.

4. **WebAPI Controller**
   - **Path**: `WebAPI/Controllers/{FeatureName}sController.cs` (Remember to pluralize the Controller name if applicable).
   - **Content**: REST API endpoints. **Requirement:** Must receive `IMediator` via Dependency Injection in the constructor to dispatch requests (e.g., `await _mediator.Send(command)`).

## 2. Dependency Injection Configuration (NOT REQUIRED)

⚠️ **Note:** As per project standards, the CQRS flow **DOES NOT REQUIRE** updating Dependency Injection configuration files.

The MediatR library automatically scans and registers all Handlers inheriting from `IRequestHandler`. Therefore, you **ABSOLUTELY MUST SKIP** updating the DI config files for Handlers.
