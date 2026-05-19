FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Veiculo_gestao.csproj .
RUN dotnet restore Veiculo_gestao.csproj

COPY . .
RUN dotnet publish Veiculo_gestao.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Veiculo_gestao.dll"]
