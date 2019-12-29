FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY . .
WORKDIR /src/Presentation.Website
RUN dotnet restore "Presentation.Website.csproj"
RUN dotnet build "Presentation.Website.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR /src/Presentation.Website
RUN dotnet publish "Presentation.Website.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Presentation.Website.dll"]