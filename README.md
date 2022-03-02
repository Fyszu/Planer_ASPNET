Projekt uczelniany - planer tras dla samochodów elektrycznych.

O aplikacji:
Aplikacja ma ułatwić planowanie podróży posiadaczom samochodów elektrycznych. Zakładając sytuację, że posiadacz takiego samochodu chce wyjechać w podróż do celu, który jest oddalony dalej niż zasięg jego samochodu, musiałby "ręcznie" szukać na mapie lub w innych źródłach ładowarek, które posiadają odpowiednie gniazdo ładowania, oraz obliczać czy dojedzie do tej ładowarki.
Aplikacja ta automatyzuje ten proces, biorąc pod uwagę realny zasięg samochodu (czynniki atmosferyczne, stan baterii), oraz pokazując zasięg na trasie jazdy wraz z wyznaczoną trasą (google api), z możliwością dodania ładowarki w zasięgu bezpośrednio jako przystanek w google maps.

Technologie:
Logika aplikacji jest realizowana w języku C#, na platformie .NET. Technologie wykorzystywane to ASP .NET, a kod jest pisany w modelu MVC z wykorzystaniem m. in. wzorca projektowego repozytorium. Obsługa danych odbywa się za pomocą Entity Framework, a baza danych była tworzona podejściem Code First.
Frontend jest tworzony na razor pages w HTML, JS i CSS i z wykorzystaniem C# (zarówno w razor pages jak i modelach stron).
