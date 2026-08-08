# Build and publish .NET backend
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files first to leverage Docker layer cache for restore
# By copying only the project files (the minimal files that define package dependencies) and then running dotnet restore,
# the restore step is cached as long as the project dependency files don’t change.
# Copy only backend project and restore to leverage Docker layer cache
COPY Backend/Backend.csproj Backend/

RUN dotnet restore Backend/Backend.csproj

# Copy the rest of the sources
COPY . .

RUN dotnet publish Backend/Backend.csproj -c Release -o /app/publish


# Runtime image - only runtime and published output
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copy published backend
COPY --from=build /app/publish .

# If you previously hosted a SPA in wwwroot, copy its built files here.
# SPA removed, so no frontend copy step.

ENV ASPNETCORE_URLS=http://+:5112
EXPOSE 5112

ENTRYPOINT ["dotnet", "Backend.dll"]
