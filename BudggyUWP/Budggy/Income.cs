using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budggy
{
     public class Income
    {
        public string ValueStr { get; set; }
        public double Value { get; set; }
       
        public string Description { get; set; }
        public string Bin { get; set; }
        public DateTime Date { get; set; }
        public string DateStr { get; set; }

        public Income(double value, string destr, string bin, DateTime date)
        {
            Value = value;
            ValueStr = String.Format("{0:C}", value);
            Description = destr;
            Bin = bin;
            Date = date;
            DateStr = date.ToString("d");
               
        }

        public override string ToString() 
        {
            return $"Income: {Value} {Bin} {DateStr}";
            
        }
    }
}
