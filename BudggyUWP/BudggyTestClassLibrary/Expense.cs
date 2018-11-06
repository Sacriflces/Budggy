using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budggy
{
    internal class Expense : Income
    {
        internal Expense(double value, string destr, DateTime date) : base(value, destr, date)
        {
            
        } 

        
    }
}
