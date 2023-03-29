FROM mcr.microsoft.com/dotnet/runtime:7.0

WORKDIR /app
COPY ./publish ./

ARG CONNECTION_STRING
ENV ConnectionStrings__SqlDb $CONNECTION_STRING

ARG CONFIG_PATH
ENV Migrations__ConfigPath $CONFIG_PATH

ENTRYPOINT ["dotnet", "/app/GGroupp.Infra.Sql.Migration.Console.dll"]