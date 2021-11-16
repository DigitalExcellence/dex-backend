ROM ubuntu:18.04

##############################################################################
# This forces the image to be build on every push!
##############################################################################

ARG CACHEBUST=0

##############################################################################
# Replace shell with bash so we can source files
##############################################################################

RUN rm /bin/sh && \
	ln -s /bin/bash /bin/sh && \
	mkdir -p /root/.nvm

##############################################################################
# Install dependencies from Ubuntu
##############################################################################

ENV NVM_DIR /root/.nvm
ENV NODE_VERSION 10.15.3

RUN apt-get update --fix-missing && \
	apt-get install -y curl && \
	##############################################################################
	# Install: nvm, node and npm
	# @see: http://stackoverflow.com/questions/25899912/install-nvm-in-docker
	##############################################################################
	curl -o- https://raw.githubusercontent.com/creationix/nvm/v0.34.0/install.sh | bash && \
	source $NVM_DIR/nvm.sh && \
	nvm install $NODE_VERSION && \
	nvm cache clear && \
	apt-get remove -y curl && \
	rm -rf /var/lib/apt/lists/*

ENV PATH $NVM_DIR/versions/node/v$NODE_VERSION/bin:$PATH

FROM mcr.microsoft.com/mssql/server:2017-latest
ENV MSSQL_SA_PASSWORD = ''
ENV ACCEPT_EULA=Y
