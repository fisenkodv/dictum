FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine

WORKDIR /src

COPY ./src/ ./

RUN dotnet test ./Quote.Tests
