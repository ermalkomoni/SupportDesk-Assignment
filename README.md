# SupportDesk

## Tech Stack

Backend:
- .NET 10
- Entity Framework Core
- PostgreSQL
- MediatR
- FluentValidation
- AutoMapper

Frontend:
- Angular 20
- Reactive Forms
- RxJS
- Tailwind CSS

## How to Run Locally

### Backend

Prerequisites:
- .NET 10 SDK
- PostgreSQL

The backend uses **user secrets** for local sensitive configuration. Secrets are not committed to the repository and will be provided separately.

Set the connection string:

```bash
cd SupportDesk.API
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<provided-connection-string>"
```

Run the API:

```bash
dotnet run --project SupportDesk.API
```

The API applies migrations on startup and seeds the database with sample data.

### Frontend

Prerequisites:
- Node.js
- npm

Install dependencies and run the Angular app:

```bash
cd frontend
npm install
npm start
```

The frontend expects the backend API to run at:

```text
http://localhost:5070/api/v1
```

## Business Rules

Business rules are enforced on the backend so the API does not rely on the frontend for correctness.

The main rules are placed in the domain and application layers:
- `TicketDueDate` calculates due dates based on priority.
- `Ticket => AllowedTransitions` defines valid status transitions.
- Command handlers enforce workflow rules when creating, updating, assigning, deleting, commenting, and changing status.

A dedicated status endpoint is used because changing a ticket status is a workflow action with its own validation and side effects, such as setting resolved or closed dates.

## Design Choices

- I used Clean Architecture with CQRS & MediatR, even though this design choice can be considered as over-engineering for a project of this size, a simpler N-Tier approach would also work. I chose this structure to demonstrate how I usually think about scalable applications and how I separate responsibilities when business rules matter.

- The domain layer contains the core ticket workflow concepts, such as due date calculation and allowed status transitions. This keeps the most important rules close to the business model instead of spreading them through controllers or frontend code.

- The application layer uses commands and queries to make each use case explicit. Creating a ticket, assigning an agent, changing status, adding a comment, and deleting a ticket are different operations with different rules, so modeling them separately makes the code easier to read, test, and extend.

- Usually I try to keep the contorollers thin and that's where MediatR comes handy in our solution.

- API Versioning and CancellationToken were also added intepreting how a real product is developed properly.

## Time Spent

Development `10-14 hours` (Backend consumed most of the time, if I would have choose with layered architecture for example, I would develop it a lot faster)
