FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Src/EventFlow/ApiEF/ApiEF.csproj", "Src/EventFlow/ApiEF/"]
RUN dotnet restore "Src/EventFlow/ApiEF/ApiEF.csproj"
COPY . .
WORKDIR "/src/Src/EventFlow/ApiEF"
RUN dotnet build "ApiEF.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ApiEF.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ApiEF.dll"]