# AzilEdu

Web aplikacija za vođenje azila za životinje.
Web application for managing an animal shelter.

## Tehnologije / Technologies

- **C# / .NET 10**
- **ASP.NET Core Web API** (poslužitelj / backend)
- **Blazor Server** + **MudBlazor** (web sučelje / UI)
- **Entity Framework Core** + **SQLite** (baza / database)
- **JWT** autentikacija (prijava i uloge / auth and roles)
- **OpenAI .NET SDK** (opcionalni AI provider, uz ugrađeni Mock / optional AI provider, with built-in Mock)

---

## Hrvatski

Kroz aplikaciju se na jednom mjestu prate životinje, volonteri i njihovi zadaci,
donatori i donacije te djelatnici azila. Aplikacija ima prijavu s ulogama, pa svaki
korisnik vidi ono što se tiče njega, i ugrađenog AI pomoćnika koji predlaže tekstove
(opise, zahvale, sažetke).

Projekt se sastoji od tri dijela:
- **API** — poslužitelj koji radi s bazom i sadrži svu logiku i pravila,
- **App** — Blazor web sučelje kojim se korisnik služi,
- **Shared** — zajednički modeli podataka.

### Što aplikacija nudi
- Evidencija životinja s profilom, galerijom slika i videom.
- Volonteri i zadaci, donatori i donacije, djelatnici.
- Početni dashboard s pregledom stanja azila.
- Prijava korisnika i uloge: **Admin, Djelatnik, Volonter, Donator** — svaka uloga
  vidi svoj izbornik i svoje podatke.
- Administrator upravlja korisničkim računima i dodjeljuje uloge.
- AI pomoćnik: opis za udomljavanje, zahvala donatoru, objava za društvene mreže,
  dnevni sažetak i sažetak zadataka. AI samo **predlaže** — korisnik pregleda i odluči.

### Uloge i prijava
Aplikacija se otvara na stranici za prijavu; sadržaj se prikazuje tek nakon prijave,
prema ulozi. Demo računi (lokalni, samo za vježbu):

| Uloga | Email | Lozinka |
|---|---|---|
| Administrator | admin@aziledu.local | Admin123! |
| Djelatnik | employee@aziledu.local | Employee123! |
| Volonter | volunteer@aziledu.local | Volunteer123! |
| Donator | donor@aziledu.local | Donor123! |

### Pokretanje
Potreban je .NET SDK 10. Pokrenu se oba projekta (`AzilEdu.Api` pa `AzileduApp`),
najlakše iz Visual Studija. Baza se stvara i puni početnim podacima sama pri prvom
pokretanju.

### Sigurnost
Prava zaštita je na API-ju, ne samo u sučelju: bez prijave se podacima ne može
pristupiti, a svaka operacija provjerava ima li korisnik odgovarajuću ulogu. Lozinke
se spremaju kao hash, a AI se poziva isključivo preko API-ja (ključ nikad nije u sučelju).

---

## English

The application tracks shelter animals, volunteers and their tasks, donors and
donations, and staff — all in one place. It has role-based login, so each user sees
what concerns them, and a built-in AI assistant that suggests texts (descriptions,
thank-you notes, summaries).

The project has three parts:
- **API** — the server that works with the database and holds all logic and rules,
- **App** — the Blazor web interface used by end users,
- **Shared** — shared data models.

### Features
- Animal records with a profile, image gallery and video.
- Volunteers and tasks, donors and donations, staff.
- A dashboard with an overview of the shelter's status.
- Login and roles: **Admin, Employee, Volunteer, Donor** — each role sees its own
  menu and data.
- The administrator manages user accounts and assigns roles.
- AI assistant: adoption description, donor thank-you, social-media post, daily
  summary and task summary. The AI only **suggests** — the user reviews and decides.

### Roles and login
The app opens on the login page; content is shown only after signing in, based on role.
Demo accounts (local, for practice only):

| Role | Email | Password |
|---|---|---|
| Administrator | admin@aziledu.local | Admin123! |
| Employee | employee@aziledu.local | Employee123! |
| Volunteer | volunteer@aziledu.local | Volunteer123! |
| Donor | donor@aziledu.local | Donor123! |

### Running
Requires .NET SDK 10. Start both projects (`AzilEdu.Api`, then `AzileduApp`), easiest
from Visual Studio. The database is created and seeded automatically on first run.

### Security
Real protection lives on the API, not just in the UI: without logging in the data
cannot be accessed, and every operation checks whether the user has the right role.
Passwords are stored as hashes, and the AI is called only through the API (the key is
never in the UI).
