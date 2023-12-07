# Use the official .NET Core SDK as a parent image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
RUN dotnet tool install --global dotnet-ef --version 7.*

# Set the working directory inside the container
WORKDIR /app

# Copy the project files
COPY *.sln .
COPY MangaUpdater.Application/*.csproj ./MangaUpdater.Application/
COPY MangaUpdater.Application.Tests/*.csproj  ./MangaUpdater.Application.Tests/
COPY MangaUpdater.Domain/*.csproj ./MangaUpdater.Domain/
COPY MangaUpdater.Infra/*.csproj ./MangaUpdater.Infra/
COPY MangaUpdater.Infra.Tests/*.csproj ./MangaUpdater.Infra.Tests/
COPY MangaUpdater.IoC/*.csproj ./MangaUpdater.IoC/
COPY MangaUpdaterAPI/*.csproj ./MangaUpdaterAPI/
COPY MangaUpdater.API.Tests/*.csproj ./MangaUpdater.API.Tests/

# Restore dependencies for the entire solution
RUN dotnet restore 


# Copy the rest of the application code
COPY . ./

# Publish the application
RUN dotnet publish -c Release -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Expose the port your application will run on
EXPOSE 80
ENTRYPOINT ["dotnet", "MangaUpdater.API.dll"]