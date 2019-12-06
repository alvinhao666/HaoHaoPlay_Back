FROM mcr.microsoft.com/dotnet/core/sdk:3.1
WORKDIR /app
COPY /. /app
RUN dotnet restore
RUN dotnet publish -o /app -c Release 
EXPOSE 8000
ENTRYPOINT ["dotnet", "HaoHaoPlay.ApiHost.dll"]