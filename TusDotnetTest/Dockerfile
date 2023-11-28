FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TusDotnetTest/TusDotnetTest.csproj", "TusDotnetTest/"]
RUN dotnet restore "TusDotnetTest/TusDotnetTest.csproj"
COPY . .
WORKDIR "/src/TusDotnetTest"
RUN dotnet build "TusDotnetTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TusDotnetTest.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TusDotnetTest.dll"]
