FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /code
COPY src/*/*/*.csproj ./

# 根据项目文件名称创建项目文件夹，并移动项目文件到对应的项目目录下
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done 
COPY . .
WORKDIR "/code/src/HaoHaoPlay_Back.ApiHost"
RUN dotnet build "HaoHaoPlay_Back.ApiHost.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HaoHaoPlay_Back.ApiHost.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HaoHaoPlay_Back.ApiHost.dll"]