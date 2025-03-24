# ActielijstApi

## Beschrijving
ActielijstApi is een ASP.NET Core API voor het beheren van memo's, inspecties en het genereren van Word-documenten op basis van sjablonen. Het project ondersteunt het bijwerken van custom properties in Word-documenten en zorgt ervoor dat velden automatisch worden bijgewerkt bij het openen.

## Installatie
Volg deze stappen om de API lokaal te draaien:

1. **Clone de repository**
   git clone <repository-url>

Vervang `<repository-url>` door de URL van je repository.

2. **Stel de databaseverbinding in**:
- Open het bestand `appsettings.json` in de hoofdmap van het project.
- Pas de connection string aan naar jouw SQL Server-instantie:
  ```json
  "ConnectionStrings": {
      "DefaultConnection": "Server=jouw-server;Database=ActielijstDb;Trusted_Connection=True;"
  }
  ```
- Zorg ervoor dat de SQL Server-instantie toegankelijk is en dat de database `ActielijstDb` bestaat of kan worden aangemaakt.

3. **Voer de migraties uit**:
- Gebruik Entity Framework Core om de database te maken of bij te werken:
  ```
  dotnet ef migrations add InitialCreate
  dotnet ef database update
  ```
- Zorg ervoor dat de EF Core-tools zijn geïnstalleerd (`dotnet tool install --global dotnet-ef`).

4. **Start de applicatie**:
- Navigeer naar de projectmap en start de API:
  ```
  dotnet run
  ```
- De API is nu beschikbaar op `http://localhost:5000` (of een andere poort, afhankelijk van je configuratie).

## Gebruik
De API biedt verschillende endpoints voor het beheren van memo's, inspecties en documentgeneratie. Hier zijn enkele belangrijke endpoints:

- **Login**:
  - **Endpoint**: `POST /api/login`
  - **Beschrijving**: Authenticeert een gebruiker op basis van voornaam en loginnaam.
  - **Body**:
 ```json
 {
     "Voornaam": "Jan",
     "FldLoginNaam": "jan.login"
 }
 ```
  - **Response** (succes): `{ "WerknId": 1, "Voornaam": "Jan", "Initialen": "J.D." }`

- **Werknemers ophalen**:
  - **Endpoint**: `GET /api/werknemers`
  - **Beschrijving**: Haalt een lijst van alle werknemers op.
  - **Response** (succes): Een lijst van werknemers in JSON-formaat.

- **Memo's ophalen**:
  - **Endpoint**: `GET /api/memos/user/{userId}/{filterType}`
  - **Beschrijving**: Haalt memo's op voor een specifieke gebruiker.
  - **Parameters**:
 - `userId`: De ID van de gebruiker (integer).
 - `filterType`: `"assigned"` (toegewezen memo's), `"created"` (aangemaakte memo's), of een andere waarde (alle memo's).
  - **Response** (succes): Een lijst van memo's in JSON-formaat.

- **Document genereren**:
  - **Endpoint**: `POST /api/documents/generate/{inspectieId}`
  - **Beschrijving**: Genereert een Word-document op basis van een inspectie-ID.
  - **Parameters**:
 - `inspectieId`: De ID van de inspectie (integer).
  - **Response** (succes): `{ "FilePath": "M:\\Projectdossier\\2025\\documenten\\rapport_13729.docx", "CorrespondentieID": 13729 }`
  - **Zie ook**: [Documentgeneratie](docs/document-generation.md)

## Technische details
- **Database**: Gebruikt Entity Framework Core met SQL Server. Zie [Database-instellingen](docs/database-setup.md) voor meer informatie.
- **Documentgeneratie**: Gebruikt de Open XML SDK om Word-documenten te genereren en custom properties bij te werken.
- **UpdateFieldsOnOpen-instelling**:
  - In de `/api/documents/generate/{inspectieId}` endpoint wordt de `UpdateFieldsOnOpen`-instelling toegevoegd aan het Word-document.
  - Dit zorgt ervoor dat `DOCPROPERTY`-velden (bijv. `{ DOCPROPERTY "Bedrijf" }`) automatisch worden bijgewerkt bij het openen van het document.
  - Gebruikers zien een melding in Word dat velden worden bijgewerkt, wat normaal gedrag is.
- **Sjabloonlocatie**:
  - Sjablonen worden verwacht op `M:\Projectdossier\sjablonen\adressjabloon.docx`.
  - Zorg ervoor dat dit pad toegankelijk is en dat het sjabloon de juiste `DOCPROPERTY`-velden bevat.
- **Assembly-instellingen**:
  - Dit project is een SDK-style project. Assembly-attributen zoals `ComVisible` en `Guid` worden automatisch gegenereerd of kunnen worden gedefinieerd in `AssemblyInfo.vb`.
  - COM-interoperabiliteit is uitgeschakeld (`ComVisible=False`), wat geschikt is voor dit project.

## Documentatie
Voor meer gedetailleerde informatie, zie de volgende documenten:
- [API-endpoints](docs/api-endpoints.md): Een overzicht van alle beschikbare endpoints.
- [Documentgeneratie](docs/document-generation.md): Details over hoe documenten worden gegenereerd en hoe de `UpdateFieldsOnOpen`-instelling werkt.
- [Database-instellingen](docs/database-setup.md): Instructies voor het instellen van de database.

## Contact
Voor vragen, neem contact op met ed blom via [edblom@gmail.com].