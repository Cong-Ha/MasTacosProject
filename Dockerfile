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

# Create the directory Railway expects
RUN mkdir -p /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy published files to the location Railway is looking for
COPY --from=build /app/publish .
COPY --from=build /app/publish ./publish/

ENTRYPOINT ["dotnet", "MasTacos.dll"]