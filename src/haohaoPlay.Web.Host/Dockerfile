FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["src/haohaoPlay.Web.Host/haohaoplay.Web.Host.csproj", "src/haohaoPlay.Web.Host/"]
COPY ["src/1-WebApi/Hao.WebApi/Hao.WebApi.csproj", "src/1-WebApi/Hao.WebApi/"]
COPY ["src/4-Infrastructure/Hao.Library/Hao.Library.csproj", "src/4-Infrastructure/Hao.Library/"]
COPY ["src/5-Common/Hao.Encrypt/Hao.Encrypt.csproj", "src/5-Common/Hao.Encrypt/"]
COPY ["src/2-Application/Hao.AppService/Hao.AppService.csproj", "src/2-Application/Hao.AppService/"]
COPY ["src/5-Common/Hao.Core/Hao.Core.csproj", "src/5-Common/Hao.Core/"]
COPY ["src/5-Common/Hao.Utility/Hao.Utility.csproj", "src/5-Common/Hao.Utility/"]
COPY ["src/5-Common/Hao.File/Hao.File.csproj", "src/5-Common/Hao.File/"]
COPY ["src/5-Common/Hao.AutoMapper/Hao.AutoMapper.csproj", "src/5-Common/Hao.AutoMapper/"]
COPY ["src/3-Domain/Hao.Repository/Hao.Repository.csproj", "src/3-Domain/Hao.Repository/"]
COPY ["src/3-Domain/Hao.Model/Hao.Model.csproj", "src/3-Domain/Hao.Model/"]
COPY ["src/3-Domain/Hao.Event/Hao.Event.csproj", "src/3-Domain/Hao.Event/"]
COPY ["src/4-Infrastructure/Hao.DbContext/Hao.DbContext.csproj", "src/4-Infrastructure/Hao.DbContext/"]
RUN dotnet restore "src/haohaoPlay.Web.Host/haohaoplay.Web.Host.csproj"
COPY . .
WORKDIR "/src/src/haohaoPlay.Web.Host"
RUN dotnet build "haohaoplay.Web.Host.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "haohaoplay.Web.Host.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "haohaoplay.Web.Host.dll"]