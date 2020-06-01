FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY */*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done
RUN dotnet restore
COPY . ./
WORKDIR "/src"
RUN dotnet build  -c Release -o /app


FROM build AS publish
RUN dotnet publish  -c Release -o /app

FROM base AS final 
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ExamService.dll"]