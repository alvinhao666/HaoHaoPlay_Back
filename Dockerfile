FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
COPY *.csproj ./app/
WORKDIR /app
RUN dotnet restore

COPY . ./
RUN dotnet publish -o out /p:PublishWithAspNetCoreTargetManifest="false"

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS runtime
ENV ASPNETCORE_URLS http://+:8000
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "HaoHaoPlay_Back.ApiHost.dll"]