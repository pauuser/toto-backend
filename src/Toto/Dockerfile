FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Toto.csproj", "Toto/"]
RUN dotnet restore "Toto/Toto.csproj"

COPY . Toto/.
RUN dotnet publish -c Release -o /app Toto/Toto.csproj 

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS publish
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Toto.dll"]