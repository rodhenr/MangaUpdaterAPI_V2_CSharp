# Use the official .NET Core SDK as a parent image
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS Build

# Initial configuration
WORKDIR /app
COPY . .
COPY *.csproj .

# Restore packages
RUN dotnet restore
COPY . .

# Publish the application
RUN dotnet publish -c Release -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Globalization configuration
RUN apk add --no-cache icu-libs
RUN apk add gcompat
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Expose the port your application will run on
EXPOSE 80

ENTRYPOINT ["dotnet", "MangaUpdater.API.dll"]

