using System;
using System.Collections.Generic;
using System.Text;

namespace NeuroshimaDB.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp) =>
            source?.IndexOf(toCheck, comp) >= 0;
    }
}