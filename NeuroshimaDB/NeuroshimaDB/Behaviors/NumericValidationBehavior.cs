using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace NeuroshimaDB.Behaviors
{
    public class NumericValidationBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        private static void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(args.NewTextValue))
            {
                if (args.NewTextValue.ToCharArray().All(x => x == '0'))
                {
                    ((Entry)sender).Text = "0";
                    return;
                }

                bool isNumber = args.NewTextValue.ToCharArray().All(x => char.IsDigit(x)); //Make sure all characters are numbers

                var text = isNumber ?
                    args.NewTextValue :
                    args.NewTextValue.Remove(args.NewTextValue.Length - 1);

                bool isWithinLimit = isNumber ?
                    int.Parse(text) <= 50 && int.Parse(text) >= 0 :
                    false;

                var number = isWithinLimit ?
                    text :
                    "50";

                ((Entry)sender).Text = number;
            }
        }
    }
}