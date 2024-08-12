FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS Build

# Initial configuration
WORKDIR /app
COPY . .
COPY *.csproj .

# Install dotnet tools
ENV PATH $PATH:/root/.dotnet/tools
RUN dotnet tool install -g dotnet-ef --version 8.0.7

# Create dotnet migration bundle
RUN dotnet ef migrations bundle -p src/MangaUpdater/MangaUpdater.csproj -o bundle --verbose --self-contained

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app
COPY --from=build /app/src/MangaUpdater/appsettings.json .
COPY --from=build /app/bundle .

# Give permission to execute
RUN chmod +x bundle

# Globalization configuration
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
RUN apk add --no-cache icu-libs
RUN apk add gcompat

CMD ["/app/bundle", "--connection", "Server=db:5432; Database=MangaUpdater; User Id=postgres; Password=1234567Aa;"]