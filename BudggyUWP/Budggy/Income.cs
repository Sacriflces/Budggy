using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budggy
{
     internal class Income
    {
        internal double Value { get; set; }
        internal string Description { get; set; }
        internal string Bin { get; set; }
        internal DateTime Date { get; set; }

      internal Income(double value, string destr, string bin, DateTime date)
        {
            Value = value;
            Description = destr;
            Bin = bin;
            Date = date;
        }
    }
}
