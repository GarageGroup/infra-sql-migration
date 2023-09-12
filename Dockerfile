FROM mcr.microsoft.com/dotnet/runtime:7.0

WORKDIR /app
COPY ./publish ./

ENTRYPOINT ["dotnet", "/app/GarageGroup.Infra.Sql.Migration.Console.dll"]
