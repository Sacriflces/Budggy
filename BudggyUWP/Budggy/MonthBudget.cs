using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Budggy
{
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

        internal double BudgetVal { get; set; }

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

        internal DateTime Month { get; set; }
        public string MonthStr { get; set; }
        public int YearInt { get; set; }

        internal MonthBudget(double value, int month, int year)
        {
            Value = value;
            BudgetVal = value;
            Month = new DateTime(year, month, 1);
            MonthStr = MonthConvert(Month.Month);
            YearInt = Month.Year;
            IncAmount = 0;
            ExpAmount = 0;
        }

        internal void SubtractExpense(Expense exp)
        {
            //if(exp.Date.Month == Month.Month && exp.Date.Year == Month.Year)
           // {
                Value -= exp.Value;
                ExpAmount += exp.Value;
            //}
        }

        internal void RemoveExpense(Expense exp)
        {
            Value += exp.Value;
            ExpAmount -= exp.Value;
        }

        internal void AddIncome(Income inc)
        {
            //if(inc.Date.Month == Month.Month && inc.Date.Year == Month.Year)
            //{
                IncAmount += inc.Value;
           //}
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
                    return "Sepetember";
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
