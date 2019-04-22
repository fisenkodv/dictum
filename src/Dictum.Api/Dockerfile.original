FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["Dictum.Api.csproj", "Dictum.Api/"]
RUN dotnet restore "Dictum.Api/Dictum.Api.csproj"
COPY . .
WORKDIR "/src/Dictum.Api"
RUN dotnet build "Dictum.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Dictum.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Dictum.Api.dll"]