using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budggy
{
    public class Transaction //make this abstract
    {
        public string ValueStr { get; set; }
        public decimal Value { get; set; }

        public string Description { get; set; }
        public string Bin { get; set; }
        private myDateTime _date;
        public myDateTime Date { get { return _date; }
            set {
                _date = value;
                DateStr = _date.ToString();
            }
        }
        public string DateStr { get; set; }
        
        public Transaction()
        {

        }

        public Transaction(decimal value, string destr, string bin, DateTime date)
        {
            Value = value; // Math.Round(value, 2, MidpointRounding.AwayFromZero);
            ValueStr = String.Format("{0:C}", value);
            Description = destr;
            Bin = bin;
            Date = new myDateTime(date);
            DateStr = date.ToString("d");
        }

        public virtual bool IsIncome() { return false;  } //make this abstract

        public virtual string GetDrawer() { return ""; }

        public virtual bool GetDrawerExp() { return false; }
    }
}
