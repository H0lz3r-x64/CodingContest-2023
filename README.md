# Infos
## Introduction
- Handelsunternehmen - mehrere Lagern - verschiedene Produkte in verschiedenen Mengen-
- Lieferung Kunden einem oder mehreren Lagern.
<br>
___

## Ziel
- Kunde alle Produkte erhält - Kosten für Auslieferung möglichst gering.
___

## Codebase Infos
- Einsprungpunkt ist ```run()``` in Solution.cs

- Bestellungen als Liste<br>
    ```input.GetOrderLines()```

- Checke ob Vorrätig<br>
    ```warehouse.HasStock()```

- An Kunden senden<br>
    ```operations.Ship(…)```

- Entfernung von Position zu anderer<br>
    ```position.CalculateDistance(Position other)```
___

## Berechnung
- Kosten errechnen sich aus gesamte Größe (```Summe von Product.Size```) und Distanz.<br>
Es werden immer alle Sendungen aufsummiert.
___

## Hints
- Zu einem Kunden kann es mehrere OrderLines geben, diese sind nicht
zwingend aufeinander folgend in der Liste.
- Die Gruppierung der einzelnen versendeten Produkte zu Sendungen erfolgt
durch den KNAPP – Code, nachdem alle Produkte verschickt wurden. Die
Reihenfolge ist dafür nicht wichtig.
- Ein Kunde kann aus einem oder mehreren Lagern beliefert werden.
- Ein Lager hat nur eine begrenzte Menge eines bestimmten Produktes.
- Die Menge eines Produktes ist von Lager zu Lager unterschiedlich. Ein
Produkt muss auch nicht in jedem Lager vorhanden sein. 
___
## Definitionen
- **Auftragszeile (OrderLine)**<br>
    Eine Auftragszeile ist ein Produkt, das von einem Kunden bestellt wurde und an diesen
    geliefert werden soll.

    Alle Auftragszeilen stehen von Anfang an zur Verfügung, es gibt keine vorgegebene
    Reihenfolge, in der diese abzuarbeiten sind.

- **Produkte (Product)**<br>
    Ein Produkt ist das Erzeugnis, das der Kunde bestellt hat. Die Produkte werden von Erzeugern an das Lager geliefert, dort eventuell zwischengelagert, für einen Auftrag kommissioniert und danach an den Kunden versendet.<br>

    Ein Produkt hat
    - Einen Code der es eindeutig identifiziert
    - Eine Größe (Size)
    Die Größe eines Produktes beeinflusst die Kosten für den Versand mit.

- **Kunde (Customer)**<br>
    Ein Kunde kann Produkte bestellen und bekommt diese geliefert. Ein Kunde hat immer
    eine Adresse die als Position mit X und Y Koordinaten bestimmt ist.

- **Lager (Warehouse)**<br>
    In einem Lager sind verschiedene Produkte in unterschiedlichen Mengen gelagert, und
    können von dort versendet werden. Ein Lager hat auch eine Adresse die als Position
    mit X und Y Koordinaten bestimmt ist.
___

## Conditions
- Ein Produkt kann nur aus einem Lager versendet werden, in dem es noch gelagert ist. Wenn es versendet wird, verringert sich die verfügbare Anzahl des Produktes im Lager um 1.
- Eine bestellte Auftragszeile darf nur einmal an den Kunden geliefert werden.
- Nur bestellte Zeilen können aus einem Lager versendet werden.
___

<img src="https://user-images.githubusercontent.com/91200978/225241150-bb39670d-c319-401b-b36d-fd1ae9bc141b.png" alt= “class-diagram” width="40%">
