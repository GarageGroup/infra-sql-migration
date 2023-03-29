FROM mcr.microsoft.com/dotnet/runtime:7.0

WORKDIR /app
COPY ./publish ./

ENV ConnectionStrings__SqlDb $INPUT_CONNECTION_STRING
ENV Migrations__ConfigPath $INPUT_CONFIG_PATH

ENTRYPOINT ["/bin/sh", "/app/GGroupp.Infra.Sql.Migration.Console.dll"]
CMD ["ConnectionString: ${INPUT_CONNECTION_STRING}, Path: ${INPUT_CONFIG_PATH}"]