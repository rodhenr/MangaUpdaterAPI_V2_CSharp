FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS Build

# Initial configuration
WORKDIR /app
COPY . .
COPY *.csproj .

# Install dotnet tools
ENV PATH $PATH:/root/.dotnet/tools
RUN dotnet tool install -g dotnet-ef  --version 7.0.10

# Create dotnet migration bundle
RUN dotnet ef migrations bundle -p MangaUpdater.Infra/MangaUpdater.Infra.Data.csproj -s MangaUpdaterAPI/MangaUpdater.API.csproj -o bundle --verbose --self-contained

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS runtime
WORKDIR /app
COPY --from=build /app/MangaUpdaterAPI/appsettings.json .
COPY --from=build /app/bundle .

# Give permission to execute
RUN chmod +x bundle

# Globalization configuration
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
RUN apk add --no-cache icu-libs
RUN apk add gcompat

CMD ["/app/bundle", "--connection", "Server=sqlserver,1433;Database=MangaUpdater;User ID=sa;Password=Aa123456;TrustServerCertificate=True"]