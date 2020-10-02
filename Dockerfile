FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build

# Install node
RUN curl -sL https://deb.nodesource.com/setup_14.x | bash
RUN apt-get update && apt-get install -y nodejs
RUN apt-get install -y postgresql-client

WORKDIR /workspace
COPY .config .config
RUN dotnet tool restore
COPY .paket .paket
COPY paket.dependencies paket.lock ./
COPY packages-local packages-local
RUN dotnet paket restore
RUN dotnet tool install dotnet-ef

FROM build as server-build
COPY src/Shared src/Shared
COPY src/Server src/Server
RUN cd src/Server && dotnet publish -c release -o ../../deploy

#FROM build as client-build
#COPY package.json package-lock.json ./
#RUN npm install
#COPY webpack.config.js ./
#COPY src/Shared src/Shared
#COPY src/Client src/Client
#RUN npm run build

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
COPY --from=server-build /workspace/deploy /app
#COPY --from=client-build /workspace/deploy /app
WORKDIR /app
EXPOSE 8085
ENTRYPOINT [ "dotnet", "Server.dll" ]

