using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace NeuroshimaDB.Models
{
    public class Article
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int PriceDefault { get; set; }
        public int Availability { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }

        public string ArticleTypeText
        {
            get
            {
                string output = "";
                switch (ArticleType)
                {
                    case ArticleType.paliwo:
                        output = "Paliwo";
                        break;

                    case ArticleType.elektronika:
                        output = "Elektronika";
                        break;

                    case ArticleType.mechanika:
                        output = "Mechanika";
                        break;

                    case ArticleType.prochy:
                        output = "Prochy";
                        break;

                    case ArticleType.bron:
                        output = "Broń";
                        break;

                    case ArticleType.zywnosc:
                        output = "Żywność";
                        break;

                    case ArticleType.uslugi:
                        output = "Usługi speców";
                        break;

                    default:
                        output = "Inne";
                        break;
                }
                return output;
            }
        }

        public ArticleType ArticleType { get; set; }

        public string this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return Name;

                    case 1:
                        return ArticleTypeText;

                    case 2:
                        return Keywords;

                    case 3:
                        return Description;

                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public Article(string name, string price, string availability, string desc, string keywords, string articleType)
        {
            try
            {
                Id = Guid.NewGuid().ToString();
                Name = name;
                PriceDefault = int.Parse(price);
                Price = PriceDefault;
                Availability = int.Parse(availability);
                Description = desc;
                Keywords = keywords;
                ArticleType = articleType switch
                {
                    "paliwo" => ArticleType.paliwo,
                    "elektronika" => ArticleType.elektronika,
                    "mechanika" => ArticleType.mechanika,
                    "prochy" => ArticleType.prochy,
                    "bron" => ArticleType.bron,
                    "zywnosc" => ArticleType.zywnosc,
                    "uslugi" => ArticleType.uslugi,
                    _ => ArticleType.inne
                };
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }

    public enum ArticleType
    {
        inne,
        paliwo,
        elektronika,
        mechanika,
        prochy,
        bron,
        zywnosc,
        uslugi
    }
}