# Use the .NET SDK 8.0 for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY ["DealManagementSystem/DealManagementSystem.csproj", "./"]
RUN dotnet restore

# Copy the remaining application files and build
COPY . .
RUN dotnet build -c Release -o /app/build

# Use the .NET Runtime 8.0 for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "DealManagementSystem.dll"]
