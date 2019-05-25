using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budggy
{
     public class Income : Transaction
    {
        private decimal _percentage;
        public decimal Percentage
        {
            get { return _percentage; }
            set
            {
                if (value <= 1)
                   _percentage = value;
                else if (value > 1 && value <= 100)
                   _percentage = value / 100;
                else
                   _percentage = 0;
            }
        }

        public Income()
        {

        }

        public Income(decimal value, string destr, string bin, DateTime date) : base(value, destr, bin, date)
        {
            Percentage = 0;
        }

        public override string ToString() 
        {
            return $"Income: {Value} {Bin} {DateStr}";
            
        }

        public override bool IsIncome() 
        {
            return true;
        }
    }
}
