using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NeuroshimaDB.Models
{
    [Serializable]
    public class Location
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool HasDifficulty { get; set; }
        public int DifficultyLevel { get; set; }
        public int Modifier { get; set; }
        public int Size { get; set; }
        public string Danger { get; set; }
        public string Articles1 { get; set; }
        public string Articles2 { get; set; }
        public string Articles3 { get; set; }
        public string Articles4 { get; set; }

        public Location()
        {
        }

        public Location(int gambleType, int difficultyType, int size)
        {
            Id = Guid.NewGuid().ToString();
            Dice dice = new();
            var roll1 = dice.Roll;
            var difficultyMax = difficultyType switch
            {
                0 => 5,
                _ => 10
            };
            HasDifficulty = roll1 <= difficultyMax ?
                true :
                false;
            DifficultyLevel = HasDifficulty ?
                roll1 :
                0;
            var roll2 = dice.Roll;
            var gambleModifier = gambleType switch
            {
                0 => 2,
                1 => 1,
                _ => 0
            };
            Modifier = roll2 <= DifficultyLevel ?
                2 * gambleModifier :
                1 * gambleModifier;
            var roll3 = dice.Roll;
            var result = size switch
            {
                0 => RandomizeBigLocation(roll3),
                1 => RandomizeSmallLocation(roll3),
                _ => RandomizeHomeLocation(roll3)
            };

            Name = result;
            Size = size;
            RandomizeArticles();
            RandomizeDanger();
        }

        /// <summary>
        /// Use when you want to randomize location bound articles again.
        /// </summary>
        /// <param name="oldLocation"></param>
        /// <returns>A new Location based on the same properties.</returns>
        public static async Task<Location> RandomizeLocationAgain(Location oldLocation)
        {
            string json = JsonConvert.SerializeObject(oldLocation);
            Location newLocation = JsonConvert.DeserializeObject<Location>(json);

            newLocation.RandomizeArticles();
            newLocation.RandomizeDanger();
            return await Task.FromResult(newLocation);
        }

        private string RandomizeBigLocation(int roll)
        {
            var result = roll switch
            {
                1 => "Centrum administracyjne, sąd",
                2 => "Centrum administracyjne, sąd",
                3 => "Dworzec kolejowy, metro",
                4 => "Hala sportowa",
                5 => "Hipermarket",
                6 => "Hipermarket",
                7 => "Muzeum",
                8 => "Park rozrywki",
                9 => "Parkingi i garaże",
                10 => "Parkingi i garaże",
                11 => "Mały obiekt przemysłowy",
                12 => "Mały obiekt przemysłowy",
                13 => "Większy punkt widokowy",
                14 => "Większy punkt widokowy",
                15 => "Stadion",
                16 => "Szpital",
                17 => "Teatr, opera",
                18 => "Uniwersytet",
                19 => "Więzienie",
                _ => "Zoo"
            };
            return result;
        }

        private string RandomizeSmallLocation(int roll)
        {
            var result = roll switch
            {
                1 => "Bank",
                2 => "Biurowiec",
                3 => "Duży sklep",
                4 => "Duży sklep",
                5 => "Kanały",
                6 => "Mały sklep",
                7 => "Mały sklep",
                8 => "Mały sklep",
                9 => "Mniejszy punkt widokowy",
                10 => "Restauracja, bar, pub, kawiarnia, fastfood",
                11 => "Restauracja, bar, pub, kawiarnia, fastfood",
                12 => "Restauracja, bar, pub, kawiarnia, fastfood",
                13 => "Salon, komis samochodowy, złomowisko",
                14 => "Stacja benzynowa",
                15 => "Szkoła",
                16 => "Urząd, poczta",
                17 => "Warsztat",
                18 => "Zakład, manufaktura",
                19 => RandomizeRareLocation(),
                _ => RandomizeRareLocation()
            };
            return result;
        }

        private string RandomizeRareLocation()
        {
            var roll = new Dice().Roll;
            var result = roll switch
            {
                1 => ("Biblioteka"),
                2 => ("Biblioteka"),
                3 => ("Hotel"),
                4 => ("Hotel"),
                5 => ("Kaplica"),
                6 => ("Kaplica"),
                7 => ("Kasyno, klub"),
                8 => ("Kasyno, klub"),
                9 => ("Kino"),
                10 => ("Kino"),
                11 => ("Komisariat, areszt"),
                12 => ("Komisariat, areszt"),
                13 => ("Kościół"),
                14 => ("Kościół"),
                15 => ("Park, cmentarz"),
                16 => ("Park, cmentarz"),
                17 => ("Przychodnia, gabinety lekarskie"),
                18 => ("Przychodnia, gabinety lekarskie"),
                19 => ("Targ"),
                _ => ("Targ")
            };
            return result;
        }

        private string RandomizeHomeLocation(int roll)
        {
            var result = roll switch
            {
                1 => ("Mieszkanie"),
                2 => ("Mieszkanie"),
                3 => ("Mieszkanie"),
                4 => ("Mieszkanie"),
                5 => ("Mieszkanie"),
                6 => ("Mieszkanie"),
                7 => ("Mieszkanie"),
                8 => ("Mieszkanie"),
                9 => ("Mieszkanie"),
                10 => ("Mieszkanie"),
                11 => ("Mieszkanie"),
                12 => ("Mieszkanie"),
                13 => ("Mieszkanie"),
                14 => ("Mieszkanie"),
                15 => ("Mieszkanie"),
                16 => ("Apartament"),
                17 => ("Apartament"),
                18 => ("Dom"),
                19 => ("Dom"),
                _ => ("Dom")
            };
            return result;
        }

        // Tu funkcja do przycisku
        private void RandomizeArticles()
        {
            string articles1 = "";
            for (int i = 0; i < Modifier; i++)
                articles1 += $"{SelectGenerator()}";
            Articles1 = articles1 == "" ?
                "Nic tu nie ma." :
                articles1;

            string articles2 = "";
            for (int i = 0; i < Modifier; i++)
                articles2 += $"{SelectGenerator()}";
            Articles2 = articles2 == "" ?
                "Nic tu nie ma." :
                articles2;

            string articles3 = "";
            for (int i = 0; i < Modifier; i++)
                articles3 += $"{SelectGenerator()}";
            Articles3 = articles3 == "" ?
                "Nic tu nie ma." :
                articles3;

            string articles4 = "";
            Dice dice = new();
            if (dice.Roll == 20)
                for (int i = 0; i < 3; i++)
                    articles4 += $"{SelectGenerator()}";
            Articles4 = articles4 == "" ?
                "Nie występuje." :
                articles4;

            //string articles = $"Poziom ukrycia 1:\n{articles1}\n\nPoziom ukrycia 2:\n{articles2}\n\nPoziom ukrycia 3:\n{articles3}\n\nGruzowisko: {articles4}";
        }

        private void RandomizeDanger()
        {
            if (new Dice().Roll > 17)
                Danger = new Dice(50).Roll switch
                {
                    1 => "Gracze spadają – fragment dachu",
                    2 => "Gracze spadają – schody",
                    3 => "Gracze spadają – drabinka",
                    4 => "Gracze spadają – niepewny grunt",
                    5 => "Gracze spadają – osuwisko",
                    6 => "Coś spada na głowę – strop",
                    7 => "Coś spada na głowę – belka stropowa",
                    8 => "Coś spada na głowę – konstrukcja metalowa",
                    9 => "Coś spada na głowę – szyba",
                    10 => "Coś spada na głowę – drzewo",
                    11 => "Gracze się zatrzasnęli – klapa od piwnicy",
                    12 => "Gracze się zatrzasnęli – zakratowane drzwi w fabryce",
                    13 => "Gracze się zatrzasnęli – winda",
                    14 => "Gracze się zatrzasnęli – krata pułapka w korytarzu",
                    15 => "Gracze się zatrzasnęli – osuwisko",
                    16 => "Gracze wdepnęli – woda",
                    17 => "Gracze wdepnęli – paliwo",
                    18 => "Gracze wdepnęli – trutka",
                    19 => "Gracze wdepnęli – kwas",
                    20 => "Gracze wdepnęli – pułapka",
                    21 => "Gracze uruchomili zabezpieczenia – zabezpieczenia na drabinkach i schodach",
                    22 => "Gracze uruchomili zabezpieczenia – linka w korytarzu",
                    23 => "Gracze uruchomili zabezpieczenia – przycisk do urządzenia mechanicznego",
                    24 => "Gracze uruchomili zabezpieczenia – kusza w pokoju",
                    25 => "Gracze uruchomili zabezpieczenia – fałszywa bomba",
                    26 => "Gracze się zgubili – kiedy nie mają żarcia",
                    27 => "Gracze się zgubili – kiedy są ranni",
                    28 => "Gracze się zgubili – kiedy są ścigani",
                    29 => "Gracze się zgubili – kiedy włączyły się zabezpieczenia",
                    30 => "Gracze się zgubili – kiedy coś się wali",
                    31 => "Gracze się zatruli – niewidzialny gaz",
                    32 => "Gracze się zatruli – toksyczne wyziewy",
                    33 => "Gracze się zatruli – zatruta woda",
                    34 => "Gracze się zatruli – jadowite paskudztwo",
                    35 => "Gracze się zatruli – trujące pyłki",
                    36 => "Wąskie gardło – most",
                    37 => "Wąskie gardło – klatka schodowa",
                    38 => "Wąskie gardło – konstrukcja dachu",
                    39 => "Wąskie gardło – pajęczyna",
                    40 => "Wąskie gardło – kable",
                    41 => "Spotkanie – maszyny",
                    42 => "Spotkanie – gang",
                    43 => "Spotkanie – mutanci",
                    44 => "Spotkanie – bestia",
                    45 => "Spotkanie – techmorwa",
                    46 => "Inne – szyb wentylacyjny",
                    47 => "Inne – maszyna",
                    48 => "Inne – podziemny świat",
                    49 => "Inne – pożar",
                    _ => "Inne – powódź"
                };
            else Danger = "Brak niebezpieczeństw (przynajmniej tych z podręcznika).";
        }

        // Wybieramy generator dla konkretnej lokacji

        #region lokacje

        private string SelectGenerator()
        {
            return Name switch
            {
                "Mieszkanie" => Mieszkanie(),
                "Apartament" => Apartament(),
                "Dom" => Dom(),
                "Bank" => Bank(),
                "Biurowiec" => Biurowiec(),
                "Duży sklep" => DSklep(),
                "Kanały" => Kanaly(),
                "Mały sklep" => MSklep(),
                "Mniejszy punkt widokowy" => MPWidokowy(),
                "Restauracja, bar, pub, kawiarnia, fastfood" => Restauracja(),
                "Salon, komis samochodowy, złomowisko" => Komis(),
                "Stacja benzynowa" => Stacja(),
                "Szkoła" => Szkola(),
                "Urząd, poczta" => Urzad(),
                "Warsztat" => Warsztat(),
                "Zakład, manufaktura" => Manufaktura(),
                "Centrum administracyjne, sąd" => Sad(),
                "Dworzec kolejowy, metro" => Dworzec(),
                "Hala sportowa" => Hala(),
                "Hipermarket" => Hipermarket(),
                "Muzeum" => Muzeum(),
                "Park rozrywki" => PRozrywki(),
                "Parkingi i garaże" => Parkingi(),
                "Mały obiekt przemysłowy" => MPrzemyslowy(),
                "Większy punkt widokowy" => WPWidokowy(),
                "Stadion" => Stadion(),
                "Szpital" => Szpital(),
                "Teatr, opera" => Teatr(),
                "Uniwersytet" => Uniwersytet(),
                "Więzienie" => Wiezienie(),
                "Zoo" => Zoo(),
                "Biblioteka" => Biblioteka(),
                "Hotel" => Hotel(),
                "Kaplica" => Kaplica(),
                "Kasyno, klub" => Kasyno(),
                "Kino" => Kino(),
                "Komisariat, areszt" => Komisariat(),
                "Kościół" => Kosciol(),
                "Park, cmentarz" => Park(),
                "Przychodnia, gabinety lekarskie" => Przychodnia(),
                "Targ" => Targ(),
                _ => Mieszkanie()
            };
        }

        private string Targ()
        {
            string output = "";
            for (int i = 0; i < 6; i++)
            {
                Dice dice = new(20);
                output += dice.Roll switch
                {
                    1 => TowarZeSklepiku(),
                    2 => TowarZeSklepiku(),
                    3 => TowarZeSklepiku(),
                    4 => TowarZeSklepiku(),
                    5 => TowarZeSklepiku(),
                    6 => TowarZeStraganu(),
                    7 => TowarZeStraganu(),
                    8 => TowarZeStraganu(),
                    9 => TowarZeStraganu(),
                    10 => ZywnoscIUzywki(),
                    11 => ZywnoscIUzywki(),
                    12 => ZywnoscIUzywki(),
                    13 => ZywnoscIUzywki(),
                    14 => ZywnoscIUzywki(),
                    15 => Zakupy(),
                    16 => Zakupy(),
                    17 => Zwloki(),
                    18 => TowarZeStraganu(),
                    19 => Zarowka(),
                    20 => NiespodziankaZWraku(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Przychodnia()
        {
            string output = "";
            for (int i = 0; i < 7; i++)
            {
                Dice dice = new(17);
                output += dice.Roll switch
                {
                    1 => SprzetMedyczny(),
                    2 => AkcesoriaDoSprzatania(),
                    3 => BiurowyMebel(),
                    4 => BiurowyDrobiazg(),
                    5 => Medykamenty(),
                    6 => SprzetMedyczny(),
                    7 => AkcesoriaToaletowePubliczne(),
                    8 => BiurowySprzetKomputerowy(),
                    9 => Medykamenty(),
                    10 => SprzetMedyczny(),
                    11 => ArchiwumDanych(),
                    12 => ArchiwumDanych(),
                    13 => ArtykulPapierniczy(),
                    14 => ArtykulPapierniczy(),
                    15 => BiurowyMebel(),
                    16 => Zarowka(),
                    17 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Park()
        {
            string output = "";
            for (int i = 0; i < 11; i++)
            {
                Dice dice = new(13);
                output += dice.Roll switch
                {
                    1 => OzdobaArchitektoniczna(),
                    2 => OzdobaArchitektoniczna(),
                    3 => AkcesoriaToaletowePubliczne(),
                    4 => AkcesoriaToaletowePubliczne(),
                    5 => OzdobaArchitektoniczna(),
                    6 => OzdobaArchitektoniczna(),
                    7 => OzdobaArchitektoniczna(),
                    8 => OzdobaArchitektoniczna(),
                    9 => OzdobaArchitektoniczna(),
                    10 => AkcesoriaRemontowe(),
                    11 => AkcesoriaRemontowe(),
                    12 => Zwloki(),
                    13 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Kosciol()
        {
            string output = "";
            for (int i = 0; i < 14; i++)
            {
                Dice dice = new(20);
                output += dice.Roll switch
                {
                    1 => DzieloSztuki(),
                    2 => DzieloSztuki(),
                    3 => OzdobaArchitektoniczna(),
                    4 => OzdobaArchitektoniczna(),
                    5 => AkcesoriaDoSprzatania(),
                    6 => DzieloSztuki(),
                    7 => BiurowyDrobiazg(),
                    8 => DomowaKolekcjaSztuki(),
                    9 => AparaturaRadiowezla(),
                    10 => DomowaKolekcjaSztuki(),
                    11 => DzieloSztuki(),
                    12 => DzieloSztuki(),
                    13 => OzdobaArchitektoniczna(),
                    14 => OzdobaArchitektoniczna(),
                    15 => OzdobneAkcesoriaKoscielne(),
                    16 => OzdobneAkcesoriaKoscielne(),
                    17 => Zarowka(),
                    18 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Komisariat()
        {
            string output = "";
            for (int i = 0; i < 9; i++)
            {
                Dice dice = new(20);
                output += dice.Roll switch
                {
                    1 => BiurowySprzetKomputerowy(),
                    2 => ZabezpieczenieElektroniczne(),
                    3 => EkwipunekPolicyjny(),
                    4 => NiespodziankaZWraku(),
                    5 => SprzetGasniczy(),
                    6 => ArchiwumDanych(),
                    7 => ArtykulPapierniczy(),
                    8 => BiurowyMebel(),
                    9 => BiurowyDrobiazg(),
                    10 => BiurowaKuchnia(),
                    11 => EkwipunekPolicyjny(),
                    12 => ZabezpieczenieElektroniczne(),
                    13 => Zarowka(),
                    14 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Kino()
        {
            string output = "";
            for (int i = 0; i < 6; i++)
            {
                Dice dice = new(16);
                output += dice.Roll switch
                {
                    1 => BiurowaKuchnia(),
                    2 => SprzetGasniczy(),
                    3 => AkcesoriaDoSprzatania(),
                    4 => Zwloki(),
                    5 => ZywnoscIUzywki(),
                    6 => Kasa(),
                    7 => ElektronikaWiekszySprzet(),
                    8 => ZywnoscIUzywki(),
                    9 => AkcesoriaDoSprzatania(),
                    10 => SprzetSceniczny(),
                    11 => SprzetSceniczny(),
                    12 => Ekran(),
                    13 => AGDDomOgrod(),
                    14 => BiurowyDrobiazg(),
                    15 => Zarowka(),
                    16 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Kasyno()
        {
            string output = "";
            for (int i = 0; i < 13; i++)
            {
                Dice dice = new(20);
                output += dice.Roll switch
                {
                    1 => Barek(),
                    2 => BiurowySprzetKomputerowy(),
                    3 => SprzetGasniczy(),
                    4 => AkcesoriaDoSprzatania(),
                    5 => WyposazenieDomowe(),
                    6 => Kasa(),
                    7 => WyposazenieKlubowe(),
                    8 => AkcesoriaDoSprzatania(),
                    9 => ZywnoscIUzywki(),
                    10 => BiurowyMebel(),
                    11 => OzdobaArchitektoniczna(),
                    12 => BiurowyMebel(),
                    13 => CennaKolekcja(),
                    14 => DomowaKolekcjaSztuki(),
                    15 => DomowaKolekcjaSztuki(),
                    16 => Ekran(),
                    17 => Ekran(),
                    18 => BiurowyDrobiazg(),
                    19 => BiurowyDrobiazg(),
                    20 => DomowaKolekcjaSztuki(),
                    21 => DomowaKolekcjaSztuki(),
                    22 => ZabezpieczenieElektroniczne(),
                    23 => ZabezpieczenieElektroniczne(),
                    24 => Zarowka(),
                    25 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Kaplica()
        {
            string output = "";
            for (int i = 0; i < 3; i++)
            {
                Dice dice = new(11);
                output += dice.Roll switch
                {
                    1 => DzieloSztuki(),
                    2 => OzdobneAkcesoriaKoscielne(),
                    3 => AkcesoriaDoSprzatania(),
                    4 => DzieloSztuki(),
                    5 => DzieloSztuki(),
                    6 => OzdobaArchitektoniczna(),
                    7 => OzdobaArchitektoniczna(),
                    8 => OzdobneAkcesoriaKoscielne(),
                    9 => OzdobneAkcesoriaKoscielne(),
                    10 => Zarowka(),
                    11 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Hotel()
        {
            string output = "";
            for (int i = 0; i < 9; i++)
            {
                Dice dice = new(20);
                output += dice.Roll switch
                {
                    1 => Barek(),
                    2 => AkcesoriaKuchenne(),
                    3 => AkcesoriaDoSprzatania(),
                    4 => ZSzafy(),
                    5 => UrzadzenieKuchenne(),
                    6 => BiurowySprzetKomputerowy(),
                    7 => SprzetGasniczy(),
                    8 => WyposazenieDomowe(),
                    9 => Bagaz(),
                    10 => BiurowyDrobiazg(),
                    11 => AkcesoriaToaletowe(),
                    12 => ArtykulPapierniczy(),
                    13 => BiurowaKuchnia(),
                    14 => DomowaApteczka(),
                    15 => ZywnoscIUzywki(),
                    16 => BiurowyMebel(),
                    17 => DomowaKolekcjaSztuki(),
                    18 => ZSzafy(),
                    19 => Zarowka(),
                    20 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Biblioteka()
        {
            string output = "";
            for (int i = 0; i < 8; i++)
            {
                Dice dice = new(15);
                output += dice.Roll switch
                {
                    1 => BiurowySprzetKomputerowy(),
                    2 => Ksiazka(),
                    3 => Ksiazka(),
                    4 => BiurowyDrobiazg(),
                    5 => Ksiazka(),
                    6 => Ksiazka(),
                    7 => BiurowyDrobiazg(),
                    8 => BiurowyMebel(),
                    9 => BiurowyMebel(),
                    10 => Ksiazka(),
                    11 => Ksiazka(),
                    12 => Gazeta(),
                    13 => Gazeta(),
                    14 => Zarowka(),
                    15 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Zoo()
        {
            string output = "";
            for (int i = 0; i < 11; i++)
            {
                Dice dice = new(20);
                output += dice.Roll switch
                {
                    1 => Kasa(),
                    2 => SprzetSceniczny(),
                    3 => OzdobaArchitektoniczna(),
                    4 => ZywnoscIUzywki(),
                    5 => AkcesoriaDoSprzatania(),
                    6 => Medykamenty(),
                    7 => AkcesoriaDoSprzatania(),
                    8 => AkcesoriaRemontowe(),
                    9 => SprzetSceniczny(),
                    10 => AkcesoriaRemontowe(),
                    11 => AkcesoriaDoSprzatania(),
                    12 => AkcesoriaDoSprzatania(),
                    13 => OdziezObuwie(),
                    14 => TowarZeSklepiku(),
                    15 => TowarZeSklepiku(),
                    16 => TowarZeStraganu(),
                    17 => TowarZeStraganu(),
                    18 => Zwloki(),
                    19 => Zarowka(),
                    20 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Wiezienie()
        {
            string output = "";
            for (int i = 0; i < 16; i++)
            {
                Dice dice = new(20);
                output += dice.Roll switch
                {
                    1 => AparaturaRadiowezla(),
                    2 => ZabezpieczenieElektroniczne(),
                    3 => ZabezpieczenieElektroniczne(),
                    4 => ZabezpieczenieElektroniczne(),
                    5 => DomowaApteczka(),
                    6 => EkwipunekPolicyjny(),
                    7 => AkcesoriaDoSprzatania(),
                    8 => ArchiwumDanych(),
                    9 => AparaturaRadiowezla(),
                    10 => ZabezpieczenieElektroniczne(),
                    11 => AparaturaRadiowezla(),
                    12 => BiurowySprzetKomputerowy(),
                    13 => AkcesoriaDoSprzatania(),
                    14 => AparaturaRadiowezla(),
                    15 => AkcesoriaRemontowe(),
                    16 => EkwipunekPolicyjny(),
                    17 => ArchiwumDanych(),
                    18 => ZabezpieczenieElektroniczne(),
                    19 => Zarowka(),
                    20 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Uniwersytet()
        {
            string output = "";
            for (int i = 0; i < 15; i++)
            {
                Dice dice = new(20);
                output += dice.Roll switch
                {
                    1 => AparaturaRadiowezla(),
                    2 => Naukowa(),
                    3 => Naukowa(),
                    4 => SprzetGasniczy(),
                    5 => AkcesoriaRemontowe(),
                    6 => BiurowySprzetKomputerowy(),
                    7 => AparaturaRadiowezla(),
                    8 => AkcesoriaDoSprzatania(),
                    9 => ArchiwumDanych(),
                    10 => ArtykulPapierniczy(),
                    11 => ArtykulPapierniczy(),
                    12 => BiurowyDrobiazg(),
                    13 => BiurowyDrobiazg(),
                    14 => BiurowyMebel(),
                    15 => BiurowyMebel(),
                    16 => AkcesoriaToaletowePubliczne(),
                    19 => Zarowka(),
                    20 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Teatr()
        {
            string output = "";
            for (int i = 0; i < 9; i++)
            {
                Dice dice = new(17);
                output += dice.Roll switch
                {
                    1 => OzdobaArchitektoniczna(),
                    2 => Kasa(),
                    3 => RekwizytTeatralny(),
                    4 => SprzetSceniczny(),
                    5 => TowarZeSklepiku(),
                    6 => AkcesoriaDoSprzatania(),
                    7 => RekwizytTeatralny(),
                    8 => RekwizytTeatralny(),
                    9 => RekwizytTeatralny(),
                    10 => SprzetSceniczny(),
                    11 => SprzetSceniczny(),
                    12 => AkcesoriaDoSprzatania(),
                    13 => AkcesoriaDoSprzatania(),
                    14 => SprzetGasniczy(),
                    16 => Zarowka(),
                    17 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Szpital()
        {
            string output = "";
            for (int i = 0; i < 18; i++)
            {
                Dice dice = new(20);
                output += dice.Roll switch
                {
                    1 => ZywnoscIUzywki(),
                    2 => Medykamenty(),
                    3 => Naukowa(),
                    4 => SprzetGasniczy(),
                    5 => ArchiwumDanych(),
                    6 => AkcesoriaRemontowe(),
                    7 => Zwloki(),
                    8 => TowarZeSklepiku(),
                    9 => SprzetMedyczny(),
                    10 => AkcesoriaDoSprzatania(),
                    11 => BiurowaKuchnia(),
                    12 => SprzetMedyczny(),
                    13 => SprzetMedyczny(),
                    14 => AparaturaRadiowezla(),
                    15 => BiurowyMebel(),
                    16 => AkcesoriaRemontowe(),
                    17 => ArtykulPapierniczy(),
                    18 => Bagaz(),
                    19 => SprzetMedyczny(),
                    20 => BiurowySprzetKomputerowy(),
                    21 => Medykamenty(),
                    22 => Naukowa(),
                    23 => SprzetMedyczny(),
                    24 => ArchiwumDanych(),
                    25 => AkcesoriaDoSprzatania(),
                    26 => AkcesoriaToaletowe(),
                    27 => BiurowyDrobiazg(),
                    28 => BiurowyMebel(),
                    29 => Zarowka(),
                    30 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Stadion()
        {
            string output = "";
            for (int i = 0; i < 13; i++)
            {
                Dice dice = new(20);
                output += dice.Roll switch
                {
                    1 => BiurowySprzetKomputerowy(),
                    2 => Kasa(),
                    3 => SprzetSceniczny(),
                    4 => AkcesoriaRemontowe(),
                    5 => SprzetSportowy(),
                    6 => AkcesoriaDoSprzatania(),
                    7 => BiurowyDrobiazg(),
                    8 => AkcesoriaDoSprzatania(),
                    9 => AkcesoriaDoSprzatania(),
                    10 => AparaturaRadiowezla(),
                    11 => AparaturaRadiowezla(),
                    12 => BiurowyDrobiazg(),
                    13 => BiurowyDrobiazg(),
                    14 => SprzetSportowy(),
                    15 => SprzetSportowy(),
                    16 => SprzetSportowy(),
                    19 => Zarowka(),
                    20 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string WPWidokowy()
        {
            string output = "";
            return output;
        }

        private string MPrzemyslowy()
        {
            string output = "";
            for (int i = 0; i < 13; i++)
            {
                Dice dice = new(22);
                output += dice.Roll switch
                {
                    1 => BiurowySprzetKomputerowy(),
                    2 => NarzedziaWarsztatowe(),
                    3 => AkcesoriaRemontowe(),
                    4 => SprzetGasniczy(),
                    5 => AkcesoriaToaletowePubliczne(),
                    6 => AkcesoriaDoSprzatania(),
                    7 => BiurowaKuchnia(),
                    8 => NarzedziaWarsztatowe(),
                    9 => ButleZChemikaliami(),
                    10 => NarzedziaDomowe(),
                    11 => DomowaApteczka(),
                    12 => AkcesoriaRemontowe(),
                    13 => NarzedziaDomowe(),
                    14 => NarzedziaDomowe(),
                    15 => NarzedziaDomowe(),
                    16 => OdziezObuwie(),
                    17 => OdziezObuwie(),
                    18 => ZabezpieczenieElektroniczne(),
                    19 => NarzedziaWarsztatowe(),
                    20 => NarzedziaWarsztatowe(),
                    21 => Zarowka(),
                    22 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Parkingi()
        {
            string output = "";
            for (int i = 0; i < 18; i++)
            {
                Dice dice = new(13);
                output += dice.Roll switch
                {
                    1 => ZabezpieczenieElektroniczne(),
                    2 => BiurowyDrobiazg(),
                    3 => NiespodziankaZWraku(),
                    4 => AkcesoriaRemontowe(),
                    5 => AkcesoriaRemontowe(),
                    6 => Bagaz(),
                    7 => Bagaz(),
                    8 => NiespodziankaZWraku(),
                    9 => NiespodziankaZWraku(),
                    10 => NiespodziankaZWraku(),
                    11 => Zakupy(),
                    12 => Zarowka(),
                    13 => Zwloki(),
                    14 => NiespodziankaZWraku(),
                    15 => NiespodziankaZWraku(),
                    16 => NiespodziankaZWraku(),
                    17 => NiespodziankaZWraku(),
                    18 => NiespodziankaZWraku(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string PRozrywki()
        {
            string output = "";
            for (int i = 0; i < 9; i++)
            {
                Dice dice = new(22);
                output += dice.Roll switch
                {
                    1 => Kasa(),
                    2 => AkcesoriaRemontowe(),
                    3 => OzdobaArchitektoniczna(),
                    4 => TowarZeStraganu(),
                    5 => SprzetSceniczny(),
                    6 => OzdobaArchitektoniczna(),
                    7 => TowarZeStraganu(),
                    8 => SprzetSceniczny(),
                    9 => OzdobaArchitektoniczna(),
                    10 => TowarZeSklepiku(),
                    11 => RekwizytTeatralny(),
                    12 => TowarZeSklepiku(),
                    13 => AparaturaRadiowezla(),
                    14 => AparaturaRadiowezla(),
                    15 => SprzetSceniczny(),
                    16 => SprzetSceniczny(),
                    17 => AutomatPubliczny(),
                    18 => AutomatPubliczny(),
                    19 => TowarZeStraganu(),
                    20 => TowarZeStraganu(),
                    21 => Zarowka(),
                    22 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Muzeum()
        {
            string output = "";
            for (int i = 0; i < 14; i++)
            {
                Dice dice = new(14);
                output += dice.Roll switch
                {
                    1 => BiurowyDrobiazg(),
                    2 => DzieloSztuki(),
                    3 => AkcesoriaRemontowe(),
                    4 => DzieloSztuki(),
                    5 => AkcesoriaDoSprzatania(),
                    6 => CennaKolekcja(),
                    7 => CennaKolekcja(),
                    8 => DzieloSztuki(),
                    9 => OzdobaArchitektoniczna(),
                    10 => OzdobaArchitektoniczna(),
                    11 => SprzetGasniczy(),
                    12 => ZabezpieczenieElektroniczne(),
                    13 => Zarowka(),
                    14 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Hipermarket()
        {
            string output = "";
            for (int i = 0; i < 18; i++)
            {
                Dice dice = new(20);
                output += dice.Roll switch
                {
                    1 => AutomatPubliczny(),
                    2 => AkcesoriaDoSprzatania(),
                    3 => EkwipunekPolicyjny(),
                    4 => Kasa(),
                    5 => TowarZeSklepiku(),
                    6 => OzdobaArchitektoniczna(),
                    7 => SprzetGasniczy(),
                    8 => BiurowySprzetKomputerowy(),
                    9 => AsortymentSklepow(),
                    10 => TowarZeStraganu(),
                    11 => AparaturaRadiowezla(),
                    12 => AsortymentSklepow(),
                    13 => AsortymentSklepow(),
                    14 => Ekran(),
                    15 => Kasa(),
                    16 => TowarZeSklepiku(),
                    17 => AsortymentSklepow(),
                    18 => AsortymentSklepow(),
                    19 => ZabezpieczenieElektroniczne(),
                    20 => Zakupy(),
                    21 => Zarowka(),
                    22 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Hala()
        {
            string output = "";
            for (int i = 0; i < 6; i++)
            {
                Dice dice = new(20);
                output += dice.Roll switch
                {
                    1 => BiurowySprzetKomputerowy(),
                    2 => SprzetSceniczny(),
                    3 => Ekran(),
                    4 => AkcesoriaDoSprzatania(),
                    5 => SprzetSportowy(),
                    6 => BiurowyDrobiazg(),
                    7 => SprzetSportowy(),
                    8 => AutomatPubliczny(),
                    9 => SprzetSportowy(),
                    10 => AkcesoriaDoSprzatania(),
                    11 => AparaturaRadiowezla(),
                    12 => BiurowyMebel(),
                    13 => BiurowyMebel(),
                    14 => SprzetGasniczy(),
                    15 => SprzetSportowy(),
                    16 => SprzetSportowy(),
                    17 => SprzetSportowy(),
                    18 => ZabezpieczenieElektroniczne(),
                    19 => Zarowka(),
                    20 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Dworzec()
        {
            string output = "";
            for (int i = 0; i < 18; i++)
            {
                Dice dice = new(20);
                output += dice.Roll switch
                {
                    1 => BiurowyDrobiazg(),
                    2 => Bagaz(),
                    3 => Kasa(),
                    4 => SprzetGasniczy(),
                    5 => Bagaz(),
                    6 => Bagaz(),
                    7 => TowarZeSklepiku(),
                    8 => Zwloki(),
                    9 => AkcesoriaRemontowe(),
                    10 => AutomatPubliczny(),
                    11 => BiurowySprzetKomputerowy(),
                    12 => Zakupy(),
                    13 => AparaturaRadiowezla(),
                    14 => Bagaz(),
                    15 => Bagaz(),
                    16 => Ekran(),
                    17 => ZabezpieczenieElektroniczne(),
                    18 => ZabezpieczenieElektroniczne(),
                    19 => Zarowka(),
                    20 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Sad()
        {
            string output = "";
            for (int i = 0; i < 16; i++)
            {
                Dice dice = new(24);
                output += dice.Roll switch
                {
                    1 => ArchiwumDanych(),
                    2 => BiurowaKuchnia(),
                    3 => SprzetGasniczy(),
                    4 => BiurowyMebel(),
                    5 => EkwipunekPolicyjny(),
                    6 => DomowySprzetKomputerowy(),
                    7 => AkcesoriaDoSprzatania(),
                    8 => ArchiwumDanych(),
                    9 => BiurowySprzetKomputerowy(),
                    10 => OzdobaArchitektoniczna(),
                    11 => ArchiwumDanych(),
                    12 => EkwipunekPolicyjny(),
                    13 => BiurowySprzetKomputerowy(),
                    14 => AkcesoriaDoSprzatania(),
                    15 => ArtykulPapierniczy(),
                    16 => ArtykulPapierniczy(),
                    17 => BiurowyDrobiazg(),
                    18 => BiurowyDrobiazg(),
                    19 => BiurowyDrobiazg(),
                    20 => BiurowyMebel(),
                    21 => BiurowyMebel(),
                    22 => ZabezpieczenieElektroniczne(),
                    23 => Zarowka(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Manufaktura()
        {
            string output = "";
            for (int i = 0; i < 10; i++)
            {
                Dice dice = new(18);
                output += dice.Roll switch
                {
                    1 => BiurowySprzetKomputerowy(),
                    2 => AkcesoriaDoSprzatania(),
                    3 => NarzedziaDomowe(),
                    4 => AkcesoriaToaletowePubliczne(),
                    5 => BiurowaKuchnia(),
                    6 => AkcesoriaRemontowe(),
                    7 => OdziezObuwie(),
                    8 => DomowaApteczka(),
                    9 => OdziezObuwie(),
                    10 => NarzedziaWarsztatowe(),
                    11 => NarzedziaWarsztatowe(),
                    12 => NarzedziaDomowe(),
                    13 => NarzedziaDomowe(),
                    14 => BiurowyDrobiazg(),
                    15 => ArtykulPapierniczy(),
                    16 => SprzetGasniczy(),
                    17 => Zarowka(),
                    18 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Warsztat()
        {
            string output = "";
            for (int i = 0; i < 14; i++)
            {
                Dice dice = new(15);
                output += dice.Roll switch
                {
                    1 => BiurowyDrobiazg(),
                    2 => NarzedziaWarsztatowe(),
                    3 => AkcesoriaRemontowe(),
                    4 => NarzedziaWarsztatowe(),
                    5 => NiespodziankaZWraku(),
                    6 => AkcesoriaToaletowePubliczne(),
                    7 => NarzedziaWarsztatowe(),
                    8 => NarzedziaWarsztatowe(),
                    9 => ButleZChemikaliami(),
                    10 => OdziezObuwie(),
                    11 => OdziezObuwie(),
                    12 => NarzedziaDomowe(),
                    13 => NarzedziaDomowe(),
                    14 => Zarowka(),
                    15 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Urzad()
        {
            string output = "";
            for (int i = 0; i < 12; i++)
            {
                Dice dice = new(23);
                output += dice.Roll switch
                {
                    1 => ArchiwumDanych(),
                    2 => BiurowySprzetKomputerowy(),
                    3 => Kasa(),
                    4 => AkcesoriaDoSprzatania(),
                    5 => ArchiwumDanych(),
                    6 => AkcesoriaToaletowePubliczne(),
                    7 => BiurowySprzetKomputerowy(),
                    8 => BiurowySprzetKomputerowy(),
                    9 => BiurowyMebel(),
                    10 => BiurowySprzetKomputerowy(),
                    11 => BiurowyMebel(),
                    12 => BiurowyMebel(),
                    13 => OzdobaArchitektoniczna(),
                    14 => ArchiwumDanych(),
                    15 => BiurowyMebel(),
                    16 => ArchiwumDanych(),
                    17 => AparaturaRadiowezla(),
                    18 => ArtykulPapierniczy(),
                    19 => BiurowyDrobiazg(),
                    20 => SprzetGasniczy(),
                    21 => ZabezpieczenieElektroniczne(),
                    22 => Zarowka(),
                    23 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Szkola()
        {
            string output = "";
            for (int i = 0; i < 14; i++)
            {
                Dice dice = new(18);
                output += dice.Roll switch
                {
                    1 => Podreczniki(),
                    2 => BiurowySprzetKomputerowy(),
                    3 => SprzetGasniczy(),
                    4 => BiurowyMebel(),
                    5 => AkcesoriaRemontowe(),
                    6 => Mapa(),
                    7 => AkcesoriaToaletowePubliczne(),
                    8 => AkcesoriaDoSprzatania(),
                    9 => ArtykulPapierniczy(),
                    10 => ArtykulPapierniczy(),
                    11 => Podreczniki(),
                    12 => Podreczniki(),
                    13 => BiurowyDrobiazg(),
                    14 => BiurowyDrobiazg(),
                    15 => BiurowyMebel(),
                    16 => AparaturaRadiowezla(),
                    17 => Zarowka(),
                    18 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Stacja()
        {
            string output = "";
            for (int i = 0; i < 3; i++)
            {
                Dice dice = new(20);
                output += dice.Roll switch
                {
                    1 => Kasa(),
                    2 => TowarZeSklepiku(),
                    3 => AkcesoriaToaletowePubliczne(),
                    4 => Mapa(),
                    5 => AsortymentSklepow(),
                    6 => AkcesoriaSamochodowe(),
                    7 => BiurowaKuchnia(),
                    8 => AkcesoriaDoSprzatania(),
                    9 => ArtykulPapierniczy(),
                    10 => Bagaz(),
                    11 => Mapa(),
                    12 => Mapa(),
                    13 => NiespodziankaZWraku(),
                    14 => SprzetGasniczy(),
                    15 => TowarZeSklepiku(),
                    16 => ZabezpieczenieElektroniczne(),
                    17 => Zakupy(),
                    18 => Zarowka(),
                    19 => Zwloki(),
                    20 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Komis()
        {
            string output = "";
            for (int i = 0; i < 5; i++)
            {
                Dice dice = new(16);
                output += dice.Roll switch
                {
                    1 => Kasa(),
                    2 => AkcesoriaDoSprzatania(),
                    3 => NiespodziankaZWraku(),
                    4 => BiurowySprzetKomputerowy(),
                    5 => ArtykulPapierniczy(),
                    6 => NiespodziankaZWraku(),
                    7 => AkcesoriaDoSprzatania(),
                    8 => AkcesoriaDoSprzatania(),
                    9 => BiurowyDrobiazg(),
                    10 => BiurowyDrobiazg(),
                    11 => NiespodziankaZWraku(),
                    12 => NiespodziankaZWraku(),
                    13 => ZabezpieczenieElektroniczne(),
                    14 => ZabezpieczenieElektroniczne(),
                    15 => Zarowka(),
                    16 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Restauracja()
        {
            string output = "";
            for (int i = 0; i < 7; i++)
            {
                Dice dice = new(20);
                output += dice.Roll switch
                {
                    1 => Barek(),
                    2 => UrzadzenieKuchenne(),
                    3 => AkcesoriaToaletowePubliczne(),
                    4 => GratyPiwniczne(),
                    5 => DomowaKolekcjaSztuki(),
                    6 => AkcesoriaKuchenne(),
                    7 => ZywnoscIUzywki(),
                    8 => ZywnoscIUzywki(),
                    9 => AkcesoriaDoSprzatania(),
                    10 => Ekran(),
                    11 => AkcesoriaToaletowePubliczne(),
                    12 => DomowaKolekcjaSztuki(),
                    13 => DomowaKolekcjaSztuki(),
                    14 => AkcesoriaKuchenne(),
                    15 => AkcesoriaKuchenne(),
                    16 => ZywnoscIUzywki(),
                    17 => ZywnoscIUzywki(),
                    18 => BiurowaKuchnia(),
                    19 => Zarowka(),
                    20 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string MPWidokowy()
        {
            return "";
        }

        private string MSklep()
        {
            string output = "";
            for (int i = 0; i < 5; i++)
            {
                Dice dice = new(18);
                output += dice.Roll switch
                {
                    1 => AsortymentSklepow(),
                    2 => AsortymentSklepow(),
                    3 => AkcesoriaDoSprzatania(),
                    4 => AsortymentSklepow(),
                    5 => AsortymentSklepow(),
                    6 => AsortymentSklepow(),
                    7 => BiurowaKuchnia(),
                    8 => BiurowySprzetKomputerowy(),
                    9 => Kasa(),
                    10 => Zakupy(),
                    11 => Zakupy(),
                    12 => Zarowka(),
                    13 => Zwloki(),
                    14 => AsortymentSklepow(),
                    15 => AsortymentSklepow(),
                    16 => AsortymentSklepow(),
                    17 => AsortymentSklepow(),
                    18 => AsortymentSklepow(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Kanaly()
        {
            string output = "";
            for (int i = 0; i < 14; i++)
            {
                Dice dice = new(2);
                output += dice.Roll switch
                {
                    1 => AkcesoriaRemontowe(),
                    2 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string DSklep()
        {
            string output = "";
            for (int i = 0; i < 10; i++)
            {
                Dice dice = new(15);
                output += dice.Roll switch
                {
                    1 => Kasa(),
                    2 => AsortymentSklepow(),
                    3 => AsortymentSklepow(),
                    4 => AsortymentSklepow(),
                    5 => BiurowaKuchnia(),
                    6 => AsortymentSklepow(),
                    7 => AsortymentSklepow(),
                    8 => AsortymentSklepow(),
                    9 => AsortymentSklepow(),
                    10 => AsortymentSklepow(),
                    11 => BiurowaKuchnia(),
                    12 => BiurowySprzetKomputerowy(),
                    13 => ZabezpieczenieElektroniczne(),
                    14 => Zarowka(),
                    15 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Biurowiec()
        {
            string output = "";
            for (int i = 0; i < 7; i++)
            {
                Dice dice = new(20);
                output += dice.Roll switch
                {
                    1 => BiurowySprzetKomputerowy(),
                    2 => AkcesoriaToaletowePubliczne(),
                    3 => AkcesoriaDoSprzatania(),
                    4 => AkcesoriaRemontowe(),
                    5 => DomowaKolekcjaSztuki(),
                    6 => ArchiwumDanych(),
                    7 => AutomatPubliczny(),
                    8 => SprzetGasniczy(),
                    9 => AkcesoriaDoSprzatania(),
                    10 => Barek(),
                    11 => ArtykulPapierniczy(),
                    12 => ArtykulPapierniczy(),
                    13 => BiurowaKuchnia(),
                    14 => BiurowyDrobiazg(),
                    15 => BiurowyDrobiazg(),
                    16 => BiurowyMebel(),
                    17 => BiurowyMebel(),
                    18 => ZabezpieczenieElektroniczne(),
                    19 => Zarowka(),
                    20 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Bank()
        {
            string output = "";
            for (int i = 0; i < 8; i++)
            {
                Dice dice = new(20);
                output += dice.Roll switch
                {
                    1 => BiurowySprzetKomputerowy(),
                    2 => Kasa(),
                    3 => EkwipunekPolicyjny(),
                    4 => Ekran(),
                    5 => ZabezpieczenieElektroniczne(),
                    6 => AkcesoriaDoSprzatania(),
                    7 => ArchiwumDanych(),
                    8 => Kasa(),
                    9 => EkwipunekPolicyjny(),
                    10 => AutomatPubliczny(),
                    11 => ZabezpieczenieElektroniczne(),
                    12 => SprzetGasniczy(),
                    13 => ArtykulPapierniczy(),
                    14 => BiurowyDrobiazg(),
                    15 => BiurowyMebel(),
                    16 => SprzetGasniczy(),
                    17 => ArchiwumDanych(),
                    18 => ZabezpieczenieElektroniczne(),
                    19 => Zarowka(),
                    20 => Zwloki(),
                    21 => BiurowyMebel(),
                    22 => ArchiwumDanych(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Dom()
        {
            string output = "";
            for (int i = 0; i < 5; i++)
            {
                Dice dice = new(26);
                output += dice.Roll switch
                {
                    1 => NiespodziankaZWraku(),
                    2 => DomowySprzetSportowy(),
                    3 => AkcesoriaKuchenne(),
                    4 => CennaKolekcja(),
                    5 => DrobiazgDomowy(),
                    6 => Ekran(),
                    7 => NarzedziaDomowe(),
                    8 => NiespodziankaZWraku(),
                    9 => ZabezpieczenieElektroniczne(),
                    10 => UrzadzenieKuchenne(),
                    11 => GratyPiwniczne(),
                    12 => DomowySprzetKomputerowy(),
                    13 => DomowaKolekcjaSztuki(),
                    14 => AkcesoriaDoSprzatania(),
                    15 => AkcesoriaToaletowe(),
                    16 => DrobiazgDomowy(),
                    17 => ArtykulPapierniczy(),
                    18 => Barek(),
                    19 => DomowaApteczka(),
                    20 => DrobiazgHobbystyczny(),
                    21 => WyposazenieDomowe(),
                    22 => ZSzafy(),
                    23 => ZywnoscIUzywki(),
                    24 => Zarowka(),
                    25 => Zwloki(),
                    _ => WyposazenieDomowe(),
                };
                output += Stan();
            }
            return output;
        }

        private string Mieszkanie()
        {
            string output = "";
            for (int i = 0; i < 5; i++)
            {
                Dice dice = new(20);
                output += dice.Roll switch
                {
                    1 => GratyPiwniczne(),
                    2 => DomowySprzetKomputerowy(),
                    3 => AkcesoriaDoSprzatania(),
                    4 => DomowySprzetSportowy(),
                    5 => NarzedziaDomowe(),
                    6 => DrobiazgHobbystyczny(),
                    7 => UrzadzenieKuchenne(),
                    8 => AkcesoriaToaletowe(),
                    9 => AkcesoriaKuchenne(),
                    10 => ArtykulPapierniczy(),
                    11 => ZywnoscIUzywki(),
                    12 => DomowaApteczka(),
                    13 => DomowaKolekcjaSztuki(),
                    14 => DrobiazgDomowy(),
                    15 => DrobiazgDomowy(),
                    16 => Ekran(),
                    17 => WyposazenieDomowe(),
                    18 => ZSzafy(),
                    19 => Zarowka(),
                    20 => Zwloki(),
                    _ => Zwloki()
                };
                output += Stan();
            }
            return output;
        }

        private string Apartament()
        {
            string output = "";
            for (int i = 0; i < 7; i++)
            {
                Dice dice = new(21);
                output += dice.Roll switch
                {
                    1 => AkcesoriaKuchenne(),
                    2 => DomowySprzetKomputerowy(),
                    3 => DomowaKolekcjaSztuki(),
                    4 => CennaKolekcja(),
                    5 => UrzadzenieKuchenne(),
                    6 => DrobiazgDomowy(),
                    7 => DrobiazgHobbystyczny(),
                    8 => Ekran(),
                    9 => AkcesoriaToaletowe(),
                    10 => ArtykulPapierniczy(),
                    11 => DrobiazgHobbystyczny(),
                    12 => Barek(),
                    13 => WyposazenieDomowe(),
                    14 => WyposazenieDomowe(),
                    15 => DrobiazgDomowy(),
                    16 => DomowaApteczka(),
                    17 => ZSzafy(),
                    18 => ZSzafy(),
                    19 => Zarowka(),
                    20 => Zwloki(),
                    _ => WyposazenieDomowe()
                };
                output += Stan();
            }
            return output;
        }

        #endregion lokacje

        // Losujemy przedmiot

        #region Przedmioty

        private string AkcesoriaDoSprzatania()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Miotła 1g",
                2 => "Miotła 1g",
                3 => "Miotła 1g",
                4 => "Miska 1g",
                5 => "Miska 1g",
                6 => "Odkurzacz 10g",
                7 => "Odkurzacz 10g",
                8 => "Szczotka do czyszczenia 1g",
                9 => "Szczotka do czyszczenia 1g",
                10 => "Szczotka do czyszczenia 1g",
                11 => SrodkiCzystosci(),
                12 => SrodkiCzystosci(),
                13 => SrodkiCzystosci(),
                14 => SrodkiCzystosci(),
                15 => SrodkiCzystosci(),
                16 => SrodkiCzystosci(),
                17 => "Plastikowe wiadro 1g",
                18 => "Plastikowe wiadro 1g",
                19 => "Zmiotek i łopatka 1g",
                _ => "Zmiotek i łopatka 1g"
            };
        }

        private string AkcesoriaKuchenne()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Aluminiowy termos 20g",
                2 => "Kieliszek 1g",
                3 => "Metalowa miska 8g",
                4 => "Metalowa taca 3g",
                5 => "Metalowy kubek 6g",
                6 => "Nożyce do mięsa 10g",
                7 => "Nóż kuchenny 5g",
                8 => "Ozdobna filiżanka 10g",
                9 => "Paczka zapałek 5g",
                10 => "Patelnia 5g",
                11 => "Plastikowa miska 3g",
                12 => "Sztućce 2g",
                13 => "Porcelanowa miska 2g",
                14 => "Porcelanowy kubek 2g",
                15 => "Porcelanowy talerz 1g",
                16 => "Słoik z zakrętką 1g",
                17 => "Szklanka 1g",
                18 => "Plastikowy pojemnik na zywność 4g",
                19 => $"Worki foliowe na śmieci ({dice.Roll} sztuk) 1g/10szt",
                _ => "Zamykana puszka 2g"
            };
        }

        private string AkcesoriaRemontowe()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => $"Cement w worku ({dice.Roll} kg) 3g/kg",
                2 => "Farba w puszce 5g",
                3 => "Pistolet lakierniczy 20g",
                4 => "Składana aluminiowa drabina 10g",
                5 => "Szlifierka ręczna 15g",
                6 => "Mocne rękawice robocze 5g",
                7 => "Mocne ubranie robocze 5g",
                8 => NarzedziaDomowe(),
                9 => "Paczcka wkrętów i gwoździ 5g",
                10 => "Pędzel do emulsji 1g",
                11 => "Kilof 10g",
                12 => "Pistolet na bolce 20g",
                13 => "Łopata 10g",
                14 => "Spawarka 40g",
                15 => "Młot 10g",
                16 => "Taczka 5g",
                17 => "Wiadro metalowe 2g",
                18 => "Wiertarka 20g",
                19 => "Obcęgi 10g",
                _ => Zarowka()
            };
        }

        private string AkcesoriaToaletowe()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Czysty ręcznik 1g",
                2 => "Dezodorant 5g",
                3 => "Dozownik z mydłem 3g",
                4 => "Farba do włosów 2g",
                5 => "Gąneczka do kąpieli 1g",
                6 => "Grzebie 1g",
                7 => "Kaczuszka do kąpieli 1g",
                8 => "Krem do golenia 2g",
                9 => "Krem do twarzy 1g",
                10 => "Maszynka do golenia 5g",
                11 => "Mydło 3g",
                12 => "Mydło w płynie 3g",
                13 => "Odświeżacz powietrze 3g",
                14 => "Papier toaletory 2g",
                15 => "Pasta do zębów 5g",
                16 => "Perfumy 5g",
                17 => "Płyn do kąpieli 2g",
                18 => "Pumeks 1g",
                19 => "Szampon 3g",
                _ => "Szczoteczcka do zębów 2g"
            };
        }

        private string AkcesoriaToaletowePubliczne()
        {
            Dice dice = new(4);
            return dice.Roll switch
            {
                1 => "Dozownik z mydlem 3g",
                2 => "Odświeżacz powietrza 3g",
                3 => "Mydło w płynie 3g",
                _ => "Papier toaletowy 2g"
            };
        }

        private string AmunicjaWadliwa()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Wadliwy nabój gumowy (strzelba) 1g",
                2 => "Wadliwy nabój gumowy (strzelba) 1g",
                3 => "Wadliwy nabój gumowy (strzelba) 1g",
                4 => "Wadliwy nabój gumowy (strzelba) 1g",
                5 => "Wadliwy nabój pistoletowy .38\" 2g",
                6 => "Wadliwy nabój pistoletowy .38\" 2g",
                7 => "Wadliwy nabój pistoletowy .38\" 2g",
                8 => "Wadliwy nabój pistoletowy .38\" 2g",
                9 => "Wadliwy nabój pistoletowy 9mm 3g",
                10 => "Wadliwy nabój pistoletowy 9mm 3g",
                11 => "Wadliwy nabój pistoletowy 9mm 3g",
                12 => "Wadliwy nabój pistoletowy 9mm 3g",
                13 => "Wadliwy nabój karabinowy 5.56mm 3g",
                14 => "Wadliwy nabój śrutowy 3g",
                15 => "Wadliwy nabój śrutowy 3g",
                16 => "Wadliwy nabój śrutowy 3g",
                17 => $"Paczka wadliwych nabojów pistoletowych .38\" ({dice.Roll} sztuk) 2g/szt",
                18 => $"Paczka wadliwych nabojów pistoletowych 9mm ({dice.Roll} sztuk) 3g/szt",
                19 => $"Paczka wadliwych nabojów śrutowych ({dice.Roll} sztuk) 3g/szt",
                _ => "Wadliwy granat gazowy 80g"
            };
        }

        private string AparaturaRadiowezla()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Głośnik 5g",
                2 => "Głośnik 5g",
                3 => "Głośnik 5g",
                4 => "Głośnik 5g",
                5 => "Głośnik 5g",
                6 => "Głośnik 5g",
                7 => "Głośnik 5g",
                8 => "Głośnik 5g",
                9 => "Głośnik 5g",
                10 => "Głośnik 5g",
                11 => "Głośnik 5g",
                12 => "Głośnik 5g",
                13 => "Głośnik 5g",
                14 => "Głośnik 5g",
                15 => "Głośnik 5g",
                16 => "Mikrofon 3g",
                17 => "Mikrofon 3g",
                18 => "Mikrofon 3g",
                19 => "Urządzenie nagrywające 3g",
                _ => "Urządzenie nagrywające 3g"
            };
        }

        private string ArchiwumDanych()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Kość pamięci 5g",
                2 => "Dysk twardy 10g",
                3 => "Dysk twardy 10g",
                4 => "Karta pamięci 5g",
                5 => $"Płyty ({dice.Roll} sztuk) -g",
                6 => $"Płyty ({dice.Roll} sztuk) -g",
                7 => $"Płyty ({dice.Roll} sztuk) -g",
                8 => "Segregator z dokumentami -g",
                9 => "Segregator z dokumentami -g",
                10 => "Segregator z dokumentami -g",
                11 => "Segregator z dokumentami -g",
                12 => "Segregator z dokumentami -g",
                13 => "Szpula z taśmą -g",
                14 => "Szpula z taśmą -g",
                15 => "Teczka z kliszami -g",
                16 => "Teczka z dokuemntami -g",
                17 => "Teczka z dokuemntami -g",
                18 => "Teczka z dokuemntami -g",
                19 => "Teczka z dokuemntami -g",
                _ => "Teczka z dokuemntami -g"
            };
        }

        private string ArtykulPapierniczy()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Cyrkiel 1g",
                2 => "Czysty zeszyt 3g",
                3 => "Długopis 3g",
                4 => "Długopis 3g",
                5 => "Długopis – zabawka 3g",
                6 => "Kalkulator 3g",
                7 => "Kalkulator 3g",
                8 => $"Karton ({dice.Roll} arkuszy) 1g/5szt",
                9 => "Linijka 1g",
                10 => "Mała tablica ścieralna 2g",
                11 => "Mocna teczka 1g",
                12 => "Nożyczki 2g",
                13 => "Nożyk 3g",
                14 => "Ołówek",
                15 => $"Papier ({dice.Roll} arkuszy) 1g/10szt",
                16 => $"Papier ({dice.Roll} arkuszy) 1g/10szt",
                17 => "Pinezki (pudełko) 3g",
                18 => "Plastelina 1g",
                19 => "Szpilki (pudełko) 2g",
                _ => "Zeszyt częściowo zapisany 1g"
            };
        }

        private string AsortymentSklepow()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => AGDDomOgrod(),
                2 => AkcesoriaSamochodowe(),
                3 => DomowaApteczka(),
                4 => DomowaKolekcjaSztuki(),
                5 => AkcesoriaToaletowe(),
                6 => Elektronika(),
                7 => InnySklep(),
                8 => TowarZeSklepiku(),
                9 => Ksiazka(),
                10 => Barek(),
                11 => Narzedzia(),
                12 => OdziezObuwie(),
                13 => OdziezObuwie(),
                14 => ArtykulPapierniczy(),
                15 => SklepZBronia(),
                16 => Sportowy(),
                17 => ZywnoscIUzywki(),
                18 => ZywnoscIUzywki(),
                19 => ZywnoscIUzywki(),
                _ => Zabawka()
            };
        }

        private string AGDDomOgrod()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => AkcesoriaDoSprzatania(),
                2 => AkcesoriaKuchenne(),
                3 => "Aluminiowy termos 20g",
                4 => "Aparatura łazienkowa 5g",
                5 => "Bojler elektryczny 10g",
                6 => "Elektryczny grzejnik 15g",
                7 => "Kłódka z kluczem 5g",
                8 => "Krzesło obrotowe 10g",
                9 => "Lampka biurkowa 10g",
                10 => "Lampka stojąca 10g",
                11 => "Listwa halogenowa 10g",
                12 => "Lustro ścienne 5g",
                13 => "Mały stolik konferencyjny 10g",
                14 => "Metalowy kosz na śmieci 2g",
                15 => "Pralka 15g",
                16 => "Stylowy wieszak 5g",
                17 => "Światło ogrodowe (na bateria słoneczne) 20g",
                18 => "Wentylator 5g",
                19 => "Zamek z kluczem 3g",
                _ => "Zegar meblowy 3g"
            };
        }

        private string AkcesoriaSamochodowe()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Butelka oleju silnikowego 6g",
                2 => "Butelka oleju silnikowego 6g",
                3 => "Butelka oleju silnikowego 6g",
                4 => "Gaśnica samochodowa 6g",
                5 => "Klucz do kół 3g",
                6 => "Komplet łańcuchów na koła 3g",
                7 => "Komplet opon 8g",
                8 => "Lewarek 10g",
                9 => "Odmrażacz w sprayu 2g",
                10 => "Odmrażacz w sprayu 2g",
                11 => "Odrdzewiacz w sprayu 5g",
                12 => "Odrdzewiacz w sprayu 5g",
                13 => "Prostownik 10g",
                14 => "Stalowa linka holownicza z hakiem 10g",
                15 => "Stalowa linka holownicza z hakiem 10g",
                16 => "Śrubokręt 2g",
                17 => "Śrubokręt 2g",
                18 => "Żarówka do reflektora 1g",
                19 => "Żarówka do reflektora 1g",
                _ => "Apteczka samochodowa 8g"
            };
        }

        private string Elektronika()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Audio-player spacerowy 10g",
                2 => "Dyktafon 15g",
                3 => ElektronicznaMapa(),
                4 => "Golarka elektryczna 15g",
                5 => "Czystnik kart pamięci 5g",
                6 => "Kalkulator 3g",
                7 => "Klawiatura 3g",
                8 => "Latarka 10g",
                9 => "Ładowarka do akumulatorków 2g",
                10 => "Mikrofon 3g",
                11 => "Mini głośnik 5g",
                12 => "Mysz / manipulator 3g",
                13 => Film(),
                14 => "Płyta z muzyką 5g",
                15 => "Słuchawki 5g",
                16 => "Telefon osobisty 1g",
                17 => "Zegarek na rękę 8g",
                18 => Zarowka(),
                19 => ElektronikaWiekszySprzet(),
                _ => ElektronikaWiekszySprzet()
            };
        }

        private string ElektronikaWiekszySprzet()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Aparat fotograficzny 30g",
                2 => "Drukarka 5g",
                3 => "Dysk twardy 10g",
                4 => Ekran(),
                5 => "Głośniki 3g",
                6 => "Kamera ręczna 30g",
                7 => "Keyboard 20g",
                8 => "Komputer 30g",
                9 => "Konsola do gier 10g",
                10 => "Laptop 50g",
                11 => "Krótkofalówka 25g",
                12 => "Monitor 15g",
                13 => "Odbiornik satelitarny 3g",
                14 => "Odtwarzacz audio z radioodbiornikiem 20g",
                15 => "Odtwarzacz video 25g",
                16 => "Skaner 5g",
                17 => "Telewizor 3g",
                18 => "Wideofon domowy 1g",
                19 => "Wzmacniacz 10g",
                _ => "Zestaw radio-CB 40g"
            };
        }

        private string InnySklep()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Drobna biżuteria – 1szt. 3g",
                2 => "Globus 3g",
                3 => "Gogle pływackie 5g",
                4 => "Holograficzny obrazek 3g",
                5 => "Igła i nici 5g",
                6 => "Imitacja broni palnej (asg) 8g",
                7 => "Kalendarz ozdobny 2g",
                8 => "Kask motocyklowy 5g",
                9 => "Kask rowerowy 3g",
                10 => "Kolekcjonerski nóż 6g",
                11 => "Kompas 5g",
                12 => "Lornetka 30g",
                13 => "Luneta 30g",
                14 => "Mocna walizka 5g",
                15 => "Okulary 3g",
                16 => "Ozdobna ramka na zdjęcie 1g",
                17 => "Porcja narkotyku 10g",
                18 => "Scyzoryk wielofunkcyjny 6g",
                19 => "Wędka i żyłka 5g",
                _ => "Wyjątkowy plakat 1g"
            };
        }

        private string Narzedzia()
        {
            Dice dice = new(40);
            Dice dice20 = new();
            return dice.Roll switch
            {
                1 => "Farba w puszce 5g",
                2 => "Klucz francuski 15g",
                3 => "Kłodka i klucz 5g",
                4 => "Kombinerki 10g",
                5 => "Komplet kluczy 5g",
                6 => "Krążek taśmy izolacyjnej 5g",
                7 => "Młotek 8g",
                8 => "Mcone rękawice robocze 5g",
                9 => "Mocne ubranie robocze 5g",
                10 => "Papier ścierny 2g",
                11 => "Pilnik 3g",
                12 => "Pudełeczko z wiertłami 5g",
                13 => "Pudełeczko z gwoździami 5g",
                14 => "Pudełko ze śrubkami 5g",
                15 => "Puszka smaru 10g",
                16 => "Słoik mocnego kleju 10g",
                17 => "Śrubokręt 2g",
                18 => "Śrubokręt 2g",
                19 => "Śrubokręt 2g",
                20 => "Śrubokręt 2g",
                21 => $"Cement w worku ({dice.Roll}kg 3g/kg)",
                22 => "Latarka 10g",
                23 => "Lutownica i lut 30g",
                24 => "Łopata 10g",
                25 => "Nitownica i nity 40g",
                26 => "Obcęgi",
                27 => "Palnik acetylenowo-tlenowy 20g",
                28 => "Pilarka 10g",
                29 => "Piła do drewna 6g",
                30 => "Piłka do metalu 10g",
                31 => "Pistolet lakierniczy 20g",
                32 => "Pistolet na bolce 40g",
                33 => "Poziomica 2g",
                34 => "Puszka lakieru w losowym kolorze 5g",
                35 => "Siekiera 10g",
                36 => "Składana aluminiowa drabina 10g",
                37 => "Spawarka 40g",
                38 => "Szlifierka ręczna 15g",
                39 => "Wiadro metalowe 2g",
                _ => "Wierkatka 20g"
            };
        }

        //private string WiekszeNarzedzia()
        //{
        //    Dice dice = new();
        //    return dice.Roll switch
        //    {
        //        1 => "",
        //        2 => "",
        //        3 => "",
        //        4 => "",
        //        5 => "",
        //        6 => "",
        //        7 => "",
        //        8 => "",
        //        9 => "",
        //        10 => "",
        //        11 => "",
        //        12 => "",
        //        13 => "",
        //        14 => "",
        //        15 => "",
        //        16 => "",
        //        17 => "",
        //        18 => "",
        //        19 => "",
        //        _ => ""
        //    };
        //}

        private string OdziezObuwie()
        {
            Dice dice = new();
            Dice dice5 = new(5);
            string type = dice5.Roll switch
            {
                1 => "na kobietę",
                2 => "na kobietę",
                3 => "na mężczyznę",
                4 => "na mężczyznę",
                _ => "na dziecko (1/4 podanej obok wartości)"
            };

            return dice.Roll switch
            {
                1 => $"Bluzka {type} 4g",
                2 => $"Buty {type} 10g",
                3 => $"Buty zimowe {type} 10g",
                4 => $"Dres {type} 4g",
                5 => "Garnitur 30g",
                6 => "Kapelusz 5g",
                7 => "Kurtka wojskowa 20g",
                8 => "Kurtka zimowa 15g",
                9 => "Lustro 5g",
                10 => "Manekin-wieszak 4g",
                11 => $"Mocne spodnie {type} 15g",
                12 => "Okulary przeciwsłoneczne 5g",
                13 => "Pasek do spodni 5g",
                14 => "Pasta do impregnacji butów 5g",
                15 => $"Płaszcz {type} 8g",
                16 => "Skórzana kurtka 20g",
                17 => "Skórzane spodnie 20g",
                18 => $"Spodnie {type} 8g",
                19 => "Spodnie wojskowe 15g",
                _ => "Sweter 8g"
            };
        }

        private string SklepZBronia()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => AmunicjaWadliwa(),
                2 => BronUszkodzona(),
                3 => "Kabura i szelki 6g",
                4 => "Kabura i szelki 6g",
                5 => "Kamizelka ochronna 70g",
                6 => "Kurtka wojskowa 20g",
                7 => "Kurtka wojskowa 20g",
                8 => "Nóż myśliwski 10g",
                9 => "Nóż sprężynowy 6g",
                10 => "Pałka policyjna 3g",
                11 => "Pałka policyjna 3g",
                12 => "Pusty magazynek pistoletowy 9mm 3g",
                13 => "Pusty magazynek pistoletowy 9mm 3g",
                14 => "Spodnie wojskowe 15g",
                15 => "Spodnie wojskowe 15g",
                16 => "Zestaw do czyszczenia broni 10g",
                17 => "Zestaw do czyszczenia broni 10g",
                18 => "Katalog broni 2g",
                19 => "Katalog broni 2g",
                _ => "Katalog broni 2g"
            };
        }

        private string Sportowy()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Deskorolka 3g",
                2 => "Hełm ochronny do rugby 6g",
                3 => "Kij baseballowy 5g",
                4 => "Maska do nurkowania 10g",
                5 => "Ochraniacze 5g",
                6 => "Piłeczka baseballowa 3g",
                7 => "Piłeczka pingpongowa 3g",
                8 => "Piłeczka tenisowa 5g",
                9 => "Piłeczka do koszykówki 10g",
                10 => "Piłka do piłki nożnej 10g",
                11 => "Piłka do rugby 10g",
                12 => "Płetwy 4g",
                13 => "Rakieta do tenisa stołowego 4g",
                14 => "Rakieta do tenisa ziemnego 4g",
                15 => "Rollery 8g",
                16 => "Rower 30g",
                17 => "Sprzęt do wspinaczki 25g",
                18 => "Strój rugby 15g",
                19 => "Sztanga i talerze 8g",
                _ => "Worek treningowy 5g"
            };
        }

        private string AutomatPubliczny()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => $"Automat do gier ({dice.Roll} przedwojennych monet)",
                2 => $"Automat do gier ({dice.Roll} przedwojennych monet)",
                3 => $"Automat hazardowy ({dice.Roll} przedwojennych monet)",
                4 => $"Automat hazardowy ({dice.Roll} przedwojennych monet)",
                5 => AutomatycznyBarman(),
                6 => AutomatycznyBarman(),
                7 => AutomatycznyBarman(),
                8 => AutomatycznyBarman(),
                9 => "Automatyczny fotograf (zintegrowany aparat fotograficzny) 10g",
                10 => AutomatycznySprzedawca(),
                11 => AutomatycznySprzedawca(),
                12 => AutomatycznySprzedawca(),
                13 => AutomatycznySprzedawca(),
                14 => $"Bankomat ({dice.Roll} przedwojennych banknotów)",
                15 => $"Bankomat ({dice.Roll} przedwojennych banknotów)",
                16 => $"Bankomat ({dice.Roll} przedwojennych banknotów)",
                17 => $"Bankomat ({dice.Roll} przedwojennych banknotów)",
                18 => $"Bankomat ({dice.Roll} przedwojennych banknotów)",
                19 => LadowarkaPubliczna(),
                _ => Bagaz()
            };
        }

        private string AutomatycznyBarman()
        {
            Dice dice = new(4);
            Dice dice20 = new();
            return dice.Roll switch
            {
                1 => $"Cappuccino ({dice20.Roll} porcji) 1g/5p",
                2 => $"Cukier ({dice20.Roll} porcji) 1g/5p",
                3 => $"Kawa ({dice20.Roll} porcji) 1g/5p",
                _ => $"Plastikowe kubki ({dice20.Roll} porcji) 1g/10p",
            };
        }

        private string AutomatycznySprzedawca()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Batonik 3g",
                2 => "Batonik 3g",
                3 => "Batonik 3g",
                4 => "Batonik 3g",
                5 => $"Paczka gum do żucia ({dice.Roll} sztuk) 1g/3szt",
                6 => $"Paczka gum do żucia ({dice.Roll} sztuk) 1g/3szt",
                7 => $"Pudełko papierosów ({dice.Roll} sztuk) 1g/szt",
                8 => $"Pudełko papierosów ({dice.Roll} sztuk) 1g/szt",
                9 => $"Pudełko papierosów ({dice.Roll} sztuk) 1g/szt",
                10 => "Puszka Coca Coli 10g",
                11 => "Puszka Coca Coli 10g",
                12 => "Puszka Coca Coli 10g",
                13 => "Puszka energy drink 4g",
                14 => "Puszka energy drink 4g",
                15 => "Puszka napoju gazowanego 3g",
                16 => "Puszka napoju gazowanego 3g",
                17 => "Puszka napoju gazowanego 3g",
                18 => "Puszka orzechów solonych 3g",
                19 => "Tabliczka czekolady 8g",
                _ => "Zapalniczka 8g"
            };
        }

        private string LadowarkaPubliczna()
        {
            Dice dice = new(2);
            return dice.Roll switch
            {
                1 => "Akumulator 30g",
                _ => "Uniwersalny zestaw wtyczek 6g"
            };
        }

        private string Bagaz()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => Ksiazka(),
                2 => Ksiazka(),
                3 => Ksiazka(),
                4 => "Aluminiowy termos 20g",
                5 => Portfel(),
                6 => Portfel(),
                7 => $"Przedwojenne banknoty ({dice.Roll} sztuk)",
                8 => $"Przedwojenne banknoty ({dice.Roll} sztuk)",
                9 => $"Przedwojenny bilon ({dice.Roll} sztuk)",
                10 => $"Przedwojenny bilon ({dice.Roll} sztuk)",
                11 => "Laptop 50g",
                12 => ZSzafy(),
                13 => ZSzafy(),
                14 => ButelkiIPuszki(),
                15 => ButelkiIPuszki(),
                16 => Konserwy(),
                17 => Konserwy(),
                18 => SlodyczeIUzywki(),
                19 => SlodyczeIUzywki(),
                _ => SlodyczeIUzywki()
            };
        }

        private string Barek()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Butelka szampana 10g",
                2 => "Butelka szampana 10g",
                3 => "Butelka szampana 10g",
                4 => "Butelka whisky 10g",
                5 => "Butelka whisky 10g",
                6 => "Butelka whisky 10g",
                7 => "Butelka whisky 10g",
                8 => "Butelka whisky 10g",
                9 => "Butelka wina 10g",
                10 => "Butelka wina 10g",
                11 => "Butelka wina 10g",
                12 => "Butelka wina 10g",
                13 => "Butelka wina 10g",
                14 => "Butelka wódki 10g",
                15 => "Butelka wódki 10g",
                16 => "Kieliszek 1g",
                17 => "Kieliszek 1g",
                18 => "Porcja narkotyki 10g",
                19 => $"Pudełko cygar ({dice.Roll} sztuk) 2g/szt",
                _ => $"Pudełko cygar ({dice.Roll} sztuk) 2g/szt"
            };
        }

        private string BiurowaKuchnia()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Czajnik elektryczny 8g",
                2 => "Czajnik elektryczny 8g",
                3 => "Czajnik elektryczny 8g",
                4 => "Ekspres do kawy 7g",
                5 => "Ekspres do kawy 7g",
                6 => FiltrWody(),
                7 => "Kuchenka mikrofalowa 10g",
                8 => "Kuchenka mikrofalowa 10g",
                9 => "Mała zamrażarka 10g",
                10 => "Mała zamrażarka 10g",
                11 => $"Paczka cukru ({dice.Roll} porcji) 1g/5p",
                12 => $"Paczka cukru ({dice.Roll} porcji) 1g/5p",
                13 => $"Paczka herbaty ({dice.Roll} porcji) 1g/5p",
                14 => $"Paczka herbaty ({dice.Roll} porcji) 1g/10p",
                15 => $"Paczka kawy mielonej ({dice.Roll} porcji) 1g/5p",
                16 => $"Paczka kawy mielonej ({dice.Roll} porcji) 1g/5p",
                17 => $"Paczka kawy rozpuszczalnej ({dice.Roll} porcji) 1g/5p",
                18 => $"Paczka kawy rozpuszczalnej ({dice.Roll} porcji) 1g/5p",
                19 => "Porcelanowy kubek",
                _ => "Porcelanowy kubek"
            };
        }

        private string BiurowyDrobiazg()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => ArtykulPapierniczy(),
                2 => BiurowaKuchnia(),
                3 => BiurowySprzetKomputerowy(),
                4 => "Dyktafon 15g",
                5 => ElektronicznaMapa(),
                6 => Gazeta(),
                7 => "Golarka elektryczna 15g",
                8 => "Kalendarz ozdobny 2g",
                9 => "Kalkulator 3g",
                10 => "Kamera ręczna 30g",
                11 => "Lampka biurkowa 10g",
                12 => "Ładowarka do akumulatorków 2g",
                13 => $"Ścienna (cena x2) {Mapa()}",
                14 => "Ozdobna ramka na zdjęcie 1g",
                15 => $"Paczka papierosów ({dice.Roll} sztuk) 1g/szt",
                16 => "Radioodbiornik 15g",
                17 => "Tablica korkowa 2g",
                18 => "Wideofon 1g",
                19 => $"Worki foliowe na śmieci ({dice.Roll} sztuk) 1g/10szt",
                _ => "Zegar 3g"
            };
        }

        private string BiurowyMebel()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => Ekran(),
                2 => "Elektryczny grzejnik 15g",
                3 => "Elektryczny grzejnik 15g",
                4 => "Fotel menedżerski 15g",
                5 => "Fotel menedżerski 15g",
                6 => "Krzesło obrotowe 10g",
                7 => "Krzesło obrotowe 10g",
                8 => "Krzesło obrotowe 10g",
                9 => "Krzesło obrotowe 10g",
                10 => "Krzesło obrotowe 10g",
                11 => "Lampka stojąca 10g",
                12 => "Lampka stojąca 10g",
                13 => "Listwa halogenowa 10g",
                14 => "Lustro ścienne 5g",
                15 => "Mały stolik konferencyjny 10g",
                16 => "Metalowy kosz na śmieci 2g",
                17 => "Metalowy kosz na śmieci 2g",
                18 => "Mini akwarium 4g",
                19 => "Stalowa zamykana szafka na akta 15g",
                _ => "Stylowy wieszak 5g"
            };
        }

        private string BiurowySprzetKomputerowy()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Drukarka 5g",
                2 => "Dysk twardy 10g",
                3 => "Głośniki 3g",
                4 => "Karta pamięci 5g",
                5 => "Klawiatura 3g",
                6 => "Klawiatura 3g",
                7 => "Komputer 30g",
                8 => "Kserokopiarka 6g",
                9 => "Laptop 50g",
                10 => "Monitor 15g",
                11 => "Monitor 15g",
                12 => "Mysz / manipulator 3g",
                13 => "Mysz / manipulator 3g",
                14 => $"Płyty ({dice.Roll} sztuk) -g",
                15 => "Rzutnik do komputera 20g",
                16 => "Skaner 5g",
                17 => "Skaner 5g",
                18 => "Skaner ręczny 10g",
                19 => "Zasilacz 3g",
                _ => "Zasilacz 3g"
            };
        }

        private string BronUszkodzona()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Uszkodzona dwururka 50g",
                2 => "Uszkodzona dwururka 50g",
                3 => "Uszkodzona pompka 6g",
                4 => "Uszkodzona pompka 6g",
                5 => "Uszkodzony rewolwer - Trzydziestka ósemka (S&W) 35g",
                6 => "Uszkodzony rewolwer - Trzydziestka ósemka (S&W) 35g",
                7 => "Uszkodzony rewolwer - Magnum 44 60g",
                8 => "Uszkodzony rewolwer - Colt \"Peacemaker\" 40g",
                9 => "Uszkodzony pistolet - Glock 17 55g",
                10 => "Uszkodzony pistolet - Browning",
                11 => "Uszkodzony pistolet – Sig-Sauer P226 40g",
                12 => "Uszkodzony pistolet – Beretta 45g",
                13 => "Uszkodzony pistolet - Colt M1911 A1 40g",
                14 => "Uszkodzony pistolet - Walther P-99 50g",
                15 => "Uszkodzony PM - H&K MP5 75g",
                16 => "Uszkodzony karabin - Winchester 40g",
                17 => "Uszkodzony karabin - Winchester 40g",
                18 => "Uszkodzony karabin - Springfield 45g",
                19 => "Uszkodzony karabin - SKS 65g",
                _ => "Uszkodzony karabin - SKS 65g"
            };
        }

        private string BronHukowa()
        {
            return "Broń hukowa (GS) 20g";
        }

        private string ButleZChemikaliami()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Acetylen 1g/l",
                2 => "Acetylen 1g/l",
                3 => "Alkohol metylowy 1g/l",
                4 => "Alkohol metylowy 1g/l",
                5 => "Amoniak 1g/l",
                6 => "Amoniak 1g/l",
                7 => "Słaba benzyna 3g/l",
                8 => "Słaba benzyna 3g/l",
                9 => "Elektrolit 1g/l",
                10 => "Elektrolit 1g/l",
                11 => "Gaz grzewczy 3g/l",
                12 => "Gaz grzewczy 3g/l",
                13 => "Kwas azotowy 3g/l",
                14 => "Kwas azotowy 3g/l",
                15 => "Kwas siarkowy 3g/l",
                16 => "Kwas siarkowy 3g/l",
                17 => "Ropa naftowa 3g/l",
                18 => "Ropa naftowa 3g/l",
                19 => "Woda destylowana 1g/5l",
                _ => "Woda destylowana 1g/5l"
            };
        }

        private string CennaKolekcja()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Cenny obraz 10g",
                2 => "Cenny obraz 10g",
                3 => $"Kolekcja medali ({dice.Roll} sztuk) 1g/szt",
                4 => $"Kolekcja medali ({dice.Roll} sztuk) 1g/szt",
                5 => $"Kolekcja monet ({dice.Roll} sztuk) 1g/szt",
                6 => $"Kolekcja monet ({dice.Roll} sztuk) 1g/szt",
                7 => Sejf(),
                8 => Sejf(),
                9 => "Srebrna figurka 15g",
                10 => "Srebrna figurka 15g",
                11 => "Srebrna figurka 15g",
                12 => "Srebrna taca 10g",
                13 => "Srebrna taca 10g",
                14 => "Srebrny ozdobny kubek 20g",
                15 => "Srebrny ozdobny kubek 20g",
                16 => "Srebrny ozdobny kubek 20g",
                17 => "Srebrny ozdobny świecznik 15g",
                18 => "Srebrny ozdobny świecznik 15g",
                19 => "Srebrny ozdobny świecznik 15g",
                _ => "Złota figurka 50g"
            };
        }

        private string Dehydryzator()
        {
            return "Dehydryzator (GS) 100g";
        }

        private string DomowaApteczka()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Bandaż 5g",
                2 => "Bandaż 5g",
                3 => "Dawka morfiny 15g",
                4 => $"Buteleczka psychotropów antydepresyjnych ({dice.Roll} tabletek) 1g/szt",
                5 => "Buteleczka spirytusu 15g",
                6 => "Buteleczka spirytusu 15g",
                7 => "Butelka ze środkiem dezynfekującym 20g",
                8 => "Komplet plastrów 3g",
                9 => "Strzykawka i igła 10g",
                10 => "Krem na poparzenia 8g",
                11 => $"Opakowanie antybiotyków ({dice.Roll} tabletek) 5g/szt",
                12 => $"Opakowanie aspiryny ({dice.Roll} tabletek) 1g/szt",
                13 => $"Opakowanie aspiryny ({dice.Roll} tabletek) 1g/szt",
                14 => $"Opakowanie leków na grypę ({dice.Roll} tabletek) 1g/szt",
                15 => $"Opakowanie leków na zatrucie ({dice.Roll} tabletek) 1g / szt",
                16 => $"Opakowanie leków na zatrucie ({dice.Roll} tabletek) 1g/szt",
                17 => $"Paczka tabletek przeciwbólowych ({dice.Roll} tabletek) 3g/szt",
                18 => $"Paczka tabletek przeciwbólowych ({dice.Roll} tabletek) 3g/szt",
                19 => $"Paczka witamin ({dice.Roll} tabletek) 1g/5szt",
                _ => $"Pudełko ziół ({dice.Roll} porcji) 1g/10p"
            };
        }

        private string DomowaKolekcjaSztuki()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Fabryczna miniatura rzeźbiarska 3g",
                2 => "Fabryczna miniatura rzeźbiarska 3g",
                3 => "Fabryczna miniatura rzeźbiarska 3g",
                4 => "Grafika ze szkłem 3g",
                5 => "Grafika ze szkłem 3g",
                6 => "Kryształowa ozdobna pantera 3g",
                7 => "Kryształowa ozdobna pantera 3g",
                8 => "Kryształowa ozdobna pantera 3g",
                9 => "Kryształowa ozdobna pantera 3g",
                10 => "Ozdobna waza 5g",
                11 => "Ozdobny swiecznik 5g",
                12 => "Ozdobny swiecznik 5g",
                13 => "Ozdobna filiżanka 10g",
                14 => "Reprodukcja 3g",
                15 => "Reprodukcja 3g",
                16 => "Reprodukcja 3g",
                17 => "Reprodukcja 3g",
                18 => "Tani obraz 3g",
                19 => "Tani obraz 3g",
                _ => "Tani obraz 3g"
            };
        }

        private string DomowySprzetKomputerowy()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Drukarka 5g",
                2 => "Dysk twardy 10g",
                3 => "Głośniki 3g",
                4 => "Głośniki 3g",
                5 => "Klawiatura 3g",
                6 => "Klawiatura 3g",
                7 => "Karta pamięci 5g",
                8 => "Komputer 30g",
                9 => "Konsola do gier 10g",
                10 => "Laptop 50g",
                11 => "Monitor 15g",
                12 => "Monitor 15g",
                13 => "Mysz / manipulator 3g",
                14 => "Mysz / manipulator 3g",
                15 => "Okulary 3D 5g",
                16 => $"Płyty ({dice.Roll} sztuk) -g",
                17 => $"Płyty ({dice.Roll} sztuk) -g",
                18 => "Skaner 5g",
                19 => "Zasilacz 3g",
                _ => "Zasilacz 3g"
            };
        }

        private string DomowySprzetSportowy()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Deskorolka 3g",
                2 => "Kij baseballowy 5g",
                3 => "Kij golfowy 5g",
                4 => "Maska do nurkowania 10g",
                5 => "Narty 15g",
                6 => "Piłeczka pingpongowa 3g",
                7 => "Piłeczka baseballowa 3g",
                8 => "Piłka do koszykówki 10g",
                9 => "Piłka do piłki nożnej 10g",
                10 => "Piłka do rugby 10g",
                11 => "Piłeczka golfowa 3g",
                12 => "Piłeczka tenisowa 5g",
                13 => "Rakieta do tenisa stołowego 4g",
                14 => "Rakieta do tenisa ziemnego 4g",
                15 => "Rollery 8g",
                16 => "Uszkodzony rower 30g",
                17 => "Sportowy floret 15g",
                18 => "Sprzęt do wspinaczki 25g",
                19 => "Sztanga i talerze 8g",
                _ => "Worek treningowy 5g"
            };
        }

        private string DrobiazgDomowy()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => ArtykulPapierniczy(),
                2 => "Audio-player spacerowy 10g",
                3 => "Drobna biżuteria (1 sztuka) 3g",
                4 => "Dyktafon 15g",
                5 => Gazeta(),
                6 => "Globus 3g",
                7 => "Igła i nici 5g",
                8 => "Kamera ręczna 30g",
                9 => "Komiks 12g",
                10 => "Ładowarka do akumulatorków 2g",
                11 => "Maskotka 2g",
                12 => "Odtwarzacz audio z radiem 20g",
                13 => "Pamiątkowa figurka z podróży 1g",
                14 => Film(),
                15 => "Płyta z muzyką 5g",
                16 => $"Przedwojenne banknoty ({dice.Roll} sztuk)",
                17 => $"Przedwojenny bilon ({dice.Roll} sztuk)",
                18 => "Waga domowa 15g",
                19 => "Wyjątkowy plakat 1g",
                _ => Zabawka()
            };
        }

        private string DrobiazgHobbystyczny()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Aparat fotograficzny 30g",
                2 => ElektronicznaMapa(),
                3 => "Gogle pływackie 5g",
                4 => Gra(),
                5 => "Imitacja broni palnej (asg) 8g",
                6 => "Imitacja mieczca (mocna) 20g",
                7 => InstrumentMuzyczny(),
                8 => "Kask motocyklowy 5g",
                9 => "Kask rowerowy 3g",
                10 => $"Kolekcja płyt ({dice.Roll} sztuk) -g",
                11 => "Kolekcjonerski nóż 6g",
                12 => "Kompas 5g",
                13 => "Krótkofalówka 25g",
                14 => "Lornetka 30g",
                15 => "Luneta 30g",
                16 => "Porcja narkotyku 10g",
                17 => "ręczna kamera 30g",
                18 => "Wędka i żyłka 5g",
                19 => "Zdalnie sterowany model 20g",
                _ => "Zestaw radio-CB"
            };
        }

        private string Gra()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Bierki 5g",
                2 => "Bierki 5g",
                3 => "Chińczyk 10g",
                4 => "Chińczyk 10g",
                5 => "Domino 10g",
                6 => "Domino 10g",
                7 => "Monopoly 10g",
                8 => "Monopoly 10g",
                9 => "Scrabble 10g",
                10 => "Scrabble 10g",
                11 => "Skomplikowana gra planszowa 5g",
                12 => "Szachy 20g",
                13 => "Szachy 20g",
                14 => "Talia kart 15g",
                15 => "Talia kart 15g",
                16 => "Talia kart 15g",
                17 => "Warcaby 15g",
                18 => "Warcaby 15g",
                19 => "Zestaw kostek10g",
                _ => "Zestaw kostek 10g"
            };
        }

        private string InstrumentMuzyczny()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Bębenek 5g",
                2 => "Bęnenek 5g",
                3 => "Skrzypce 10g",
                4 => "Flet 10g",
                5 => "Flet 10g",
                6 => "Gitara elektryczna 10g",
                7 => "Gitara elektryczna 10g",
                8 => "Gitara elektryczna 10g",
                9 => "Gitara elektryczna 10g",
                10 => "Gitara klasyczna 30g",
                11 => "Gitara klasyczna 30g",
                12 => "Gitara klasyczna 30g",
                13 => "Gitara klasyczna 30g",
                14 => "Gitara klasyczna 30g",
                15 => "Keyboard 20g",
                16 => "Keyboard 20g",
                17 => "Keyboard 20g",
                18 => "Mikrofon sceniczny 8g",
                19 => "Mikrofon sceniczny 8g",
                _ => "Zestaw perkusyjny 10g"
            };
        }

        private string DzieloSztuki()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Bogato zdobiony świecznik 20g",
                2 => "Bogato zdobiony świecznik 20g",
                3 => "Bogato zdobiony świecznik 20g",
                4 => "Bogato zdobiony żyrandol 15g",
                5 => "Bogato zdobiony żyrandol 15g",
                6 => "Bogato zdobiony żyrandol 15g",
                7 => "Cenny gobelin 20g",
                8 => "Cenny obraz współczesny 20g",
                9 => "Cenny obraz współczesny 20g",
                10 => "Cenny obraz współczesny 20g",
                11 => "Cenny obraz współczesny 20g",
                12 => "Cenny obraz współczesny 20g",
                13 => "Ozdobna ciężka waza 10g",
                14 => "Ozdobna ciężka waza 10g",
                15 => "Ozdobna ciężka waza 10g",
                16 => "Rzeźba 20g",
                17 => "Rzeźba 20g",
                18 => "Stary obraz 30g",
                19 => "Stary obraz 30g",
                _ => "Stary obraz 30g"
            };
        }

        private string Ekran()
        {
            return "Ekran 5g";
        }

        private string EkwipunekPolicyjny()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => AmunicjaWadliwa(),
                2 => AmunicjaWadliwa(),
                3 => BronUszkodzona(),
                4 => BronUszkodzona(),
                5 => ElektronicznaMapa(),
                6 => "Kajdanki 5g",
                7 => "Kajdanki 5g",
                8 => "Kamizelka ochronna 70g",
                9 => "Kamizelka ochronna 70g",
                10 => "Krótkofalówka 25g",
                11 => "Krótkofalówka 25g",
                12 => "Wadliwy paralizator elektryczny 100g",
                13 => "Mundur strażnika / gliniarza 15g",
                14 => "Mundur strażnika / gliniarza 15g",
                15 => "Zestaw komunikatorów (zasięg do 100m) 20g",
                16 => "Zestaw komunikatorów (zasięg do 100m) 20g",
                17 => "Maska gazowa 20g",
                18 => "Taśma zaciskowa 3g",
                19 => "Pałka policyjna 3g",
                _ => "Pałka policyjna 3g"
            };
        }

        private string ElektronicznaMapa()
        {
            return "Elektroniczna mapa 10-50g";
        }

        private string Film()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Film animowany",
                2 => "Film – biografia",
                3 => "Film dla dzieci",
                4 => "Film dokumentalny - astronomia",
                5 => "Film dokumentalny - historia",
                6 => "Film dokumentalny - militaria",
                7 => "Film dokumentalny - podróże",
                8 => "Film dokumentalny - przyroda",
                9 => "Film dokumentalny - społeczeństwo",
                10 => "Film dokumentalny – sztuka",
                11 => "Film dokumentalny - technologia",
                12 => "Film - dramat obyczajowy",
                13 => "Film - fantastyka / horror",
                14 => "Film historyczny",
                15 => "Film akcji",
                16 => "Film - kabaret",
                17 => "Film - klasyka",
                18 => "Film komediowy",
                19 => "Film kryminalny",
                _ => "Film romantyczny"
            };
        }

        private string FiltrWody()
        {
            return "Uszkodzony filtr wody 50g";
        }

        private string Gazeta()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Gazeta codzienna 1g",
                2 => "Gazeta lokalna 2g",
                3 => "Gazeta telewizyjna 1g",
                4 => "Krzyżówki 1g",
                5 => "Magazyn fantastyczny 1g",
                6 => "Magazyn historyczny 1g",
                7 => "Magazyn komputerowy 1g",
                8 => "Magazyn kulturalny 1g",
                9 => "Magazyn literacki 1g",
                10 => "Magazyn militarystyczny 3g",
                11 => "Magazyn muzyczny 1g",
                12 => "Magazyn o głupim hobby 1g",
                13 => "Magazyn samochodowy 1g",
                14 => "Magazyn sportowy 1g",
                15 => "Magazyn techniczny 5g",
                16 => "Pisemko dla panów 5g",
                17 => "Pismo dla dzieci 1g",
                18 => "Skandalizujący tygodnik 1g",
                19 => "Tygodnik ekonomiczny 1g",
                _ => "Żurnal 1g"
            };
        }

        private string GratyPiwniczne()
        {
            Dice dice = new(10);
            Dice dice20 = new();
            return dice.Roll switch
            {
                1 => "Brezentowa płachta 2g",
                2 => NarzedziaDomowe(),
                3 => "Parciany worek 1g",
                4 => $"Puste słoiki ({dice20.Roll} sztuk) 1g/5szt",
                5 => "Słoik z zapasami 3g",
                6 => "Uszkodzony stary rower 30g",
                7 => "Wielki foliowy worek 1g",
                8 => "Słoik z zapasami 3g",
                9 => "Kłódka z kluczem 5g",
                _ => Zarowka()
            };
        }

        private string KameraWziernikowa()
        {
            return "Kamera wziernikowa (GS) 40g";
        }

        private string Kasa()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Metalowa kasetka i kluczyk 1g",
                2 => "Metalowa kasetka i kluczyk 1g",
                3 => $"Przedwojenne banknoty ({dice.Roll} sztuk)",
                4 => $"Przedwojenne banknoty ({dice.Roll} sztuk)",
                5 => $"Przedwojenne banknoty ({dice.Roll} sztuk)",
                6 => $"Przedwojenne banknoty ({dice.Roll} sztuk)",
                7 => $"Przedwojenne monety ({dice.Roll} sztuk)",
                8 => $"Przedwojenne monety ({dice.Roll} sztuk)",
                9 => $"Przedwojenne monety ({dice.Roll} sztuk)",
                10 => $"Przedwojenne monety ({dice.Roll} sztuk)",
                11 => "Rolka papieru 1g",
                12 => "Rolka papieru 1g",
                13 => "Rolka papieru 1g",
                14 => "Rolka papieru 1g",
                15 => "Rolka papieru 1g",
                16 => "Zintegrowany monitor 5g",
                17 => "Zintegrowany monitor 5g",
                18 => "Zintegrowany monitor 5g",
                19 => "Zintegrowany monitor 5g",
                _ => "Zintegrowany monitor 5g"
            };
        }

        private string Ksiazka()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => Beletrystyka(),
                2 => Beletrystyka(),
                3 => Beletrystyka(),
                4 => Beletrystyka(),
                5 => Beletrystyka(),
                6 => Beletrystyka(),
                7 => Beletrystyka(),
                8 => Beletrystyka(),
                9 => Beletrystyka(),
                10 => Beletrystyka(),
                11 => Beletrystyka(),
                12 => Beletrystyka(),
                13 => Beletrystyka(),
                14 => Beletrystyka(),
                15 => Beletrystyka(),
                16 => Naukowa(),
                17 => Naukowa(),
                18 => Naukowa(),
                19 => Podreczniki(),
                _ => Podreczniki()
            };
        }

        private string Beletrystyka()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Książka - album 12g",
                2 => "Książka - biografia 10g",
                3 => "Książka - biografia 10g",
                4 => "Książka - dowcipy 10g",
                5 => "Książka - dramat 10g",
                6 => "Książka - dziecięca 5g",
                7 => "Książka - dziecięca 5g",
                8 => "Książka - fantasy 10g",
                9 => "Książka - fantasy 10g",
                10 => "Książka - gra RPG 5g",
                11 => "Książka - historyczna 10g",
                12 => "Książka - horror 5g",
                13 => "Książka - klasyka 15g",
                14 => "Książka - klasyka 15g",
                15 => "Komiks 12g",
                16 => "Książka - kryminał 10g",
                17 => "Książka - obyczajowa 10g",
                18 => "Książka - poezja 10g",
                19 => "Książka - romans 10g",
                _ => "Książka - SF 10g"
            };
        }

        private string Naukowa()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Książka naukowa - atlas 25g",
                2 => "Książka naukowa - biologia 25g",
                3 => "Książka naukowa - chemia 30g",
                4 => "Książka naukowa - ekonomia 5g",
                5 => "Książka naukowa - elektronika 30g",
                6 => "Książka naukowa - encyklopedia 30g",
                7 => "Książka naukowa - fizyka 15g",
                8 => "Książka naukowa - geografia 15g",
                9 => "Książka naukowa - historia 20g",
                10 => "Książka naukowa - historia sztuki 10g",
                11 => "Książka naukowa - informatyka 20g",
                12 => "Książka naukowa - język obcy 5g",
                13 => "Książka naukowa - mechanika 30g",
                14 => "Książka naukowa - medycyna 30g",
                15 => "Książka naukowa - militaria 20g",
                16 => "Książka naukowa - prawo 5g",
                17 => "Książka naukowa - psychologia 10g",
                18 => "Książka naukowa - robotyka 30g",
                19 => "Książka naukowa - technika 30g",
                _ => "Książka naukowa - turystyka 10g"
            };
        }

        private string Podreczniki()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Podręcznik – atlas 15g",
                2 => "Podręcznik – atlas 15g",
                3 => "Podręcznik – atlas 15g",
                4 => "Podręcznik – biologia 15g",
                5 => "Podręcznik – biologia 15g",
                6 => "Podręcznik – biologia 15g",
                7 => "Podręcznik – chemia 15g",
                8 => "Podręcznik – chemia 15g",
                9 => "Podręcznik – chemia 15g",
                10 => "Podręcznik – geografia 10g",
                11 => "Podręcznik – geografia 10g",
                12 => "Podręcznik – geografia 10g",
                13 => "Podręcznik – historia 10g",
                14 => "Podręcznik – historia 10g",
                15 => "Podręcznik – historia 10g",
                16 => "Podręcznik – język obcy 5g",
                17 => "Podręcznik – język obcy 5g",
                18 => "Podręcznik – język obcy 5g",
                19 => "Podręcznik – literatura 10g",
                _ => "Podręcznik – litaratura 10g"
            };
        }

        private string LatarkaMachajka()
        {
            return "Latarka - machajka (GS) 30g";
        }

        private string Mapa()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Mapa miasta, w którym znalazłeś tę mapę 40g",
                2 => "Mapa miasta, w którym znalazłeś tę mapę 40g",
                3 => "Mapa miasta, w którym znalazłeś tę mapę 40g",
                4 => "Mapa gospodarczo-geograficzna stanu, w którym znalazłeś tę mapę 40g",
                5 => "Mapa gospodarczo-geograficzna stanu, w którym znalazłeś tę mapę 40g",
                6 => "Mapa gospodarczo-geograficzna stanu, w którym znalazłeś tę mapę 40g",
                7 => "Mapa samochodowa stanu, w którym znalazłeś tę mapę 40g",
                8 => "Mapa samochodowa stanu, w którym znalazłeś tę mapę 40g",
                9 => "Mapa samochodowa stanu, w którym znalazłeś tę mapę 40g",
                10 => "Mapa świata geograficzna 10g",
                11 => "Mapa świata geograficzna 10g",
                12 => "Mapa świata polityczna 10g",
                13 => "Mapa świata polityczna 10g",
                14 => "Mapa administracyjna USA 20g",
                15 => "Mapa administracyjna USA 20g",
                16 => "Mapa gospodarczo-geograficzna USA 20g",
                17 => "Mapa gospodarczo-geograficzna USA 20g",
                18 => "Mapa samochodowa USA 40g",
                19 => "Mapa samochodowa USA 40g",
                _ => "Mapa samochodowa USA 40g"
            };
        }

        private string Medskaner()
        {
            return "Medskaner (GS)";
        }

        private string Medykamenty()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Antybiotyki (20 tabletek) 5g/szt",
                2 => "Antyseeptyk w aerozolu 25g",
                3 => "Aspiryna (20 tabletek) 1g.szt",
                4 => "Dawka morfiny 15g",
                5 => $"Buteleczka psychotropów antydepresyjnych ({dice.Roll} tabletek) 1g/szt",
                6 => "Butelka spirytusu 15g",
                7 => "Butelka ze środkiem antyseptycznym 20g",
                8 => "Hermetyczny pojemnik 5g",
                9 => "Strzykawka i igła 10g",
                10 => "Kroplówka z glukozą 5g",
                11 => "Kroplówka z płynem wieloelektrolitowym 5g",
                12 => "Odczynniki laboratoryjne 20g",
                13 => $"Paczka leków na zatrucie ({dice.Roll} tabletek) 1g/szt",
                14 => "Opatrunek 5g",
                15 => $"Paczka tabletek przeciwbólowych ({dice.Roll} tabletek) 3g/szt",
                16 => $"Paczka witamin ({dice.Roll} tabletek) 1g/5szt",
                17 => "Silny środek usypiający (wymaga strzykawki) 10g",
                18 => "Silny środek znieczulający (wymaga strzykawki) 10g",
                19 => "Słoiczzek kremu na oparzenia 15g",
                _ => "Tubka żelu anstyseptycznego 20g"
            };
        }

        private string MultimeddialnaEncyklopedia()
        {
            return "Multimedialna Encyklopedia Techniki Mr Hammera (GS)";
        }

        private string NarzedziaDomowe()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Klucz francuski 15g",
                2 => "Kluczc nasadowy 5g",
                3 => "Kłodka i klucz 5g",
                4 => "Kombinerki 10g",
                5 => "Komplet kluczy 8g",
                6 => "Krążek taśmy izolacyjnej 5g",
                7 => "Latarka 10g",
                8 => "Młotek 8g",
                9 => "Mocne rękawice robocze 5g",
                10 => "NNożyk do strugania 5g",
                11 => "Papier ścierny 2g",
                12 => "Piłka do metalu 10g",
                13 => "Pudełeczko z wiertłami 5g",
                14 => "Pudełko z gwoździami 5g",
                15 => "Pudełko ze śrubkami 5g",
                16 => "Słoik mocnego kleju 10g",
                17 => "Śrubokręt 2g",
                18 => "Wiertarka 20g",
                19 => "Zwój drutu 5g",
                _ => RzadszeNarzedziaDomowe()
            };
        }

        private string RzadszeNarzedziaDomowe()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Hebel 4g",
                2 => "Hebel 4g",
                3 => "Lewarek samochodowy 10g",
                4 => "Lewarek samochodowy 10g",
                5 => "Lutownica i lut 30g",
                6 => "Lutownica i lut 30g",
                7 => "Lutownica i lut 30g",
                8 => "Pilnik 3g",
                9 => "Pilnik 3g",
                10 => "Pilnik 3g",
                11 => "Piła do drewna 6g",
                12 => "Piła do drewna 6g",
                13 => "Poziomica 2g",
                14 => "Poziomica 2g",
                15 => "Puszka smaru 10g",
                16 => "Puszka smaru 10g",
                17 => "Puszka smaru 10g",
                18 => "Siekiera 10g",
                19 => "Siekiera 10g",
                _ => "Siekiera 10g"
            };
        }

        private string NarzedziaWarsztatowe()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => AkcesoriaRemontowe(),
                2 => "Dmuchawa 5g",
                3 => $"Elektrody spawalnicze ({dice.Roll} sztuk) 3g/szt",
                4 => "Kompresor 15g",
                5 => "Komputerowe urządzenia diagnostyczne 20g",
                6 => "Kowadło 3g",
                7 => "Mocne rękawice robocze 5g",
                8 => NarzedziaDomowe(),
                9 => "Nitownica i nity 40g",
                10 => "Palnik acetylenowo-tlenowy 20g",
                11 => "Pilarka 10g",
                12 => "Pompa 10g",
                13 => "Prostownik 10g",
                14 => "Puszka lakieru w losowym kolorze 5g",
                15 => "Smarownica 8g",
                16 => "Spawarka 40g",
                17 => "Sprężarka 15g",
                18 => "Szlifierka 15g",
                19 => "Wkrętarka 1g",
                _ => "Zwój kabla 3g"
            };
        }

        private string NiespodziankaZWraku()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Akumulator 30g",
                2 => "Alarm samochodowy 10g",
                3 => Bagaz(),
                4 => "Butelka oleju silnikowego 6g",
                5 => "Butelka oleju silnikowego 6g",
                6 => "Car audio 15g",
                7 => "CB radio 40g",
                8 => DomowySprzetSportowy(),
                9 => DrobiazgDomowy(),
                10 => DrobiazgHobbystyczny(),
                11 => ElektronicznaMapa(),
                12 => "Klucz do kół 3g",
                13 => "Klucz do kół 3g",
                14 => "Lewarek 10g",
                15 => NarzedziaDomowe(),
                16 => $"Paliwo w baku ({dice.Roll} litrów) 3g/l",
                17 => "Prostownik 10g",
                18 => "Stalowa linka holownicza z hakiem 10g",
                19 => Zakupy(),
                _ => Zwloki()
            };
        }

        private string OzdobaArchitektoniczna()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Bogato rzeźbiona rozeta 5g",
                2 => "Bogato rzeźbiona rozeta 5g",
                3 => "Bogato rzeźbiona rozeta 5g",
                4 => "Płyta z płaskorzeźbą 10g",
                5 => "Płyta z płaskorzeźbą 10g",
                6 => "Płyta z płaskorzeźbą 10g",
                7 => "Rzeźba wolnostojąca 15g",
                8 => "Rzeźba wolnostojąca 15g",
                9 => "Rzeźba wolnostojąca 15g",
                10 => "Rzeźba wolnostojąca 15g",
                11 => "Rzeźba wolnostojąca 15g",
                12 => "Rzeźba wolnostojąca 15g",
                13 => "Rzeźba wolnostojąca 15g",
                14 => "Rzeźba ozdobna 10g",
                15 => "Rzeźba ozdobna 10g",
                16 => "Rzeźba ozdobna 10g",
                17 => "Rzeźba ozdobna 10g",
                18 => "Rzeźba ozdobna 10g",
                19 => "Witraż 5g",
                _ => "Witraż 5g"
            };
        }

        private string OzdobneAkcesoriaKoscielne()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Biblia 20g",
                2 => "Bogate szaty liturgiczne 15g",
                3 => "Bogato zdobiona mała szafka 15g",
                4 => "Bogato zdobiona mała szafka 15g",
                5 => "Bogato zdobiona mała szafka 15g",
                6 => "Bogato zdobiona monstrancja 15g",
                7 => "Bogato zdobiona monstrancja 15g",
                8 => "Bogato zdobiony krzyż 15g",
                9 => "Bogato zdobiony krzyż 15g",
                10 => "Bogato zdobiony krzyż 15g",
                11 => "Bogato zdobiony świecznik 15g",
                12 => "Bogato zdobiony świecznik 15g",
                13 => "Bogato zdobiony świecznik 15g",
                14 => "Bogato zdobiony żyrandol 10g",
                15 => "Bogato zdobiony żyrandol 10g",
                16 => "Bogato zdobiony żyrandol 10g",
                17 => "Świeca - gromnica 5g",
                18 => "Świeca - gromnica 5g",
                19 => "Świeca - gromnica 5g",
                _ => $"Zdobiona skarbonka z monetami ({dice.Roll} sztuk)6g"
            };
        }

        private string RekwizytTeatralny()
        {
            Dice dice = new(10);
            return dice.Roll switch
            {
                1 => "Element scenografii - koń na biegunnach 6g",
                2 => "Element scenografii - sztuczcne słońce 3g",
                3 => "Kostium - dawny strój wieczorowy 20g",
                4 => "Kostium anioła 5g",
                5 => "Kostium błazna 5g",
                6 => "Kostium damy dworu 10g",
                7 => "Kostium diabła 5g",
                8 => "Kostium króla 6g",
                9 => "Kostium panicza 5g",
                _ => "Kostium rycerza 6g"
            };
        }

        private string Sejf()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Sejf: " + BronUszkodzona(),
                2 => "Sejf: " + BronUszkodzona(),
                3 => "Sejf: " + CennaKolekcja(),
                4 => "Sejf: " + CennaKolekcja(),
                5 => "Sejf: " + CennaKolekcja(),
                6 => "Sejf: " + CennaKolekcja(),
                7 => "Sejf: " + DzieloSztuki(),
                8 => "Sejf: " + DzieloSztuki(),
                9 => "Sejf: " + $"Przedwojenne banknoty ({dice.Roll} plików)",
                10 => "Sejf: " + $"Przedwojenne banknoty ({dice.Roll} plików)",
                11 => "Sejf: " + $"Przedwojenne banknoty ({dice.Roll} plików)",
                12 => "Sejf: " + $"Przedwojenne banknoty ({dice.Roll} plików)",
                13 => "Sejf: " + $"Przedwojenne banknoty ({dice.Roll} plików)",
                14 => "Sejf: " + $"Przedwojenne banknoty ({dice.Roll} plików)",
                15 => "Sejf: " + $"Przedwojenne banknoty ({dice.Roll} plików)",
                16 => "Sejf: " + "Ważne dokumenty",
                17 => "Sejf: " + "Ważne dokumenty",
                18 => "Sejf: " + "Ważne dokumenty",
                19 => "Sejf: " + "Ważne dokumenty",
                _ => "Sejf: " + "Ważne dokumenty"
            };
        }

        private string SprzetGasniczy()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Alarm - czujnik dymu 1g",
                2 => "Alarm - czujnik dymu 1g",
                3 => "Alarm - lampa ostrzegawcza 3g",
                4 => "Alarm - syrena 10g",
                5 => "Duża gaśnica pianowa 10g",
                6 => "Duża gaśnica śniegowa 10g",
                7 => "Mała gaśnica pianowa 6g",
                8 => "Mała gaśnica pianowa 6g",
                9 => "Mała gaśnica pianowa 6g",
                10 => "Mała gaśnica pianowa 6g",
                11 => "Mała gaśnica śniegowa 16g",
                12 => "Mała gaśnica śniegowa 16g",
                13 => "Mała gaśnica śniegowa 16g",
                14 => "Mała gaśnica śniegowa 16g",
                15 => "Siekiera strażacka 20g",
                16 => "Zwój węża strażackiego 3g",
                17 => "Zwój węża strażackiego 3g",
                18 => "Zwój węża strażackiego 3g",
                19 => "Zwój węża strażackiego 3g",
                _ => "Zwój węża strażackiego 3g"
            };
        }

        private string SprzetMedyczny()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Ciśnieniomierz 5g",
                2 => "Fartuch 5g",
                3 => "Kaftan bezpieczeństwa 10g",
                4 => "Komplet nowej bielizny 3g",
                5 => "Kule (do chodzenia) 5g",
                6 => "Lampa chirurgiczna 10g",
                7 => "Maseczka 2g",
                8 => "Nosze 5g",
                9 => "Rękawiczki lateksowe (50 sztuk) 10g",
                10 => "Piła chirurgiczna 12g",
                11 => $"Woda destylowana ({dice.Roll} litrów) 1g/5l",
                12 => "Rehabilitacyjny stepper 10g",
                13 => RzadszySprzetMedyczny(),
                14 => "Skalpel 5g",
                15 => "Stetoskop 8g",
                16 => "Szkło laboratoryjne 10g",
                17 => "Termometr 5g",
                18 => "Waga lekarska 15g",
                19 => "Wózek inwalidzki 10g",
                _ => "Zestaw do kroplówki 12g"
            };
        }

        private string RzadszySprzetMedyczny()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Bieżnia rehabilitacyjna 10g",
                2 => "Bieżnia rehabilitacyjna 10g",
                3 => "Lodówka 10g",
                4 => "Lodówka 10g",
                5 => "Mikroskop 50g",
                6 => "Respirator 30g",
                7 => "Respirator 30g",
                8 => "Tlen (butla 10l) 10g",
                9 => "Tlen (butla 10l) 10g",
                10 => "Urządzenie do prześwielteń RTG 30g",
                11 => "Urządzenie do prześwielteń RTG 30g",
                12 => "Wirówka laboratoryjna 15g",
                13 => "Zestaw do narkozy 15g",
                14 => "Zestaw do narkozy 15g",
                15 => "Zestaw EKG 25g",
                16 => "Zestaw EKG 25g",
                17 => "Zestaw przyrządów chirurgicznych 20g",
                18 => "Zestaw przyrządów chirurgicznych 20g",
                19 => "Zestaw USG 25g",
                _ => "Zestaw USG 25g"
            };
        }

        private string SprzetSceniczny()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Duży głośnik sceniczny 8g",
                2 => "Duży głośnik sceniczny 8g",
                3 => "Duży głośnik sceniczny 8g",
                4 => "Duży głośnik sceniczny 8g",
                5 => "Duży głośnik sceniczny 8g",
                6 => "Duży głośnik sceniczny 8g",
                7 => "Dymiarka sceniczna 6g",
                8 => "Działko do konfetti 6g",
                9 => "Konsola dźwiękowca 10g",
                10 => "Megafon",
                11 => "Megafon",
                12 => "Mikrofon sceniczny",
                13 => "Mikrofon sceniczny",
                14 => "Mikrofon sceniczny",
                15 => "Mikrofon sceniczny",
                16 => "Rama z oświetleniem scenicznym (lampy, halogeny, stroboskopy) 20g",
                17 => "Rama z oświetleniem scenicznym (lampy, halogeny, stroboskopy) 20g",
                18 => "Rama z oświetleniem scenicznym (lampy, halogeny, stroboskopy) 20g",
                19 => "Rama z oświetleniem scenicznym (lampy, halogeny, stroboskopy) 20g",
                _ => "Reflektor laserowy 20g"
            };
        }

        private string SprzetSportowy()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Buty korkowe 10g",
                2 => "Gwizdek 2g",
                3 => "Hełm ochronny do rugby 6g",
                4 => "Kij baseballowy 5g",
                5 => "Kij baseballowy 5g",
                6 => "Kij golfowy 5g",
                7 => "Ochraniacze 5g",
                8 => "Piłeczka baseballowa 3g",
                9 => "Piłeczka golfowa 3g",
                10 => "Piłka do piłki nożnej 10g",
                11 => "Piłka do koszykówki 10g",
                12 => "Piłka tenisowa 5g",
                13 => "Płetwy 4g",
                14 => "Pompka 3g",
                15 => "Rakieta tenisowa 4g",
                16 => "Rękawice bramkarskie 3",
                17 => "Siatka bramkowa 5g",
                18 => "Siatka do siatkówki 5g",
                19 => "Strój rugby 15g",
                _ => "Sztanga i talerze do podnoszenia ciężarów 8g"
            };
        }

        private string SrodkiCzystosci()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Mydło w płynie 3g",
                2 => "Mydło w płynie 3g",
                3 => "Płyn do mycia naczcyń 3g",
                4 => "Płyn do mycia naczcyń 3g",
                5 => "Płyn do mycia naczcyń 3g",
                6 => "Proszek do czcyszczenia 4g",
                7 => "Proszek do czcyszczenia 4g",
                8 => "Proszek do prania 5g",
                9 => "Proszek do prania 5g",
                10 => "Rozpuszczalnik 4g",
                11 => "Silny płyn do czyszczenia 5g",
                12 => "Silny płyn do czyszczenia 5g",
                13 => "Silny płyn do czyszczenia 5g",
                14 => "Środek do czyszczenia w sprayu 3g",
                15 => "Środek do czyszczenia w sprayu 3g",
                16 => "Zmywacz 4g",
                17 => "Zmywacz 4g",
                18 => "Żrący płyn do czyszczenia ceramiki 5g",
                19 => "Żrący płyn do czyszczenia ceramiki 5g",
                _ => "Żrący płyn do czyszczenia ceramiki 5g"
            };
        }

        private string TasmaPermo()
        {
            return "Taśma Permo (GS) 10g";
        }

        private string TesterHopkinsa()
        {
            return "Tester Hopkinsa (GS) 25g";
        }

        private string TowarZeSklepiku()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Butelka wina 10g",
                2 => "Butellka wódki 10g",
                3 => Gazeta(),
                4 => Gazeta(),
                5 => $"Jednorazowe porcje kawy rozpuszczalnej ({dice.Roll} sztuk) 1g/5szt",
                6 => Mapa(),
                7 => $"Paczka gum do żucia ({dice.Roll} sztuk) 1g/3szt",
                8 => $"Paczka gum do żucia ({dice.Roll} sztuk) 1g/3szt",
                9 => $"Paczka kawy ({dice.Roll} porcji) 1g/5p",
                10 => $"Paczka papierosów ({dice.Roll} sztuk) 1g/szt",
                11 => $"Paczka tabletek przeciwbólowych ({dice.Roll} tabletek) 3g/szt",
                12 => "Puszka energy-drink 4g",
                13 => "Puszka piwa 5g",
                14 => "Tabliczka czekolady 8g",
                15 => "Zapalniczka 8g",
                16 => "Mocna reklamówka 1g",
                17 => ButelkiIPuszki(),
                18 => ButelkiIPuszki(),
                19 => SlodyczeIUzywki(),
                _ => SlodyczeIUzywki()
            };
        }

        private string TowarZeStraganu()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Kostium diabła 3g",
                2 => "Lizak 3g",
                3 => "Lizak 3g",
                4 => "Maska halloweenowa 2g",
                5 => $"Opakowanie balonów ({dice.Roll} sztuk) 1g/10szt",
                6 => $"Opakowanie balonów ({dice.Roll} sztuk) 1g/10szt",
                7 => "Opakowanie zimnych ogni 2g",
                8 => "Paczka korków / kapiszonów 2g",
                9 => "Peruka imprezowa 3g",
                10 => "Waga sklepowa 15g",
                11 => Zabawka(),
                12 => Zabawka(),
                13 => "Paczka wafli 3g",
                14 => "Zabawkowa broń 1g",
                15 => "Paczka cukierków 5g",
                16 => "Paczka cukierków 5g",
                17 => "Paczka orzechów 4g",
                18 => "Paczka orzechów 4g",
                19 => "Paczka bakalii 4g",
                _ => "Paczka bakalii 4g"
            };
        }

        private string UrzadzenieKuchenne()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Czajnik elektryczny 8g",
                2 => "Czajnik elektryczny 8g",
                3 => "Czajnik elektryczny 8g",
                4 => "Ekspres do kawy 7g",
                5 => "Ekspres do kawy 7g",
                6 => FiltrWody(),
                7 => "Grill elektryczny 5g",
                8 => "Grzałka 3g",
                9 => "Iskrownik 3g",
                10 => "Kuchenka elektryczna 10g",
                11 => "Kuchanka mikrofalowa 10g",
                12 => "Mała lodówka 10g",
                13 => "Mała lodówka 10g",
                14 => "Mikser 2g",
                15 => "Ostrzałka do noży 5g",
                16 => "Ostrzałka do noży 5g",
                17 => "Toster 2g",
                18 => "Toster 2g",
                19 => "Zamrażarka przenośna 10g",
                _ => "Krajalnica do chleba 2g"
            };
        }

        private string WyposazenieDomowe()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Adapter 20g",
                2 => "Bojler elektryczny 10g",
                3 => "Dobry zamek w drzwiach plus klucz 3g",
                4 => Ekran(),
                5 => "Elektryczny grzejnik 15g",
                6 => "Lampka biurowa 10g",
                7 => "Lampka stojąca 10g",
                8 => "Lustro ścienne 5g",
                9 => "Mini głośnik 5g",
                10 => "Odbiornik satelitarny 3g",
                11 => "Odkuurzacz 10g",
                12 => "Odtwarzacz audio z radiem 20g",
                13 => "Odtwarzacz video 25g",
                14 => "Pralka 15g",
                15 => "Telewizor 3g",
                16 => "Waga 15g",
                17 => "Wentylator 5g",
                18 => "Wideofon 1g",
                19 => "Wzmacniacz 10g",
                _ => "Żyrandol 3g"
            };
        }

        private string WyposazenieKlubowe()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => $"Automat do gier ({dice.Roll} przedwojennych monet)",
                2 => $"Automat do gier ({dice.Roll} przedwojennych monet)",
                3 => $"Automat do gier ({dice.Roll} przedwojennych monet)",
                4 => $"Automat hazardowy ({dice.Roll} przedwojennych monet)",
                5 => $"Automat hazardowy ({dice.Roll} przedwojennych monet)",
                6 => $"Automat hazardowy ({dice.Roll} przedwojennych monet)",
                7 => "Zestaw kręgli 10g",
                8 => "Zestaw kręgli 10g",
                9 => "Kij bilardowy 3g",
                10 => "Kij bilardowy 3g",
                11 => "Kula do kręgli 5g",
                12 => "Kula do kręgli 5g",
                13 => $"Kule bilardowe ({dice.Roll} sztuk) 2g/szt",
                14 => $"Kule bilardowe ({dice.Roll} sztuk) 2g/szt",
                15 => "Stół bilardowy 20g",
                16 => "Stół bilardowy 20g",
                17 => "Stół bilardowy 20g",
                18 => "Stół do ruletki 20g",
                19 => "Stół do ruletki 20g",
                _ => "Ręczny wykrywacz metalu 15g"
            };
        }

        private string ZSzafy()
        {
            Dice dice = new();
            Dice dice5 = new(5);
            string type = dice5.Roll switch
            {
                1 => "na kobietę",
                2 => "na kobietę",
                3 => "na mężczyznę",
                4 => "na mężczyznę",
                _ => "na dziecko (1/4 podanej obok wartości)"
            };
            return dice.Roll switch
            {
                1 => Akcesoria(),
                2 => $"Bluzka {type} 4g",
                3 => $"Buty {type} 10g",
                4 => $"Buty zimowe {type} 10g",
                5 => Dodatki(),
                6 => Dodatki(),
                7 => $"Dres {type} 4g",
                8 => $"Garnitur 30g",
                9 => $"Kurtka wojskowa 20g",
                10 => $"Kurtka zimowa 15g",
                11 => $"Kurtka zimowa 15g",
                12 => $"Mocne spodnie {type} 15g",
                13 => $"Płaszcz {type} 8g",
                14 => $"Skórzana kurtka 20g",
                15 => $"Skórzane spodnie 20g",
                16 => $"Spodnie {type} 8g",
                17 => $"Spodnie {type} 8g",
                18 => $"Spodnie wojskowe 15g",
                19 => $"Suknia wieczorowa 20g",
                _ => $"Sweter 8g"
            };
        }

        private string Akcesoria()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Choinka świąreczna 5g",
                2 => "Koc 5g",
                3 => "Koc 5g",
                4 => "Koc 5g",
                5 => "Mocna torba 6g",
                6 => "Mocna torba 6g",
                7 => "Mocna torba 6g",
                8 => "Namiot 12g",
                9 => "Ozdobna kołdra jak nowa 3g",
                10 => "Plecak 8h",
                11 => "Plecak 8h",
                12 => "Plecak 8h",
                13 => "Poduszka jak nowa 3g",
                14 => "Skórzana torba 5g",
                15 => "Skórzana torba 5g",
                16 => "Skórzana torba 5g",
                17 => "Szczotka do czyszczenia ubrań 1g",
                18 => "Śpiwór 6g",
                19 => "Śpiwór 6g",
                _ => "Zasłona, firana 1g"
            };
        }

        private string Dodatki()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Czapka zimowa 6g",
                2 => "Czapka zimowa 6g",
                3 => "Czapka zimowa 6g",
                4 => "Kapelusz 5g",
                5 => "Krawat 1g",
                6 => "Okulary przeciwsłoneczne 5g",
                7 => "Okulary przeciwsłoneczne 5g",
                8 => "Pasek do spodni 5g",
                9 => "Pasek do spodni 5g",
                10 => "Pasek do spodni 5g",
                11 => "Peleryna przeciwdeszczowa 15g",
                12 => "Rękawice zimowe 4g",
                13 => "Rękawice zimowe 4g",
                14 => "Rękawice zimowe 4g",
                15 => "Szal 1g",
                16 => "Szal 1g",
                17 => "Szelki 2g",
                18 => "Pasta do impregnacji butów 5g",
                19 => "Pasta do impregnacji butów 5g",
                _ => "Kombinezon do sportów zimowych 15g"
            };
        }

        private string Zabawka()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Domek dla lalek 4g",
                2 => "Hamak 6g",
                3 => "Imitacja broni palnej 6g",
                4 => "Klocki 3g",
                5 => "Kredki 5g",
                6 => "Lalka 2g",
                7 => "Maska halloweenowa 2g",
                8 => "Maskotka 2g",
                9 => "Maskotka na baterię 4g",
                10 => "Model 3g",
                11 => "Piłka dziecięca 2g",
                12 => "Piszczałka 2g",
                13 => "Puzzle 5g",
                14 => "Skakanka 1g",
                15 => "Tarcza i rzutki 5g",
                16 => "Wózek dziecięcy 5g",
                17 => "Wskaźnik laserowy 2g",
                18 => "Zabawkowa broń 1g",
                19 => "Zdalnie sterowany model jeżdżący 20g",
                _ => "Zdalnie sterowany model latający 30g"
            };
        }

        private string ZabezpieczenieElektroniczne()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Czujnik ruchu 10g",
                2 => "Czujnik ruchu 10g",
                3 => "Czujnik ruchu 10g",
                4 => "Czujnik ruchu 10g",
                5 => "Fotokomówka 3g",
                6 => "Fotokomówka 3g",
                7 => "Fotokomówka 3g",
                8 => "Fotokomówka 3g",
                9 => "Głośnik alarmowy 10g",
                10 => "Głośnik alarmowy 10g",
                11 => "Głośnik alarmowy 10g",
                12 => "Głośnik alarmowy 10g",
                13 => "Kamera (elektronika) 10g",
                14 => "Kamera (elektronika) 10g",
                15 => "Kamera (elektronika) 10g",
                16 => "Kamera (elektronika) 10g",
                17 => "Mikrofon (elektronika) 3g",
                18 => "Mikrofon (elektronika) 3g",
                19 => "Mikrofon (elektronika) 3g",
                _ => "Mikrofon (elektronika) 3g"
            };
        }

        private string Zakupy()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => AkcesoriaKuchenne(),
                2 => AkcesoriaToaletowe(),
                3 => AkcesoriaToaletowe(),
                4 => ArtykulPapierniczy(),
                5 => ArtykulPapierniczy(),
                6 => DomowaApteczka(),
                7 => DomowaApteczka(),
                8 => DrobiazgHobbystyczny(),
                9 => Gazeta(),
                10 => Gazeta(),
                11 => Gazeta(),
                12 => Ksiazka(),
                13 => SrodkiCzystosci(),
                14 => TowarZeSklepiku(),
                15 => TowarZeSklepiku(),
                16 => TowarZeSklepiku(),
                17 => Zabawka(),
                18 => ZywnoscIUzywki(),
                19 => ZywnoscIUzywki(),
                _ => ZywnoscIUzywki()
            };
        }

        private string ZdalnieSterowanyModel()
        {
            return "Zdalnie sterowany model (GS) 30g";
        }

        private string ZestawDoCharakteryzacji()
        {
            return "Zestaw do charakteryzacji (GS) 20g";
        }

        private string Zwloki()
        {
            Dice dice = new();
            return "Zwłoki: " + dice.Roll switch
            {
                1 => Bagaz(),
                2 => Bizuteria(),
                3 => "Dobre buty 10g",
                4 => "Kluczyki do samochodu 0g",
                5 => "Mini odtwarzacz audio i słuchawki 10g",
                6 => "Mini odtwarzacz audio i słuchawki 10g",
                7 => "Okulary 3g",
                8 => $"Paczka papierosów ({dice.Roll} sztuk) 1g/szt",
                9 => $"Paczka papierosów ({dice.Roll} sztuk) 1g/szt",
                10 => "Parasol 6g",
                11 => "Plecak 8g",
                12 => "Skórzana torba 5g",
                13 => "Telefon osobisty 1g",
                14 => "Telefon osobisty 1g",
                15 => "Telefon osobisty 1g",
                16 => Zakupy(),
                17 => Zakupy(),
                18 => "Zapalniczka 8g",
                19 => "Zegarek 8g",
                _ => "Pasek 5g"
            };
        }

        private string Bizuteria()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Bransoletka 3g",
                2 => "Bransoletka 3g",
                3 => "Kolczyki 3g",
                4 => "Kolczyki 3g",
                5 => "Obrączka 3g",
                6 => "Obrączka 3g",
                7 => "Obrączka 3g",
                8 => "Obrączka 3g",
                9 => "Obrączka 3g",
                10 => "Obrączka 3g",
                11 => "Obrączka 3g",
                12 => "Obrączka 3g",
                13 => "Pierścionek 3g",
                14 => "Pierścionek 3g",
                15 => "Spinka 3g",
                16 => "Spinka 3g",
                17 => "Sygnet 3g",
                18 => "Sygnet 3g",
                19 => "Wisiorek 3g",
                _ => "Wisiorek 3g"
            };
        }

        private string Portfel()
        {
            Dice dice = new();
            return $"Portfel: Przedwojenne banknoty ({dice.Roll} sztuk), przedwojenny bilon ({dice.Roll} sztuk), śmieci (karty kredytowe, zdjęcia, przepustki, legitymacje, dowód osobisty), klucze do domu";
        }

        private string Zarowka()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Zwykła żarówka 3g",
                2 => "Zwykła żarówka 3g",
                3 => "Zwykła żarówka 3g",
                4 => "Zwykła żarówka 3g",
                5 => "Zwykła żarówka 3g",
                6 => "Zwykła żarówka 3g",
                7 => "Zwykła żarówka 3g",
                8 => "Zwykła żarówka 3g",
                9 => "Zwykła żarówka 3g",
                10 => "Żarówka LED 3g",
                11 => "Żarówka LED 3g",
                12 => "Żarówka LED 3g",
                13 => "Żarówka LED 3g",
                14 => "Żarówka LED 3g",
                15 => "Żarówka LED 3g",
                16 => "Żarówka LED 3g",
                17 => "Żarówka LED 3g",
                18 => "Żarówka jarzeniowa 3g",
                19 => "Żarówka jarzeniowa 3g",
                _ => "Żarówka halogenowa 4g"
            };
        }

        private string ZywnoscIUzywki()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => ButelkiIPuszki(),
                2 => ButelkiIPuszki(),
                3 => Konserwy(),
                4 => Konserwy(),
                5 => $"Paczka cukru ({dice.Roll} porcji) 1g/5p",
                6 => $"Paczka cukru ({dice.Roll} porcji) 1g/5p",
                7 => $"Paczka cukru ({dice.Roll} porcji) 1g/5p",
                8 => "Paczka mąki 4g",
                9 => "Paczka mąki 4g",
                10 => "Paczka mąki 4g",
                11 => $"Paczka soli ({dice.Roll} porcji) 1g/5p",
                12 => $"Paczka soli ({dice.Roll} porcji) 1g/5p",
                13 => SlodyczeIUzywki(),
                14 => SlodyczeIUzywki(),
                15 => "Torebka pieprzu 2g",
                16 => "Torebka pierpzu 2g",
                17 => "Torebka przypraw 2g",
                18 => "Torebka przypraw 2g",
                19 => TrwalaZywnosc(),
                _ => TrwalaZywnosc()
            };
        }

        private string ButelkiIPuszki()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Butelka denaturatu 3g",
                2 => "Butelka denaturatu 3g",
                3 => "Butelka likieru 6g",
                4 => "Butelka niegazowanej wody 1g",
                5 => "Butelka niegazowanej wody 1g",
                6 => "Butelka octu 3g",
                7 => "Butelka octu 3g",
                8 => "Butelka oleju 3g",
                9 => "Butelka oleju 3g",
                10 => "Butelka oleju 3g",
                11 => "Butelka oleju 3g",
                12 => "Butelka piwa 5g",
                13 => "Butelka spirytusu 12g",
                14 => "Butelka spirytusu 12g",
                15 => "Butelka whisky 10g",
                16 => "Butelka whisky 10g",
                17 => "Butelka wina 10g",
                18 => "Butelka wina 10g",
                19 => "Butelka wódki 10g",
                _ => "Butelka wódki 10g"
            };
        }

        private string Konserwy()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Konserwa - owoce 4g",
                2 => "Konserwa - owoce 4g",
                3 => "Konserwa - owoce 4g",
                4 => "Konserwa - smalec 6g",
                5 => "Konserwa - smalec 6g",
                6 => "Konserwa - smalec 6g",
                7 => "Konserwa - warzywa 4g",
                8 => "Konserwa - warzywa 4g",
                9 => "Konserwa - warzywa 4g",
                10 => "Konserwa - warzywa 4g",
                11 => "Konserwa mięsna 5g",
                12 => "Konserwa mięsna 5g",
                13 => "Konserwa mięsna 5g",
                14 => "Konserwa mięsna 5g",
                15 => "Konserwa mięsna 5g",
                16 => "Konserwa rybna 4g",
                17 => "Konserwa rybna 4g",
                18 => "Konserwa rybna 4g",
                19 => "Konserwa rybna 4g",
                _ => "Konserwa rybna 4g"
            };
        }

        private string SlodyczeIUzywki()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Batonik 3g",
                2 => "Batonik 3g",
                3 => "Chipsy 2g",
                4 => "Lizak 3g",
                5 => "Lizak 3g",
                6 => $"Paczka gum do żucia ({dice.Roll} sztuk) 1g/3szt",
                7 => $"Paczka gum do żucia ({dice.Roll} sztuk) 1g/3szt",
                8 => "Paczka herbatników 4g",
                9 => $"Paczka kawy ({dice.Roll} sztuk) 1g/5p",
                10 => "Paczka landrynek 5g",
                11 => "Paczka landrynek 5g",
                12 => $"Paczka papierosów ({dice.Roll} sztuk) 1g/szt",
                13 => "Paczka wafli \"Pryncypałki\" 3g",
                14 => $"Pudełko herbaty ({dice.Roll} porcji) 1g/10p",
                15 => "Puszka energy drink 4g",
                16 => "Puszka piwa 5g",
                17 => "Słodzik 3g",
                18 => "Słoik marmolady 5g",
                19 => "Słoik suchego napoju 6g",
                _ => "Tabliczka czekolady 8g"
            };
        }

        private string TrwalaZywnosc()
        {
            Dice dice = new();
            return dice.Roll switch
            {
                1 => "Danie w proszku 4g",
                2 => "Paczka fasoli 6g",
                3 => "Paczka fasoli 6g",
                4 => "Paczka kaszy 6g",
                5 => "Paczka kaszy 6g",
                6 => "Paczka kukurydzy suszonej 6g",
                7 => "Paczka kukurydzy suszonej 6g",
                8 => "Paczka makaronu 6g",
                9 => "Paczka makaronu 6g",
                10 => "Paczka ryżu 6g",
                11 => "Paczka ryżu 6g",
                12 => "Paczka sucharków 5g",
                13 => "Paczka sucharków 5g",
                14 => "Paczka suszonych grzybów 3g",
                15 => "Płatki kukurydziane 4g",
                16 => "Płatki kukurydziane 4g",
                17 => "Suche pieczywo 5g",
                18 => "Suche pieczywo 5g",
                19 => "Zupa w proszku 4g",
                _ => "Zupa w proszku 4g"
            };
        }

        #endregion Przedmioty

        private string Stan()
        {
            return new Dice(4).Roll switch
            {
                1 => ", w dobrym stanie\n",
                2 => ", zabrudzone\n",
                3 => ", wadliwe\n",
                _ => ", uszkodzone\n"
            };
        }
    }
}