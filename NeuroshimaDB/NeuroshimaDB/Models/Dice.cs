using System;
using System.Collections.Generic;
using System.Text;

namespace NeuroshimaDB.Models
{
    public class Dice
    {
        private int _dice;

        public int Roll
        {
            get
            {
                Random r = new();
                return r.Next(1, _dice + 1);
            }
        }

        public Dice(int dice = 20)
        {
            _dice = dice;
        }
    }
}