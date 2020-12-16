# Blickkontakt

This repository contains the software for the administration of the non-profit organization [Blickkontakt Schwäbisch Gmünd](https://blickkontakt-gd.de/). The software is provided as Open Source under the MIT license to allow other cities or districts in Germany to easily adopt this concept.

## Running the Web Application

The back office application is provided as a docker composition. Checkout the repository and run the following commands on a docker-enabled host:

```bash
docker build -f Dockerfile.linux-x64 -t blickkontakt .
docker-compose up -d
```

You might want to customize the docker composition or choose another architecture as needed. The composition will launch a database server as well as the back office web application used to many customers and their announces. You can access this application via http://localhost:8081/ (username: `admin`, password: `admin`).

## Deploying to Production

To use this application in production, make the application available on a public web domain using a reverse proxy such as nginx and ensure, that the database is not exposed and that connections are encrypted using HTTPS. 

## Building and Contributing

The web applications are written as a [GenHTTP MVC app](https://genhttp.org/documentation/content/controllers). To build the project, install the .NET SDK and run the following command.

```bash
cd Blickkontakt.Office
dotnet run
```

You might want to install an IDE such as Visual Studio Community for development. Contributions are welcome!
