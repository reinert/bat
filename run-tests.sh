#!/bin/sh

dotnet ef --project src/Server/ database update --context SqliteContext -v
dotnet ef --project src/Server/ database update --context PgsqlContext -v
cd tests/Server
dotnet run
