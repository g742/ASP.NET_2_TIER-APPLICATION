# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["TodoApp.csproj", "./"]
RUN dotnet restore "TodoApp.csproj"

# Copy the rest of the code
COPY . .

# Publish the application
RUN dotnet publish "TodoApp.csproj" -c Release -o /app/publish

# Stage 2: Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install netcat-openbsd for wait-for-it.sh
RUN apt-get update && apt-get install -y netcat-openbsd && rm -rf /var/lib/apt/lists/*

# Copy published app from build stage
COPY --from=build /app/publish .

# Copy wait-for-it script
COPY wait-for-it.sh .

# Make sure the script is executable
RUN chmod +x wait-for-it.sh

# Expose port 80 inside container
EXPOSE 80

# Wait for MySQL before running the app
ENTRYPOINT ["./wait-for-it.sh", "mysql-container", "3306", "dotnet", "TodoApp.dll"]
