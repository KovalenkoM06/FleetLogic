# Âèêîðèñòîâóºìî îáðàç ç .NET SDK äëÿ çá³ðêè
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Êîï³þºìî ôàéëè ïðîºêòó
COPY . .

# Â³äíîâëþºìî çàëåæíîñò³ òà áóäóºìî
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Âèêîðèñòîâóºìî ëåãêèé îáðàç äëÿ çàïóñêó
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Íàëàøòîâóºìî ïîðò, ÿêèé âèìàãàº Render (8080)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Çàïóñêàºìî (ïåðåâ³ð, ùî ³ì'ÿ DLL ïðàâèëüíå)
ENTRYPOINT ["dotnet", "FleetLogic.dll"]
