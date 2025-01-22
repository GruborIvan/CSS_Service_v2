# Use the official .NET SDK image as a build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file and restore dependencies
COPY CssService.API/CssService.API.sln ./
COPY CssService.API/CssService.API.csproj CssService.API/
COPY CssService.Domain/CssService.Domain.csproj CssService.Domain/
COPY CssService.Infrastructure/CssService.Infrastructure.csproj CssService.Infrastructure/
RUN dotnet restore

# Copy the rest of the solution files
COPY . .

# Build and publish the API project
WORKDIR /src/CssService.API
RUN dotnet publish -c Release -o /app/publish

# Use the official .NET runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Expose the port the app runs on
EXPOSE 5000

# Set the entry point for the application
ENTRYPOINT ["dotnet","CssService.API.dll"]