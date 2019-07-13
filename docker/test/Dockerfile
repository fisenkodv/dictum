FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch

WORKDIR /src

COPY ./src/ ./

RUN dotnet test ./Quote.Tests
