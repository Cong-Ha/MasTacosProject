FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy only the project file
COPY ["MasTacos/MasTacos.csproj", "MasTacos/"]

# Restore dependencies
WORKDIR /app/MasTacos
RUN dotnet restore

# Copy everything else and build
WORKDIR /app
COPY . .
WORKDIR /app/MasTacos
RUN dotnet publish -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MasTacos.dll"]