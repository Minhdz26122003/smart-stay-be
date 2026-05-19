# Smart Stay Backend

![Smart Stay Logo](docs/assets/smart-stay-logo.png)

Chào mừng đến với kho lưu trữ Smart Stay Backend! Dự án này cung cấp nền tảng mạnh mẽ và có khả năng mở rộng cho một hệ thống quản lý cho thuê bất động sản, được thiết kế để tạo điều kiện cho các tương tác liền mạch giữa chủ trọ, người thuê và bất động sản. Được xây dựng bằng .NET, nó cung cấp một nền tảng an toàn và hiệu quả để quản lý bất động sản, phòng, hợp đồng, hóa đơn và hơn thế nữa.

## 🌟 Các Tính Năng

*   **Quản lý Người dùng & Xác thực**: Đăng ký, đăng nhập và phân quyền an toàn cho chủ trọ và người thuê bằng JWT.
*   **Quản lý Bất động sản & Phòng**: Các hoạt động CRUD để quản lý bất động sản cho thuê và các phòng riêng lẻ trong chúng.
*   **Quản lý Hợp đồng**: Tạo, cập nhật, xem và xóa hợp đồng thuê với người thuê.
*   **Hệ thống Hóa đơn & Thanh toán**: Tạo và theo dõi hóa đơn, quản lý thanh toán và xử lý chi tiết tài chính.
*   **Theo dõi Sự cố & Vấn đề**: Hệ thống cho phép người thuê báo cáo sự cố và chủ trọ theo dõi cũng như giải quyết chúng.
*   **Hệ thống Thông báo**: Tích hợp cập nhật thời gian thực cho các sự kiện quan trọng.
*   **Công việc Nền tảng**: Sử dụng Hangfire cho các tác vụ được lên lịch và các hoạt động chạy lâu dài.
*   **Seeding Dữ liệu**: Tự động seeding dữ liệu ban đầu cho phát triển và kiểm thử.

## 🛠️ Ngăn xếp Công nghệ

*   **.NET 9.0**: Khung công tác cốt lõi để xây dựng các dịch vụ backend.
*   **ASP.NET Core Web API**: Để xây dựng các API RESTful.
*   **Entity Framework Core**: ORM để tương tác với cơ sở dữ liệu.
*   **PostgreSQL**: Cơ sở dữ liệu quan hệ mạnh mẽ để lưu trữ dữ liệu.
*   **AutoMapper**: Để ánh xạ đối tượng sang đối tượng.
*   **FluentValidation**: Để xác thực đầu vào yêu cầu.
*   **MediatR**: Để triển khai mô hình CQRS.
*   **Hangfire**: Để xử lý công việc nền tảng.
*   **Docker & Docker Compose**: Để containerization và điều phối môi trường.
*   **Serilog**: Để ghi nhật ký có cấu trúc.

## 🚀 Bắt Đầu

Làm theo các hướng dẫn này để có được một bản sao của dự án chạy trên máy cục bộ của bạn cho mục đích phát triển và kiểm thử.

### Điều kiện Tiên quyết

Đảm bảo bạn đã cài đặt những điều sau:

*   [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
*   [PostgreSQL](https://www.postgresql.org/download/)
*   [Docker Desktop](https://www.docker.com/products/docker-desktop) (Tùy chọn, nhưng được khuyến nghị để thiết lập môi trường nhất quán)

### Cài đặt

1.  **Clone kho lưu trữ:**
    ```bash
    git clone https://github.com/Minhdz26122003/smart-stay-be.git
    cd smart-stay-be
    ```

2.  **Thiết lập Cơ sở dữ liệu (sử dụng Docker Compose - Được khuyến nghị)**
    Dự án này sử dụng Docker Compose để dễ dàng thiết lập PostgreSQL và PgAdmin.
    ```bash
    docker-compose up -d
    ```
    Điều này sẽ khởi động một container PostgreSQL và một container PgAdmin. Bạn có thể truy cập PgAdmin tại `http://localhost:5050`.
    *   **Chi tiết Kết nối PostgreSQL**:
        *   Host: `localhost` (hoặc `postgres` nếu kết nối từ một container Docker khác)
        *   Port: `5432`
        *   Người dùng: `admin`
        *   Mật khẩu: `admin`
        *   Cơ sở dữ liệu: `smartstaydb`

3.  **Chạy Migrations Cơ sở dữ liệu & Seed Dữ liệu**
    Điều hướng đến thư mục `src/SmartStay.API` và áp dụng các migrations:
    ```bash
    cd src/SmartStay.API
    dotnet ef database update
    ```
    Sau đó, bạn có thể seed dữ liệu ban đầu bằng cách chạy dự án API (nó bao gồm một cơ chế seeding khi khởi động).

### Chạy Ứng dụng

Sau khi thiết lập cơ sở dữ liệu, bạn có thể chạy API từ thư mục gốc của giải pháp:

1.  **Điều hướng đến thư mục gốc của dự án:**
    ```bash
    cd F:\Documents\BE\dotnet\smart-stay-be
    ```

2.  **Chạy API:**
    ```bash
    dotnet run
    ```
    Lệnh này sẽ tự động xây dựng và chạy dự án `SmartStay.API`.

    API thường sẽ chạy trên `https://localhost:7049` và `http://localhost:5049`.

### Tài liệu API (Swagger/OpenAPI)

Khi ứng dụng đang chạy, bạn có thể truy cập tài liệu API tương tác (Swagger UI) tại:

*   `https://localhost:7049/swagger`

## 📋 Cấu trúc Dự án

```
smart-stay-be/
├── src/
│   ├── SmartStay.API/              # ASP.NET Core Web API
│   ├── SmartStay.Application/      # Lớp ứng dụng (DTOs, Services, Interfaces)
│   ├── SmartStay.Domain/           # Lớp miền (Entities, Enums, Exceptions)
│   └── SmartStay.Infrastructure/   # Lớp cơ sở hạ tầng (Repositories, Migrations)
├── tests/
│   └── SmartStay.Tests/            # Các bài kiểm thử đơn vị
├── docs/                           # Tài liệu dự án
├── mockdata.sql                    # Dữ liệu seed cho cơ sở dữ liệu
├── docker-compose.yml              # Cấu hình Docker Compose
└── README.md                       # Tệp này
```

## 🔑 Các Endpoint API Chính

### Xác thực
*   `POST /api/v1/auth/register` - Đăng ký người dùng mới
*   `POST /api/v1/auth/login` - Đăng nhập người dùng

### Bất động sản
*   `GET /api/v1/properties` - Lấy danh sách bất động sản
*   `POST /api/v1/properties` - Tạo bất động sản mới
*   `GET /api/v1/properties/{id}` - Lấy chi tiết bất động sản
*   `PUT /api/v1/properties/{id}` - Cập nhật bất động sản
*   `DELETE /api/v1/properties/{id}` - Xóa bất động sản

### Phòng
*   `GET /api/v1/rooms` - Lấy danh sách phòng
*   `POST /api/v1/rooms` - Tạo phòng mới
*   `GET /api/v1/rooms/{id}` - Lấy chi tiết phòng
*   `PUT /api/v1/rooms/{id}` - Cập nhật phòng
*   `DELETE /api/v1/rooms/{id}` - Xóa phòng

### Hợp đồng
*   `GET /api/v1/contracts/landlord` - Lấy tất cả hợp đồng của chủ trọ hiện tại
*   `GET /api/v1/contracts/property/{propertyId}` - Lấy hợp đồng theo khu trọ
*   `GET /api/v1/contracts/room/{roomId}` - Lấy hợp đồng theo phòng
*   `POST /api/v1/contracts` - Tạo hợp đồng mới
*   `PUT /api/v1/contracts/{id}` - Cập nhật hợp đồng
*   `DELETE /api/v1/contracts/{id}` - Xóa hợp đồng

### Hóa đơn
*   `GET /api/v1/invoices` - Lấy danh sách hóa đơn
*   `POST /api/v1/invoices` - Tạo hóa đơn mới
*   `GET /api/v1/invoices/{id}` - Lấy chi tiết hóa đơn
*   `PUT /api/v1/invoices/{id}` - Cập nhật hóa đơn

### Sự cố
*   `GET /api/v1/tickets` - Lấy danh sách sự cố
*   `POST /api/v1/tickets` - Báo cáo sự cố mới
*   `GET /api/v1/tickets/{id}` - Lấy chi tiết sự cố
*   `PUT /api/v1/tickets/{id}` - Cập nhật sự cố

## 🤝 Đóng góp

Chúng tôi hoan nghênh các đóng góp! Vui lòng làm theo các bước sau:

1.  Fork kho lưu trữ.
2.  Tạo nhánh tính năng của bạn (`git checkout -b feature/TinhNangTuyetVoi`).
3.  Commit các thay đổi của bạn (`git commit -m 'Thêm một số TinhNangTuyetVoi'`).
4.  Push đến nhánh (`git push origin feature/TinhNangTuyetVoi`).
5.  Mở một Pull Request.

## 📝 Ghi chú Phát triển

*   Đảm bảo tuân theo các quy ước đặt tên C# và các tiêu chuẩn mã hóa.
*   Viết các bài kiểm thử đơn vị cho các tính năng mới.
*   Cập nhật tài liệu khi cần thiết.
*   Sử dụng các thông báo commit có ý nghĩa.

## 📄 Giấy phép

Được phân phối theo Giấy phép MIT. Xem `LICENSE` để biết thêm thông tin.

## 📧 Liên hệ

Nếu bạn có bất kỳ câu hỏi hoặc đề xuất, vui lòng liên hệ với chúng tôi qua:

*   Email: [minhdz26122003@gmail.com](mailto:minhdz26122003@gmail.com)
*   GitHub: [Minhdz26122003](https://github.com/Minhdz26122003)

