# AzilEdu

Full-stack aplikacija za upravljanje azilom: **AzilEdu.Api** (ASP.NET Core Web API + EF Core/SQLite),
**AzileduApp** (Blazor Server + MudBlazor) i **AzilEdu.Shared** (modeli i DTO-ovi).
Aplikacija pokriva životinje i multimediju, volontere i zadatke, donatore i donacije, djelatnike,
prijavu s JWT tokenom, administraciju korisnika i uloga te AI pomoćni sloj (Mock ili OpenAI).

## Pokretanje

Preduvjet: .NET SDK 10, alat `dotnet-ef` (`dotnet tool install --global dotnet-ef`).

1. **API** (kreira/migrira bazu i sam seeda početne podatke i demo korisnike pri pokretanju):
   ```bash
   cd AzilEdu.Api
   dotnet run --launch-profile https
   ```
   API sluša na `https://localhost:7185` (Swagger na `/swagger`).

2. **App** (u drugom terminalu):
   ```bash
   cd AzileduApp
   dotnet run --launch-profile https
   ```
   App sluša na `https://localhost:7235`. Otvori tu adresu i prijavi se.

> App-ov `HttpClient` gađa `https://localhost:7185`. Ako promijeniš port API-ja, uskladi ga u
> `AzileduApp/Program.cs`.

Provjera migracijskog lanca na lokalnoj bazi:
```bash
cd AzilEdu.Api
dotnet ef database update 0   # vrati na prazno
dotnet ef database update     # ponovno primijeni sve
```

## Demo računi (samo lokalno, za nastavu)

Kreira ih `AzilEdu.Api/Data/AppUserSeeder.cs` pri prvom pokretanju. Nisu produkcijske lozinke.

| Email | Lozinka | Uloge |
|---|---|---|
| admin@aziledu.local | Admin123! | Admin, User |
| employee@aziledu.local | Employee123! | Employee, User |
| volunteer@aziledu.local | Volunteer123! | Volunteer, User |
| donor@aziledu.local | Donor123! | Donor, User |

## Relacije računa i poslovnih profila

Korisnički račun (`AppUser`) odvojen je od poslovnih zapisa; povezuje se opcionalnim stranim ključevima.

- **AppUser ↔ AppRole** — više-na-više preko spojne tablice `AppUserRole`; jedan račun može imati više uloga.
- **AppUser → Volunteer** — opcionalni FK `VolunteerId`; volonterov račun vidi svoje zadatke (`/mine`).
- **AppUser → Donor** — opcionalni FK `DonorId`; donatorov račun vidi svoje donacije (`/mine`).
- **AppUser → Employee** — opcionalni FK `EmployeeId`; povezuje prijavu s profilom djelatnika.

Svi ti FK-ovi su `OnDelete = SetNull` — brisanje poslovnog zapisa ne briše račun, samo raskida vezu.

## Sigurnost: 401 vs 403

- **401 Unauthorized** — identitet nije potvrđen: token nedostaje, neispravan je ili je istekao.
- **403 Forbidden** — identitet je potvrđen, ali korisnik nema traženu ulogu ili povezani profil.

API provodi zaštitu neovisno o UI-ju: `FallbackPolicy` traži prijavljenog korisnika za sve endpointe,
policy **Staff** (Admin ili Employee) i **AdminOnly** (Admin) štite operacije, a `/mine` rute čitaju
identitet iz potpisanog JWT claima (nikad iz ID-a poslanog iz preglednika).

## AI endpointi i podaci koji se šalju provideru

Svi AI pozivi idu kroz API (`AiController`); ključ i odabir providera ostaju na serveru.

| Endpoint | Pristup | Što se šalje provideru |
|---|---|---|
| `GET /api/ai/status` | Staff | ništa (vraća naziv providera/modela, bez ključa) |
| `POST /api/ai/text` | Staff | `purpose` (s allowliste) + kratki tekstualni kontekst zapisa |
| `GET /api/ai/daily-summary` | Staff | samo agregirani brojevi iz baze (broj životinja, zadataka, donacija…) |
| `GET /api/ai/volunteer-summary/mine` | Volunteer | naslovi/rokovi/statusi vlastitih otvorenih zadataka (do 10) |
| `POST /api/ai/animal-intake` | Staff | slobodna bilješka o životinji (max 4000 znakova) |
| `POST /api/ai/animal-data-check` | Staff | polja `SaveAnimalDto` (bez slika i internih putanja) |

Nikad se ne šalju lozinke, `PasswordHash`, JWT ni API ključevi. AI **predlaže**; korisnik pregledava i
tek onda sprema kroz redoviti CRUD tok.

## Mock i OpenAI način (bez ključa u repozitoriju)

Zadano je `Ai:Provider = "Mock"` (radi bez interneta i ključa). Za stvarni provider koristi user secrets:
```bash
cd AzilEdu.Api
dotnet user-secrets init
dotnet user-secrets set "Ai:Provider" "OpenAI"
dotnet user-secrets set "Ai:ApiKey" "<lokalni-kljuc>"
dotnet user-secrets set "Ai:Model" "gpt-5.6-luna"
```
Povratak na Mock: `dotnet user-secrets set "Ai:Provider" "Mock"`.
Ključ **nikad** ne ide u `appsettings.json`, App projekt ni Git. Isto vrijedi za `Jwt:SigningKey`
(razvojni ključ u `appsettings.json` je samo za učionicu).

## Poznata ograničenja

- Prijava je bez refresh tokena — kad JWT istekne (zadano 60 min), potrebna je ponovna prijava.
- Nema ograničenja broja AI poziva po korisniku ni audit loga AI zahtjeva.
- Multimodalni AI (opis slike) nije implementiran; AI ne obrađuje video.
- SQLite baza je za razvoj; nema pravog produkcijskog deploya ni backup strategije.

## Prijedlozi za sljedeću verziju

1. Refresh tokeni + rate-limiting i audit log za AI endpointe.
2. Prelazak na produkcijsku bazu (PostgreSQL/SQL Server) i cloud storage za multimediju,
   uz opcionalni multimodalni AI opis slika životinja.
