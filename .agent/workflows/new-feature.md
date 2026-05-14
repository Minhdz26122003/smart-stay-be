---
name: new-feature
description: End-to-end workflow to create a complete new feature module — from brainstorm through implementation to verification and documentation.
---

# /new-feature — Tạo Feature Module Mới

## Tổng Quan

Workflow khép kín cho việc tạo một module/feature hoàn chỉnh trong Smart Stay BE.
Bao gồm 6 phase: Clarify → Plan → Scaffold → Implement → Verify → Document.

---

## Phase 1: Clarify (Làm rõ yêu cầu)

**Mục tiêu**: Hiểu chính xác user muốn gì trước khi viết code.

1. Đọc context:
   - `docs/plans/master-plan.md` — feature này thuộc phase nào?
   - `docs/BRD.md` / `docs/PRD.md` — có yêu cầu chi tiết cho feature này không?

2. Hỏi user xác nhận (nếu chưa rõ):
   - Feature Name (PascalCase, singular)
   - Danh sách properties + kiểu dữ liệu
   - Role authorization (Landlord / Tenant / cả hai)
   - CRUD scope (Create, Read, Update, Delete — cái nào cần?)
   - Business rules đặc biệt (nếu có)
   - Quan hệ với entity khác (FK nào?)

3. Tóm tắt lại cho user confirm trước khi tiếp tục.

---

## Phase 2: Plan (Lên kế hoạch)

**Mục tiêu**: Liệt kê rõ những gì sẽ tạo.

1. Liệt kê tất cả files sẽ tạo mới:
   ```
   ✅ src/SmartStay.Domain/Entities/{Feature}.cs
   ✅ src/SmartStay.Application/DTOs/{Feature}/Create{Feature}Request.cs
   ✅ src/SmartStay.Application/DTOs/{Feature}/{Feature}Dto.cs
   ✅ src/SmartStay.Application/DTOs/{Feature}/Update{Feature}Request.cs  (nếu cần)
   ✅ src/SmartStay.Application/Interfaces/I{Feature}Service.cs
   ✅ src/SmartStay.Application/Validators/{Feature}/Create{Feature}RequestValidator.cs
   ✅ src/SmartStay.Infrastructure/Services/{Feature}Service.cs
   ✅ src/SmartStay.API/Controllers/{Feature}Controller.cs
   ```

2. Liệt kê files sẽ SỬA:
   ```
   ✏️ src/SmartStay.Application/Mappings/MappingProfile.cs  (thêm mapping)
   ✏️ src/SmartStay.Infrastructure/DependencyInjection.cs   (đăng ký DI)
   ✏️ src/SmartStay.Infrastructure/Persistence/AppDbContext.cs  (thêm DbSet)
   ```

3. Liệt kê API endpoints sẽ tạo:
   ```
   POST   /api/v1/{features}           — Tạo mới
   GET    /api/v1/{features}           — Lấy danh sách
   GET    /api/v1/{features}/{id}      — Lấy chi tiết
   PUT    /api/v1/{features}/{id}      — Cập nhật
   DELETE /api/v1/{features}/{id}      — Xoá
   ```

4. Cập nhật `docs/plans/task.md` với task list cho feature này.

---

## Phase 3: Scaffold (Tạo khung code)

**Mục tiêu**: Tạo tất cả files theo đúng pattern của dự án.

1. Load skill: `view_file .agent/skills/generate-full-feature/SKILL.md`
2. Thực hiện **9 bước** trong skill theo thứ tự:
   - Bước 1: Entity
   - Bước 2: DTOs
   - Bước 3: Service Interface
   - Bước 4: Service Implementation (basic CRUD, chưa cần business logic phức tạp)
   - Bước 5: Controller
   - Bước 6: AutoMapper Mapping
   - Bước 7: FluentValidation Validator
   - Bước 8: DI Registration
   - Bước 9: Migration

---

## Phase 4: Implement (Triển khai business logic)

**Mục tiêu**: Thêm business rules cụ thể vào Service layer.

1. Review service implementation — thêm:
   - Authorization checks (kiểm tra quyền sở hữu)
   - Validation rules đặc thù
   - Cross-entity queries (nếu cần)
   - Error handling cho edge cases

2. Review validator — thêm:
   - Validation rules theo business requirements
   - Custom validators nếu cần

3. Build project:
   ```bash
   dotnet build
   ```
   **PHẢI** pass không lỗi trước khi tiếp tục.

---

## Phase 5: Verify (Xác minh)

**Mục tiêu**: Đảm bảo mọi thứ hoạt động đúng.

1. **Build check**:
   ```bash
   dotnet build
   ```

2. **Liệt kê tất cả endpoints mới** — bảng: Method | Path | Role | Mô tả

3. **Hướng dẫn manual test** cho MỖI endpoint:
   - Curl command hoặc Swagger steps
   - Request body mẫu (JSON)
   - Expected response (status code + body)
   - Edge cases: thiếu field, sai role, ID không tồn tại

4. **Đề xuất test data** nếu cần thêm vào `mockdata.sql`

---

## Phase 6: Document (Cập nhật tài liệu)

**Mục tiêu**: Ghi lại những gì đã làm.

1. Cập nhật `docs/plans/task.md` — đánh ✅ tất cả tasks đã hoàn thành

2. Cập nhật `docs/CHANGELOG.md`:
   ```markdown
   ## [Unreleased]

   ### Added
   - Module {Feature}: quản lý {mô tả}
     - `POST /api/v1/{features}` — Tạo {feature}
     - `GET /api/v1/{features}` — Lấy danh sách
     - ... (liệt kê tất cả endpoints)
   ```

3. Cập nhật `be_smartstay.md` — thêm bảng endpoints mới vào danh sách API

4. Tóm tắt cho user:
   - Số files tạo mới / sửa
   - Số endpoints mới
   - Nhắc user test qua Swagger

---

## Lưu Ý Quan Trọng

- **KHÔNG skip Phase 1** — Hiểu sai yêu cầu = làm lại từ đầu
- **KHÔNG skip Phase 5** — Build phải pass, test guide phải có
- **KHÔNG skip Phase 6** — CHANGELOG và task tracker PHẢI được cập nhật
- Nếu feature phức tạp (> 5 endpoints), cân nhắc chia thành 2 lần thực hiện
