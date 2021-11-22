FROM mcr.microsoft.com/mssql/server:2017-latest
ENV MSSQL_SA_PASSWORD = ''
ENV ACCEPT_EULA=Y

SHELL ["/bin/bash", "--login", "-c"]

RUN curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.35.3/install.sh | bash
RUN nvm install 10.15.3
