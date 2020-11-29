FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
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