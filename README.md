# 🏨 Hotel Booking System – Backend API
*A clean, modular, production-grade booking platform built with CQRS, Clean Architecture, and .NET.*

## ⭐ Overview
The **Hotel Booking System** is a full-featured backend service for searching hotels, managing bookings, handling payments, and tracking user activity through visit logs and reviews.
It follows **Clean Architecture**, **CQRS**, and **Domain-Driven Design (DDD)** principles to ensure scalability, maintainability, and testability.

This backend can power:
- Hotel booking websites
- Mobile applications
- Internal management dashboards
- Academic and enterprise-grade systems

## 🧱 Architecture
The system follows a strict multi-layer structure:

```
/Api
/Application
/Domain
/Infrastructure
/Tests
```

### ✔ Clean Architecture Layers

| Layer | Responsibilities |
|------|------------------|
| **Domain** | Entities, relationships, business rules |
| **Application** | CQRS commands/queries, handlers, validation |
| **Infrastructure** | EF Core, Identity, email, PDF generation |
| **Api** | Controllers, JWT auth, Swagger, filters |

### ✔ CQRS + MediatR
Every operation is modeled as a **command** or **query**, ensuring:
- Zero business logic in controllers  
- Easy testability  
- Explicit, predictable system behavior  

Examples:
- `CreateBookingCommand`
- `GetHotelsQuery`
- `CreatePaymentForBookingCommand`
- `GetHotelDetailsByIdQuery`

## ✨ Key Features

### 🔍 Hotel Search Engine
- Filter by city, country, stars, amenities, hotel groups
- Multi-room **capacity matching algorithm**
- Availability checks using date ranges
- Dynamic pricing with discounts
- Sorting (price, stars, popularity)
- Returns the **best fitting room combination per hotel**

### 🏨 Hotel Details
- Room types + availability
- Reviews + ratings
- Amenities
- Pricing (original + discounted)
- Images & metadata

### 👤 Guest Management
- Linked to authenticated user
- CRUD operations
- Owner-only access enforcement

### 📅 Booking System
- Multiple rooms per booking
- Capacity validation
- Overlapping booking prevention
- Total price calculation
- Automatic confirmation code generation

### 💳 Payments
- Partial payments supported
- Overpayment prevention
- Only booking owner or manager can pay
- Sends confirmation **email**
- Generates **PDF invoice** via QuestPDF

### ⭐ Reviews
- Users can create, edit, delete reviews
- Aggregated hotel rating

### 📈 Visit Logs & Analytics
- Recently visited hotels
- Trending cities by visit count

### 🔐 Authentication & Authorization
- ASP.NET Core Identity
- JWT tokens
- Role-based access (User / Manager)
- Global exception middleware

## 🧠 Database ERD
<img width="2143" height="2990" alt="diagram-export-12-4-2025-5_49_02-PM" src="https://github.com/user-attachments/assets/d05c1f26-2859-4a4b-9b50-7a6a7440df15" />


## 🛠️ Technologies Used
- .NET 9.0 
- Entity Framework Core  
- MediatR (CQRS)  
- FluentValidation  
- ASP.NET Core Identity  
- Swagger / OpenAPI  
- QuestPDF  
- xUnit + Moq + FluentAssertions  

## 🚀 Getting Started

### 1️⃣ Clone the Repository
```bash
git clone https://github.com/yourusername/HotelBookingSystem.git
cd HotelBookingSystem
```

### 2️⃣ Apply EF Core Migrations
```bash
dotnet ef database update --project HotelBookingSystem.Infrastructure --startup-project HotelBookingSystem.Api
```

### 3️⃣ Run the API
```bash
dotnet run --project HotelBookingSystem.Api
```

Open Swagger:
```
https://localhost:7047/swagger
```

## 🧪 Unit Testing
Run:
```bash
dotnet test
```

## 📁 Folder Structure
```
├── Api
│   ├── Controllers
│   ├── Middleware
│   └── Services
│
├── Application
│   ├── Behaviors
│   ├── Common
│   │   ├── Dtos
│   │   ├── Exceptions
│   │   │   └── Handlers
│   │   ├── Interfaces
│   │   └── Models
│   ├── Features
│   │   ├── Amenities
│   │   │   ├── Commands
│   │   │   ├── Queries
│   │   │   └── Mapping
│   │   ├── Authentication
│   │   │   └── Commands
│   │   ├── Bookings
│   │   │   ├── Commands
│   │   │   └── Queries
│   │   ├── Cities
│   │   │   ├── Commands
│   │   │   ├── Queries
│   │   │   └── Mapping
│   │   ├── Guests
│   │   │   ├── Commands
│   │   │   └── Queries
│   │   ├── HotelGroups
│   │   │   ├── Commands
│   │   │   ├── Queries
│   │   │   └── Mapping
│   │   ├── HotelImages
│   │   │   ├── Commands
│   │   │   └── Queries
│   │   ├── HotelRooms
│   │   │   ├── Commands
│   │   │   └── Queries
│   │   ├── HotelRoomTypes
│   │   │   ├── Commands
│   │   │   ├── Queries
│   │   │   └── Mapping
│   │   ├── Hotels
│   │   │   ├── Commands
│   │   │   ├── Queries
│   │   │   └── Mapping
│   │   ├── PaymentMethods
│   │   │   ├── Commands
│   │   │   ├── Queries
│   │   │   └── Mapping
│   │   ├── Payments
│   │   │   ├── Commands
│   │   │   └── Queries
│   │   ├── Reviews
│   │   │   ├── Commands
│   │   │   └── Queries
│   │   └── RoomTypeImages
│   │       ├── Commands
│   │       └── Queries
│   └── Mappings
│
├── Domain
│   ├── Entities
│   │   ├── Amenities
│   │   ├── Bookings
│   │   ├── Cities
│   │   ├── Discounts
│   │   ├── Guests
│   │   ├── Hotels
│   │   ├── Payments
│   │   ├── Reviews
│   │   ├── Rooms
│   │   └── Vists
│   └── Enums
│
├── Infrastructure
│   ├── Data
│   │   ├── Configurations
│   │   └── Repositories
│   ├── Identity
│   │   ├── Configurations
│   │   ├── JwtTokens
│   │   ├── Mapping
│   │   ├── Models
│   │   ├── Seeders
│   │   └── Services
│   ├── Services
│   └── Migrations
│
└── Tests
    ├── Bookings
    ├── Payments
    └── Hotels
```

## 📘 API Documentation
Visit:
```
https://localhost:7047/swagger
