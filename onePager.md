# Space escape (Horror/immersive enviroment)

## Team
Haast Jelle, Spruyt Sem

## Concept
Je zweeft met je groep door de ruimte in je schip wanneer er iets onbekend op de radar verschijnt op de vlieg route van je schip. Na de crash is er iets onbekend aan boord en je groep is nergens meer te vinden, vind je groepsleden en een manier om te ontsnappen van het schip voordat je gevangen wordt door wat er ook aan boord je schip is.

## "Draaiboek"

De speler start in een de bestuurderskamer van het schip, er verschijnt iets op het scherm dat niet te identificeren valt. Er is een crash met het object (scherm zwart), speler wordt wakker in het donkere schip wat al beschadigingen heeft gekregen van het onidentificeerbare wezen. De speler zal opzoek moeten gaan naar zijn mede collega's (als deze nog leven) en naar items voor de "escape pod" om zo het spel te kunnen voltooien.

## Gameplay
- Sluipen en schuilen in je omgeving.
- Ontdek de kamers van het ship en vind de nodige items.
- Gebruik maken van eenvoudige puzzels via omgevings interactie.
- Spanningsopbouw door gebruik van geluid en licht.

## Opbouw AI
De alien zal getrained worden via meerdere iteraties en curriculum-learning(= eenvoudig naar moeilijk).

Het doel is dat de gangen als een soort patrouille route te laten dienen via de ingebouwde nav-agent van unity en als er dan bijvoorbeeld een trigger(geluid, oppakken van mission item) gebeurd dan zal deze naar de kamer waar deze voorgekomen is navigeren en dan zal de nav-agent worden uitgeschakeld en zal deze de ruimte doorzoeken gebruik makend van onze eigen getrainde ML-agent over een bepaalde tijd.

## Setting & sfeer

- sci-fi ruimteschip en een crisis toestand
- donkere gangen en kamers met minimale belichting
- geluids design zal belangrijk zijn aangezien dit de hoofd drijver is naar een spannende ervaring.
- het gevoel creëren dat je alleen met dit wezen zit opgeloten.

## Doel

Een immersieve horrorervaring creëren in de diepte van de ruimte waar de speler zich constant opgejaagd zal voelen. De eigen getrainde ML-Agent zal een gevoel van onbekendheid/stress creëren aangezien je niet weet waar deze zich naar gaat begeven in de ruimte. Via VR kunnen we het maken dat de speler actief in plekken kan schuilen en kan gaan bukken om onder objecten door te raken.

## Innovatie
###  Waarom AI
- Een ML-Agent die zelf heeft getrained achter een speler te zoeken.
- De AI is een "single agent" waar de schuiler (speler) tegen moet spelen.
- Dynamisch gedrag in een horror setting.
- Nadeel: 
1. Moeilijk om te trainen voor over gangen te lopen/echt goede navigatie te doen wat deze te makkelijk maakt. 
2. De speler zal nooit echt van de vele verschillende patronen kunnen leren waardoor een level misschien te moeilijk/onmogelijk kan worden.
- Voordelen: 
1. Echt bijna gestructureerd "random" gedrag wat het goed maakt voor herspeelbaarheid/verrassingen.

#### Wat als zonder AI?=>
- voorgelegde routes.
- voorspelbaar.

- voordeel: echt random ML-Agent gedrag kan misschien te moeilijk/onmogelijk worden.
- nadeel: als de speler genoeg opnieuw speelt zal deze de acties kunnen voorspellen =>kan ervoor dat een spel juist saai wordt.

### Waarom VR
- actief bukken om te kunnen verstoppen.
- Een interactieve omgeving.
- trekt je meer in het verhaal/gameplay (meer spanning opbouw).
- (interactie met deuren)
- het oprapen van de missie items

### interactie
- de speler kan door met zijn hand naar missie item + grip-click een item missie item oprapen.
- de speler kan door met zijn hand naar missie item + grip-click een deur openen.
- Door fysiek te bukken kan de speler onder objecten bewegen en schuilen.
- interactie vooral omgevings gebonden houden zonder te veel gebruikt van de knoppen buiten de grip knop van de controller.

