FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["StudentManager.Service/StudentManager.Service.csproj", "StudentManager.Service/"]
RUN dotnet restore "StudentManager.Service/StudentManager.Service.csproj"
COPY . .
WORKDIR "/src/StudentManager.Service"
RUN dotnet build "StudentManager.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StudentManager.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudentManager.Service.dll"]
