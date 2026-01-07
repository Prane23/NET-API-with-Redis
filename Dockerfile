# Build stage
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file and restore
COPY ["NET API with Redis.csproj", "."]
RUN dotnet restore "./NET API with Redis.csproj"

# Copy everything and build
COPY . .
WORKDIR "/src/."
RUN dotnet build "./NET API with Redis.csproj"

# Publish stage
FROM build AS publish
RUN dotnet publish "./NET API with Redis.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Expose port 9082
EXPOSE 9082
ENV ASPNETCORE_URLS=http://0.0.0.0:9082

ENTRYPOINT ["dotnet", "NET API with Redis.dll"]
