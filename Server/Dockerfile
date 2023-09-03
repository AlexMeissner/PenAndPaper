# Use the official .NET 6.0 SDK image as the build image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory
WORKDIR /app

# Copy the .csproj and restore as distinct layers
COPY ./Server/Server.csproj ./Server/
RUN dotnet restore ./Server/Server.csproj

# Copy the remaining source code
COPY ./Server ./Server/

# Build the application
RUN dotnet publish ./Server/Server.csproj -c Release -o /app/publish

# Use the official ASP.NET Core runtime image as the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the published application
COPY --from=build /app/publish .

# Expose the port that your API will run on
EXPOSE 80

# Define the entry point for your application
CMD ["./Server.dll"]