FROM mcr.microsoft.com/dotnet/core/sdk:5.0

WORKDIR /src

COPY ./src/ ./

RUN dotnet test ./Quote.Tests
