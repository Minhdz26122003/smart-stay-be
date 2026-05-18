# Smart Stay API Hoan Thanh

> Tong hop tu swagger.json. Moi endpoint duoi day la API da duoc implement o tang backend.

- Module: Smart Stay
- Port mac dinh: 5043
- Base URL: http://localhost:5043
- So module/controller: 15
- So endpoint da hoan thanh: 61

## Danh sach API da hoan thanh

### Announcement

| Method | Endpoint | Xac thuc |
|--------|----------|----------|
| POST | /api/v1/announcements |  |
| GET | /api/v1/announcements/property/{propertyId} |  |
| GET | /api/v1/announcements/tenant |  |
| DELETE | /api/v1/announcements/{id} |  |

### Auth

| Method | Endpoint | Xac thuc |
|--------|----------|----------|
| POST | /api/v1/auth/register |  |
| POST | /api/v1/auth/login |  |
| POST | /api/v1/auth/refresh-token |  |
| POST | /api/v1/auth/revoke-token |  |
| GET | /api/v1/auth/me |  |

### Contract

| Method | Endpoint | Xac thuc |
|--------|----------|----------|
| POST | /api/v1/contracts |  |
| GET | /api/v1/contracts/room/{roomId} |  |
| GET | /api/v1/contracts/tenant/{tenantId} |  |
| GET | /api/v1/contracts/{id} |  |
| PUT | /api/v1/contracts/{id} |  |
| DELETE | /api/v1/contracts/{id} |  |

### InventoryItem

| Method | Endpoint | Xac thuc |
|--------|----------|----------|
| POST | /api/v1/inventory-items |  |
| GET | /api/v1/inventory-items/contract/{contractId} |  |
| PUT | /api/v1/inventory-items/{id}/checkout |  |
| DELETE | /api/v1/inventory-items/{id} |  |

### Invoice

| Method | Endpoint | Xac thuc |
|--------|----------|----------|
| POST | /api/v1/invoices |  |
| GET | /api/v1/invoices/contract/{contractId} |  |
| GET | /api/v1/invoices/{id} |  |
| PUT | /api/v1/invoices/{id} |  |
| DELETE | /api/v1/invoices/{id} |  |

### Listing

| Method | Endpoint | Xac thuc |
|--------|----------|----------|
| POST | /api/v1/listings |  |
| GET | /api/v1/listings |  |
| PUT | /api/v1/listings/{id}/toggle |  |
| DELETE | /api/v1/listings/{id} |  |

### MeterReading

| Method | Endpoint | Xac thuc |
|--------|----------|----------|
| POST | /api/v1/meter-readings |  |
| GET | /api/v1/meter-readings/room/{roomId} |  |
| GET | /api/v1/meter-readings/property/{propertyId} |  |
| DELETE | /api/v1/meter-readings/{id} |  |

### Property

| Method | Endpoint | Xac thuc |
|--------|----------|----------|
| POST | /api/v1/properties |  |
| GET | /api/v1/properties |  |
| GET | /api/v1/properties/{id} |  |
| PUT | /api/v1/properties/{id} |  |
| DELETE | /api/v1/properties/{id} |  |

### Room

| Method | Endpoint | Xac thuc |
|--------|----------|----------|
| POST | /api/v1/rooms |  |
| GET | /api/v1/rooms/property/{propertyId} |  |
| GET | /api/v1/rooms/{id} |  |
| PUT | /api/v1/rooms/{id} |  |
| DELETE | /api/v1/rooms/{id} |  |

### Roommate

| Method | Endpoint | Xac thuc |
|--------|----------|----------|
| POST | /api/v1/roommates |  |
| GET | /api/v1/roommates/contract/{contractId} |  |
| PUT | /api/v1/roommates/{id}/approve |  |
| DELETE | /api/v1/roommates/{id} |  |

### ServiceConfig

| Method | Endpoint | Xac thuc |
|--------|----------|----------|
| POST | /api/v1/service-configs |  |
| GET | /api/v1/service-configs/property/{propertyId} |  |
| PUT | /api/v1/service-configs/{id} |  |
| DELETE | /api/v1/service-configs/{id} |  |

### Statistics

| Method | Endpoint | Xac thuc |
|--------|----------|----------|
| GET | /api/v1/statistics/finance-summary |  |

### Ticket

| Method | Endpoint | Xac thuc |
|--------|----------|----------|
| POST | /api/v1/tickets |  |
| GET | /api/v1/tickets/tenant |  |
| GET | /api/v1/tickets/landlord |  |
| PUT | /api/v1/tickets/{id}/status |  |

Trang thai ticket hop le:
- `Pending`
- `InProgress`
- `Resolved`
- `Cancelled`

### Vehicle

| Method | Endpoint | Xac thuc |
|--------|----------|----------|
| POST | /api/v1/vehicles |  |
| GET | /api/v1/vehicles |  |
| DELETE | /api/v1/vehicles/{id} |  |

### VisitorLog

| Method | Endpoint | Xac thuc |
|--------|----------|----------|
| POST | /api/v1/visitor-logs |  |
| GET | /api/v1/visitor-logs/tenant |  |
| GET | /api/v1/visitor-logs/landlord |  |
