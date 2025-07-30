# HouseKeeping Web Application

A modern platform for managing, booking, and providing housekeeping and home services. Built with ASP.NET Core 8 (C#) for the backend and Vue 3 for the frontend, HouseKeeping Web Application streamlines service management for providers, administrators, and customers.

## Features

- **Service Management:** Create, update, and categorize housekeeping and home services.
- **User Authentication:** Secure login and registration with JWT-based authentication.
- **Provider Dashboard:** Manage offered services, view bookings, and handle customer feedback.
- **Admin Dashboard:** Oversee users, services, categories, and generate reports/statistics.
- **Booking System:** Customers can browse, book, and review services.
- **Comments & Ratings:** Users can leave feedback and rate services.
- **Responsive UI:** Modern, mobile-friendly interface built with Vue 3.
- **Role-Based Access:** Separate interfaces and permissions for admins, providers, and customers.

## Tech Stack

- **Backend:** ASP.NET Core 8, Entity Framework Core, MySQL, Identity, JWT
- **Frontend:** Vue 3, Pinia, Vue Router
- **API Documentation:** Swagger/OpenAPI

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js & npm](https://nodejs.org/)
- [MySQL](https://www.mysql.com/)

### Backend Setup

1. Navigate to the API directory:
   ```bash
   cd api/HousekeepingAPI
   ```
2. Configure your database connection in `appsettings.json`.
3. Run migrations and start the API:
   ```bash
   dotnet ef database update
   dotnet run
   ```

### Frontend Setup

1. Navigate to the frontend directory:
   ```bash
   cd frontend
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Start the development server:
   ```bash
   npm run dev
   ```

### API Documentation

- Visit `/swagger` on the backend server for interactive API docs.

## Folder Structure

- `api/HousekeepingAPI/` - ASP.NET Core backend
- `frontend/` - Vue 3 frontend
