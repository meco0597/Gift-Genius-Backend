FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:5001

EXPOSE 5001
ENV PORT 5001

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT [ "dotnet", "GiftSuggestionService.dll", "--urls=", "http://localhost:5001/" ]