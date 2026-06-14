FROM mcr.microsoft.com/dotnet/runtime:10.0

WORKDIR /app
COPY ./publish ./

ENTRYPOINT ["dotnet", "/app/GarageGroup.Infra.Sql.Migration.Console.dll"]
