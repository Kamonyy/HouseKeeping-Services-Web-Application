# HouseKeeping Services Web Application

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download)
[![Vue.js](https://img.shields.io/badge/Vue.js-3.0-green.svg)](https://vuejs.org/)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-orange.svg)](https://www.mysql.com/)

A full-stack web application for managing housekeeping and home services. Built with ASP.NET Core 8 and Vue 3, featuring user authentication, role-based access control, and comprehensive service management capabilities.

## Features

- **Authentication & Authorization**: JWT-based authentication with role-based access control
- **Multi-Role System**: Admin, Service Provider, and Customer interfaces
- **Service Management**: Create, update, and categorize housekeeping services
- **Admin Dashboard**: User management, service oversight, and analytics
- **Provider Portal**: Service management and customer interaction tools
- **Responsive Design**: Mobile-friendly interface with modern UI/UX

## Tech Stack

### Backend

- ASP.NET Core 8.0
- Entity Framework Core with MySQL
- JWT Authentication with Identity
- AutoMapper for object mapping
- Swagger/OpenAPI documentation

### Frontend

- Vue 3 with Composition API
- Pinia for state management
- Vue Router 4
- Axios for HTTP requests
- Tailwind CSS for styling

## Project Structure

```
HouseKeeping/
├── api/HousekeepingAPI/     # ASP.NET Core backend
│   ├── Controllers/         # API endpoints
│   ├── Models/             # Entity models
│   ├── Repository/         # Data access layer
│   └── Service/            # Business logic
├── frontend/               # Vue 3 frontend
│   └── src/
│       ├── components/     # Vue components
│       ├── views/          # Page components
│       ├── store/          # Pinia stores
│       └── services/       # API services
└── README.md
```

## Getting Started

### Prerequisites

- .NET 8 SDK
- Node.js 18+
- MySQL 8.0+

### Backend Setup

1. Navigate to the API directory:

   ```bash
   cd api/HousekeepingAPI
   ```

2. Configure database connection in `appsettings.json`:

   ```json
   {
   	"ConnectionStrings": {
   		"DefaultConnection": "Server=localhost;Database=HouseKeepingDB;Uid=root;Pwd=your_password;"
   	}
   }
   ```

3. Run migrations and start the server:
   ```bash
   dotnet ef database update
   dotnet run
   ```

### Frontend Setup

1. Navigate to the frontend directory:

   ```bash
   cd frontend
   ```

2. Install dependencies and start development server:
   ```bash
   npm install
   npm run dev
   ```

### API Documentation

- Swagger UI available at `/swagger` on the backend server

## Development

### Database Migrations

```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Production Build

```bash
# Backend
dotnet publish -c Release

# Frontend
npm run build
```

## Key Technical Achievements

- RESTful API design with proper HTTP status codes
- Secure authentication and authorization implementation
- Database design with proper relationships and constraints
- Component-based architecture with reusable Vue components
- Responsive design with mobile-first approach
- Error handling and user feedback systems
- Clean code architecture following best practices

---

_Built with ASP.NET Core 8 and Vue 3_
