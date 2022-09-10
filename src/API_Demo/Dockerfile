#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN sed -i 's/TLSv1.2/TLSv1.0/g' /etc/ssl/openssl.cnf

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/API_Demo/API_Demo.csproj", "src/API_Demo/"]
RUN dotnet restore "src/API_Demo/API_Demo.csproj"
COPY . .
WORKDIR "/src/src/API_Demo"
RUN dotnet build "API_Demo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API_Demo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API_Demo.dll"]