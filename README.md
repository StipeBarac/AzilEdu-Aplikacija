# AzilEdu

Web aplikacija za vođenje azila za životinje. Kroz nju se na jednom mjestu prate
životinje, volonteri i njihovi zadaci, donatori i donacije te djelatnici azila.
Aplikacija ima prijavu s ulogama, pa svaki korisnik vidi ono što se tiče njega,
i ugrađenog AI pomoćnika koji predlaže tekstove (opise, zahvale, sažetke).

Projekt je izrađen kroz nekoliko lekcija, a sastoji se od tri dijela:
- **API** — poslužitelj koji radi s bazom i sadrži svu logiku i pravila,
- **App** — Blazor web sučelje kojim se korisnik služi,
- **Shared** — zajednički modeli podataka.

## Što aplikacija nudi

- Evidencija životinja s profilom, galerijom slika i videom.
- Volonteri i zadaci, donatori i donacije, djelatnici.
- Početni dashboard s pregledom stanja azila.
- Prijava korisnika i uloge: **Admin, Djelatnik, Volonter, Donator**.
  Svaka uloga vidi svoj izbornik i svoje podatke (npr. volonter svoje zadatke,
  donator svoje donacije).
- Administrator upravlja korisničkim računima i dodjeljuje uloge.
- AI pomoćnik: prijedlog opisa za udomljavanje, zahvala donatoru, objava za
  društvene mreže, dnevni sažetak i sažetak zadataka. AI samo **predlaže** —
  korisnik uvijek pregleda i odluči prije spremanja.

## Uloge i prijava

Aplikacija se otvara na stranici za prijavu; sadržaj se prikazuje tek nakon prijave,
prema ulozi korisnika. Za isprobavanje postoje demo računi (lokalni, samo za vježbu):

| Uloga | Email | Lozinka |
|---|---|---|
| Administrator | admin@aziledu.local | Admin123! |
| Djelatnik | employee@aziledu.local | Employee123! |
| Volonter | volunteer@aziledu.local | Volunteer123! |
| Donator | donor@aziledu.local | Donor123! |

## Pokretanje

Potreban je .NET SDK 10. Pokrenu se oba projekta (`AzilEdu.Api` pa `AzileduApp`),
najlakše iz Visual Studija. Baza se stvara sama pri prvom pokretanju i puni se
početnim podacima, pa nije potrebna nikakva ručna priprema.

## Sigurnost (ukratko)

Prava zaštita je na API-ju, ne samo u sučelju: bez prijave se podacima ne može
pristupiti, a svaka operacija provjerava ima li korisnik odgovarajuću ulogu.
Lozinke se spremaju kao hash, a AI se poziva isključivo preko API-ja (ključ nikada
nije u web sučelju).
