# Landlord Dashboard & Property Management Mock Data Plan

> **For Antigravity:** REQUIRED WORKFLOW: Use `.agent/workflows/execute-plan.md` to execute this plan in single-flow mode.

**Goal:** Làm sâu và toàn diện hóa dữ liệu cho Module **Trang chủ Dashboard (Thống kê Doanh thu / Công nợ)** và **Quản lý Bất động sản** của Chủ trọ để Frontend có thể test chức năng vẽ Biểu đồ (Charts), Phân trang (Pagination), Bộ lọc (Filter) và Tìm kiếm (Search).

**Architecture:** Bổ sung vào `mockdata.sql` một `Property` cực lớn (Chung cư 15-20 phòng với đủ mọi trạng thái) và data `Invoices` kéo dài xuyên suốt 12-24 tháng qua để tạo biến động cho biểu đồ doanh thu. Tạo thêm các hợp đồng đa dạng (sắp hết hạn, đã thanh lý).

**Tech Stack:** PostgreSQL (via `mockdata.sql`).

---

### Task 1: Tạo Tòa nhà lớn (Property C) & Các phòng đa dạng

**Files:**
- Modify: `mockdata.sql` (Section 2 & 3)

**Step 1: Code implementation**
- Thêm `Property C` ("Khu Trọ Làng Đại Học" - 20 phòng) vào bảng `properties`.
- Viết script `INSERT` 15-20 rooms cho Property C chia thành các nhóm trạng thái rõ ràng (Occupied, Empty, Maintenance, sắp hết hợp đồng) phục vụ test Filter & Pagination Frontend.

**Step 2: Verification**
- Đảm bảo SQL không lỗi cú pháp.

**Step 3: Commit**
- Git commit: "chore(mock): add large property C with 20 rooms for pagination testing"

---

### Task 2: Tạo Dữ liệu Lịch sử Doanh thu (Invoices) trong 1-2 năm

**Files:**
- Modify: `mockdata.sql` (Section 8: Invoices & Section 7: Meter Readings)

**Step 1: Code implementation**
- Tạo vòng lặp hoặc chuỗi `INSERT` cho 1 năm lịch sử hóa đơn (ví dụ từ Tháng 1/2025 đến nay) với dòng tiền lên xuống (ví dụ 50 triệu - 70 triệu / tháng), để Frontend vẽ Chart Dashboard.
- Tạo một số Invoice bị trễ hạn (Late), thanh toán một nửa (Partial), hoặc nợ đọng để test list cảnh báo.

**Step 2: Verification**
- Đảm bảo SQL không lỗi cú pháp.

**Step 3: Commit**
- Git commit: "chore(mock): add 1-year historical invoices for dashboard chart testing"

---

### Task 3: Tạo Cảnh báo Hợp đồng và Thông báo Hệ thống

**Files:**
- Modify: `mockdata.sql` (Section 5: Contracts, Section 13: Announcements, Section 9: Tickets)

**Step 1: Code implementation**
- Thêm 3-5 Contracts vào trạng thái "Sắp hết hạn" (< 30 ngày) để hiển thị lên bảng cảnh báo Dashboard.
- Thêm 10+ Tickets (Sự cố) trải dài để hiện thống kê loại sự cố thường gặp.

**Step 2: Verification**
- Kiểm tra lại toàn bộ file `mockdata.sql` bằng công cụ linter/dry-run nếu có, hoặc manual check các dấu phẩy.

**Step 3: Commit**
- Git commit: "chore(mock): add dashboard warnings, expiring contracts, and massive tickets"

---
