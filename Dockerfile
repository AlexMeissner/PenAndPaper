FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Install Node.js in the build stage
RUN apt-get update && \
    apt-get install -y curl && \
    curl -fsSL https://deb.nodesource.com/setup_current.x | bash - && \
    apt-get install -y nodejs

# Set working directory inside the container
WORKDIR /src

# Copy source files
COPY . ./

# Restore all projects
RUN dotnet restore AppHost/AppHost.csproj

# Build all projects in Release mode
RUN dotnet build AppHost/AppHost.csproj -c Release -o /app/build

# Publish the AppHost project, which includes starting the Web API and Blazor Server
RUN dotnet publish AppHost/AppHost.csproj -c Release -o /app/publish

# Use an official .NET 9 runtime image for running
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS runtime

# Set working directory inside the container
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/build ./

# Expose necessary ports for the Web API and Blazor app
EXPOSE 80

# Specify the entry point for the container
ENTRYPOINT ["dotnet", "AppHost.dll"]
