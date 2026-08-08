# Build frontend with Node
FROM node:20-alpine AS node_builder
WORKDIR /app/frontend

# Install dependencies (use package-lock for deterministic install)
COPY Frontend/package*.json ./
RUN npm ci --silent

# Copy source and build frontend
COPY Frontend/ ./
RUN npm run build


# Build and publish .NET backend
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files first to leverage Docker layer cache for restore
# By copying only the project files (the minimal files that define package dependencies) and then running dotnet restore,
# the restore step is cached as long as the project dependency files don’t change.
COPY Backend/Backend.csproj Backend/
COPY Frontend/Frontend.esproj Frontend/

RUN dotnet restore Backend/Backend.csproj

# Copy the rest of the sources
COPY . .

RUN dotnet publish Backend/Backend.csproj -c Release -o /app/publish


# Runtime image - only runtime and published output + built frontend
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copy published backend
COPY --from=build /app/publish .

# Copy built frontend into wwwroot so Kestrel will serve it
COPY --from=node_builder /app/frontend/dist ./wwwroot

ENV ASPNETCORE_URLS=http://+:5112
EXPOSE 5112

ENTRYPOINT ["dotnet", "Backend.dll"]
