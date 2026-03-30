---
name: GenerateNTierFeature
description: Guide to generating an N-Tier architecture feature (including Entity, Interface, Service, Controller, and Dependency Injection configuration).
---

# GenerateNTierFeature Skill

When a user requests to create a feature using the **N-Tier** flow, providing the **Feature Name** (e.g., `Category`), you **MUST** follow these steps to generate the corresponding code and files:

## 1. Directory and File Outputs

Create the following files with appropriate content for the {FeatureName}:

1. **Domain Entity**
   - **Path**: `Domain/Entities/{FeatureName}.cs`
   - **Content**: A basic Entity class defining the properties.

2. **Service Interface**
   - **Path**: `Application/Interfaces/I{FeatureName}Service.cs`
   - **Content**: Interfaces defining the method contracts for {FeatureName}.

3. **Service Implementation**
   - **Path**: `Application/Services/{FeatureName}Service.cs`
   - **Content**: A class that inherits from `I{FeatureName}Service`. **Requirement:** Use constructor Dependency Injection (DI) to receive instances (e.g., `IRepository<{FeatureName}>`).

4. **WebAPI Controller**
   - **Path**: `WebAPI/Controllers/{FeatureName}sController.cs` (Remember to pluralize the Controller name).
   - **Content**: The Controller class. **Requirement:** Must receive `I{FeatureName}Service` via constructor DI and define the corresponding endpoints.

## 2. Dependency Injection Configuration (MANDATORY STEP)

⚠️ **Important:** Unlike CQRS with MediatR mentioned above, the N-Tier flow **REQUIRES** you to manually register the newly created service into the IoC Container. 

You **MUST** use the appropriate tool to open the project's DI configuration file (could be `DependencyInjection.cs` or `ServiceCollectionExtensions.cs` depending on the directory structure) and inject the following code block into the relevant service configuration method:

```csharp
services.AddScoped<I{FeatureName}Service, {FeatureName}Service>();
```

Make sure to use an accurate file editing tool (`replace_file_content` or `multi_replace_file_content`) to avoid breaking the DI file's syntax.
