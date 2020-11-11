FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezoness

EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /code
COPY src/*/*/*.csproj ./

# 根据项目文件名称创建项目文件夹，并移动项目文件到对应的项目目录下
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done 
COPY . .
WORKDIR "/code/src/HaoHaoPlay_Back.Host"
RUN dotnet build "HaoHaoPlay_Back.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HaoHaoPlay_Back.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HaoHaoPlay_Back.Host.dll"]