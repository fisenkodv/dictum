FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build

WORKDIR /src

COPY ./src/Dictum.Api/Dictum.Api.csproj ./Dictum.Api/Dictum.Api.csproj
COPY ./src/Dictum.Business/Dictum.Business.csproj ./Dictum.Business/Dictum.Business.csproj
COPY ./src/Dictum.Data/Dictum.Data.csproj ./Dictum.Data/Dictum.Data.csproj

RUN dotnet restore ./Dictum.Api/Dictum.Api.csproj

COPY ./src/ .
#RUN dotnet build ./Dictum.Api/Dictum.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ./Dictum.Api/Dictum.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENV ConnectionStrings__Dictum=
ENV ASPNETCORE_ENVIRONMENT=production
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000/tcp
ENTRYPOINT ["dotnet", "Dictum.Api.dll"]
