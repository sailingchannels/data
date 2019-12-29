FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim
WORKDIR /app
EXPOSE 80
COPY . .
ENTRYPOINT ["dotnet", "Infrastructure.WebsiteSPA.dll"]