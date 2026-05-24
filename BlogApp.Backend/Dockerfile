# Use lightweight alpine image for build and publish
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine@sha256:0191ff386e93923edf795d363ea0ae0669ce467ada4010b370644b670fa495c1 AS build
WORKDIR /app

# Copy project metadata first to speed up restore
# (restore won't need to redownload nuget packages if only source code changes)
COPY BlogApp.Backend.slnx ./
COPY src/BlogApp.csproj src/
RUN dotnet restore src/BlogApp.csproj

# Copy the rest of the source code, don't include tests in final image
COPY src/ src/
RUN dotnet publish src/BlogApp.csproj --no-restore --output /app/publish

# Use lightweight alpine image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine@sha256:60eb031b554df75a4b9f358290a2fa15d8961a3bc79b47bb34a00e31f7b78c69
WORKDIR /app
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BlogApp.dll"]
