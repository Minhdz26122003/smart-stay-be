# Smart Stay — Backend Base Implementation Plan

> **For Antigravity:** REQUIRED WORKFLOW: Use `.agent/workflows/execute-plan.md` to execute this plan in single-flow mode.

**Goal:** Xây dựng backend .NET 9 N-Tier hoàn chỉnh cho hệ thống Rental Room Manager (RRM), bao gồm Auth (JWT + Refresh Token), quản lý phòng/hợp đồng/hóa đơn, real-time (SignalR + Firebase FCM), upload file (Cloudinary), email queue (Hangfire), với đầy đủ versioning API, RBAC, validation, logging, pagination, và bảo mật.

**Architecture:** N-Tier Multi-Project Solution: `SmartStay.Domain` → `SmartStay.Application` → `SmartStay.Infrastructure` → `SmartStay.API`. Tầng phụ thuộc một chiều (API → Infrastructure → Application → Domain).

**Tech Stack:**
- **Framework:** .NET 9, ASP.NET Core Web API
- **Database:** PostgreSQL + Entity Framework Core 9 (Npgsql)
- **Auth:** JWT Bearer + Refresh Token Rotation, BCrypt.Net
- **Real-time:** SignalR + Firebase Admin SDK (FCM)
- **File Storage:** Cloudinary SDK
- **Background Jobs:** Hangfire + Hangfire.PostgreSql
- **Email:** MailKit + Hangfire queue
- **Logging:** Serilog + Seq
- **Validation:** FluentValidation
- **API Docs:** Swashbuckle with API versioning
- **Mapping:** AutoMapper
- **Test:** xUnit + Moq + Testcontainers

---

## User Review Required

> [!IMPORTANT]
> **Phạm vi (Phase 1 — MVP):** Auth, Property/Room, Contract/Inventory, Billing cơ bản, Ticketing, và toàn bộ Infrastructure (SignalR, Hangfire, Cloudinary, Email). OCR và Marketplace nâng cao là Phase 2.

> [!WARNING]
> **Database:** PostgreSQL. Nếu muốn SQL Server, đổi package `Npgsql.EntityFrameworkCore.PostgreSQL` thành `Microsoft.EntityFrameworkCore.SqlServer`.

> [!CAUTION]
> **CCCD:** Cloudinary authenticated mode + signed URL (TTL 15 phút). Không bao giờ expose publicUrl cho ảnh CCCD.

---

## Proposed Changes

### Component 0: Solution Setup

```
SmartStay.sln
├── src/
│   ├── SmartStay.Domain/           (Class Library)
│   ├── SmartStay.Application/      (Class Library)
│   ├── SmartStay.Infrastructure/   (Class Library)
│   └── SmartStay.API/              (ASP.NET Core Web API)
├── tests/
│   └── SmartStay.Tests/            (xUnit)
├── docs/plans/
├── docker-compose.yml
└── .gitignore
```

**Project References (một chiều):**
- `API` ref `Infrastructure` + `Application`
- `Infrastructure` ref `Application`
- `Application` ref `Domain`
- `Domain` — không phụ thuộc ai

---

### Component 1: Domain Layer

#### [NEW] Entities — `src/SmartStay.Domain/Entities/`

| Entity | Các trường chính |
|--------|-----------------|
| `BaseEntity` | Id(Guid), CreatedAt, UpdatedAt, IsDeleted |
| `User` | FullName, Phone, Email?, PasswordHash, Roles[], FcmToken, IsActive |
| `RefreshToken` | UserId, TokenHash, ExpiresAt, IsRevoked, DeviceInfo |
| `Property` | LandlordId, Name, Address(ValueObject), Rules, SharedAmenities |
| `Room` | PropertyId, Name, Type(enum), BasePrice, AreaM2, Status(enum), MaxOccupants |
| `ServiceConfig` | PropertyId, RoomId?, Type(enum), UnitPrice, CalcMethod(enum) |
| `Contract` | RoomId, TenantId, DepositAmount, StartDate, EndDate, Status(enum), ScannedContractUrl |
| `Roommate` | ContractId, FullName, Phone, CccdPhotoUrl(encrypted), IsApproved |
| `InventoryItem` | ContractId, ItemName, CheckInPhotos[], CheckOutPhotos[], Condition(enum) |
| `MeterReading` | RoomId, Type(E/W), OldUnit, NewUnit, PhotoUrl, Month, Year |
| `Invoice` | RoomId, ContractId, Month, Year, BreakdownJson(JSONB), TotalAmount, Status(enum), PaidAmount |
| `Ticket` | RoomId, TenantId, Category(enum), Title, Description, PhotoUrls[], Status(enum), Priority(enum) |
| `Announcement` | PropertyId, RoomId?, Title, Content, CreatedBy |
| `Listing` | RoomId, PhotoUrls[], Description, IsActive |
| `Booking` | ListingId, GuestId, ProposedDateTime, Status(enum), Note |
| `ChatMessage` | ConversationId, SenderId, Content, MessageType(enum), SentAt |
| `Review` | ReviewerId, RevieweeId, ContractId, Rating(1-5), Comment |
| `Vehicle` | TenantId, PlateNumber, VehicleType(enum), PhotoUrl |
| `VisitorLog` | TenantId, VisitorName, Phone, StayOvernight, ArrivedAt |
| `Notification` | UserId, Title, Body, Type(enum), IsRead, Payload(JSON) |

#### [NEW] Enums, Value Objects, Exceptions, Repository Interfaces

```csharp
// Enums/: RoomStatus, ContractStatus, InvoiceStatus, TicketStatus, ServiceCalcMethod
// Roles constants: Landlord, Tenant, Guest
// ValueObjects/: Address record(Street, Ward, District, City)
// Exceptions/: DomainException(base), NotFoundException(404), UnauthorizedException(401), ConflictException(409)
// Interfaces/: IRepository<T>, IUnitOfWork, IUserRepository, IRefreshTokenRepository
```

---

### Component 2: Application Layer

**NuGet:** `FluentValidation`, `AutoMapper`, `Microsoft.Extensions.DependencyInjection.Abstractions`

#### [NEW] Common wrappers

```csharp
ApiResponse<T>  { bool Success, T? Data, string? Message, List<string>? Errors }
PagedResult<T>  { List<T> Items, int TotalCount, int Page, int PageSize, int TotalPages }
PagedQuery      { int Page = 1, int PageSize = 20 }  // base for all list queries
```

#### [NEW] Service Interfaces — `src/SmartStay.Application/Interfaces/Services/`

| Interface | Chức năng chính |
|-----------|----------------|
| `IAuthService` | Register, Login, RefreshToken, Logout, ChangePassword |
| `ITokenService` | GenerateAccessToken, GenerateRefreshToken, HashToken |
| `IPropertyService` | CRUD, GetDashboard |
| `IRoomService` | CRUD, UpdateStatus, GetAvailable |
| `IServiceConfigService` | CRUD per property/room |
| `IContractService` | Create, Renew, Terminate, GetActive |
| `IInventoryService` | AddItem, CheckOut, GetByContract |
| `IMeterReadingService` | RecordReading, GetHistory |
| `IBillingService` | CalculateInvoice, CreateInvoice, MarkPaid, GetHistory |
| `ITicketService` | Create, UpdateStatus, GetByRoom, GetByProperty |
| `IAnnouncementService` | Create, GetByProperty |
| `IListingService` | Publish, Unpublish, Search(paginated), GetDetail |
| `IBookingService` | Create, Confirm, Reject |
| `IChatService` | SendMessage, GetConversation, GetConversations |
| `IReviewService` | Create, GetByReviewee |
| `IFileUploadService` | UploadImageAsync, UploadPdfAsync, DeleteAsync, GetSignedUrl |
| `IEmailService` | SendEmailAsync (enqueued via Hangfire) |
| `INotificationService` | SendPushAsync(FCM), SendToTopicAsync |
| `ISignalRService` | NotifyUserAsync, NotifyGroupAsync |
| `ICurrentUserService` | UserId, Roles, IsAuthenticated |

#### [NEW] DTOs, Validators, AutoMapper Profiles, DI Registration

```
DTOs/: Auth, Property, Room, Contract, Inventory, Billing, Ticket, Listing, Booking, Chat, Review, Notification
Validators/: RegisterDto, LoginDto, CreateRoomDto, CreateMeterReadingDto, CreateInvoiceDto, SearchListingQuery
Mappings/: một Profile per module
DependencyInjection.cs: AddAutoMapper + AddValidatorsFromAssembly
```

---

### Component 3: Infrastructure Layer

**NuGet:** Npgsql EF Core, JwtBearer, BCrypt.Net-Next, CloudinaryDotNet, Hangfire.PostgreSql, MailKit/MimeKit, FirebaseAdmin, Serilog, AutoMapper

#### [NEW] Persistence — `src/SmartStay.Infrastructure/Persistence/`

```csharp
// AppDbContext:
//   - Tất cả DbSets
//   - SaveChangesAsync override: auto set CreatedAt/UpdatedAt
//   - Global query filter: !IsDeleted (soft delete toàn cục)

// Configurations/ (1 file per entity):
//   - UserConfiguration: unique index Phone, Email
//   - InvoiceConfiguration: BreakdownJson → JSONB
//   - ServiceConfigConfiguration: unique(PropertyId, RoomId, Type)
//   - Review: Check constraint Rating BETWEEN 1 AND 5
```

#### [NEW] Auth Services

```csharp
// TokenService:
//   GenerateAccessToken: claims=[sub, roles, jti], exp=15min
//   GenerateRefreshToken: Guid → SHA-256 hash lưu DB, plain text gửi client
//
// AuthService:
//   Register: validate phone unique, BCrypt(cost=12), tạo User + tokens
//   Login: BCrypt.Verify → generate AT+RT → lưu hashed RT
//   RefreshToken: hash input → query DB → check IsRevoked + ExpiresAt
//                 → revoke cũ + tạo cặp mới (Refresh Token Rotation)
//   Logout: revoke RT theo deviceInfo
```

#### [NEW] File Storage (Cloudinary)

```csharp
// UploadImageAsync/UploadPdfAsync: return { SecureUrl, PublicId }
// GetSignedUrl(publicId, ttl=15min): tạo signed URL
// CCCD: folder "sensitive/cccd", AccessMode=Authenticated
// KHÔNG bao giờ trả publicUrl CCCD — chỉ trả signed URL
```

#### [NEW] Background Jobs (Hangfire)

```
SmartDunningJob   — RecurringJob mỗi 3 ngày: tìm Invoice Unpaid → gửi FCM + email
ContractExpiryJob — RecurringJob hàng ngày 8:00: tìm HĐ hết hạn trong 30 ngày → notify Landlord
ListingAutoHide   — Triggered: Room → Rented → ẩn Listing
```

#### [NEW] SignalR Hubs

```csharp
// BillingHub   [Authorize]: group "landlord_{id}"/"tenant_{id}", NotifyInvoiceCreated
// OperationHub [Authorize]: NotifyTicketUpdated, NotifyAnnouncement
// AlertHub     [Authorize]: NotifySOS, NotifySecurityAlert
// SignalRService: inject IHubContext, expose NotifyUserAsync/NotifyGroupAsync
```

#### [NEW] Firebase FCM

```csharp
// FirebaseNotificationService: SendPushAsync(fcmToken, title, body, data)
// SendToTopicAsync("property_{id}", ...): push cho toàn khu trọ
```

#### [NEW] DI Registration

```csharp
// AddInfrastructure(services, config):
//   EF Core + PostgreSQL, JWT (đăng ký 1 lần duy nhất)
//   All repositories + UnitOfWork
//   All service implementations
//   Cloudinary singleton, Hangfire + PostgreSQL, Firebase init, SignalR, IHttpContextAccessor
```

---

### Component 4: API Layer

**NuGet:** `Asp.Versioning.Mvc`, `Asp.Versioning.ApiExplorer`, `Swashbuckle.AspNetCore`, `Serilog.AspNetCore`, `AspNetCoreRateLimit`

#### [NEW] API Versioning

```
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
URL: GET /api/v1/properties
```

#### [NEW] Controllers — `src/SmartStay.API/Controllers/v1/`

**AuthController** (public):
```
POST /api/v1/auth/register
POST /api/v1/auth/login
POST /api/v1/auth/refresh
POST /api/v1/auth/logout          [Authorize]
POST /api/v1/auth/change-password [Authorize]
```

**PropertyController** `[Authorize(Roles="Landlord")]`:
```
GET/POST       /api/v1/properties
GET/PUT/DELETE /api/v1/properties/{id}
GET            /api/v1/properties/{id}/dashboard
```

**RoomController** `[Authorize(Roles="Landlord")]`:
```
GET/POST            /api/v1/properties/{propertyId}/rooms          (paginated, filter status)
GET/PUT/DELETE      /api/v1/properties/{propertyId}/rooms/{id}
PATCH               /api/v1/properties/{propertyId}/rooms/{id}/status
```

**ContractController** `[Authorize]`:
```
POST  /api/v1/contracts                     [Landlord]
GET   /api/v1/contracts/{id}                [Landlord + Tenant của HĐ]
GET   /api/v1/contracts/my                  [Tenant]
PUT   /api/v1/contracts/{id}/renew          [Landlord]
PATCH /api/v1/contracts/{id}/terminate      [Landlord]
POST/GET /api/v1/contracts/{id}/inventory   [Landlord / Landlord+Tenant]
POST/GET /api/v1/contracts/{id}/roommates   [Tenant / Landlord+Tenant]
```

**BillingController** `[Authorize]`:
```
POST   /api/v1/billing/meter-readings
POST   /api/v1/billing/invoices                        [Landlord]
GET    /api/v1/billing/invoices?roomId&month&year      (paginated)
GET    /api/v1/billing/invoices/{id}
PATCH  /api/v1/billing/invoices/{id}/payment           [Landlord]
```

**TicketController** `[Authorize]`:
```
POST  /api/v1/tickets                    [Tenant]
GET   /api/v1/tickets?propertyId         [Landlord, paginated]
GET   /api/v1/tickets/my                 [Tenant, paginated]
GET   /api/v1/tickets/{id}
PATCH /api/v1/tickets/{id}/status        [Landlord]
```

**AnnouncementController** `[Authorize]`:
```
POST/GET /api/v1/properties/{propertyId}/announcements
```

**ListingController** (mixed):
```
GET/POST          /api/v1/listings         (public search paginated / [Landlord] create)
GET/PUT           /api/v1/listings/{id}    (public / [Landlord owner])
PATCH             /api/v1/listings/{id}/toggle         [Landlord]
POST/GET          /api/v1/listings/{id}/bookings       [Authorize / Landlord]
PATCH             /api/v1/bookings/{id}/status         [Landlord]
```

**ChatController, NotificationController, UploadController** `[Authorize]`:
```
Chat:         GET/POST /api/v1/chats, GET/POST /api/v1/chats/{conversationId}
Notifications: GET /api/v1/notifications, PATCH /read, /read-all
Upload:       POST /api/v1/upload/image|pdf|cccd
              (cccd response chỉ có signedUrl — không có publicUrl)
```

#### [NEW] Middleware & Filters

```csharp
// ExceptionHandlingMiddleware:
//   NotFoundException → 404, ConflictException → 409, UnauthorizedException → 401
//   ValidationException → 422 với field errors
//   Mọi Exception khác → 500, log Serilog, không expose stack trace production

// RequestLoggingMiddleware:
//   Log: Method, Path, StatusCode, Duration, UserId
//   KHÔNG log: body chứa "password"/"token"/"cccd"

// ValidationFilter: FluentValidation → 422 với [{ field, message }]
```

#### [NEW] Program.cs middleware order

```
Serilog → HTTPS → CORS → ExceptionHandler → RequestLogging
→ Authentication → Authorization → Controllers
→ MapHubs("/hubs/billing", "/hubs/operation", "/hubs/alert")
→ Hangfire Dashboard ("/hangfire", dev only)
→ Swagger (dev only)
Rate limiting: auth endpoints 5 req/min
Register recurring jobs: SmartDunning, ContractExpiry
```

---

### Component 5: Tests

```
Unit/Services/: AuthServiceTests, BillingServiceTests, TokenServiceTests
Unit/Domain/: InvoiceCalculationTests
Integration/Api/: AuthEndpointTests, PropertyEndpointTests (Testcontainers PostgreSQL)
```

---

### Component 6: Docker

**docker-compose.yml:** postgres:16 (port 5432) + seq (port 5341)
**Dockerfile:** multi-stage sdk:9.0 → aspnet:9.0

---

## Database Schema

```
users
  └── properties (landlord_id)
        └── rooms (property_id)
              ├── service_configs
              ├── meter_readings
              ├── contracts
              │     ├── inventory_items
              │     ├── roommates
              │     └── invoices
              ├── tickets
              └── listings
                    └── bookings
users ← refresh_tokens, notifications, vehicles, visitor_logs
users ← chat_messages, reviews
properties ← announcements, polls
```

---

## Security Checklist

- [x] BCrypt hash (cost=12) — không lưu plain text password
- [x] RefreshToken: SHA-256 hash lưu DB — plain text chỉ gửi client 1 lần
- [x] CCCD: Cloudinary authenticated + signed URL TTL 15 phút
- [x] Rate limiting: auth endpoints 5 req/min
- [x] CORS: whitelist cụ thể
- [x] Global query filter soft-delete
- [x] Không expose stack trace production
- [x] Pagination max 50 mọi list endpoint
- [x] API Versioning: `/api/v1/...`
- [x] RBAC: `[Authorize(Roles="...")]` theo từng endpoint

---

## Verification Plan

```bash
# Build
dotnet build SmartStay.sln

# Migrations
dotnet ef database update --project src/SmartStay.Infrastructure --startup-project src/SmartStay.API

# Tests
dotnet test tests/SmartStay.Tests --filter Category=Unit -v
dotnet test tests/SmartStay.Tests --filter Category=Integration -v
```

**Manual (Swagger):**
1. Register → nhận tokens
2. GET /properties không token → 401
3. GET /properties có token → 200
4. Refresh token cũ sau khi đã refresh → 401 (rotation hoạt động)
5. Upload CCCD → chỉ nhận signedUrl
6. SignalR connect → join group thành công
7. Tạo Invoice → Tenant nhận SignalR event < 3 giây

---

*Phase 2 (OCR, Marketplace nâng cao, Smart Dunning, Review) sẽ có plan riêng sau khi Phase 1 hoàn thành.*
