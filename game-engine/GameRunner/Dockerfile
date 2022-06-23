FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

WORKDIR /app
COPY ./publish/ .

EXPOSE 5000

CMD ["dotnet", "GameRunner.dll"]