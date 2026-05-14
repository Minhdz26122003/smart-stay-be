---
name: generate-full-feature
description: |
  Scaffold a complete N-Tier feature for Smart Stay BE: Entity, DTOs, Interface, Service, Controller, Mapping, Validator, DI registration, and Migration.
  Use when user says "tạo feature mới", "thêm module", "generate feature", or similar.
---

# Generate Full Feature — Smart Stay BE

## Khi Nào Dùng

Khi user yêu cầu tạo một **module/feature hoàn chỉnh mới** (ví dụ: "tạo module Payment", "thêm feature Poll").

> ⚠️ Nếu chỉ thêm 1 endpoint vào module có sẵn → KHÔNG dùng skill này, dùng workflow `/add-endpoint` thay thế.

---

## Input Cần Từ User

Trước khi bắt đầu, **PHẢI** hỏi/xác nhận với user:

1. **Feature Name** (singular, PascalCase) — ví dụ: `Payment`, `Poll`, `ChatMessage`
2. **Properties** — danh sách fields chính + kiểu dữ liệu
3. **Role authorization** — ai được dùng? (Landlord, Tenant, hoặc cả hai)
4. **CRUD scope** — cần những operations nào? (Create, GetAll, GetById, Update, Delete)
5. **Quan hệ** (nếu có) — FK đến entity nào? (ví dụ: `PropertyId`, `TenantId`)

---

## Quy Trình Thực Hiện — 9 Bước

### Bước 1: Entity (Domain Layer)

**Path**: `src/SmartStay.Domain/Entities/{Feature}.cs`

```csharp
using System;

namespace SmartStay.Domain.Entities;

public class {Feature} : BaseEntity
{
    // Properties do user cung cấp
    public Guid {ParentEntity}Id { get; set; }
    public string Name { get; set; } = string.Empty;
    // ... thêm properties
}
```

**Rules**:
- Kế thừa `BaseEntity` (có sẵn `Id`, `CreatedAt`, `UpdatedAt`, `IsDeleted`)
- FK dùng `Guid`, KHÔNG tạo navigation property trừ khi user yêu cầu
- String properties mặc định `= string.Empty`
- Nullable fields dùng `?`

---

### Bước 2: DTOs (Application Layer)

**Path**: `src/SmartStay.Application/DTOs/{Feature}/`

Tạo thư mục + các file:

#### `Create{Feature}Request.cs`
```csharp
namespace SmartStay.Application.DTOs.{Feature};

public class Create{Feature}Request
{
    // Chỉ chứa fields mà CLIENT gửi lên
    // KHÔNG bao gồm: Id, CreatedAt, UpdatedAt, IsDeleted
    // KHÔNG bao gồm: FK của user hiện tại (lấy từ JWT claims)
}
```

#### `{Feature}Dto.cs` (Response)
```csharp
namespace SmartStay.Application.DTOs.{Feature};

public class {Feature}Dto
{
    public Guid Id { get; set; }
    // Tất cả fields + CreatedAt
    public DateTime CreatedAt { get; set; }
}
```

#### `Update{Feature}Request.cs` (nếu có Update operation)
```csharp
namespace SmartStay.Application.DTOs.{Feature};

public class Update{Feature}Request
{
    // Chỉ chứa fields cho phép update
}
```

---

### Bước 3: Service Interface (Application Layer)

**Path**: `src/SmartStay.Application/Interfaces/I{Feature}Service.cs`

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.{Feature};

namespace SmartStay.Application.Interfaces;

public interface I{Feature}Service
{
    Task<ApiResponse<{Feature}Dto>> CreateAsync(Guid userId, Create{Feature}Request request);
    Task<ApiResponse<IEnumerable<{Feature}Dto>>> GetAllBy{Parent}Async(Guid parentId);
    Task<ApiResponse<{Feature}Dto>> GetByIdAsync(Guid id);
    Task<ApiResponse<{Feature}Dto>> UpdateAsync(Guid userId, Guid id, Update{Feature}Request request);
    Task<ApiResponse<bool>> DeleteAsync(Guid userId, Guid id);
}
```

**Rules**:
- Return type LUÔN là `Task<ApiResponse<T>>`
- Param đầu tiên thường là `Guid userId` (lấy từ JWT)
- Async suffix cho TẤT CẢ methods

---

### Bước 4: Service Implementation (Infrastructure Layer)

**Path**: `src/SmartStay.Infrastructure/Services/{Feature}Service.cs`

```csharp
using AutoMapper;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.{Feature};
using SmartStay.Application.Interfaces;
using SmartStay.Domain.Entities;
using SmartStay.Domain.Exceptions;
using SmartStay.Domain.Interfaces;

namespace SmartStay.Infrastructure.Services;

public class {Feature}Service(
    IRepository<{Feature}> {feature}Repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : I{Feature}Service
{
    public async Task<ApiResponse<{Feature}Dto>> CreateAsync(Guid userId, Create{Feature}Request request)
    {
        var entity = mapper.Map<{Feature}>(request);
        entity.{OwnerField} = userId;

        await {feature}Repository.AddAsync(entity);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<{Feature}Dto>(entity);
        return ApiResponse<{Feature}Dto>.Ok(dto, "{Feature} created successfully.");
    }

    // ... implement các methods khác theo pattern tương tự
}
```

**Rules**:
- Dùng **primary constructor** (C# 12)
- Inject: `IRepository<{Feature}>`, `IUnitOfWork`, `IMapper`
- Exception handling: `NotFoundException`, `UnauthorizedException`, `BadRequestException`
- Soft delete: dùng `repository.SoftDeleteAsync(id)` + `unitOfWork.CommitAsync()`
- Authorization check: so sánh `entity.OwnerId != userId` → throw `UnauthorizedException`

---

### Bước 5: Controller (API Layer)

**Path**: `src/SmartStay.API/Controllers/{Feature}Controller.cs`

```csharp
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStay.Application.DTOs.{Feature};
using SmartStay.Application.Interfaces;

namespace SmartStay.API.Controllers;

[Route("api/v1/{features-kebab-case}")]
[ApiController]
[Authorize(Roles = "{Role}")]
public class {Feature}Controller(I{Feature}Service {feature}Service) : ControllerBase
{
    private Guid GetUserId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            throw new UnauthorizedAccessException("Invalid user token.");
        return userId;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Create{Feature}Request request)
    {
        var userId = GetUserId();
        var response = await {feature}Service.CreateAsync(userId, request);
        return Ok(response);
    }

    // ... GET, PUT, DELETE endpoints theo CRUD scope
}
```

**Rules**:
- Route: `api/v1/{features}` (kebab-case, plural) — ví dụ: `api/v1/payments`, `api/v1/chat-messages`
- Primary constructor inject service
- `GetUserId()` private helper method lấy user ID từ JWT claims
- `[Authorize(Roles = "...")]` ở class level, override ở method level nếu cần
- POST return `Ok(response)` (pattern hiện tại, KHÔNG dùng `CreatedAtAction`)

---

### Bước 6: AutoMapper Mapping

**File**: `src/SmartStay.Application/Mappings/MappingProfile.cs`

**Thêm vào cuối constructor** `MappingProfile()`:

```csharp
// {Feature}
CreateMap<{Feature}, SmartStay.Application.DTOs.{Feature}.{Feature}Dto>();
CreateMap<SmartStay.Application.DTOs.{Feature}.Create{Feature}Request, {Feature}>();
// Nếu có Update:
CreateMap<SmartStay.Application.DTOs.{Feature}.Update{Feature}Request, {Feature}>();
```

**Rules**:
- Dùng fully-qualified namespace cho DTOs khi add vào `MappingProfile`
- Thêm comment `// {Feature}` trước mỗi block
- Nếu mapping phức tạp (flatten, custom) → dùng `.ForMember()`

---

### Bước 7: FluentValidation Validator

**Path**: `src/SmartStay.Application/Validators/{Feature}/Create{Feature}RequestValidator.cs`

```csharp
using FluentValidation;
using SmartStay.Application.DTOs.{Feature};

namespace SmartStay.Application.Validators.{Feature};

public class Create{Feature}RequestValidator : AbstractValidator<Create{Feature}Request>
{
    public Create{Feature}RequestValidator()
    {
        RuleFor(x => x.PropertyName)
            .NotEmpty().WithMessage("{PropertyName} is required.");

        // Thêm rules theo business logic
    }
}
```

**Rules**:
- Tạo thư mục `Validators/{Feature}/` nếu chưa có
- Validator naming: `Create{Feature}RequestValidator`, `Update{Feature}RequestValidator`
- Error message bằng English
- FluentValidation tự động scan & register (không cần DI thủ công)

---

### Bước 8: Dependency Injection Registration

**File**: `src/SmartStay.Infrastructure/DependencyInjection.cs`

**Thêm dòng sau** vào section `// Application Services`:

```csharp
services.AddScoped<I{Feature}Service, {Feature}Service>();
```

⚠️ **BẮT BUỘC** — Nếu quên bước này, DI container sẽ throw exception khi resolve service.

---

### Bước 9: EF Core Migration

Nếu Entity mới cần bảng trong database:

1. **Thêm `DbSet`** vào `AppDbContext`:
```csharp
public DbSet<{Feature}> {Feature}s { get; set; }
```

2. **Tạo migration**:
```bash
dotnet ef migrations add Add{Feature}Table --project src/SmartStay.Infrastructure --startup-project src/SmartStay.API
```

3. **Apply migration** (nếu user đồng ý):
```bash
dotnet ef database update --project src/SmartStay.Infrastructure --startup-project src/SmartStay.API
```

---

## Checklist Sau Khi Hoàn Thành

Sau khi tạo xong tất cả files, AI **PHẢI** thực hiện:

- [ ] Build project: `dotnet build` — phải pass không lỗi
- [ ] Liệt kê tất cả files đã tạo/sửa
- [ ] Liệt kê tất cả endpoints mới (Method + Path + Role)
- [ ] Hướng dẫn manual test (curl hoặc Swagger steps) cho MỖI endpoint
- [ ] Cập nhật `docs/plans/task.md`
- [ ] Cập nhật `docs/CHANGELOG.md` (Added: new {Feature} module with X endpoints)
- [ ] Đề xuất test data nếu cần

---

## Ví Dụ Sử Dụng

**User**: "Tạo module Payment để quản lý thanh toán"

**AI sẽ hỏi**:
1. Properties nào? (Amount, Method, InvoiceId, Status, Note, ...)
2. Ai được dùng? (Landlord tạo, Tenant xem)
3. CRUD nào? (Create, GetByInvoice, GetById, UpdateStatus, Delete)

**AI sẽ tạo 9 bước** → build → test guide → CHANGELOG.
