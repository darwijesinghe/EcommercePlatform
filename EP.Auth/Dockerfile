# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
EXPOSE 8080

# Copy csproj files
COPY EP.Auth/EP.Auth.csproj EP.Auth/
COPY SharedLibrary/SharedLibrary.csproj SharedLibrary/

# Restore dependencies
RUN dotnet restore EP.Auth/EP.Auth.csproj

# Copy all files
COPY . .

# Build and publish the app
WORKDIR /src/EP.Auth
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EP.Auth.dll"]