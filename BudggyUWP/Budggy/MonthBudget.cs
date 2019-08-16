using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Budggy
{
   /* public class myDateTime : IComparable
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
                {
                    if(lhs.Day < rhs.Day)
                    {
                        return -1;
                    }
                    else if(lhs.Day > rhs.Day)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                    
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
            Day = date.Day;
        }

        public override string ToString() 
        {
            return $"{Month}\\{Day}\\{Year}";
        }

        public int Month;
        public int Year;
        public int Day;
    } */

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

        public decimal BudgetVal { get; set; }

        private decimal val;
        public decimal Value { get { return val; }
            set {
                val = Math.Round(value, 2, MidpointRounding.AwayFromZero);
                OnPropertyChanged("Value");
                if (value > 0.00m)
                    ValString = val.ToString("C");
                else
                {
                    ValString = $"-${(-1 * val).ToString("G")}";
                }
            }
        }

        private string valString;
        public string ValString
        {
            get { return valString; }
            set
            {
                valString = value;
                OnPropertyChanged("ValString");
            }

        }

        private decimal incAmount;
        public decimal IncAmount { get { return incAmount; }
            set
            {
                incAmount = Math.Round(value, 2, MidpointRounding.AwayFromZero);
                OnPropertyChanged("IncAmount");
                IncString = incAmount.ToString("C");

            }
        }

        private string incString;
        public string IncString
        {
            get { return incString; }
            set
            {
                incString = value;
                OnPropertyChanged("IncString");
            }

        }

        private decimal expAmount;
        public decimal ExpAmount {
            get { return expAmount; }
            set
            {
                expAmount = Math.Round(value, 2, MidpointRounding.AwayFromZero);
                ExpString = expAmount.ToString("C");
                OnPropertyChanged("ExpAmount");
                
            }
        }

        private string expString;
        public string ExpString
        {
            get { return expString; }
            set
            {
                expString = value;
                OnPropertyChanged("ExpString");
            }

        }

        public DateTime Date { get; set; }
        //public myDateTime Date { get; set; }
        public string MonthStr { get; set; }
        public int YearInt { get; set; }

        public MonthBudget()
        {
            Value = 0;
            BudgetVal = 0;
            Date = DateTime.Now;//new myDateTime();
            MonthStr = MonthConvert(Date.Month);
            YearInt = Date.Year;
            IncAmount = 0;
            ExpAmount = 0;
        }
        public MonthBudget(decimal value, int month, int year)
        {
            Value = value;
            BudgetVal = value;
            Date = new DateTime(year, month, 1);//new myDateTime(month, year);
            MonthStr = MonthConvert(Date.Month);
            YearInt = Date.Year;
            IncAmount = 0;
            ExpAmount = 0;
        }
        internal void AddTransaction(Transaction transaction)
        {
            if(transaction.Value > 0)
            {
                IncAmount += transaction.Value;
            } else
            {                
                ExpAmount += transaction.Value;
            }
            Value += transaction.Value;
        }

        internal void RemoveTransaction(Transaction transaction)
        {
            if (transaction.Value > 0)
            {
                IncAmount -= transaction.Value;
            }
            else
            {
                Value -= transaction.Value;
                ExpAmount -= transaction.Value;
            }
        }
       /* internal void SubtractExpense(Expense exp)
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
        } */

        internal void NewBudget(decimal newVal)
        {
            decimal diff = BudgetVal - Value;
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
