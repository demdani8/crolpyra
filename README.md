# crolpyra

## Python adatbázis-kapcsolat

A Python modulok a `Patika_db.sql` által létrehozott SQL Server-adatbázist használják.
Az alapértelmezett kapcsolat Windows-hitelesítést és a `localhost\SQLEXPRESS`
példányt, valamint a `Patika raktárkezelő` adatbázist használja.

1. Telepítsd az ODBC Driver 17 for SQL Server drivert.
2. Telepítsd a Python-függőségeket: `python -m pip install -r requirements.txt`.
3. Szükség esetén állítsd be a kapcsolatot környezeti változókkal:
   - `PATIKA_DB_SERVER`
   - `PATIKA_DB_NAME`
   - `PATIKA_DB_DRIVER`
   - opcionális SQL-hitelesítéshez `PATIKA_DB_USER` és `PATIKA_DB_PASSWORD`
   - teljes egyedi kapcsolat megadásához `PATIKA_DB_CONNECTION_STRING`

A termékek adatai a `dbo.[Termék jellemzők]`, a készlettételek a
`dbo.[Konkrét termék]`, az értékesítési előzmények pedig a `dbo.[Eladások]`
táblában vannak. A `inv.py`, `invdetails.py`, `invfo.py` és `invprojects.py`
ezeket a sémákat használja; a korábbi projekt-/alkatrészműveletek helyett
termékkatalógus- és készlettétel-műveleteket biztosít.