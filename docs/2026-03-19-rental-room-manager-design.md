# Tài Liệu Thiết Kế: Rental Room Manager (RRM)

**Ngày cập nhật:** 2026-03-19
**Phiên bản:** 2.0
**Công Nghệ Cốt Lõi:** React Native, .NET 9, SignalR, Firebase FCM

## 1. Tổng Quan Hệ Thống (System Overview)

RRM là một "Siêu ứng dụng" (Super App) khép kín dành cho việc quản lý nhà trọ, chung cư mini và hệ sinh thái cho thuê. Ứng dụng kết nối **Chủ nhà (Landlord)**, **Người thuê (Tenant)** và **Người tìm phòng (Room Seeker)** trên cùng một nền tảng với cơ chế thay đổi giao diện theo phân quyền (Role-based UI). Toàn bộ nghiệp vụ từ tìm phòng, ký hợp đồng, đóng tiền, cho đến khi chuyển đi đều được số hóa hoàn toàn.

## 2. Kiến Trúc Ứng Dụng (Architecture)

### 2.1 Backend (.NET 9)
- **API Layer:** RESTful API xử lý logic kinh doanh và dữ liệu.
- **Micro-services / Module Logic:** 
  - Billing Engine (Tính toán điện/nước/phí tự động).
  - OCR Engine (Xử lý hình ảnh công tơ điện/nước thành số liệu).
- **Real-time Engine (SignalR Hubs):**
  - `BillingHub`: Đẩy thông báo có hoá đơn mới.
  - `OperationHub`: Cập nhật trạng thái sự cố sửa chữa, tin nhắn chat.
  - `AlertHub`: Báo khẩn cấp (SOS/Cháy nổ/An ninh).
- **Background Workers:** Cron jobs tự động gửi lời nhắc nợ, thông báo hợp đồng sắp hết hạn.

### 2.2 Frontend (React Native)
- **Role-based Navigation:** Chuyển đổi linh hoạt giữa `MarketplaceStack` (Trang xem phòng chung), `TenantStack` (Workspace người thuê) và `LandlordStack` (Workspace chủ nhà).
- **State Management:** Redux Toolkit / Zustand để quản lý trạng thái User, Auth, và Dữ liệu đồng bộ.
- **Push Notification:** Tích hợp Firebase Cloud Messaging (FCM) báo nợ, có khách đặt lịch xem phòng, báo động khẩn.
- **Hardware Integrations:** Sử dụng Camera cho OCR số điện/nước, chụp ảnh Hợp đồng, quét ảnh check-in thiết bị.

## 3. Các Module Cốt Lõi (Core Modules)

### 3.1 Giao Dịch & Tìm Kiếm (Marketplace - Room Seeker)
- **Listing & Discovery:** Danh sách các căn phòng trống. Tích hợp bộ lọc tìm kiếm theo giá, khu vực, diện tích, tiện ích phụ (có máy giặt chung/riêng, nuôi pet).
- **Minh Bạch Chi Phí:** Hiển thị trọn vẹn giá trị thật: Giá thuê, Giá điện, Giá nước, Phí rác, Phí vệ sinh, Đặt cọc.
- **Tương Tác Nhanh:** In-app chat với chủ nhà hoặc bấm nút "Đặt lịch hẹn xem phòng".
- **Hệ Thống Đánh Giá (Reviews):** Người thuê cũ có thể để lại nhận xét văn minh về khu trọ.

### 3.2 Kế Toán & Thanh Toán (Accounting & Billing)
- **Tự Động Hóa Số Liệu:** Ứng dụng công nghệ OCR để đọc chữ số trên đồng hồ điện/nước từ ảnh chụp, tự động tính "Số Tieu Thụ = Số Mới - Số Cũ".
- **Gói Dịch Vụ Linh Hoạt:** Động cơ tính tiền (Billing Engine) hỗ trợ tính giá điện bậc thang, phí dịch vụ thu theo "Đầu người" hoặc theo "Phòng".
- **Nhắc Nợ Thông Minh (Smart Dunning):** Tự động gửi tin nhắn/thông báo nhắc thanh toán định kỳ cho những hoá đơn "Còn nợ".
- **Quy Trình Trả Phòng (Check-out):** Tự động chốt công nợ tháng cuối, khấu trừ tình trạng thiết bị hư hỏng vào tiền cọc, tính số tiền cọc gửi lại cho người thuê.

### 3.3 Hợp Đồng & Pháp Lý (Legal & Registry)
- **Ký Kết & Lưu Trữ:** Scan/Chụp ảnh bản cứng Hợp đồng. Tạo Hợp đồng điện tử. Cảnh báo tự động trước 30 ngày hết hạn.
- **Hồ Sơ Cư Trú (Identity):** Lưu ảnh CCCD 2 mặt an toàn. Trích xuất danh sách lưu trú ra Excel/PDF chuẩn form biểu Công An để đăng ký tạm trú dễ dàng.
- **Bàn Giao Tài Sản:** Tạo phiếu bàn giao tài sản lúc dọn vào kèm bằng chứng hình ảnh (condition check).

### 3.4 Vận Hành & Hỗ Trợ (Operations & Maintenance)
- **Báo Cáo Sự Cố (Ticketing):** Người thuê tạo Ticket báo hỏng (kèm hình rõ ràng). Chủ nhà cập nhật quá trình xử lý: "Đã tiếp nhận" -> "Đang sửa" -> "Xong".
- **Bảng Tin Chung (Notice Board):** Đăng thông báo nhắc nhở dọn vệ sinh, thông báo lịch cúp điện từ điện lực.
- **Thăm Dò Ý Kiến (Polls/Survey):** Bầu chọn online để chủ nhà biết số đông người thuê có đồng ý sửa chữa, đóng thêm tiền trang bị wifi mới hay không.

### 3.5 An Ninh & Trật Tự (Security & Access Control)
- **Kiểm Soát Nhập Xuất:** Người thuê khai báo danh sách bạn bè/khách đến chơi ngủ qua đêm.
- **Quản Lý Bãi Bãi Đỗ Xe:** Đăng ký biển số xe. Tích hợp nhận diện camera ghi vết ra/vào.
- **Báo Động Khẩn Cấp (SOS):** Nút đỏ ấn 1 phát gửi thông báo báo động (kèm còi hú trên app) tới Chủ nhà và các phòng ban cạnh trong tình huống khẩn cấp.

## 4. Mô Hình Dữ Liệu Logic (Logical Data Model)

1. **User:** `ID`, `Name`, `Phone`, `Role`, `FCM_Token`, `CCC_Photos`.
2. **Property (Nhà/Khu trọ):** `ID`, `LandlordID`, `Address`, `Rules`, `Amenities`.
3. **Room (Phòng):** `ID`, `PropertyID`, `Name`, `BasePrice`, `Status` (Trống/Đang thuê/Đang sửa/Giữ chỗ).
4. **Contract:** `ID`, `RoomID`, `TenantID`, `DepositAmount`, `StartDate`, `EndDate`.
5. **Inventory (Thiết bị):** `ContractID`, `ItemName`, `Images`, `Condition` (Check-in/Check-out).
6. **MeterReading (Điện/Nước):** `RoomID`, `Type`, `OldUnit`, `NewUnit`, `Photos[OCR]`.
7. **Invoice (Hoá đơn):** `ID`, `RoomID`, `TotalAmount`, `DetailsJSON`, `PaymentStatus`.
8. **Ticket (Sự cố):** `ID`, `RoomID`, `Title`, `Status`, `Photos`.
9. **Vehicle:** `ResidentID`, `PlateNumber`, `VehicleImage`.

## 5. Kế Hoạch Triển Khai Tiếp Theo (Next Steps)
- Khởi tạo quy trình `writing-plans` để chia nhỏ task.
- Tạo Project React Native.
- Phân hoạch cấu trúc thư mục (Features, Navigation, Core Services).
