📘 .NET 10 Web API with Redis Caching (Dockerized)
A clean, containerized .NET 10 Web API that demonstrates how to integrate Redis caching using a traditional controller‑based architecture with a mock repository.
This project is ideal for learning, demos, and portfolio use.

🚀 Features
.NET 10 Web API (controller‑based)

Mock repository for simple in‑memory data access

Redis caching layer for fast repeated reads

Swagger/OpenAPI for testing endpoints

Docker Compose to run API + Redis together

Clean, extensible project structure

🏗️ Architecture Overview
```
Code
┌────────────────────────────┐
│        .NET 10 API         │
│  - Controllers              │
│  - Mock Repository          │
│  - Redis Cache Integration  │
└──────────────┬─────────────┘
               │
               ▼
┌────────────────────────────┐
│           Redis            │
│  - In‑memory key/value     │
│  - Fast caching layer      │
└────────────────────────────┘
```
📦 Prerequisites
.NET 10 SDK

Docker Desktop

Redis CLI (optional)

🐳 Running with Docker
Start the API + Redis:

Code
docker-compose up --build
This will:

Build the .NET 10 API container

Pull and run Redis

Link both services

API URL:

Code
http://localhost:5000
Swagger UI:

Code
http://localhost:5000/swagger
Redis:

Code
localhost:6379
🧪 Testing the API
Open Swagger:

Code
http://localhost:5000/swagger
You can test:

GET all products

GET product by ID

POST new product

PUT update product

DELETE product

Cached responses via Redis

🗂️ Project Structure
```
Code
NET-API-with-Redis/
│
├── Controllers/
│   └── ProductController.cs
│
├── Repository/
│   └── MockProductRepository.cs
│
├── Services/
│   └── RedisCacheService.cs
│
├── Program.cs
├── Dockerfile
├── docker-compose.yml
└── README.md

```
