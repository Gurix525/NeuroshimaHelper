using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;
using System.Diagnostics;
using Newtonsoft.Json;

namespace NeuroshimaDB.Models
{
    [Serializable]
    [JsonObject]
    public class Npc : IEnumerable<int>
    {
        private Dictionary<string, int> _skills = new();

        public string Id { get; set; }
        public string Name { get; set; }
        public int Zr { get; set; }
        public int Pr { get; set; }
        public int Cha { get; set; }
        public int Spr { get; set; }
        public int Bd { get; set; }
        public string Perk { get; set; }
        public string PerkDescription { get; set; }

        public Dictionary<string, int> Skills
        {
            get => _skills;
            set => _skills = value;
        }

        public string SkillsString
        {
            get
            {
                var skillsSorted = Skills.OrderBy(x => x.Key).ToList();
                string output = "";
                foreach (var item in skillsSorted)
                    output += $"{item.Key}: <b>{item.Value}</b>;<br>";
                return output;
            }
        }

        [JsonIgnore]
        public int[] Stats =>
            new int[] { Zr, Pr, Cha, Spr, Bd };

        [JsonIgnore]
        public int StatsSum => this.Sum(x => x);

        [JsonIgnore]
        public int this[int index]
        {
            get
            {
                return index switch
                {
                    0 => Zr,
                    1 => Pr,
                    2 => Cha,
                    3 => Spr,
                    4 => Bd,
                    _ => throw new IndexOutOfRangeException()
                };
            }
            set
            {
                _ = index switch
                {
                    0 => Zr = value,
                    1 => Pr = value,
                    2 => Cha = value,
                    3 => Spr = value,
                    4 => Bd = value,
                    _ => throw new IndexOutOfRangeException()
                };
            }
        }

        public Npc()
        {
        }

        public Npc(int power, string profession)
        {
            Name = profession;
            _ = power switch
            {
                0 => Initialize(40, 50, 0),
                2 => Initialize(60, 70, 2),
                _ => Initialize(50, 60, 1)
            };
        }

        public Npc(int minStatsSum = 50, int maxStatsSum = 60)
        {
            Name = "Ganger";
            Initialize(minStatsSum, maxStatsSum, 1);
        }

        public override string ToString()
        {
            return $"{Name}: {string.Join(", ", Stats)}";
        }

        public IEnumerator<int> GetEnumerator()
        {
            return new NpcEnum(Stats);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<int> RandomizeStats()
        {
            Dice dice = new();
            List<int> newStats = new();
            for (int i = 0; i < 6; i++)
            {
                newStats.Add((int)Math.Round((dice.Roll + dice.Roll + dice.Roll) / 3F));
            }
            for (int i = 0; i < newStats.Count; i++)
                if (newStats[i] < 6)
                    newStats[i] = 6;
            newStats = newStats
                .OrderByDescending(x => x)
            .Take(newStats.Count - 1).ToList();

            return newStats;
        }

        private IEnumerable<int> OrderStatsByPriority(List<int> randomizedStats)
        {
            var priority = PT.Templates.ContainsKey(Name) ?
                PT.Templates[Name].StatsPriority.ToList() :
                PT.Templates["Ganger"].StatsPriority.ToList();
            for (int i = 0; i < priority.Count; i++)
                priority[i] *= 100;
            int max = priority.Max();
            Dice dice2 = new(100);
            for (int i = max; i > 0; i--)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (priority[j] == i)
                        priority[j] += dice2.Roll;
                }
            }

            //Debug.WriteLine(string.Join(", ", priority));

            List<int> finalStats = new int[5].ToList();

            for (int i = priority.Max(); i > 100; i--)
            {
                for (int j = 0; j < priority.Count; j++)
                {
                    if (i == priority[j])
                    {
                        finalStats[j] = randomizedStats[0];
                        randomizedStats.RemoveAt(0);
                    }
                }
            }

            return finalStats;
        }

        private void RandomizeSkills(int power)
        {
            int skillPoints = GetSkillPoints(power);

            var priority = PT.Templates.ContainsKey(Name) ?
                PT.Templates[Name].SkillsPriority.ToList() :
                PT.Templates["Ganger"].SkillsPriority.ToList();
            for (int i = 0; i < priority.Count; i++)
                priority[i] *= 100;
            int max = priority.Max();
            Dice dice = new(100);
            for (int i = max; i > 0; i--)
            {
                for (int j = 0; j < priority.Count; j++)
                {
                    if (priority[j] == i)
                        priority[j] += dice.Roll;
                }
            }

            for (int i = priority.Max(); i > 100; i--)
            {
                for (int j = 0; j < priority.Count; j++)
                {
                    if (priority[j] == i)
                        RandomizeSkillsDeeper(j, i, ref skillPoints);
                    if (skillPoints <= 0)
                        break;
                }
                if (skillPoints <= 0)
                    break;
            }
        }

        private void RandomizeSkillsDeeper(int index, int priority, ref int skillPoints)
        {
            string[] skills = index switch
            {
                0 => new string[] { "Bijatyka", "Broń ręczna", "Rzucanie" },
                1 => new string[] { "Samochód", "Motocykl", "Ciężarówka" },
                2 => new string[] { "Kradzież kieszonkowa", "Otwieranie zamków", "Zwinne dłonie" },
                3 => new string[] { "Pistolety", "Karabiny", "Broń maszynowa" },
                4 => new string[] { "Łuk", "Kusza", "Proca" },
                5 => new string[] { "Wyczucie kierunku", "Przygotowywanie pułapki", "Tropienie" },
                6 => new string[] { "Nasłuchiwanie", "Wypatrywanie", "Czujność" },
                7 => new string[] { "Skradanie się", "Ukrywanie się", "Maskowanie" },
                8 => new string[] { "Łowiectwo", "Znajomość terenu", "Zdobywanie wody" },
                9 => new string[] { "Zastraszanie", "Perswazja", "Zdolności przywódcze" },
                10 => new string[] { "Postrzeganie emocji", "Blef", "Opieka nad zwierzętami" },
                11 => new string[] { "Odporność na ból", "Niezłomność", "Morale" },
                12 => new string[] { "Pierwsza pomoc", "Leczenie ran", "Leczenie chorób" },
                13 => new string[] { "Mechanika", "Elektronika", "Komputery" },
                14 => new string[] { "Maszyny ciężkie", "Wozy bojowe", "Kutry" },
                15 => new string[] { "Rusznikarstwo", "Wyrzutnie", "Materiały wybuchowe" },
                16 => new string[] { "Kondycja", "Pływanie", "Wspinaczka" },
                17 => new string[] { "Jazda konna", "Powożenie", "Ujeżdżanie" },
                _ => PT.Templates.ContainsKey(Name) ?
                PT.Templates[Name].SpecialSkills :
                PT.Templates["Ganger"].SpecialSkills
            };
            CalculateSkills(skills, priority, ref skillPoints);
        }

        private void RandomizeTrait()
        {
            var perks = PT.Templates.ContainsKey(Name) ?
                PT.Templates[Name].Traits :
                PT.Templates["Ganger"].Traits;
            var perkDecsriptions = PT.Templates.ContainsKey(Name) ?
                PT.Templates[Name].TraitDescriptions :
                PT.Templates["Ganger"].TraitDescriptions;
            Dice dice = new(4);
            int roll = dice.Roll;
            Perk = perks[roll - 1];
            PerkDescription = perkDecsriptions[roll - 1];
        }

        private void CalculateSkills(string[] skills, int priority, ref int skillPoints)
        {
            Dice dice = priority switch
            {
                > 400 => new(5),
                > 300 => new(4),
                > 200 => new(3),
                _ => new(2)
            };
            foreach (string skill in skills)
            {
                if (skillPoints > 0)
                {
                    int result = dice.Roll - 1;
                    if (result > 0)
                        _skills.Add(skill, result);
                    skillPoints -= result;
                }
            }
        }

        private int GetSkillPoints(int power)
        {
            Dice dice = new(10);
            return power switch
            {
                0 => 15 + dice.Roll,
                2 => 35 + dice.Roll,
                _ => 25 + dice.Roll
            };
        }

        private bool Initialize(int minStatsSum, int maxStatsSum, int power)
        {
            Id = Guid.NewGuid().ToString();
            List<int> stats = new();
            int statsSum = 0;
            while (statsSum < minStatsSum || statsSum > maxStatsSum)
            {
                stats = RandomizeStats().ToList();
                statsSum = stats.Sum(x => x);
            }

            var outputStats = OrderStatsByPriority(stats).ToList();
            //Debug.WriteLine(string.Join(", ", outputStats));

            for (int i = 0; i < 5; i++)
                this[i] = outputStats[i]; // Przypisanie współczynników

            RandomizeSkills(power);

            return true;
        }

        public class NpcEnum : IEnumerator<int>
        {
            private int[] _stats;
            private int _position = -1;

            public int Current => _stats[_position];

            object IEnumerator.Current => Current;

            public NpcEnum(int[] stats) =>
                _stats = stats;

            public bool MoveNext()
            {
                _position++;
                return (_position < _stats.Length);
            }

            public void Reset()
            {
                _position = -1;
            }

            public void Dispose()
            {
            }
        }

        private static class PT
        {
            public static Dictionary<string, ProfessionTemplate> Templates { get; } =
                new()
                {
                    {"Chemik", new ProfessionTemplate(
                        new int[]{ 1, 1, 1, 2, 1 },
                        new int[]{ 2, 1, 1, 2, 2, 1, 2, 1, 1, 2, 1, 2, 3, 1, 1, 2, 2, 1, 4 },
                        new string[]{ "Chemia", "Biologia", "Matematyka"},
                        new string[]{ "Smakuje jak arszenik", "Farmaceuta", "Kwas i zasada", "Rozcieńczanie" },
                        new string[]{
                            "Lata babrania się w toksycznych świństwach zrobiły swoje. Łykasz uran jak czekoladki, popijasz naftą. Nie zatrudnią cię w kopalni jako detektor gazu, bo jesteś odporny. Otrzymujesz modyfikator -30% w testach Kondycji na skażenie i radiację.",
                            "Jesteś Mesjaszem Nowej Ery. Potrafisz produkować środki błyskawicznego leczenia: Painkillery, Medpaki i DeadLine. PT testów Chemii są odpowiednio:<br>Painkiller – Trudny;<br>Medpak – B. Trudny; <br>Deadline – Ch. Trudny.<br>Musisz dysponować laboratorium i składnikami. Produkcja jednej działki zajmuje ci około 4h.",
                            "Patrzysz na czarną kałużę i mówisz: olej. Wąchasz i dodajesz: z domieszką benzyny. Zamaczasz palec i smakujesz – ołowiowa kiepskiej jakości. Chodzi o to, że potrafisz rozpoznać substancję po wyglądzie, zapachu i smaku. Proste i często spotykane chemikalia rozpoznajesz automatycznie, zaś te rzadsze za pomocą testu Chemii o PT ustalonym przez MG. Za każdy użyty zmysł (wzrok, zapach, smak) otrzymujesz w teście bonus +1 do Sprytu.",
                            "Byłby z ciebie świetny handlarz prochami albo paliwem. Wyciągasz 10% z dowolnego specyfiku, dodajesz szczyn, kredy, pieprzu lub aspirynny i nawet nałogowiec nie zauważy. Żeby rozpoznać, że substancja została rozcieńczona, trzeba zdać test Percepcji: Cholernie trudny, jeśli wyjąłeś 10%, Bardzo trudny, jeśli przywłaszczyłeś sobie 20% lub Trudny, gdy zabrałeś aż 30%. Poza tym substancja odpowiednio słabiej działa (słabsza benzyna, narkotyki, alkohol, leki, proch itd.) lub powoduje niechciane skutki uboczne (dolegliwości, szybsze zużycie silnika itd.)." }) },
                    {"Ganger", new ProfessionTemplate(
                        new int[]{ 3, 2, 3, 1, 3 },
                        new int[]{ 4, 2, 1, 4, 2, 1, 2, 1, 1, 3, 2, 2, 1, 1, 1, 1, 2, 1, 0 },
                        new string[]{ },
                        new string[]{ "Jeden z nich", "Odważny czy głupi?", "Dwóch na jednego", "Po prostu killer" },
                        new string[]{
                            "Posiadasz widoczny symbol przynależności do siejącego grozę gangu. Nikt, kto nie ma jaj ze stali, ci nie podskoczy. Bylo kto nie będzie ci pyskować, anis ię z tobą sprzeczać, nie odważą się spojrzeć ci w oczy, będą się także bali tak po prostu cię lać. ALE. Ale ci z jajami ze stali mogą przerobić cię na pasztet za to tylko, że jesteś \"jednym z nich\". Dodatkowo 100 gambli na Znajomości w Gangach.",
                            "Nikt nie jest w stanie cię zastraszyć, przynajmniej do czasu, aż cię poważnie nie zrani. Po prostu nie jesteś w stanie uwierzyć, że ktokolwiek odważyłby się ci podskoczyć.<br>Masz jeden automatyczcny sukces we wszystkich testach Zastraszania i Morale.",
                            "Jesteś specem od kopania jednego frajera wespół z kolegami. Świetnie się uzupełniacie. Gdy walczysz w grupie, możecie atakować jednego przeciwnika nawet w sześciu (a nie jak normalnie – w pięciu maksymalnie). Poza tym walcząc w przewadze liczebnej możesz przerzucić jedną kość w każdej turze walki.",
                            "Rwiesz się do bitki jak nikt inny – masz bonus +1 do Zręczności w testach walki wręcz. Ale jeśli trafisz na prawdziwego twardziela, ten szybko może zgasić twój zapał. Gdy tylko zostaniesz zraniony, bez względu na test Odporności na ból otrzymujesz większą karę, a bonus znika." }) },
                    {"Gladiator", new ProfessionTemplate(
                        new int[]{ 5, 2, 3, 1, 4 },
                        new int[]{ 4, 1, 1, 2, 2, 1, 2, 1, 1, 2, 1, 4, 1, 1, 1, 1, 3, 1, 0 },
                        new string[]{ },
                        new string[]{ "Nie do zdarcia", "Łyżeczka", "Wystarczy jedna ręka", "Zawołajcie kolegów" },
                        new string[]{
                            "Ten fragment o pompowaniu posoki, to nie była poezja, mam rację? To cholerna prawda. Ciężko cię wyłączyć z gry. Mówię wyłączyć z gry, bo o tym, żeby po prostu cię zabić, nawet nie myślę. Reguła dotycząca zwiększania poziomu zadanych obrażeń (1 lub 2 na kościach podczas ataku, trafienia w głowę ird.) nie dotyczy cię w walce wręcz. Choćby twój przecinik rzucił i trzy jedynki, to wcale nie oznacza, że przebił ci serce.",
                            "Co gorsza, z tą łyżeczką też nie przesadziłem, wiem o tym doskonale. Gladiator to taki facet, który walczy tym, co wpadnie mu w łapska. Sieć, hydrant, rurka gazowa, siekierka strazacka. Cokolwiek weźmiesz w łapska – Mistrz Gry musi traktować tak, jak gdybyś miał Umiejętność walki tym czymś na poziomie minimum 4. Nawet, jeśli jest to ta zasrana łyżeczka...<br>To gwarantowane miinimum 4 nie sumuje się z Umiejętnością.<br>Cecha nie dotyczy oczywiście broni palnej!",
                            "Ciężka broń w twoich rękach jest jeszcze groźniejsza. Wymagana Budowa dla każdej broni ręcznej jest w twoim przypadku mniejsza o 4. Dodatkowo gdy zadawane obrażenia dla danej broni zależą od Budowy (nie dotyczy pięści), twoja Budowa traktowana jest tak, jakby była większa o 2.",
                            "Gdy walczsz z wieloma przecinikami, wtedy jakakolwiek broń w twoich rękach daje ci dodatkowy bonus +2 do Zręczności w testach walki wręcz." }) },
                    {"Handlarz", new ProfessionTemplate(
                        new int[]{ 1, 2, 4, 3, 1 },
                        new int[]{ 1, 2, 2, 1, 1, 1, 1, 1, 1, 4, 4, 3, 1, 1, 1, 1, 1, 1, 0 },
                        new string[]{ },
                        new string[]{ "Trakt handlowy", "Szklarz", "Umysł kupca", "Lepsza cena" },
                        new string[]{
                            "Nie jesteś jakimś tam stacjonarnym straganiarzem sprzedającym ogórki. Bliżej ci do twardzieli pokroju wędrownego sprzedawcy bojlerów. Zdeptałeś kawał świata i poznałeś niejedno miejsce, więc wielu ludzi cię rozpoznaje, a na niektórych możesz nawet liczyć. Zaczynasz z dodatkowymi 150 gamblami na Znajomych (nie Wojowników).",
                            "Szklarz to taki facet, co bawi się kitem. Głównie wciskaniem kitu. Stąd przydomek szklarz, bo świetnie wciskasz kit. Handlując, kiedy próbujesz wcisnąć komuś jakiś kit (testy Perswacji i Blefu) twój Spryt wzrasta o 2.",
                            "Budzisz się i mówisz sobie: dziś jest pierwszy dzień reszt mojego życia, dziś zbiję fortunę, taki jestem szczęśliwy, kocham wszystkich ludzi. Każdego dnia decydujesz, czy chcesz być zwyczajnym łajzą, jak każdy, czy też dokonujesz metamorfozy w wesołego akwizytora. Lata ćwiczeń i tysiące afirmacji sprawiają, że tego dnia otrzymujesz bonus +2 do Sprytu i Charakteru, ale pozostałe Współczynniki spadają o 1. Stan ten utrzyma się do czasu, aż prześpisz noc.",
                            "Gdy chcesz coś tanio kupić, sam idziesz na zakupy. Gdy twoi koledzy chcą coś tanio kupić, również... wysyłają ciebie. Automatycznie płacisz o 10% mniej, niż trzeba (czcyli 90% ceny), chyba że sprzedawca posiada tę samą Cechę. Ma się gadane, co nie? Niestety, nie znaczy to, że sprzedajesz za 110%!" }) },
                    {"Kaznodzieja Nowej Ery", new ProfessionTemplate(
                        new int[]{ 2, 1, 3, 1, 2 },
                        new int[]{ 3, 2, 1, 3, 1, 1, 2, 1, 1, 4, 2, 3, 2, 1, 1, 1, 2, 1, 0 },
                        new string[]{ },
                        new string[]{ "Spójrz mi w oczy", "Amen", "Wierni", "Moc wiary" },
                        new string[]{
                            "Kaznodzieja potrafi wyczuć kłamstwo na kilometr. To tajemnica jego władzy nad ludźmi. Zabobonni wierzą nawet, że potrafi czytać w my ślach.<br>W trakcie dłuższej rozmowy, Kaznodzieja i rozmówca wykonują Trudne testy Charakteru. Jeśli Kaznodzieja zda test, a jego rozmówca nie, lub gdy Kaznodzieja uzyska przewagę w teście Przeiwstawnym, wtedy klecha odkryje kłamstwo.",
                            "Jeśli podczas testu na jednej z kości wypadnie jedynka, liczy się ona jako dwa sukcesy (a w walce zamienia inną kość w sukces z wynikiem 1). Jednka to znak od tego z góry, że jesteś na dobrej drodze. Wypatruj znaków. Możesz tego użyć maksymalnie 3 razy na sesję. A decydujesz już po rzucie.",
                            "Gdy otwierasz usta, natychmiast wypływają słowa mądrości, dzięki czemu co głupsi słuchacze postanowią zostać twoimi uczniami i podążać za tobą. W każdym kwadransie przemowy na temat wiary wybierasz jednego konkretnego słuchacza (BN) i wykonujesz przeciwstawny test swojej Perswazji przeciko jego Charakterowi. Każdy Punkt Przewagi powyżej 10 zamienia się na punkt Powiązań – właśnie zwerbowałeś nowego wiernego! Przy Powiązaniach powyżej 3 wierny może z tobą wędrować.<br>Jednak kij ma dwa końce. Każdy dodatkowy słuchacz o bardzo słabym Charakterze (poniżej 8 – najczęściej będą to nędzarze, dzieci, staruszki itp.) może się do ciebie przyłączyć nawet wbrew twojej woli. Rzuć za kazdego takiego patałacha k20 – przy wuniku 20, chcesz czy nie chcesz, zdobywasz wiernego z Powiązaniami ustalonymi przez MG.",
                            "Bądź przewonikiem duchowym i dla siebie, i dla innych. Doradzaj, podtrzymuj na duchu, szerz swoją wiarę, a kiedy trzeba, ukręć łeb opornym. Przy testach umiejętności z pakietu Empatia i Siła woli otrzymujesz bonus _1 do Charakteru, zaś przy testach umiejętności z pakietu Negocjacje aż +2 do Charakteru. Ponadto, jeśli zechcesz, za pomocą przemowy dodajesz każdemu słuchaczowi bonus +1 do przeprowadzanego właśnie przez niego testu umiejętności z pakietu Siła woli." }) },
                    {"Kowboj", new ProfessionTemplate(
                        new int[]{ 5, 3, 2, 1, 4 },
                        new int[]{ 3, 2, 1, 4, 2, 2, 2, 1, 1, 2, 1, 2, 1, 1, 1, 1, 2, 4, 0 },
                        new string[]{ },
                        new string[]{ "Rewolwerowiec", "Ostatnia kula", "Pojedynek", "Koleś zwany koniem" },
                        new string[]{
                            "Wyciągasz pistolet z kabury w mgnieniu oka. Jeśli używasz krótkiej broni palnej, pistolet w kaburze traktuje się tak, jakbyś miał go już dobytego w dłoni.",
                            "Jeśli w twoim rewolwerze została jeszcze tylko jedna kula, strzał ten będzie dla ciebie łatwiejszy o 1 Poziom Trudności testu.",
                            "Billy Kid, Jessie James – każdy z nich posiadał tę Sztuczkę. Test pierwszego strzału po dobyciu broni krótkiej (pistolet, rewolwer) możesz przerzucić. Wynik przerzuconego testu musisz zaakceptować.",
                            "Gdy ktoś cię zezłości – yupiesz, gdy rozśmieszy – rżysz. Ale koniem zwą cię z innego powodu. Wszystkie umiejętności z pakietu Jeździectwo posiadasz od razu na poziomie 2, dodatkowo każdy z testów tych umiejętności możesz przerzucić raz na sesję. To jeszcze nie wszystko! Testy Opieki nad zwierzętami są dla ciebie łatwiejsze o poziom, jeśli dotyczą twojego wierzchowca." }) },
                    {"Kurier", new ProfessionTemplate(
                        new int[]{ 3, 2, 1, 1, 1 },
                        new int[]{ 2, 4, 3, 3, 1, 3, 3, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 2, 0 },
                        new string[]{ },
                        new string[]{ "Znajomości", "Skrytka", "Ze wschodu na zachód", "Trakt" },
                        new string[]{
                            "Szybka bryka, czy worek granatów nie zawsze pomogą przejść przez terytorium zajmowane przez pięć różnych gangów, w końcu kiedyś podwinie ci się noga i staniesz z zepsutą chłodnicą w samym centrum ziemi Brain Dogs. Wtedy przyda się znać szefa, nie sądzisz? Zaczynasz z dodatkowymi 150 gamblami, które możesz przeznaczyć na Znajomych (nie Wokowników).",
                            "Otwierasz drzwi, rzucasz okiem na wnętrze bryki, chwilę gmerasz i już masz swój tajny schowek. Nawet sam właściciel tego samochodu go nie znajdzie. W każdym pojeździe rozmiarów minimum samochodu, Kurier w kilka minut potrafi znaleźć lub spreparować skrytkę, którą bardzo trudno odkryć podczas przeszukiwania: rozmiarów pudełka papierosów (test Wypatrywania na Fart), dwa razy większe (Cholernie trudny test Wypatrywania), na pistolet (Bardzo trudny test Wypatrywania), małą walizkę (Trudny test Wypatrywania).",
                            "Gdy kierujesz jakąś dłuższą, co najmniej kilkudniową wyprawą, gdzie liczy się wybór drogi, skrótów, noclegów, postojów itd., pokazujesz co potrafisz. W ciągu każdych trzech dni podróży przebywacie drogę, jaka normalnie zajęłaby cztery dni. Czyli co trzy dni możecie sobie posiedzieć nad rzeczką, łowiąc rybki, a i tak dotrzecie na czas.",
                            "Wybierz jedną zaznaczoną na mapie trasę o długości do 1000km (wynotuj, jakie łączy ona miasta). Tę wybraną trasę znasz jak własną kieszeń, wiesz co się znajduje na 6km w każdą stronę od niej. Na tej trasie nigdy się nie zgubisz, znasz tu osady, kryjówki, skróty i wiesz, do jakich miast prowadzą odchodzące od niej drogi." }) },
                    {"Łowca", new ProfessionTemplate(
                        new int[]{ 1, 3, 2, 1, 2 },
                        new int[]{ 1, 2, 1, 1, 2, 4, 4, 2, 2, 1, 1, 2, 2, 1, 1, 1, 4, 1, 2 },
                        new string[]{ "Znajomość ruin" },
                        new string[]{ "Fotograficzna pamięć", "Nieugięty", "Kto szuka ten znajdzie", "Zdobywanie informacji" },
                        new string[]{
                            "Jesteś jak aparat cyfrowy podłączony do 40 gigowego twardziela. Masz wielką bibliotekę obrazów, wystarczy, że wejdziesz do pomieszczenia, by jego obraz utkwił na zawsze w twojej pamięci i byś potem mógł go w spokoju trawić i oglądać, dokładnie tak, jak gdyby ktoś zrobić tam zdjęcie. W czasie gdy masz prawo do wykonania Trudnego testu Percepcji za każdym razem, kiedy chcesz sobie przypomnieć wygląd jakiegoś pomieszczenia. Zdaj test, a Mistrz Gry wyśwpiewa wszystko jak na spowiedzi. Choćby minął miesiąc. Ba! Choćby minął rok! Albo i dwa lata.",
                            "Kiedy masz przed nosem swój cel, przestaje się liczyć cokolwiek innego. Przesz do przodu jak czołg, niepomny na rany, zmęczenie i strach. Jesteś łowcą, złamany obojczyk, stado mutantów, czy trzecia nieprzespana noc nie mogą ci przeszkodzić w dopadnięciu zwierzyny. Zwiększasz swój Charakter i Budowę o 2 punkty podczas wszystkich testów, kiedy niepowodzenie testu oddaliłoby cię od dopadnięcia celu.",
                            "Stanowilibyście świetny tandem z Handlarzem. Ty znajdziesz każdy towar, on załatwi lepszą cenę. Dodajesz +10% do Dostępności towarów, których szuksz, z wyjątkiem tych o dostępności 0%. Cecha dotyczy szukania towaru w okolicy, a nie w konkretnym sklepie.",
                            "Dobry Łowca to takim który zawsze, wszędzie i od każdego dowie się tego, co zechce, Po nitce do kłębka – to dewiza najlepszych. Zawsze, gdy wypytujesz o informacje, zwiększasz swoją Perswazję o 4." }) },
                    {"Łowca mutantów", new ProfessionTemplate(
                        new int[]{ 3, 1, 2, 2, 3 },
                        new int[]{ 4, 1, 1, 4, 3, 2, 2, 1, 1, 2, 2, 2, 1, 1, 1, 2, 2, 1, 0 },
                        new string[]{ },
                        new string[]{ "Bez tajemnic", "Mutant na śniadanie", "Klucz do Teksasu", "Szczcepionka" },
                        new string[]{
                            "Znasz się na mutantach, nie da się zaprzeczyć. Gdy zdasz Trudny test Sprytu, MG musi zdradzić ci słabe punkty napotkanego mutanta. MG może także podać ci niektóre Współczynniki mutanta.",
                            "Co prawda, pacyfistą w ogóle nazwać cię nie można, to jednak twoja agresja najczęściej dotyczy mutantów. Załatwiłeś już tony tego tałatajstwa i ciągle nie masz dość. Za to doświadczenie w walce z wszelkimi odmieńcami ciągle rośnie. Otrzymujesz bonus -20% do walki z mutantami (testy Charakteru, Inicjatywy, ataku, obrony).",
                            "Nietolerancyjni ludzie czują do siebie pewną słabość. Zawsze fajnie jest wspólnie ponarzekać na cały brud świata i kląć na tych, którzy winni są wszelkiemu nieszczęściu. Dlatego Teksańczycy lubią Łowców mutantów. To swoje chłopy, dobrze czynią i niech im Bóg przyświeca kagankiem. Jako Łowca mutantów otrzymujesz bonus +4 do Charakteru w kontaktach z Teksańczykami. A tak poza tym, w tak po prostu cię lubią i już.",
                            "Wieloletnie przepychanki z mutantami zszargały ci zdrowie, ale jednak żyjesz do dziś, a to znaczy, żeś twardziel i byle radioaktywne splunięcie czy zmutowany świerzb cię nie załatwi. Posiadasz większą odporność na jady mutantów i choroby roznoszone przez mutki. Świństwa te działają na ciebie zawsze z mniejszą siłą, tak jakbyś automatycznie zdał test Kondycji." }) },
                    {"Mafiozo", new ProfessionTemplate(
                        new int[]{ 3, 1, 4, 2, 3 },
                        new int[]{ 2, 2, 2, 3, 1, 1, 2, 1, 1, 4, 4, 3, 1, 1, 1, 1, 1, 1, 0 },
                        new string[]{ },
                        new string[]{ "Bezlitosny", "Klasa", "Twardy", "Test lojalności" },
                        new string[]{
                            "Z ludzim którzy cię zawiedli żyje tylko Marc. Twoi ludzie właśnie montują mu betonowe obuwie. To już jego ostatnie minuty. Z ludzi, którzy stanęli na twej drodze, do wczoraj żył Długi Johny i Szybki Paul. Dziś już nie masz wrogów. Z ludzi, którzy... Zaczynasz z jednym Punktem Sławy (złej). Reoutacja kosztuje cię tylko 20PD i zawsze jest to zła sława.",
                            "Trzeba mieć klasę. Nie można strzelić człowiekowi w głowę, mając na nogach ubłocone buty. Czy ty chciałbyś, by ktoś stuknął cię rozklekotanym, nieumytym Pontiakiem? Trzzeba mieć poziom. Trzeba szanować klientów i wrogów. Szczególnie dziś, w tych podłych czasach. Trudno nie uwierzyć facetowi takiemu jak ty. Facetowi z klasą... Możesz przerzucić każdy test Perswazji i Zastraszania.",
                            "Nie ważne, co tak naprawdę potrafisz. Ważne, czy w krytycznej sytuacji zachowujesz się jak twardziej, czy narobisz w majtki. Na szczęście ty jesteś twardy. Nie można cię w żaden sposób zastraszyć, a testy psychologiczne (pakiety Negocjacje, Empatia, Siła woli) są przeciw tobie trudniejsze o poziom.",
                            "Jesteś szefem, a jeśli nawet nie, to na  pewno nim szybko zostaniesz. Ludzie cię słuchają, ufają ci i dwa razy się zastanowią, zanim cię zdradzą. Gdy werbujesz ludzi do własnej szajki, otrzymujesz za darmo 3 punkty Powiązań z każdym z nich (patrz Bohater2: Ludzie, których potrzebujesz)." }) },
                    {"Medyk", new ProfessionTemplate(
                        new int[]{ 1, 1, 2, 3, 1 },
                        new int[]{ 2, 2, 1, 2, 2, 1, 1, 1, 1, 1, 1, 2, 4, 1, 1, 1, 2, 1, 4 },
                        new string[]{ "Biologia", "Chemia", "Psychiatria", "Chirurgia" },
                        new string[]{ "Reputacja", "Sanitariusz polowy", "Specjalista", "Gwarancja bezpieczeństwa" },
                        new string[]{
                            "Gdy wjeżdżacie do jakiejkowliek osady, najlepiej zacząć gadkę od proste stwierdzenia. \"Widzicie tamtego faceta? To lekarz.\" Nie trzeba nic więcej. Wystarczy, że zdasz Trudny test Perswazji, a miejscowi zaczną się wokół was uwijać, jakby co najmniej przyjechali z dawna oczekiwani gości. Jeśli w osadzie jest już lekarz, musisz zdać Bardzo Trudny test Perswazji.",
                            "Eksplocje, świszczące kule, krzyki i bieganina? To dla ciebie chleb powszedni. Że trochę trzęsie? Żaden problem. Że za ciemno? Chrzanienie. Ignorujesz wszelkie utrudnienia do testów leczenia, wynikające z warunków panujących w otoczeniu.",
                            "Gdy przyjdzie do ciebie ktoś ze złamaniem albo oparzeniem, musisz sobie porządnie glonąć whisky, żeby wymyślić, co z tym fantem zrobić. Ale jeśli zjawi się koleś z raną postrzałową, nawet nie muszą cię budzić, opatrzysz to przez sen. Wybierz jedną ze specjalizacji: choroby, zatrucia, rany cięte i szarpane, potrzały. Otrzymujesz dla niej bonus -20% w testach leczenia.",
                            "Czyste rączki, gumowe ręczkawiczki i tak dalej. Można się śmiać, ale fakty są takie, że nigdy nikomu nie spaprałeś rany. Nawet jeśli nie zdasz testu Pierwszej pomocy lub Leczenia ran, nie spowodujesz przez to zwiększenia się kary od rany." }) },
                    {"Monter", new ProfessionTemplate(
                        new int[]{ 1, 1, 1, 2, 1 },
                        new int[]{ 1, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 1, 3, 3, 1, 1, 0 },
                        new string[]{ },
                        new string[]{ "Zapałki i agrafka", "Kłopotliwy nadmiar", "Raz, dwa, gotowe", "Cały świat jest drucikiem" },
                        new string[]{
                            "Na agregat czekałeś dwa miesiące, a oni przywieźli ci frezarkę z fabryki łodzi podwodnych. Nawet nie mrugnąłeś. Od tygodnia wszystko gra i buczy, a ty masz pewność, że jeśli wywalą cię, albo nie dadzą premii, nikt prócz ciebie nie będzie wiedziałm jak funkcjonuje ta walcownia blachy. Po prostu masz talent do scalania najróżniejszych irządzeń w jeden mechanizm. Dostajesz bonus +2 do Sprytu podczas testów techniczcnych (gdy montujesz, naprawiasz lub przerabiasz urządzenie mechaniczne lub elektroniczne). Zawsze? Nie, tylko wtedy, gdy coś montujesz... No tak, czyli prawie zawsze.",
                            "Kiedy byłeś mały, znalazłeś budzik. Rozebrałeś go i złożyłeś na nowo, a skurczybyk dalej chodził. Tobie została garść części. Potrafisz powtórzyć ten numer niemal na każdej maszynie, wystarczy tylko, że zdasz Trudny test Mechaniki lub Elektroniki. Zostaje ci garść części o wartości 20% całego urządzenia.",
                            "Z reguły, gdy zaczniesz poganiać mechanika, to jedynie go wkurzysz i albo spieprzy to co robił, albo zdzieli cię kluczem francuskim przez łeb. Jeden na stu rzeczywiście przyspieszy i zrobi wszystko jak trzeba. Jesteś takim właśnie wyjątkiem. Możesz spróbować naprawić (zmontować itp.) daną rzecz dwa razy szybciej (czyli w połowie wymaganego czasu), ale za to Suwak w tym teście wędruje o poziom w górę.",
                            "A gdy ciebie ktoś pogania, nie musisz go nawet walić po łbie kluczcem francuskim. Po prost olewasz go, jak i wszystko inne, co się wokół dzieje. Pożary, wybuchy, świszczące kule, beczące kozy i tajfuny, wszystko to jest tylko odległym tłem. Dla ciebie cały świat jest drucikiem, który akurat skręcasz. Naprawiając lub montując cokolwiek, nie otrzymujesz kary za przeszkadzające w pracy warunki." }) },
                    {"Najemnik", new ProfessionTemplate(
                        new int[]{ 4, 2, 3, 1, 3 },
                        new int[]{ 4, 2, 1, 4, 2, 2, 3, 1, 1, 1, 1, 2, 1, 1, 1, 1, 2, 1, 0 },
                        new string[]{ },
                        new string[]{ "Reputacja", "Maszyna do zabijania", "Czwarty bieg", "Swoje przeszedłem" },
                        new string[]{
                            "Nie nawalasz. Nie wystawiasz klienta do waitru. Nie wycofujesz się ze zlece. Dlatego właśnie możesz zażądać więcej od dowolnego innego jełopa, a pracodawca i tak wybierze ciebie. Zwiększ o 15 punktów Reputację – już na starcie jesteś kimś uznanym w tej branży. Ponadto Punkt Reputacji kosztuje cię tylko 20PD.",
                            "Mówisz sobie: \"teraz albo nigdy!\" i wchodzisz w tryb maszyny do zabijania. Najemnik może tymczasowo zwiększyć o 2 punkty wartość dowolnego Współczynnika, ale bonus ten wykorzystać może tylko w walce. Ma to swoją ceną: po upływie godziny lub po wykonaniu 10 testów tego Współczynnika, bonus znika, zaś Najemnij otrzymuje karę +50% ze zmęczenia (zmniejszaną o 10% co godzinę). Cechy tej można użyć dopiero, gdy postać wypocznie (czyli całkowicie zniknie kara ze zmęczenia).",
                            "Każde dziecko wie, że trudniej trafić w ruchomy cel, niż w nieruchomą tarczę. Dlatego podczas strzelanin kolesie pod obstrzałem śmigają ile sił w nogach, klucząc przy tym, jak się da. Świetna technika, tylko coś za coś: o celnym strzelaniu w biegu można zapomnieć. He, he, to znaczy większość frajerów może zapomnieć, bo ciebie to nie dotyczy. Gdy biegniesz, nie otrzymujesz kary (+30%) do testów strzelania. Zaś gdy pędzisz na złamanie karku, możesz strzelać, otrzymujesz tylko +30% kary.",
                            "Koleś, który potrafi jedynie plewić grządki albo zbudować radio z rupieci, nie zostaje najemnikiem. Możeszy być dobrym znajperem albo umięśnionym szturmowcem, czy nawet szurniętym saperem, ale podstawy tego fachu znać musić." }) },
                    {"Ochroniarz", new ProfessionTemplate(
                        new int[]{ 4, 3, 2, 1, 3 },
                        new int[]{ 3, 1, 1, 3, 1, 1, 4, 1, 1, 2, 1, 4, 2, 1, 1, 1, 2, 1, 0 },
                        new string[]{ },
                        new string[]{ "", "", "", "" },
                        new string[]{ "", "", "", "" }) },
                    {"Sędzia", new ProfessionTemplate(
                        new int[]{ 2, 1, 3, 1, 2 },
                        new int[]{ 2, 2, 1, 3, 1, 1, 2, 1, 1, 3, 4, 3, 2, 1, 1, 1, 2, 2, 0 },
                        new string[]{ },
                        new string[]{ "", "", "", "" },
                        new string[]{ "", "", "", "" }) },
                    {"Spec", new ProfessionTemplate(
                        new int[]{ 1, 1, 1, 2, 1 },
                        new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 1, 2, 3, 1, 2, 1, 1, 4 },
                        new string[]{ "Chemia", "Biologia", "Fizyka", "Historia", "Geografia", "Chirurgia", "Matematyka", "Psychologia" },
                        new string[]{ "", "", "", "" },
                        new string[]{ "", "", "", "" }) },
                    {"Szaman", new ProfessionTemplate(
                        new int[]{ 1, 1, 1, 1, 1 },
                        new int[]{ 3, 1, 1, 2, 2, 2, 2, 2, 2, 1, 3, 2, 1, 1, 1, 1, 3, 1, 0 },
                        new string[]{ },
                        new string[]{ "", "", "", "" },
                        new string[]{ "", "", "", "" }) },
                    {"Szczur", new ProfessionTemplate(
                        new int[]{ 1, 1, 1, 1, 1 },
                        new int[]{ 1, 1, 1, 1, 1, 1, 3, 3, 2, 1, 1, 2, 1, 1, 1, 1, 3, 1, 0 },
                        new string[]{ },
                        new string[]{ "", "", "", "" },
                        new string[]{ "", "", "", "" }) },
                    {"Treser Bestii", new ProfessionTemplate(
                        new int[]{ 1, 1, 2, 1, 1 },
                        new int[]{ 2, 1, 1, 2, 2, 1, 1, 1, 1, 3, 4, 2, 2, 1, 1, 1, 2, 3, 0 },
                        new string[]{ },
                        new string[]{ "", "", "", "" },
                        new string[]{ "", "", "", "" }) },
                    {"Tropiciel", new ProfessionTemplate(
                        new int[]{ 1, 2, 1, 1, 1 },
                        new int[]{ 2, 1, 1, 2, 3, 4, 4, 2, 3, 1, 1, 2, 2, 1, 1, 1, 4, 1, 0 },
                        new string[]{ },
                        new string[]{ "", "", "", "" },
                        new string[]{ "", "", "", "" }) },
                    {"Wojownik Autostrady", new ProfessionTemplate(
                        new int[]{ 3, 2, 2, 1, 1 },
                        new int[]{ 3, 4, 1, 4, 1, 1, 2, 1, 1, 1, 1, 2, 1, 2, 2, 1, 1, 1, 0 },
                        new string[]{ },
                        new string[]{ "", "", "", "" },
                        new string[]{ "", "", "", "" }) },
                    {"Wojownik Klanu", new ProfessionTemplate(
                        new int[]{ 1, 2, 3, 1, 1 },
                        new int[]{ 4, 1, 1, 1, 4, 3, 2, 2, 3, 1, 2, 2, 1, 1, 1, 1, 3, 2, 0  },
                        new string[]{ },
                        new string[]{ "", "", "", "" },
                        new string[]{ "", "", "", "" }) },
                    {"Zabójca", new ProfessionTemplate(
                        new int[]{ 4, 3, 2, 1, 2 },
                        new int[]{ 4, 2, 1, 4, 3, 2, 2, 3, 1, 1, 1, 2, 1, 1, 1, 1, 2, 1, 0 },
                        new string[]{ },
                        new string[]{ "", "", "", "" },
                        new string[]{ "", "", "", "" }) },
                    {"Zabójca Maszyn", new ProfessionTemplate(
                        new int[]{ 2, 1, 1, 1, 2 },
                        new int[]{ 3, 1, 1, 4, 1, 1, 3, 1, 1, 1, 1, 3, 1, 4, 2, 2, 2, 1, 0 },
                        new string[]{ },
                        new string[]{ "", "", "", "" },
                        new string[]{ "", "", "", "" }) },
                    {"Złodziej", new ProfessionTemplate(
                        new int[]{ 4, 3, 2, 1, 1 },
                        new int[]{ 3, 1, 4, 2, 2, 2, 3, 4, 1, 1, 2, 1, 1, 1, 1, 1, 2, 1, 0 },
                        new string[]{ },
                        new string[]{ "", "", "", "" },
                        new string[]{ "", "", "", "" }) },
                    {"Żołnierz", new ProfessionTemplate(
                        new int[]{ 4, 2, 3, 1, 4 },
                        new int[]{ 4, 2, 1, 4, 1, 1, 3, 1, 1, 1, 1, 3, 2, 2, 2, 2, 2, 1, 0 },
                        new string[]{ },
                        new string[]{ "", "", "", "" },
                        new string[]{ "", "", "", "" }) },
                };
        }

        private class ProfessionTemplate
        {
            public int[] StatsPriority { get; }
            public int[] SkillsPriority { get; }
            public string[] SpecialSkills { get; }
            public string[] Traits { get; }
            public string[] TraitDescriptions { get; }

            public ProfessionTemplate(int[] statsPriority, int[] skillsPriority, string[] specialSkills, string[] traits, string[] traitDescriptions)
            {
                StatsPriority = statsPriority;
                SkillsPriority = skillsPriority;
                SpecialSkills = specialSkills;
                Traits = traits;
                TraitDescriptions = traitDescriptions;
            }
        }
    }
}