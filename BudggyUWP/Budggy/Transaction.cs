using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Budggy
{
    public class Transaction : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public virtual event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected virtual void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            if (!Setup) Account.UpdateTransaction(this);
        }
        public string ValueStr { get; set; }
        private decimal _value;
        public decimal Value { get { return _value; }
            set
            {
                _value = value;
                ValueStr = String.Format("{0:C}", value);
                OnPropertyChange("Value");
            } }

        private string _description;
        public string Description { get { return _description; }
            set
            {
                _description = value;
                OnPropertyChange("Description");
            } }

        private string _bin;
        public string Bin { get { return _bin; }
            set
            {
                _bin = value;
                OnPropertyChange("Bin");
            }
        }

        private int _binID;
        internal int BinID { get { return _binID; }
            set
            {
                _binID = value;
                OnPropertyChange("BinID");
            }
        }
        // add BinID and set it to -1 if the income is split. and change Split to Split Among Bins 
        // Possibly add TransactionID that'll goes by the date
        internal int TransactionID { get; set; }
        private DateTime _date;
        public DateTime Date { get { return _date; }
            set {
                _date = value;
                DateStr = _date.ToString("d");
                OnPropertyChange("Date");
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

        private string _drawerGoal = string.Empty;
        public string DrawerGoal { get { return _drawerGoal; }
            set
            {
                _drawerGoal = value;
                OnPropertyChange("DrawerGoal");
            }
        }
        public bool DrawerExp = true;
        public bool IncomeSplit = false;

        private string _incomeString;
        public string IncomeString { get { return _incomeString; }
            set
            {
                _incomeString = value;
                OnPropertyChange("IncomeString");
            }
        }

        private int _drawerGoalID;
        public int DrawerGoalID { get { return _drawerGoalID; }
            set
            {
                _drawerGoalID = value;
                OnPropertyChange("DrawerGoalID");
            }
        }
        internal bool Setup = true;
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
