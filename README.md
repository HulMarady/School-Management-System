# School Management System

An ASP.NET Core MVC web application for managing school-related data and user accounts.

This project currently includes:
- Authentication with login/register using cookie auth
- User management (create, list, search, paginate, view, edit, delete)
- Dashboard layout and pages (UI scaffold)
- Entity Framework Core + MySQL persistence
- Tailwind CSS styling for modern UI pages

## Tech Stack

- Backend: ASP.NET Core MVC (.NET 10)
- ORM: Entity Framework Core 9
- Database: MySQL (Pomelo provider)
- Auth: Cookie Authentication
- Password hashing: BCrypt.Net
- Pagination: X.PagedList
- Frontend styling: Tailwind CSS + PostCSS

## Project Structure

- `Controllers/`: MVC controllers (`AccountController`, `UserController`, `DashboardController`)
- `Models/`: Domain models (`User`, `University`, `Department`)
- `Data/`: `ApplicationDbContext`
- `Views/`: Razor views for account, dashboard, and user pages
- `Migrations/`: EF Core migration history
- `wwwroot/`: Static assets (CSS, JS, libraries)

## Features

### Authentication
- Register new users
- Login/logout with cookie-based authentication
- Passwords are hashed with BCrypt before storing
- Protected pages require authentication (`[Authorize]`)

### User Management
- User list with search by username, email, or role
- Pagination with configurable page size
- Create user with password confirmation
- View user details
- Edit user profile data
- Delete users

### Dashboard UI
- Responsive admin-style dashboard layout
- Sidebar navigation and topbar interactions
- Placeholder cards/tables for summary and activity

## Prerequisites

- .NET SDK 10.0 (or preview matching `net10.0`)
- MySQL Server
- Node.js + npm (for Tailwind CSS build)

## Configuration

Connection string is stored in `appsettings.json`:

```json
"ConnectionStrings": {
	"DefaultConnection": "server=localhost;database=school_database;user=root;password=;"
}
```

Update it to match your local MySQL credentials.

## Getting Started

1. Clone the repository:

```bash
git clone https://github.com/HulMarady/School-Management-System.git
cd School-Management-System
```

2. Restore .NET dependencies:

```bash
dotnet restore
```

3. Install frontend dependencies:

```bash
npm install
```

4. Build Tailwind CSS:

```bash
npx tailwindcss -i ./wwwroot/css/input.css -o ./wwwroot/css/output.css --watch
```

Use `--watch` during development. For one-time build, remove `--watch`.

5. Create/update database schema:

```bash
dotnet ef database update
```

If `dotnet ef` is unavailable, install the tool first:

```bash
dotnet tool install --global dotnet-ef
```

6. Run the application:

```bash
dotnet run
```

Default route redirects to the login page.

## Development Notes

- Default route is configured to `Account/Login`.
- Auth cookie settings are configured in `Program.cs`.
- Existing migrations are committed and can be applied directly.
- `University` and `Department` models/tables exist, but CRUD UI/controller flows are not yet implemented.
- Dashboard metrics and recent activity are currently static demo data.

## Useful Commands

```bash
# Build project
dotnet build

# Run project
dotnet run

# Add a new migration
dotnet ef migrations add <MigrationName>

# Apply migrations
dotnet ef database update
```

## Roadmap Ideas

- Add role-based authorization (Admin/Teacher/Student access policies)
- Add University and Department management modules
- Replace dashboard demo data with real analytics queries
- Add API endpoints and DTO validation layer
- Add unit/integration tests
- Add CI pipeline and environment-specific deployment config

## License

No license file is currently included.
