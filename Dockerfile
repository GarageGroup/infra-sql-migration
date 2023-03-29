FROM mcr.microsoft.com/dotnet/runtime:7.0

WORKDIR /app
COPY ./publish ./

ENV ConnectionStrings__SqlDb $CONNECTION_STRING
ENV Migrations__ConfigPath $CONFIG_PATH

RUN echo "ConnectionStrings__SqlDb: $ConnectionStrings__SqlDb" && \
    echo "Migrations__ConfigPath: $Migrations__ConfigPath"

ENTRYPOINT ["dotnet", "/app/GGroupp.Infra.Sql.Migration.Console.dll"]