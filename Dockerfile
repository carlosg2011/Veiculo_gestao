FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Veiculo_gestao/Veiculo_gestao.csproj", "Veiculo_gestao/"]
RUN dotnet restore "Veiculo_gestao/Veiculo_gestao.csproj"
COPY . .
WORKDIR "/src/Veiculo_gestao"
RUN dotnet publish "Veiculo_gestao.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Veiculo_gestao.dll"]
