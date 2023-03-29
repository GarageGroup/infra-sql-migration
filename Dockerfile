FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

ARG GG_NUGET_SOURCE_URL
ARG GG_NUGET_SOURCE_USER
ARG GG_NUGET_SOURCE_PASSWORD

RUN dotnet nuget add source $GG_NUGET_SOURCE_URL -n gg -u $GG_NUGET_SOURCE_USER -p $GG_NUGET_SOURCE_PASSWORD --store-password-in-clear-text

WORKDIR /src
COPY . .

RUN dotnet restore Console.csproj
RUN dotnet publish Console.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/runtime:7.0

WORKDIR /app
COPY --from=build /app .

ENV ConnectionStrings__SqlDb $CONNECTION_STRING
ENV BotInfo__BuildVersion $CONFIG_PATH

ENTRYPOINT ["dotnet", "Console.dll"]