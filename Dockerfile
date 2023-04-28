FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
EXPOSE 80
WORKDIR /app
COPY /built/ /app


RUN dir /a /s /w /q /app



ENTRYPOINT ["dotnet", "ProgrammingWithPalermo.ChurchBulletin.UI.Server.dll"]