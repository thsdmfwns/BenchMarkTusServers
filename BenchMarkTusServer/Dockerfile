
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BenchMarkTusServer/BenchMarkTusServer.csproj", "BenchMarkTusServer/"]
RUN dotnet restore "BenchMarkTusServer/BenchMarkTusServer.csproj"
COPY . .
WORKDIR "/src/BenchMarkTusServer"
RUN dotnet build "BenchMarkTusServer.csproj" -c Release -o /src/bin
RUN dotnet publish "BenchMarkTusServer.csproj" -c Release -o /src/bin/publish

WORKDIR /src/bin/publish
ENTRYPOINT ["dotnet", "BenchMarkTusServer.dll"]

