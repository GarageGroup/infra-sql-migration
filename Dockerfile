FROM mcr.microsoft.com/dotnet/runtime:7.0

WORKDIR /app
COPY ./publish ./

ENTRYPOINT ["dotnet", "/app/GGroupp.Infra.Sql.Migration.Console.dll"]