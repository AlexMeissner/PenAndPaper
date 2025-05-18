FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

# Set working directory inside the container
WORKDIR /src

# Copy source files
COPY ./AspireServiceDefaults ./AspireServiceDefaults
COPY ./DataTransfer ./DataTransfer
COPY ./Backend ./Backend

# Restore project
RUN dotnet restore Backend/Backend.csproj

# Build project in Release mode
RUN dotnet build Backend/Backend.csproj -c Release -o /app/build

# Publish the project
RUN dotnet publish Backend/Backend.csproj -c Release -o /app/publish

# Use an official .NET 9 runtime image for running
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime

# Set working directory inside the container
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/publish  ./

# Expose necessary ports for the Web API and Blazor app
EXPOSE 8080

# Specify the entry point for the container
ENTRYPOINT ["dotnet", "Backend.dll"]
