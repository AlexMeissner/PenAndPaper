FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

# Set working directory inside the container
WORKDIR /src

# Copy source files
COPY ./AspireServiceDefaults ./AspireServiceDefaults
COPY ./DataTransfer ./DataTransfer
COPY ./Server ./Server

# Restore project
RUN dotnet restore Server/Server.csproj

# Build project in Release mode
RUN dotnet build Server/Server.csproj -c Release -o /app/build

# Publish the project
RUN dotnet publish Server/Server.csproj -c Release -o /app/publish

# Use an official .NET 9 runtime image for running
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime

# Set working directory inside the container
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/publish  ./

# Expose necessary ports for the Web API and Blazor app
EXPOSE 8080

# Specify the entry point for the container
ENTRYPOINT ["dotnet", "Server.dll"]
