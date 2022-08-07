using Android.App;
using Android.OS;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;
using Android.Content.Res;

namespace NeuroshimaDB.Services
{
    public class ReadAsset
    {
        public string NsArticles
        {
            get
            {
                var stream = Application.Context.Assets.Open("NsArticles.txt");
                string content;
                using (StreamReader sr = new StreamReader(stream))
                {
                    content = sr.ReadToEnd();
                }
                return content;
            }
        }
    }
}