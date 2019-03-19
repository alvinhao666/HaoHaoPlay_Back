FROM microsoft/dotnet:2.0-sdk-jessie

ADD ./src /usr/local/src
WORKDIR /usr/local/src/Sino.ElectronicContractSystem.WebHost/

RUN cd /usr/local/src/
RUN dotnet restore -s https://api.nuget.org/v3/index.json -s http://nuget.sowl.cn
RUN dotnet publish -c Release -o /usr/publish
RUN rm -rf /usr/local/src/*

ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezoness

EXPOSE 5000

WORKDIR /usr/publish

CMD ["dotnet","Sino.ElectronicContractSystem.WebHost.dll"]