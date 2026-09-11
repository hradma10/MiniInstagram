# MiniInstagram

Webová aplikace zaměřená na sdílení fotografií a interakci mezi uživateli. Nabízí správu profilu, sledování ostatních účtů, lajkování a komentování příspěvků a personalizovaný přehled obsahu.

## Použité technologie

- **Backend:** C#, ASP.NET Core MVC (.NET 8+)
- **ORM a databáze:** Entity Framework Core, SQLite
- **Autentizace:** ASP.NET Core Identity
- **Frontend:** Razor Views, Bootstrap 5, AJAX
---

## Hlavní funkce

- **Správa účtu:** Registrace, přihlášení, správa profilu (bio, jméno, věk) a zobrazení statistik profilu.
- **Příspěvky:** Vytváření, úprava a mazání příspěvků s fotografiemi.
- **Sociální interakce:** 
  - Systém sledování uživatelů.
  - Lajkování příspěvků i jednotlivých komentářů.
  - Přidávání a mazání komentářů.
- **Personalizovaný feed:** Pro přihlášené uživatele jsou zobrazeny příspěvky sledovaných uživatelů a doporučené příspěvky na základě historie lajků. Nepřihlášeným uživatelům jsou zobrazeny poslední přidané příspěvky.

---

## Spuštění projektu

### Požadavky
- **.NET SDK 8.0+**
- **Git**

### Postup

1. **Klonování repozitáře:**
   ```bash
   git clone https://github.com/hradma10/MiniInstagram
   cd MiniInstagram
   ```

2. **Konfigurace cest:**
   V `MiniInstagramASP/appsettings.json` nahraďte následujicí cesty:
   ```json
    {
      "ConnectionStrings": {
        "AuthDb": "Data Source=C:/temp/MiniInstagram/auth.db",
        "AppDb": "Data Source=C:/temp/MiniInstagram/data.db"
      },
      "Paths": {
        "SaveFile": "C:/temp/MiniInstagram/uploads/"
      }
    }
   ```
   > Databázové složky i EF Core migrace jsou při startu vytvořeny automaticky.

3. **Spuštění:**
   ```bash
    dotnet run --project MiniInstagramASP
   ```
