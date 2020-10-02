FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build

# Install node
RUN curl -sL https://deb.nodesource.com/setup_14.x | bash
RUN apt-get update && apt-get install -y nodejs
RUN apt-get install -y postgresql-client

ARG USER
ARG USER_ID
ARG GROUP_ID

RUN addgroup --gid $GROUP_ID $USER
RUN adduser --disabled-password --gecos '' --uid $USER_ID --gid $GROUP_ID $USER
USER $USER

WORKDIR /home/$USER
COPY .config .config
RUN dotnet tool restore
#RUN dotnet tool install dotnet-ef
COPY .paket .paket
COPY paket.dependencies paket.lock ./
COPY packages-local packages-local
RUN dotnet paket restore


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
COPY --from=server-build /home/$USER/deploy /app
#COPY --from=client-build /home/$USER/deploy /app
WORKDIR /app
EXPOSE 8085
ENTRYPOINT [ "dotnet", "Server.dll" ]
