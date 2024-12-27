FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

# Install Node.js in the build stage
RUN apk add --no-cache curl nodejs npm

# Set working directory inside the container
WORKDIR /src

# Copy source files
COPY ./AspireServiceDefaults ./AspireServiceDefaults
COPY ./DataTransfer ./DataTransfer
COPY ./Website ./Website

# Restore project
RUN dotnet restore Website/Website.csproj

# Build project in Release mode
RUN dotnet build Website/Website.csproj -c Release -o /app/build

# Publish the project
RUN dotnet publish Website/Website.csproj -c Release -o /app/publish

# Use an official .NET 9 runtime image for running
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime

# Set working directory inside the container
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/publish  ./

# Expose necessary ports for the Web API and Blazor app
EXPOSE 7070

# Specify the entry point for the container
ENTRYPOINT ["dotnet", "Website.dll"]
