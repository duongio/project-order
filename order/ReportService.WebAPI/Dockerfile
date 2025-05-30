#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN apt-get update && apt-get install -y curl
RUN curl -L https://raw.githubusercontent.com/vishnubob/wait-for-it/master/wait-for-it.sh -o /wait-for-it.sh && chmod +x /wait-for-it.sh

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ReportService.WebAPI/ReportService.WebAPI.csproj", "ReportService.WebAPI/"]
RUN dotnet restore "ReportService.WebAPI/ReportService.WebAPI.csproj"
COPY . .
WORKDIR "/src/ReportService.WebAPI"
RUN dotnet build "ReportService.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReportService.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["/wait-for-it.sh", "rabbitmq:5672", "--", "dotnet", "ReportService.WebAPI.dll"]
