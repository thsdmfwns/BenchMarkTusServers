FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Install NativeAOT build prerequisites
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
       clang zlib1g-dev
WORKDIR /src
COPY ["TusDotnetAOTTest/TusDotnetAOTTest.csproj", "TusDotnetAOTTest/"]
RUN dotnet restore "TusDotnetAOTTest/TusDotnetAOTTest.csproj"
COPY . .
WORKDIR "/src/TusDotnetAOTTest"
RUN dotnet publish -o /app TusDotnetAOTTest.csproj

FROM mcr.microsoft.com/dotnet/runtime-deps:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["/app/TusDotnetAOTTest"]
