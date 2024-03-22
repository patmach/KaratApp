# KaratApp

### Poznámky k řešení XML

- Vzhledem k tomu, že nelze použít třídy pro práci s XML, aniž by nedocházelo k načtení celého dokumentu, tak jsem jako řešení zvolil odebrání ukončovacích tagů po vytvoření souboru, následný  postupný append všech výsledků a na závěr přidání ukončovacích tagů za pomoci třídy StreamWriter)
  - Resp. jsem nenašel jiné řešení než postupně číst celý soubor a znovu jej celý zapisovat (za pomoci XMLReaderu a XMLWriteru)
  - Také jsem zjednodušil strukturu XML tak, aby byla vhodnější pro zápis appendem, jinak by jednotlivé testy jedné IP adresy byly v souboru v jednom podstromu
  - Jsem si vědom nevýhod tohoto řešení, např.:
    - XML soubor se špatným syntaxem v případě dřívějšího ukončení běhu
    - Manuální práce s textem XML místo serializace => v případě změny struktury musím změnit i vytváření XML
- Pokud by byla možnost, navrhnul bych jiný formát uložení dat pro tento use case. Například CSV