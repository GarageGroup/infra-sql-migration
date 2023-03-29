#!/bin/sh

# Add these two lines to print the values of the environment variables
echo "ConnectionStrings__SqlDb: $ConnectionStrings__SqlDb"
echo "Migrations__ConfigPath: $Migrations__ConfigPath"

dotnet /app/GGroupp.Infra.Sql.Migration.Console.dll --ConnectionStrings__SqlDb="$ConnectionStrings__SqlDb" --Migrations__ConfigPath="$Migrations__ConfigPath"