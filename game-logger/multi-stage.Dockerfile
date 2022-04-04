FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY . ./
RUN dotnet restore
RUN dotnet publish --configuration Release --output ./publish/

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

WORKDIR /app
COPY --from=build /app/publish .

CMD ["dotnet", "Logger.dll"]