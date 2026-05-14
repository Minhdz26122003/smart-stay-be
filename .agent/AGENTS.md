# Smart Stay — AI Operating Manual

> **Đây là file hiến pháp.** Mọi AI agent làm việc với dự án này PHẢI đọc và tuân thủ toàn bộ nội dung bên dưới.

---

## 1. Bản Sắc Dự Án

| Key | Value |
|-----|-------|
| **Tên** | Smart Stay (Rental Room Manager — RRM) |
| **Bản chất** | Super App quản lý nhà trọ / chung cư mini tại Việt Nam |
| **Repo hiện tại** | `smart-stay-be` — Backend API |
| **Đối tượng** | Chủ nhà (Landlord), Người thuê (Tenant), Người tìm phòng (Guest) |
| **Giai đoạn** | MVP — đang hoàn thiện nền tảng |

---

## 2. Tech Stack

```
Language        : C# / .NET 9
Architecture    : Monolith — Clean Architecture (Controller → Service → Repository)
Database        : PostgreSQL + Entity Framework Core (Npgsql, Code-first, Migrations, snake_case)
Auth            : JWT Bearer + Refresh Token Rotation
Real-time       : SignalR
Caching         : Redis
Object Storage  : MinIO (S3-compatible)
Logging         : Serilog → Seq
Tracing         : OpenTelemetry → Jaeger
Messaging       : RabbitMQ + MassTransit (planned)
Container       : Docker + Docker Compose
CI/CD           : Gitea Actions
API Docs        : Swagger / OpenAPI
API Versioning  : /api/v1/...
```

---

## 3. Ngôn Ngữ & Phong Cách Phản Hồi

### Ngôn ngữ
- **Giao tiếp với user**: Tiếng Việt (khi user nói Tiếng Việt)
- **Code, commit, biến, hàm, comment trong code**: English — luôn luôn
- **Thuật ngữ kỹ thuật**: giữ nguyên tiếng Anh, giải nghĩa tiếng Việt khi cần

### Phong cách
- Ngắn gọn, thẳng vào vấn đề — giải pháp trước, giải thích sau
- Khi có nhiều cách tiếp cận → bảng so sánh pros/cons → gợi ý cách tốt nhất
- Ưu tiên bảng, bullet points, code blocks — hạn chế đoạn văn dài
- Khi trả lời câu hỏi kiến trúc / design → vẽ diagram (ASCII hoặc Mermaid)

---

## 4. Ngữ Cảnh Bắt Buộc — Đọc Trước Khi Làm Bất Kỳ Việc Gì

Mỗi khi bắt đầu một task mới, AI **PHẢI** đọc các file sau theo thứ tự ưu tiên:

| Thứ tự | File | Mục đích |
|--------|------|----------|
| 1 | `docs/plans/master-plan.md` | Biết đang ở phase nào, phase nào xong, phase nào tiếp |
| 2 | `docs/CHANGELOG.md` | Biết thay đổi gần nhất là gì, trạng thái hiện tại |
| 3 | `docs/brief.md` | Nắm tổng quan dự án (nếu chưa rõ context) |
| 4 | `docs/BRD.md` | Yêu cầu kinh doanh chi tiết |
| 5 | `docs/PRD.md` | Yêu cầu sản phẩm & kỹ thuật chi tiết |
| 6 | `docs/plans/task.md` | Live tracker — task nào đang làm dở |
| 7 | `be_smartstay.md` | Danh sách 61 API endpoints đã implement |

> **Nguyên tắc**: Không bao giờ hỏi user "Dự án này làm gì?" hay "Đang ở giai đoạn nào?" — thông tin đã có trong docs.

---

## 5. Quy Trình Làm Việc

### 5.1 TRƯỚC khi code

1. Đọc context files (mục 4)
2. Kiểm tra code hiện tại có pattern nào liên quan → **follow pattern cũ**, không tự sáng tạo pattern mới
3. Nếu task yêu cầu thêm package/thư viện → **hỏi user trước**, không tự thêm
4. Nếu task mơ hồ về business logic → **hỏi user**, không giả định

### 5.2 TRONG khi code

- Tuân thủ naming conventions (xem mục 7)
- Mỗi thay đổi phải giải thích **tại sao**, không chỉ **cái gì**
- Không tạo God class / God method — rule of thumb: > 50 lines/method là warning
- Entity **KHÔNG BAO GIỜ** return trực tiếp từ API → luôn map sang DTO/Response
- Async/await cho **tất cả** DB operations
- Validate input ở 2 tầng: DTO validation (Controller) + Business rules (Service)

### 5.3 SAU khi code

1. **Hướng dẫn manual test** — BẮT BUỘC cho mọi endpoint mới/sửa:
   - Curl commands hoặc Swagger steps
   - Request body mẫu
   - Expected response
   - Edge cases cần test thủ công
2. Cập nhật `docs/plans/task.md` (đánh ✅ task vừa hoàn thành)
3. Kiểm tra xem có cần trigger **CHANGELOG update** không (xem mục 6)
4. Nếu task phức tạp mà user phải tự làm thủ công → **đề xuất viết script hoặc dùng browser_subagent** để AI hỗ trợ

---

## 6. CHANGELOG — Quy Tắc Cập Nhật Tự Động

File: `docs/CHANGELOG.md` — Format: [Keep a Changelog](https://keepachangelog.com/en/1.1.0/)

### 6.1 Khi nào PHẢI cập nhật CHANGELOG?

AI **PHẢI** cập nhật (hoặc đề xuất cập nhật) `docs/CHANGELOG.md` khi xảy ra **BẤT KỲ** sự kiện nào sau đây:

| Trigger | Category | Ví dụ |
|---------|----------|-------|
| Thêm endpoint API mới | `Added` | `POST /api/v1/payments` |
| Thêm entity/table mới vào DB | `Added` | Bảng `Payments`, entity `Payment` |
| Thêm feature/module mới | `Added` | Hệ thống nhắc nợ tự động |
| Sửa bug | `Fixed` | Fix tính sai hoá đơn khi thiếu MeterReading |
| Thay đổi behavior của API hiện tại | `Changed` | Đổi response format của `/invoices` |
| Cải thiện performance | `Changed` | Thêm Redis cache cho room listing |
| Refactor code (ảnh hưởng public API hoặc kiến trúc) | `Changed` | Tách AuthService → AuthService + OtpService |
| Xoá endpoint / deprecated | `Deprecated` hoặc `Removed` | Xoá `GET /api/v1/old-endpoint` |
| Sửa lỗi bảo mật | `Security` | Fix JWT không validate issuer |
| Hoàn thành 1 phase trong master-plan | `Added` | Tổng kết phase |

### 6.2 Khi nào KHÔNG cần cập nhật CHANGELOG?

- Sửa typo trong code/comment
- Thêm/sửa docs (trừ khi docs là deliverable chính)
- Refactor nội bộ không ảnh hưởng API surface
- Thay đổi dev tooling (prettier, linter config, ...)

### 6.3 Format entry

```markdown
## [Unreleased]

### Added
- Mô tả ngắn gọn bằng tiếng Việt, kèm endpoint/entity nếu có
  - Chi tiết kỹ thuật nếu cần (1-2 bullet con)
```

### 6.4 Khi release (deploy)

- Chuyển `[Unreleased]` → `[x.y.z] - YYYY-MM-DD`
- Bump version theo: **Major** (breaking change) / **Minor** (feature mới) / **Patch** (bugfix)

---

## 7. Conventions

### Naming

| Đối tượng | Convention | Ví dụ |
|-----------|-----------|-------|
| Class, Entity, DTO | PascalCase | `InvoiceService`, `CreateRoomRequest` |
| Method | PascalCase | `GetByIdAsync()` |
| Property (C#) | PascalCase | `TotalAmount` |
| Property (JSON response) | camelCase | `totalAmount` |
| Private field | _camelCase | `_authService` |
| Database table | PascalCase (plural) | `Invoices`, `MeterReadings` |
| Database column | PascalCase | `CreatedAt`, `IsActive` |
| API route | kebab-case | `/api/v1/meter-readings` |
| Constant | PascalCase or UPPER_SNAKE | `MaxRetryCount` |

### Commit Message

```
type(scope): mô tả ngắn bằng English

Types: feat, fix, refactor, docs, test, chore, perf, style
Scope: module name (auth, billing, contract, room, ...)

Ví dụ:
  feat(billing): add smart dunning background worker
  fix(auth): validate JWT issuer and audience
  refactor(contract): extract inventory checkout to separate service
  docs(plans): update master-plan after completing phase 2
```

### Branch Naming

```
feature/short-description
fix/short-description  
refactor/short-description
hotfix/short-description
```

### API Response Format

```json
// Success
{
  "data": { ... },
  "message": "Success message",
  "statusCode": 200
}

// Error
{
  "error": "ErrorCode",
  "message": "Human-readable error message", 
  "statusCode": 400
}
```

### HTTP Status Code Usage

| Code | Khi nào |
|------|---------|
| 200 | GET thành công, PUT/PATCH thành công |
| 201 | POST tạo mới thành công |
| 204 | DELETE thành công (no content) |
| 400 | Validation error, bad request |
| 401 | Chưa đăng nhập / token hết hạn |
| 403 | Đăng nhập rồi nhưng không có quyền |
| 404 | Resource không tìm thấy |
| 409 | Conflict (duplicate, ...) |
| 500 | Server error (không bao giờ cố ý trả) |

---

## 8. DO / DON'T

### ✅ DO — Luôn làm

- Kiểm tra code hiện tại trước khi viết code mới (follow existing patterns)
- Viết migration cho MỌI schema change
- Dùng async/await cho tất cả I/O operations
- Trả về HTTP status code chính xác
- Hỏi user khi không chắc về business logic
- Hướng dẫn manual test sau mỗi task
- Cập nhật CHANGELOG khi trigger matched (mục 6)
- Cập nhật task tracker khi hoàn thành task
- Đề xuất viết script tự động nếu task thủ công phức tạp

### ❌ DON'T — Không bao giờ

- Hardcode secrets, connection strings trong source code
- Return entity trực tiếp từ API (luôn map sang DTO)
- Skip error handling / swallow exceptions
- Tự thêm NuGet package mà không hỏi user
- Xoá / sửa comment, docs không liên quan đến task hiện tại
- Tạo pattern mới khi đã có pattern tương tự trong codebase
- Claim "đã hoàn thành" mà chưa verify (build, test, hoặc ít nhất là review logic)
- Bỏ qua validation — luôn validate ở cả Controller và Service layer

---

## 9. Cấu Trúc Tài Liệu

```
smart-stay-be/
├── .agent/
│   └── AGENTS.md              ← [BẠN ĐANG ĐÂY] Hiến pháp AI
├── docs/
│   ├── brief.md               ← Tổng quan dự án (ngôn ngữ tự nhiên, cho người đọc)
│   ├── BRD.md                 ← Business Requirements Document (chi tiết chức năng)
│   ├── PRD.md                 ← Product Requirements Document (yêu cầu sản phẩm & kỹ thuật)
│   ├── Base_BE.md             ← Mô tả tech stack chi tiết
│   ├── CHANGELOG.md           ← Lịch sử thay đổi (AI phải cập nhật — xem mục 6)
│   ├── additional.md          ← Đánh giá hiện trạng & ưu tiên
│   ├── requirements.txt       ← Phân tích nhu cầu user gốc (raw)
│   ├── mockdata.sql           ← Mock data cho testing
│   └── plans/
│       ├── master-plan.md     ← Tất cả phases + trạng thái (source of truth cho roadmap)
│       ├── task.md            ← Live tracker cho phase đang làm
│       └── [phase-files].md   ← Chi tiết từng phase đã/đang triển khai
├── be_smartstay.md            ← Danh sách 61 API endpoints đã implement
├── src/                       ← Source code
└── tests/                     ← Unit tests & integration tests
```

---

## 10. Quick Reference — Lệnh Hay Dùng

```bash
# Build
dotnet build

# Run locally
dotnet run --project src/SmartStay.Api

# Add migration
dotnet ef migrations add <MigrationName> --project src/SmartStay.Infrastructure --startup-project src/SmartStay.Api

# Update database
dotnet ef database update --project src/SmartStay.Infrastructure --startup-project src/SmartStay.Api

# Run tests
dotnet test

# Docker
docker compose up -d
docker compose down
```

> ⚠️ Các path trong lệnh EF có thể khác tuỳ cấu trúc thực tế — luôn kiểm tra trước khi chạy.

---

_Phiên bản: 2.0 — Cập nhật lần cuối: 2026-05-14_
