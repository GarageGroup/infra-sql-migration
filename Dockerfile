FROM mcr.microsoft.com/dotnet/runtime:7.0

RUN mkdir /app
WORKDIR /app

COPY ./publish ./

ENV ConnectionStrings__SqlDb $CONNECTION_STRING
ENV BotInfo__BuildVersion $CONFIG_PATH

ENTRYPOINT ["dotnet", "GGroupp.Infra.Sql.Migration.Console.dll"]