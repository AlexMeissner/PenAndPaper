# BUILD AND PUBLISH PROJECT IN THE TARGET ENVIRONMENT
# Use the official .NET 6.0 SDK image as the build image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory
WORKDIR /app

# Expose the port that your API will run on as well as WebSocket port
EXPOSE 80
EXPOSE 443

# Copy project file from local folder to docker container
COPY . ./

# Restore dependencies
RUN dotnet restore ./Server/Server.csproj

# Build the application
RUN dotnet publish ./Server/Server.csproj -c Release -o /app/publish

# USE PUBLISHED PROJECT DATA TO BUILD DOCKER IMAGE INCLUDING ONLY THE REQUIRED FILES
# Use the official ASP.NET Core runtime image as the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the published application
COPY --from=build /app/publish .

# Define the entry point for your application
CMD ["./Server.dll"]
