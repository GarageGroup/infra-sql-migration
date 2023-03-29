FROM mcr.microsoft.com/dotnet/runtime:7.0

WORKDIR /app

COPY ./publish ./

ENV ConnectionStrings__SqlDb $CONNECTION_STRING
ENV Migrations__ConfigPath $CONFIG_PATH

ENTRYPOINT ["dotnet", "/app/GGroupp.Infra.Sql.Migration.Console.dll"]