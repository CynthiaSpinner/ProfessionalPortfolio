#build stage: .NET 9 SDK + Node (for React build during publish)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
RUN apt-get update && apt-get install -y nodejs npm && rm -rf /var/lib/apt/lists/*
WORKDIR /src

#restore .NET
COPY ["Portfolio.csproj", "./"]
RUN dotnet restore

#copy everything and publish (this runs npm install + npm run build in ClientApp via your csproj)
COPY . .
RUN dotnet publish -c Release -o /app/publish

#runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

#render sets PORT at runtime; ASP.NET Core must listen on it
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
ENTRYPOINT ["/bin/sh", "-c", "export ASPNETCORE_URLS=http://0.0.0.0:${PORT:-5000} && exec dotnet Portfolio.dll"]