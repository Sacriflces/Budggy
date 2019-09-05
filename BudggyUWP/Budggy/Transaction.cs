using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budggy
{
    public class Transaction 
    {
        public string ValueStr { get; set; }
        private decimal _value;
        public decimal Value { get { return _value; }
            set
            {
                _value = value;
                ValueStr = String.Format("{0:C}", value);
            } }

        public string Description { get; set; }
        public string Bin { get; set; }
        internal int BinID { get; set; }
        // add BinID and set it to -1 if the income is split. and change Split to Split Among Bins 
        // Possibly add TransactionID that'll goes by the date
        internal int TransactionID { get; set; }
        private DateTime _date;
        public DateTime Date { get { return _date; }
            set {
                _date = value;
                DateStr = _date.ToString("d");
            }
        }
        public string DateStr { get; set; }

        //private decimal _percentage;
        //public decimal Percentage
        //{
        //    get { return _percentage; }
        //    set
        //    {
        //        if (value <= 1)
        //            _percentage = value;
        //        else if (value > 1 && value <= 100)
        //            _percentage = value / 100;
        //        else
        //            _percentage = 0;
        //    }
        //}

        public string DrawerGoal = string.Empty;
        public bool DrawerExp = true;
        public bool IncomeSplit = false;
        public string IncomeString;
        public int DrawerGoalID;

        public Transaction()
        {
            Value = 0;
        }

        public Transaction(decimal value, string destr, string bin, int binID, DateTime date)
        {
            Value = value; // Math.Round(value, 2, MidpointRounding.AwayFromZero);       
            Description = destr;
            Bin = bin;
            BinID = binID;
            Date = date; //new myDateTime(date);
           // DateStr = date.ToString("d");           
            DrawerExp = true;
        }

        public Transaction(RepeatTransaction transaction)
        {
            Value = transaction.Value;
            Description = transaction.Description;
            Bin = transaction.Bin;
            BinID = transaction.BinID;
            Date = transaction.Date;
            DrawerGoal = transaction.DrawerGoal;
            DrawerExp = transaction.DrawerExp;
            IncomeSplit = transaction.IncomeSplit;
            DrawerGoalID = transaction.DrawerGoalID;
        }
       

        public void RemoveBin(int destID, int currID)
        {
            string[] binStrs = IncomeString.Split('_');
            string currBinStr = null;
            string destBinStr = null;
            char[] separators = { '-', '(' };
            foreach (string str in binStrs)
            {
                if (str.Contains(destID.ToString())) destBinStr = str;
                else if (str.Contains(currID.ToString())) currBinStr = str;
            }

            decimal currPerc = Convert.ToDecimal(currBinStr.Split(separators)[1]);
            decimal destPerc = Convert.ToDecimal(destBinStr.Split(separators)[1]);
            string destPercStr = destPerc.ToString();
            destPerc += currPerc;

            IncomeString.Replace(destID.ToString() + "-" + destPercStr, destID.ToString() + "-" + destPerc.ToString());
            IncomeString.Replace(currBinStr, "");
        }

        public virtual bool IsIncome() { return false;  } //make this abstract

        public virtual string GetDrawer() { return ""; }

        public virtual bool GetDrawerExp() { return false; }
    }
}
