## :ledger: Manga Updater API

Track and follow your favorite manga series effortlessly with this front-end project. This application enables you to stay updated on the latest chapter releases for all your favorite mangas. This project allow you to navigate through your manga collection with ease and never miss a new chapter.

## :man_technologist: Tecnologias

For this project, the following technologies were used:

- [.NET 8](https://learn.microsoft.com/en-us/dotnet/csharp/)
- [Entity Framework CORE](https://learn.microsoft.com/en-us/ef/core/)
- [Mediatr](https://github.com/jbogard/MediatR)
- [FluentValidation](https://fluentvalidation.net/)
- [Hangfire](https://www.hangfire.io/)
- [HtmlAgilityPack](https://html-agility-pack.net/)
- [Swagger](https://swagger.io/)
- [AutoRegisterInject](https://github.com/patrickklaeren/AutoRegisterInject)

## :dvd: How to Replicate This Project

1. To replicate this project, make sure you have Docker and .Net 8 installed.

2. Clone this repository to your local machine:

   ```bash
   git clone https://github.com/rodhenr/MangaUpdaterAPI_V2_CSharp.git
   ```

3. Open your terminal and navigate to the project folder:

   ```bash
   cd MangaUpdaterAPI_V2_CSharp
   ```

4. Use the following Docker Compose command to start the project:

   ```bash
   docker-compose -f docker-compose.yml up -d
   ```

   Wait until all the containers are running.

5. You can test the API using the base url [http://localhost:5135](http://localhost:5135).

## :desktop_computer: Getting the Frontend Project

To get the frontend project, you can access the following link: [MangaUpdaterAPI_V2_React](https://github.com/rodhenr/MangaUpdaterAPI_V2_React).
