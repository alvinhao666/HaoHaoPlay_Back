#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/HaoHaoPlay_Back.ApiHost/HaoHaoPlay_Back.ApiHost.csproj", "src/HaoHaoPlay_Back.ApiHost/"]
COPY ["src/1-Api/Hao.WebApi/Hao.WebApi.csproj", "src/1-Api/Hao.WebApi/"]
COPY ["src/4-Infrastructure/Hao.Core.Extensions/Hao.Core.Extensions.csproj", "src/4-Infrastructure/Hao.Core.Extensions/"]
COPY ["src/5-Common/Hao.File/Hao.File.csproj", "src/5-Common/Hao.File/"]
COPY ["src/5-Common/Hao.Exception/Hao.Exception.csproj", "src/5-Common/Hao.Exception/"]
COPY ["src/4-Infrastructure/Hao.Core/Hao.Core.csproj", "src/4-Infrastructure/Hao.Core/"]
COPY ["src/5-Common/Hao.Utility/Hao.Utility.csproj", "src/5-Common/Hao.Utility/"]
COPY ["src/5-Common/Hao.Entity/Hao.Entity.csproj", "src/5-Common/Hao.Entity/"]
COPY ["src/5-Common/Hao.Response/Hao.Response.csproj", "src/5-Common/Hao.Response/"]
COPY ["src/5-Common/Hao.Snowflake/Hao.Snowflake.csproj", "src/5-Common/Hao.Snowflake/"]
COPY ["src/5-Common/Hao.Query/Hao.Query.csproj", "src/5-Common/Hao.Query/"]
COPY ["src/5-Common/Hao.Dependency/Hao.Dependency.csproj", "src/5-Common/Hao.Dependency/"]
COPY ["src/4-Infrastructure/Hao.Library/Hao.Library.csproj", "src/4-Infrastructure/Hao.Library/"]
COPY ["src/5-Common/Hao.Json/Hao.Json.csproj", "src/5-Common/Hao.Json/"]
COPY ["src/5-Common/Hao.Log/Hao.Log.csproj", "src/5-Common/Hao.Log/"]
COPY ["src/2-Application/Hao.AppService/Hao.AppService.csproj", "src/2-Application/Hao.AppService/"]
COPY ["src/3-Domain/Hao.EventData/Hao.EventData.csproj", "src/3-Domain/Hao.EventData/"]
COPY ["src/5-Common/Hao.Encrypt/Hao.Encrypt.csproj", "src/5-Common/Hao.Encrypt/"]
COPY ["src/3-Domain/Hao.Repository/Hao.Repository.csproj", "src/3-Domain/Hao.Repository/"]
COPY ["src/3-Domain/Hao.Model/Hao.Model.csproj", "src/3-Domain/Hao.Model/"]
COPY ["src/3-Domain/Hao.Enum/Hao.Enum.csproj", "src/3-Domain/Hao.Enum/"]
COPY ["src/3-Domain/Hao.Event/Hao.Event.csproj", "src/3-Domain/Hao.Event/"]
RUN dotnet restore "src/HaoHaoPlay_Back.ApiHost/HaoHaoPlay_Back.ApiHost.csproj"
COPY . .
WORKDIR "/src/src/HaoHaoPlay_Back.ApiHost"
RUN dotnet build "HaoHaoPlay_Back.ApiHost.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HaoHaoPlay_Back.ApiHost.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HaoHaoPlay_Back.ApiHost.dll"]