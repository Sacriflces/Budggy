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

        private decimal _maximum;
        public decimal Maximum
        {
            get { return _maximum; }
            set
            {
                _maximum = value;
                OnPropertyChange("Maximum");
            }
        }

        private decimal Max;

        private decimal _currentSpent;
        public decimal CurrentSpent
        {
            get { return _currentSpent; }
            set
            {
                _currentSpent = value;
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

        decimal[] prevSpent = new decimal[12];
        private int index;
        public bool rollOver;

        public Drawer()
        {

        }

        public Drawer(string name, decimal maximum, DateTime date, string binName = null)
        {
            //Need to create strings to bind too... eventually
            BinName = binName;
            Name = name;
            Maximum = maximum;
            Max = maximum;
            CurrentSpent = 0;
            Month = date.Month;
            Year = date.Year;
            rollOver = false;
            index = 0;
        }

        public void Refresh()
        {
            if (rollOver)
            {
                Maximum = Max + (Maximum - CurrentSpent);
            }
            else
                Maximum = Max;

            prevSpent[index++] = CurrentSpent;
            index %= prevSpent.Length;
            Month = DateTime.Now.Month;
            Year = DateTime.Now.Year;
        }

        public void AddExpense(Expense exp)
        {
            CurrentSpent += exp.Value;
        }

        public void RemoveExpense(Expense exp)
        {
            CurrentSpent -= exp.Value;
        }

    }
}
