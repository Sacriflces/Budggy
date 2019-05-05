using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Budggy
{
    public class myDateTime : IComparable
    {
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            myDateTime otherDateTime = obj as myDateTime;
            return Compare(this, otherDateTime);
        }
        public static int Compare(myDateTime lhs, myDateTime rhs)
        {
            if(lhs.Year < rhs.Year)
            {
                return -1;
            }
            else if(lhs.Year > rhs.Year)
            {
                return 1;
            }
            else
            {
                if (lhs.Month < rhs.Month)
                {
                    return -1;
                }
                else if (lhs.Month > rhs.Month)
                {
                    return 1;
                }
                else
                    return 0;
            }
        }
        public myDateTime()
        {

        }
    
        public myDateTime(int month, int year)
        {
            Month = month;
            Year = year;
        }
        public myDateTime(DateTime date)
        {
            Month = date.Month;
            Year = date.Year;
        }
        public int Month;
        public int Year;
    }

    public class MonthBudget : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
        
        void OnPropertyChanged(string propertyName)
        {
           if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public double BudgetVal { get; set; }

        private double val;
        public double Value { get { return val; }
            set {
                val = value;
                OnPropertyChanged("Value");
                }
        }

        private double incAmount;
        public double IncAmount { get { return incAmount; }
            set
            {
                incAmount = value;
                OnPropertyChanged("IncAmount");
            }
        }

        private double expAmount;
        public double ExpAmount {
            get { return expAmount; }
            set
            {
                expAmount = value;
                OnPropertyChanged("ExpAmount");
            }
        }

        public myDateTime Date { get; set; }
        public string MonthStr { get; set; }
        public int YearInt { get; set; }

        public MonthBudget(double value, int month, int year)
        {
            Value = value;
            BudgetVal = value;
            Date = new myDateTime(month, year);
            MonthStr = MonthConvert(Date.Month);
            YearInt = Date.Year;
            IncAmount = 0;
            ExpAmount = 0;
        }

        internal void SubtractExpense(Expense exp)
        {
                Value -= exp.Value;
                ExpAmount += exp.Value;
          
        }

        internal void RemoveExpense(Expense exp)
        {
            Value += exp.Value;
            ExpAmount -= exp.Value;
        }

        internal void AddIncome(Income inc)
        {           
                IncAmount += inc.Value;
           
        }

        internal void RemoveIncome(Income inc)
        {
            IncAmount -= inc.Value;
        }

        internal void NewBudget(double newVal)
        {
            double diff = BudgetVal - Value;
            BudgetVal = newVal;
            Value = newVal - diff;
        }

        string MonthConvert(int num)
        {
            switch (num){
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
                default:
                    return "Really bro???";
            }
        }        
    }
}
