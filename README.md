# School AI Backend (.NET API)

This is the main backend service for the School AI Platform, built with ASP.NET Core.

It handles:
- Student & Teacher dashboards
- Quiz assignments
- Authentication (JWT)
- Communication with AI service (FastAPI)
- Database operations (PostgreSQL)

---

## Tech Stack

- ASP.NET Core (.NET 8)
- Entity Framework Core
- PostgreSQL (Render)
- JWT Authentication
- HttpClient (AI Service Integration)

---

## Connected Services

- AI Service (FastAPI):
  - https://ai-service-pj5r.onrender.com

- Frontend (React):
  - Deployed separately (Vercel)

---

## Features

- Student Dashboard API
- Teacher Dashboard API
- Quiz Assignment System
- AI-powered insights integration
- Secure endpoints with JWT

---

## Configuration

### 1. Environment Variables

Set the following in your environment:

ConnectionStrings__Default=YOUR_POSTGRES_CONNECTION

Jwt__Issuer=SchoolPlatform

Jwt__Audience=SchoolPlatformUsers

Jwt__Key=YOUR_SECRET_KEY

---

### 2. Database

This project uses PostgreSQL.

Make sure to run migrations:
dotnet ef database update

---

## Docker

This project is containerized and deployed using Docker on Render.

---

## Deployment

- Platform: Render
- Auto-deploy on GitHub push

---

## Architecture
React Frontend
↓
.NET API (this project)
↓
FastAPI AI Service
↓
ChromaDB (Vector DB)

---

## Author

Developed as part of School AI Platform.
