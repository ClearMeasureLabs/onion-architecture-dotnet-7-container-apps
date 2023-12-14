FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
EXPOSE 80
WORKDIR /app
COPY /built/ /app


RUN ls -lsa /app



ENTRYPOINT ["dotnet", "ProgrammingWithPalermo.ChurchBulletin.UI.Server.dll"]