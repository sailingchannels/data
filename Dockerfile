FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine3.12 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine3.12 AS build
WORKDIR /src
COPY . .
WORKDIR /src/Presentation.API
RUN dotnet restore "Presentation.API.csproj"
RUN dotnet build "Presentation.API.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR /src/Presentation.API
RUN dotnet publish "Presentation.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Presentation.API.dll"]