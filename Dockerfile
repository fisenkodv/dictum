FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build

WORKDIR /src

COPY ./src/Quote.Api/Quote.Api.csproj ./Quote.Api/Quote.Api.csproj
COPY ./src/Quote.Business/Quote.Business.csproj ./Quote.Business/Quote.Business.csproj
COPY ./src/Quote.Data/Quote.Data.csproj ./Quote.Data/Quote.Data.csproj

RUN dotnet restore ./Quote.Api/Quote.Api.csproj

COPY ./src/ .

FROM build AS publish
RUN dotnet publish ./Quote.Api/Quote.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENV ConnectionStrings__Quote=
ENV ASPNETCORE_ENVIRONMENT=production
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000/tcp
ENTRYPOINT ["dotnet", "Quote.Api.dll"]
