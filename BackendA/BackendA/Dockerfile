FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY BackendA.csproj .
RUN dotnet restore BackendA.csproj

COPY . .
RUN dotnet publish BackendA.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BackendA.dll"]