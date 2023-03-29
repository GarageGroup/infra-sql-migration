FROM mcr.microsoft.com/dotnet/runtime:7.0

WORKDIR /app
COPY ./publish ./

RUN echo '#!/bin/sh' > entrypoint.sh && \
    echo 'echo "ConnectionStrings__SqlDb: $ConnectionStrings__SqlDb"' >> entrypoint.sh && \
    echo 'echo "Migrations__ConfigPath: $Migrations__ConfigPath"' >> entrypoint.sh && \
    echo 'dotnet /app/GGroupp.Infra.Sql.Migration.Console.dll' >> entrypoint.sh && \
    chmod +x entrypoint.sh

ENTRYPOINT ["/bin/sh", "/app/entrypoint.sh"]