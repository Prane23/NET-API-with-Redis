# 🐳 NET API with Redis (Dockerized)
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
---
## 🧰 Tech Stack
Component	Purpose
- .NET 10	Web API framework
- StackExchange.Redis	Redis client
- Redis	In‑memory cache and counters
- Docker Compose	Local multi‑container orchestration
- System.Text.Json	JSON serialization
---
## 🏁 Getting Started
Prerequisites
- .NET 10 SDK installed
- Docker and Docker Compose installed
- Run Locally
- Ensure Redis is available locally (Docker or native).

Update appsettings.json if needed:
```
json
{
  "Redis": {
    "ConnectionString": "localhost:6379"
  }
}
```
Start the API:
  - Open Swagger (local run): http://localhost:7176/swagger
  - Run with Docker Compose
  - Build and start containers:
  - docker compose up --build
  - API available at: http://localhost:9082/swagger
  - Redis reachable from host at localhost:6379 and from the API container at redis:6379.
---
## 🏗️ Architecture Overview
<img width="800" height="600" alt="image" src="https://github.com/user-attachments/assets/918bcd15-babf-47a1-aa2e-2c53c428e338" />

## 🧠 Caching Pattern
---
Cache‑aside flow:
- Check Redis for cacheKey.
- If present → return cached value.
- If absent → fetch from repository (mock or real), store in Redis with TTL, return result.
Example code snippet:
```
           const string cacheKey = "products:v2";
           // 1. Try Redis first
           var cachedProducts = await _cache.GetAsync<IEnumerable<Product>>(cacheKey);
           if (cachedProducts != null) 
               return Ok(cachedProducts);
           // 2. Fetch from service
           var products = _productservice.GetAll().Where(p=>p.Id>2);
           // 3. Save to Redis for 5 minutes
           await _cache.SetAsync(cacheKey, products, TimeSpan.FromSeconds(15));
           return Ok(products);
```
## 🧭 API Versioning
Approach: route or header based versioning. Example route pattern:
/api/v1/products
/api/v2/products
Use Microsoft.AspNetCore.Mvc.Versioning or manual route prefixes. Keep controllers versioned and maintain backward compatibility by introducing new controllers or endpoints under new version routes.

## 🧪 Testing the API
Open Swagger:

http://localhost:7176/swagger
You can test:

- GET all products
- GET product by ID
- POST new product
- PUT update product
- DELETE product
Cached responses via Redis
---
## 📘 Swagger Documentation
  - V1
<img width="1670" height="883" alt="image" src="https://github.com/user-attachments/assets/961b2960-0c8c-4ec3-807f-36330da1e76f" />
  - V2
<img width="1716" height="605" alt="image" src="https://github.com/user-attachments/assets/45c01ba4-6f62-4a9c-ae93-ce9e496e8a3a" />


## 🗂️ Project Structure
```
NET-API-with-Redis/
│
├── Controllers/                     # Versioned API endpoints
│   ├── ProductController.cs         # v1 endpoints (/api/v1/products)
│   └── ProductV2Controller.cs       # v2 endpoints (/api/v2/products)
│
├── Model/                           # Domain models
│   └── Product.cs
│
├── Properties/
│   └── launchSettings.json
│
├── Repositories/                    # Data access abstractions + mock data
│   ├── IProductRepository.cs
│   └── MockProductRepository.cs     # Used for local/dev/testing
│
├── Services/                        # Business logic + Redis caching
│   ├── ProductService.cs            # Product business logic
│   └── RedisCacheService.cs         # Cache-aside, JSON serialization, TTL
│
├── Dockerfile                       # Builds API container
├── docker-compose.yml               # Runs API + Redis together
│
├── Program.cs                       # DI setup, Redis connection, versioning
├── appsettings.json                 # Local config (Redis: localhost)
├── appsettings.Development.json
│
├── Middleware/                           # Redis Rate Limit Middleware
│   └── RedisRateLimitMiddleware.cs
│
├── Versioning/                           # Swagger Versioning Config
│   └── SwaggerVersioningConfig.cs
│
├── NET API with Redis.csproj
└── NET API with Redis.http          # Sample HTTP requests for testing
```
---
## 🧩 Redis Cache Preview
To help visualize how caching and rate limiting work inside the project, here’s a RedisInsight snapshot showing the keys created by the API.

This makes it easy to see:
  - Cached product lists
  - Rate‑limit counters increasing per request
  - TTL countdowns
  - JSON‑serialized values stored by RedisCacheService
## 📸 RedisInsight Screenshot
<img width="1000" height="800" alt="image" src="https://github.com/user-attachments/assets/17b300d3-96f3-467d-9257-d992fdd5fb82" />

## 🙌 Author  
**Prashant**  
.NET Core API | Redis Caching | High-Performance Architecture
