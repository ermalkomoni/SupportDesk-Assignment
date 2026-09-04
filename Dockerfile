FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SupportDesk.API/SupportDesk.API.csproj SupportDesk.API/
COPY SupportDesk.Application/SupportDesk.Application.csproj SupportDesk.Application/
COPY SupportDesk.Core/SupportDesk.Core.csproj SupportDesk.Core/
COPY SupportDesk.Infrastructure/SupportDesk.Infrastructure.csproj SupportDesk.Infrastructure/

RUN dotnet restore SupportDesk.API/SupportDesk.API.csproj

COPY . .
RUN dotnet publish SupportDesk.API/SupportDesk.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:10000
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 10000

ENTRYPOINT ["dotnet", "SupportDesk.API.dll"]
