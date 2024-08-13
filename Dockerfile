FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS Build

# Initial configuration
WORKDIR /app
COPY src/MangaUpdater/ src/MangaUpdater/

# Restore packages && Publish the application
WORKDIR /app/src/MangaUpdater
RUN dotnet restore
RUN dotnet publish -c Release -o /app/out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Globalization configuration
RUN apk add --no-cache icu-libs
RUN apk add gcompat
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Expose the port your application will run on
EXPOSE 8080

ENTRYPOINT ["dotnet", "MangaUpdater.dll"]