FROM ubuntu:16.04

MAINTAINER Justin Dearing <zippy1981@gmail.com>

RUN apt-get update
RUN apt-get install -y apt-utils
RUN apt-get install -y net-tools vim curl apt-transport-https
RUN sh -c 'echo "deb [arch=amd64] https://apt-mo.trafficmanager.net/repos/mssql-ubuntu-xenial-release/ xenial main" > /etc/apt/sources.list.d/mssqlpreview.list'
RUN apt-key adv --keyserver apt-mo.trafficmanager.net --recv-keys 417A0893
RUN apt-get update
RUN ACCEPT_EULA=Y apt-get install -y msodbcsql mssql-tools unixodbc-dev-utf16