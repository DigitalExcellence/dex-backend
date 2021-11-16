FROM mcr.microsoft.com/mssql/server:2017-latest
ENV MSSQL_SA_PASSWORD = ''
ENV ACCEPT_EULA=Y

# Installing Node
SHELL ["/bin/bash", "--login", "-i", "-c"]
RUN curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.35.2/install.sh | bash
RUN source /root/.bashrc && nvm install 12.14.1
SHELL ["/bin/bash", "--login", "-c"]
