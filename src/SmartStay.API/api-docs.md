# Tài liệu API SmartStay

Đây là danh sách tổng quát các API cho Frontend.

---

## `POST` /api/v1/announcements

**Module:** Announcement

### Request Body
- Content-Type: `application/json`
- Model: `CreateAnnouncementRequest`

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `GET` /api/v1/announcements/property/{propertyId}

**Module:** Announcement

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `propertyId` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `GET` /api/v1/announcements/tenant

**Module:** Announcement

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `DELETE` /api/v1/announcements/{id}

**Module:** Announcement

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `id` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `POST` /api/v1/auth/register

**Module:** Auth

### Request Body
- Content-Type: `application/json`
- Model: `RegisterRequest`

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `POST` /api/v1/auth/login

**Module:** Auth

### Request Body
- Content-Type: `application/json`
- Model: `LoginRequest`

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `POST` /api/v1/auth/refresh-token

**Module:** Auth

### Request Body
- Content-Type: `application/json`
- Model: `RefreshTokenRequest`

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `POST` /api/v1/auth/revoke-token

**Module:** Auth

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `GET` /api/v1/auth/me

**Module:** Auth

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `POST` /api/v1/contracts

**Module:** Contract

### Request Body
- Content-Type: `application/json`
- Model: `CreateContractRequest`

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `GET` /api/v1/contracts/room/{roomId}

**Module:** Contract

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `roomId` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `GET` /api/v1/contracts/tenant/{tenantId}

**Module:** Contract

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `tenantId` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `GET` /api/v1/contracts/{id}

**Module:** Contract

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `id` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `PUT` /api/v1/contracts/{id}

**Module:** Contract

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `id` | `path` | Có | `string` |  |

### Request Body
- Content-Type: `application/json`
- Model: `UpdateContractRequest`

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `DELETE` /api/v1/contracts/{id}

**Module:** Contract

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `id` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `POST` /api/v1/inventory-items

**Module:** InventoryItem

### Request Body
- Content-Type: `application/json`
- Model: `CreateInventoryItemRequest`

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `GET` /api/v1/inventory-items/contract/{contractId}

**Module:** InventoryItem

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `contractId` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `PUT` /api/v1/inventory-items/{id}/checkout

**Module:** InventoryItem

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `id` | `path` | Có | `string` |  |

### Request Body
- Content-Type: `application/json`
- Model: `UpdateInventoryCheckoutRequest`

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `DELETE` /api/v1/inventory-items/{id}

**Module:** InventoryItem

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `id` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `POST` /api/v1/invoices

**Module:** Invoice

### Request Body
- Content-Type: `application/json`
- Model: `CreateInvoiceRequest`

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `GET` /api/v1/invoices/contract/{contractId}

**Module:** Invoice

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `contractId` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `GET` /api/v1/invoices/{id}

**Module:** Invoice

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `id` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `PUT` /api/v1/invoices/{id}

**Module:** Invoice

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `id` | `path` | Có | `string` |  |

### Request Body
- Content-Type: `application/json`
- Model: `UpdateInvoiceRequest`

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `DELETE` /api/v1/invoices/{id}

**Module:** Invoice

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `id` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `POST` /api/v1/listings

**Module:** Listing

### Request Body
- Content-Type: `application/json`
- Model: `CreateListingRequest`

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `GET` /api/v1/listings

**Module:** Listing

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `PUT` /api/v1/listings/{id}/toggle

**Module:** Listing

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `id` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `DELETE` /api/v1/listings/{id}

**Module:** Listing

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `id` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `POST` /api/v1/meter-readings

**Module:** MeterReading

### Request Body
- Content-Type: `application/json`
- Model: `CreateMeterReadingRequest`

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `GET` /api/v1/meter-readings/room/{roomId}

**Module:** MeterReading

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `roomId` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `GET` /api/v1/meter-readings/property/{propertyId}

**Module:** MeterReading

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `propertyId` | `path` | Có | `string` |  |
| `month` | `query` | Không | `integer` |  |
| `year` | `query` | Không | `integer` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `DELETE` /api/v1/meter-readings/{id}

**Module:** MeterReading

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `id` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `POST` /api/v1/properties

**Module:** Property

### Request Body
- Content-Type: `application/json`
- Model: `CreatePropertyRequest`

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `GET` /api/v1/properties

**Module:** Property

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `GET` /api/v1/properties/{id}

**Module:** Property

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `id` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `PUT` /api/v1/properties/{id}

**Module:** Property

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `id` | `path` | Có | `string` |  |

### Request Body
- Content-Type: `application/json`
- Model: `UpdatePropertyRequest`

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `DELETE` /api/v1/properties/{id}

**Module:** Property

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `id` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `POST` /api/v1/rooms

**Module:** Room

### Request Body
- Content-Type: `application/json`
- Model: `CreateRoomRequest`

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `GET` /api/v1/rooms/property/{propertyId}

**Module:** Room

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `propertyId` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `GET` /api/v1/rooms/{id}

**Module:** Room

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `id` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `PUT` /api/v1/rooms/{id}

**Module:** Room

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `id` | `path` | Có | `string` |  |

### Request Body
- Content-Type: `application/json`
- Model: `UpdateRoomRequest`

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `DELETE` /api/v1/rooms/{id}

**Module:** Room

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `id` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `POST` /api/v1/roommates

**Module:** Roommate

### Request Body
- Content-Type: `application/json`
- Model: `CreateRoommateRequest`

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `GET` /api/v1/roommates/contract/{contractId}

**Module:** Roommate

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `contractId` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `PUT` /api/v1/roommates/{id}/approve

**Module:** Roommate

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `id` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `DELETE` /api/v1/roommates/{id}

**Module:** Roommate

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `id` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `POST` /api/v1/service-configs

**Module:** ServiceConfig

### Request Body
- Content-Type: `application/json`
- Model: `CreateServiceConfigRequest`

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `GET` /api/v1/service-configs/property/{propertyId}

**Module:** ServiceConfig

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `propertyId` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `PUT` /api/v1/service-configs/{id}

**Module:** ServiceConfig

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `id` | `path` | Có | `string` |  |

### Request Body
- Content-Type: `application/json`
- Model: `UpdateServiceConfigRequest`

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `DELETE` /api/v1/service-configs/{id}

**Module:** ServiceConfig

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `id` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `GET` /api/v1/statistics/finance-summary

**Module:** Statistics

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `month` | `query` | Không | `integer` |  |
| `year` | `query` | Không | `integer` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `POST` /api/v1/tickets

**Module:** Ticket

### Request Body
- Content-Type: `application/json`
- Model: `CreateTicketRequest`

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `GET` /api/v1/tickets/tenant

**Module:** Ticket

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `GET` /api/v1/tickets/landlord

**Module:** Ticket

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `PUT` /api/v1/tickets/{id}/status

**Module:** Ticket

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `id` | `path` | Có | `string` |  |

### Request Body
- Content-Type: `application/json`
- Model: `UpdateTicketStatusRequest`

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `POST` /api/v1/vehicles

**Module:** Vehicle

### Request Body
- Content-Type: `application/json`
- Model: `CreateVehicleRequest`

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `GET` /api/v1/vehicles

**Module:** Vehicle

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `DELETE` /api/v1/vehicles/{id}

**Module:** Vehicle

### Parameters
| Tên | Vị trí | Bắt buộc | Kiểu dữ liệu | Mô tả |
| --- | --- | --- | --- | --- |
| `id` | `path` | Có | `string` |  |

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `POST` /api/v1/visitor-logs

**Module:** VisitorLog

### Request Body
- Content-Type: `application/json`
- Model: `CreateVisitorLogRequest`

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `GET` /api/v1/visitor-logs/tenant

**Module:** VisitorLog

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

## `GET` /api/v1/visitor-logs/landlord

**Module:** VisitorLog

### Responses
| Mã lỗi | Trả về (Model) |
| --- | --- |
| `200` |  |

---

# Schemas (Models)

## ContractStatus

## CreateAnnouncementRequest

| Thuộc tính | Kiểu dữ liệu |Nullable| Mô tả |
| --- | --- | --- | --- |
| `propertyId` | `string` | Không | |
| `roomId` | `string` | Có | |
| `title` | `string` | Có | |
| `content` | `string` | Có | |

## CreateContractRequest

| Thuộc tính | Kiểu dữ liệu |Nullable| Mô tả |
| --- | --- | --- | --- |
| `roomId` | `string` | Không | |
| `tenantId` | `string` | Không | |
| `depositAmount` | `number` | Không | |
| `startDate` | `string` | Không | |
| `endDate` | `string` | Không | |
| `scannedContractUrl` | `string` | Có | |

## CreateInventoryItemRequest

| Thuộc tính | Kiểu dữ liệu |Nullable| Mô tả |
| --- | --- | --- | --- |
| `contractId` | `string` | Không | |
| `itemName` | `string` | Có | |
| `checkInPhotos` | `array` | Có | |
| `condition` | `ItemCondition` | Không | |

## CreateInvoiceRequest

| Thuộc tính | Kiểu dữ liệu |Nullable| Mô tả |
| --- | --- | --- | --- |
| `roomId` | `string` | Không | |
| `contractId` | `string` | Không | |
| `month` | `integer` | Không | |
| `year` | `integer` | Không | |
| `breakdownJson` | `string` | Có | |
| `totalAmount` | `number` | Không | |

## CreateListingRequest

| Thuộc tính | Kiểu dữ liệu |Nullable| Mô tả |
| --- | --- | --- | --- |
| `roomId` | `string` | Không | |
| `photoUrls` | `array` | Có | |
| `description` | `string` | Có | |

## CreateMeterReadingRequest

| Thuộc tính | Kiểu dữ liệu |Nullable| Mô tả |
| --- | --- | --- | --- |
| `roomId` | `string` | Không | |
| `type` | `string` | Có | |
| `oldUnit` | `number` | Không | |
| `newUnit` | `number` | Không | |
| `photoUrl` | `string` | Có | |
| `month` | `integer` | Không | |
| `year` | `integer` | Không | |

## CreatePropertyRequest

| Thuộc tính | Kiểu dữ liệu |Nullable| Mô tả |
| --- | --- | --- | --- |
| `name` | `string` | Có | |
| `street` | `string` | Có | |
| `ward` | `string` | Có | |
| `district` | `string` | Có | |
| `city` | `string` | Có | |
| `rules` | `string` | Có | |
| `sharedAmenities` | `array` | Có | |

## CreateRoomRequest

| Thuộc tính | Kiểu dữ liệu |Nullable| Mô tả |
| --- | --- | --- | --- |
| `propertyId` | `string` | Không | |
| `name` | `string` | Có | |
| `type` | `string` | Có | |
| `basePrice` | `number` | Không | |
| `areaM2` | `number` | Có | |
| `maxOccupants` | `integer` | Có | |

## CreateRoommateRequest

| Thuộc tính | Kiểu dữ liệu |Nullable| Mô tả |
| --- | --- | --- | --- |
| `contractId` | `string` | Không | |
| `fullName` | `string` | Có | |
| `phone` | `string` | Có | |
| `cccdPhotoUrl` | `string` | Có | |

## CreateServiceConfigRequest

| Thuộc tính | Kiểu dữ liệu |Nullable| Mô tả |
| --- | --- | --- | --- |
| `propertyId` | `string` | Không | |
| `roomId` | `string` | Có | |
| `type` | `string` | Có | |
| `unitPrice` | `number` | Không | |
| `calcMethod` | `ServiceCalcMethod` | Không | |

## CreateTicketRequest

| Thuộc tính | Kiểu dữ liệu |Nullable| Mô tả |
| --- | --- | --- | --- |
| `roomId` | `string` | Không | |
| `category` | `TicketCategory` | Không | |
| `title` | `string` | Có | |
| `description` | `string` | Có | |
| `photoUrls` | `array` | Có | |
| `priority` | `TicketPriority` | Không | |

## CreateVehicleRequest

| Thuộc tính | Kiểu dữ liệu |Nullable| Mô tả |
| --- | --- | --- | --- |
| `plateNumber` | `string` | Có | |
| `vehicleType` | `VehicleType` | Không | |
| `photoUrl` | `string` | Có | |

## CreateVisitorLogRequest

| Thuộc tính | Kiểu dữ liệu |Nullable| Mô tả |
| --- | --- | --- | --- |
| `visitorName` | `string` | Có | |
| `phone` | `string` | Có | |
| `stayOvernight` | `boolean` | Không | |

## InvoiceStatus

## ItemCondition

## LoginRequest

| Thuộc tính | Kiểu dữ liệu |Nullable| Mô tả |
| --- | --- | --- | --- |
| `phoneOrEmail` | `string` | Có | |
| `password` | `string` | Có | |
| `deviceInfo` | `string` | Có | |

## RefreshTokenRequest

| Thuộc tính | Kiểu dữ liệu |Nullable| Mô tả |
| --- | --- | --- | --- |
| `token` | `string` | Có | |

## RegisterRequest

| Thuộc tính | Kiểu dữ liệu |Nullable| Mô tả |
| --- | --- | --- | --- |
| `fullName` | `string` | Có | |
| `phone` | `string` | Có | |
| `email` | `string` | Có | |
| `password` | `string` | Có | |
| `role` | `string` | Có | |

## ServiceCalcMethod

## TicketCategory

## TicketPriority

## TicketStatus

Giá trị hợp lệ:

- `Pending`
- `InProgress`
- `Resolved`
- `Cancelled`

Workflow cập nhật trạng thái:

- `Pending` -> `InProgress`
- `Pending` -> `Cancelled`
- `InProgress` -> `Resolved`
- `InProgress` -> `Cancelled`

Các giá trị cũ `Accepted`, `Open`, `Closed` không còn hợp lệ.

## UpdateContractRequest

| Thuộc tính | Kiểu dữ liệu |Nullable| Mô tả |
| --- | --- | --- | --- |
| `status` | `ContractStatus` | Không | |
| `scannedContractUrl` | `string` | Có | |

## UpdateInventoryCheckoutRequest

| Thuộc tính | Kiểu dữ liệu |Nullable| Mô tả |
| --- | --- | --- | --- |
| `checkOutPhotos` | `array` | Có | |
| `condition` | `ItemCondition` | Không | |

## UpdateInvoiceRequest

| Thuộc tính | Kiểu dữ liệu |Nullable| Mô tả |
| --- | --- | --- | --- |
| `paidAmount` | `number` | Không | |
| `status` | `InvoiceStatus` | Không | |

## UpdatePropertyRequest

| Thuộc tính | Kiểu dữ liệu |Nullable| Mô tả |
| --- | --- | --- | --- |
| `name` | `string` | Có | |
| `street` | `string` | Có | |
| `ward` | `string` | Có | |
| `district` | `string` | Có | |
| `city` | `string` | Có | |
| `rules` | `string` | Có | |
| `sharedAmenities` | `array` | Có | |

## UpdateRoomRequest

| Thuộc tính | Kiểu dữ liệu |Nullable| Mô tả |
| --- | --- | --- | --- |
| `name` | `string` | Có | |
| `type` | `string` | Có | |
| `basePrice` | `number` | Không | |
| `areaM2` | `number` | Có | |
| `maxOccupants` | `integer` | Có | |

## UpdateServiceConfigRequest

| Thuộc tính | Kiểu dữ liệu |Nullable| Mô tả |
| --- | --- | --- | --- |
| `unitPrice` | `number` | Không | |
| `calcMethod` | `ServiceCalcMethod` | Không | |

## UpdateTicketStatusRequest

| Thuộc tính | Kiểu dữ liệu |Nullable| Mô tả |
| --- | --- | --- | --- |
| `status` | `TicketStatus` | Không | |

## VehicleType
