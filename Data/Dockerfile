FROM mcr.microsoft.com/mssql/server:2017-latest
ENV MSSQL_SA_PASSWORD = ''
ENV ACCEPT_EULA=Y

RUN apt-get update -yq \
    && apt-get -yq install curl gnupg ca-certificates \
    && curl -L https://deb.nodesource.com/setup_12.x | bash \
    && apt-get update -yq \
    && apt-get install -yq \
        dh-autoreconf=19 \
        ruby=1:2.5.* \
        ruby-dev=1:2.5.* \
        nodejs
