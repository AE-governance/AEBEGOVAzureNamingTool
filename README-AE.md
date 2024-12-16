# Readme AE

Dit is de readme voor verder development binnen AE.

## Opzet custom SSO admin authentication
Er wordt gebruikgemaakt van de doorgegeven token via Azure App Services, wat te vinden is in de `X-MS-CLIENT-PRINCIPAL` header. Deze wordt in `App.razor` uit de header gehaald, en via de `ClaimsPrincipalParser.cs` omgezet naar een `ClaimsPrincipal`, die wordt bijgehouden in `IdentityProviderDetails.cs` (property `CurrentClaimsPrincipal`).

Via de `IdentityProviderDetails` kan er dan `IsAdmin` opgeroepen worden om te kijken of een gebruiker een admin is. Deze functie encapsuleert de volledige logica van het controleren van de claims. De originele app maakt zelf echter gebruik van een flag die gepersisteerd wordt in de browser storage via `ProtectedSessionStorage.SetAsync("admin", admin)`. Deze persistering gebeurt in `MainLayout.razor`.

## Configuratie en TODO
In Azure App Service env vars moet toegevoegd worden bij deployment:
* `AdminClaimType`
* `AdminClaimValue`

Daarnaast moet er een claim met een bepaald type en value toegevoegd worden aan de token voor de user die admin moet zijn (via Azure). Dit moet dus matchen met de env vars die worden ingesteld.

#### Extra TODO's
* Er is geen deftige logging library met log levels opgezet op dit moment. Er is een hoop extra debug logging toegevoegd via `Console.WriteLine($"DEBUG - ...")`. Dit moet aangepast worden voor dit naar PROD gaat.
* Er is nog wat functionaliteit van de oude admin setup over in combinatie met de REST API. Dit moet bekeken worden of dit wordt aangepast of geschrapt.

## Moeilijkheden, gekende problemen, varia info
* De standaard `ConfigurationHelper` class, werkt niet zomaar met een custom `appsettings.json` of met runtime overrides. Dit wordt gebruikt om een fixed `SiteConfiguration` object op te bouwen. Daarnaast wordt er zowel configuratie opgehaald als weggeschreven.
* JSON (de)serialization gebeurt op meerdere plaatsen via afzonderlijke options. Dit zorgt voor overrides die voor problemen kunnen zorgen. Zou best verwijderd worden. Voorbeelden: `Configuration.razor`, `ConfigurationHelper.cs`, `FileSystemHelper.cs`, ...
* De state van de admin en de user wordt in de Razor pages opgezet en via een pop-up modal bij de start. Dit zorgt voor weinig herbruikbaarheid. Er is een beperkte workaround geïmplementeerd.
