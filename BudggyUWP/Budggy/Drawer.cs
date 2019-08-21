using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Budggy
{

    public class Drawer : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private decimal _availAmount;
        public decimal AvailAmount
        {
            get { return _availAmount; }
            set
            {
                _availAmount = value;
                OnPropertyChange("AvailAmount");
            }
        }

        private decimal _monthlyAmount;

        public decimal MonthlyAmount
        {
            get { return _monthlyAmount; }
            set
            {
                _monthlyAmount = value;
                OnPropertyChange("MonthlyAmount");
            }
        }
        private decimal _currentSpent;
        public decimal CurrentSpent
        {
            get { return _currentSpent; }
            set
            {
                _currentSpent = Math.Round(value, 2, MidpointRounding.AwayFromZero);
                OnPropertyChange("CurrentSpent");
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChange("Name");
            }
        }

        public string BinName;
        internal int BinID { get; set; }
        private int _month;
        public int Month
        {
            get { return _month; }
            set
            {
                _month = value;
                OnPropertyChange("Month");
            }
        }

        private int _year;
        public int Year
        {
            get { return _year; }
            set
            {
                _year = value;
                OnPropertyChange("Month");
            }
        }

        public bool rollOver;
        public int ID;

        public Drawer()
        {

        }

        public Drawer(string name, decimal monthAmount, DateTime date, string binName = null)
        {
            //Need to create strings to bind too... eventually
            BinName = binName;
            Name = name;
            AvailAmount = monthAmount;
            MonthlyAmount = monthAmount; //remove this later
            CurrentSpent = 0;
            Month = date.Month;
            Year = date.Year;
            rollOver = false;
        }

        private Drawer(Drawer prev)
        {
            BinName = prev.BinName;
            BinID = prev.BinID;
            Name = prev.Name;            
            MonthlyAmount = prev.MonthlyAmount;
            CurrentSpent = 0;
            Month = DateTime.Now.Month;
            Year = DateTime.Now.Year;
            rollOver = prev.rollOver;
            if (rollOver)
            {
                AvailAmount = MonthlyAmount + (prev.AvailAmount - prev.CurrentSpent);
            }
            else AvailAmount = MonthlyAmount;

        }

        public Drawer Refresh()
        { //make it so that the index maps based on the month 0 - January of that year, etc.
            //decimal AvailableAmount;
            //if (rollOver)
            //{
            //    AvailableAmount = MonthlyAmount + (AvailAmount - CurrentSpent);
            //}
            //else
            //    AvailableAmount = MonthlyAmount;           
            return new Drawer(this); 
                //Month = DateTime.Now.Month;
                //Year = DateTime.Now.Year;
                //CurrentSpent = 0;                   
        }

        public void AddExpense(Transaction transaction)
        {
            CurrentSpent -= transaction.Value;
        }

        public void RemoveExpense(Transaction transaction)
        {
            CurrentSpent += transaction.Value;
        }

    }
}
