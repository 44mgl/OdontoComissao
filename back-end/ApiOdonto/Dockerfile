FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["ApiOdonto.csproj", "./"]
RUN dotnet restore "ApiOdonto.csproj"

COPY . .
RUN dotnet publish "ApiOdonto.csproj" \
    --configuration Release \
    --output /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://0.0.0.0:10000
EXPOSE 10000

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ApiOdonto.dll"]
