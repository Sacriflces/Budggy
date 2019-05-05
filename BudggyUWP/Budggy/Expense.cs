using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budggy
{
    public class Expense : Income
    {
        public Expense() : base()
        {

        }
        public Expense(double value, string destr, string bin, DateTime date) : base(value, destr, bin, date)
        {
            
        } 

        
    }
}
