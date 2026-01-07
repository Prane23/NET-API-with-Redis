# 🚀 NET API with Redis (Dockerized)
A practical, production‑oriented example showing how to integrate Redis with a .NET 10 Web API. This repository demonstrates containerized development,
a reusable caching layer, a mock data repository for local development and testing, and a simple versioning strategy.

## 🔍 Overview
This project is a minimal but complete reference for building a high‑performance .NET API that uses Redis for caching and counters.
It includes a RedisCacheService, example endpoints that use the cache‑aside pattern, a mock repository for fast local development, 
and Docker Compose for running the API and Redis together.

## ✨ Key Features
- Cache‑aside pattern for API responses
- Singleton ConnectionMultiplexer for efficient Redis connections
- Reusable RedisCacheService with JSON serialization and TTL management
- Mock repository for deterministic local development and tests
- API versioning to support multiple API versions
- Docker Compose for consistent containerized development

## 🧰 Tech Stack

Component	Purpose
- .NET 10	Web API framework
- StackExchange.Redis	Redis client
- Redis	In‑memory cache and counters
- Docker Compose	Local multi‑container orchestration
- System.Text.Json	JSON serialization

## 🏁 Getting Started
Prerequisites
- .NET 10 SDK installed
- Docker and Docker Compose installed
- Run Locally
- Ensure Redis is available locally (Docker or native).

Update appsettings.json if needed:

json
{
  "Redis": {
    "ConnectionString": "localhost:6379"
  }
}
Start the API:
bash
dotnet run --project Api/Api.csproj
Open Swagger: http://localhost:7176/swagger
Run with Docker Compose
Build and start containers:
docker compose up --build
API available at: http://localhost:9082/swagger
Redis reachable from host at localhost:6379 and from the API container at redis:6379.
## 🏗️ Architecture Overview
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

## 🧠 Caching Pattern

Cache‑aside flow:
- Check Redis for cacheKey.
- If present → return cached value.
- If absent → fetch from repository (mock or real), store in Redis with TTL, return result.
Example code snippet:
```
  const string cacheKey = "products:v2";
  var cached = await _cache.GetAsync<IEnumerable<Product>>(cacheKey);
  if (cached != null) return Ok(cached);
  var products = _productService.GetAll().Where(p => p.Id > 2).ToList();
  await _cache.SetAsync(cacheKey, products, TimeSpan.FromMinutes(5));
  return Ok(products);
```
## 🧭 API Versioning
Approach: route or header based versioning. Example route pattern:
/api/v1/products
/api/v2/products
Use Microsoft.AspNetCore.Mvc.Versioning or manual route prefixes. Keep controllers versioned and maintain backward compatibility by introducing new controllers or endpoints under new version routes.

##🧪 Testing the API
Open Swagger:

http://localhost:7176/swagger
You can test:

- GET all products
- GET product by ID
- POST new product
- PUT update product
- DELETE product

Cached responses via Redis

## 🗂️ Project Structure
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
