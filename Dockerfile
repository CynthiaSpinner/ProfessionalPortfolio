# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Portfolio.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish Portfolio.csproj -c Release -o /app/publish --no-restore

# Run stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish ./

# Render sets PORT at runtime; default 10000 for local runs
ENV ASPNETCORE_URLS=http://0.0.0.0:10000
EXPOSE 10000

# Use shell so PORT from Render is expanded at runtime
CMD sh -c 'export ASPNETCORE_URLS=http://0.0.0.0:${PORT:-10000} && exec dotnet Portfolio.dll'
