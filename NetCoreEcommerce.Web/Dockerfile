FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
WORKDIR /src
COPY ["NetCoreEcommerce.Web.csproj", "NetCoreEcommerce/"]
RUN dotnet restore "NetCoreEcommerce.Web/NetCoreEcommerce.Web.csproj"
COPY . .
WORKDIR "/src/NetCoreEcommerce"
RUN dotnet build "NetCoreEcommerce.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetCoreEcommerce.Web.csproj" -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/core/aspnet:2.1 AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetCoreEcommerce.Web.dll"]