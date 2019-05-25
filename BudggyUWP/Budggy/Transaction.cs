using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budggy
{
    public abstract class Transaction
    {
        public string ValueStr { get; set; }
        public decimal Value { get; set; }

        public string Description { get; set; }
        public string Bin { get; set; }
        public myDateTime Date { get; set; }
        public string DateStr { get; set; }
        
        public Transaction()
        {

        }

        public Transaction(decimal value, string destr, string bin, DateTime date)
        {
            Value = value;
            ValueStr = String.Format("{0:C}", value);
            Description = destr;
            Bin = bin;
            Date = new myDateTime(date);
            DateStr = date.ToString("d");
        }

        public abstract bool IsIncome();
    }
}
