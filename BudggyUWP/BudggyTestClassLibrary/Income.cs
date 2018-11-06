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
        internal DateTime Date { get; set; }

      internal Income(double value, string destr, DateTime date)
        {
            Value = value;
            Description = destr;
            Date = date;
        }
    }
}
