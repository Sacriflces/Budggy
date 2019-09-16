using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Budggy
{
    public class RepeatTransaction :  Transaction//, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        override public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        override protected void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            if (!Setup) Account.UpdateRepeatTransaction(this);
        }

       // public Transaction Transaction;

        //private string _valueStr;
        //public string ValueStr
        //{
        //    get { return _valueStr; }
        //    set
        //    {
        //        _valueStr = value;
        //        OnPropertyChange("ValueStr");
        //    }
        //}

        //private string _description;
        //public string Description
        //{
        //    get { return _description; }
        //    set
        //    {
        //        _description = value;
        //        OnPropertyChange("Description");
        //    }
        //}

        //private string _bin;
        //public string Bin
        //{
        //    get { return _bin; }
        //    set
        //    {
        //        _bin = value;
        //        OnPropertyChange("ValueStr");
        //    }
        //}

        private int _freq;
        public int Frequency
        {
            get { return _freq; }
            set
            {
                _freq = value;
                OnPropertyChange("Frequency");
            }
        }
        private bool _monthly;
        public bool Monthly
        {
            get { return _monthly; }
            set
            {
                _monthly = value;
                OnPropertyChange("Monthly");
            }
        }
        
        public RepeatTransaction()
        {

        }

        public RepeatTransaction(Transaction transaction, int freq = 1, bool monthly = true)
        {
            Monthly = monthly;
            Frequency = freq;
            Value = transaction.Value;
            Description = transaction.Description;
            Bin = transaction.Bin;
            Date = transaction.Date;
            DrawerGoal = transaction.DrawerGoal;
            DrawerExp = transaction.DrawerExp;
            IncomeSplit = transaction.IncomeSplit;
            DrawerGoalID = transaction.DrawerGoalID;            
        }
    }
}
