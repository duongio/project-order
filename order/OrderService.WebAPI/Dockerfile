#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN apt-get update && apt-get install -y curl
RUN curl -L https://raw.githubusercontent.com/vishnubob/wait-for-it/master/wait-for-it.sh -o /wait-for-it.sh && chmod +x /wait-for-it.sh

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["OrderService.WebAPI/OrderService.WebAPI.csproj", "OrderService.WebAPI/"]
RUN dotnet restore "OrderService.WebAPI/OrderService.WebAPI.csproj"
COPY . .
WORKDIR "/src/OrderService.WebAPI"
RUN dotnet build "OrderService.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderService.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["/wait-for-it.sh", "rabbitmq:5672", "--", "dotnet", "OrderService.WebAPI.dll"]
