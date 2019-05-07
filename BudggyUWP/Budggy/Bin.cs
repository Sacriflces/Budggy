using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Budggy
{

   public class Bin : INotifyPropertyChanged 
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

       

        void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Drawer> Drawers = new ObservableCollection<Drawer>();




        private string _description;
        public string Description
        {
            get { return _description;}
            set
            {
                _description = value;
                OnPropertyChange("Description");
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

        private decimal _balance;
        public decimal Balance {
            get { return _balance; }
            set
            {
                _balance = value;
                OnPropertyChange("Balance");
                if(value > 0.00m)
                BalanceString = _balance.ToString("C");
                else
                {
                    BalanceString = $"-${(-1 * value).ToString("G")}";
                }
            }
        }

        private string _balanceString;
        public string BalanceString
        {
            get { return _balanceString; }
            set
            {
                _balanceString = value;
                OnPropertyChange("BalanceString");
            }
        }

        private decimal _percentage;
        public decimal Percentage {
            get {return _percentage * 100; }
            set
            {
                if (value <= 1)
                    _percentage = value;
                else if (value > 1 && value <= 100)
                    _percentage = value / 100;
                else
                    _percentage = 0;
                OnPropertyChange("Percentage");
            }
        }


        public decimal MinimumBalance { get; set; }

        private decimal _goalBalance;
        public decimal GoalBalance {
            get
            {
                return _goalBalance;
            }
            set
            {
                _goalBalance = value;
                OnPropertyChange("GoalBlanace");
            }
        }

        internal decimal Multiplier { get; set; }

        public Bin()
        {

        }

        public Bin(string name, string description, decimal percentage, decimal minimumBalance, decimal goalBalance, decimal multiplier)
        {
            Name = name;
            Description = description;
            Percentage = percentage;
            MinimumBalance = minimumBalance;
            GoalBalance = goalBalance;
            Multiplier = multiplier;
            Balance = 0;
        }

        public Bin(string name, string description, decimal percentage)
        {
            Name = name;
            Description = description;
            Percentage = percentage;
            Balance = 0;
        }


       /* public void AddExpense(double value, string destr, DateTime date)
        {
            Expense expense = new Expense(value, destr, date);
            Expenses.Add(expense);
            CalcBalance();
            OrganizeExpensesByDate();
        } 

        public void DeleteExpense(int index)
        {
            Expenses.RemoveAt(index);
            CalcBalance();
            OrganizeExpensesByDate();
        }   

        public int ExpenseSize()
        {
            return Expenses.Count();
        }

      /*  public void AddIncome(double value, string destr, DateTime date)
        {
            Income income = new Income(value, destr, date);
            Incomes.Add(income);            
            CalcBalance();
            OrganizeIncomesByDate();
        } 

        public void DeleteIncome(int index)
        {
            Incomes.RemoveAt(index);
            CalcBalance();
            OrganizeIncomesByDate();
        }

        public int IncomeSize()
        {
            return Incomes.Count();
        }

        public double GetEValue(int index)
        {
            return Expenses[index].Value;
        }

        public double GetIValue(int index)
        {
            return Incomes[index].Value;
        } */
        
        public decimal GetBalance()
        {
            return Balance;
        }
        /*
        public double CalcBalance()
        {
            double sum = 0;

            foreach(Income inc in Incomes)
            {
                sum += inc.Value;
            }

            foreach(Expense exp in Expenses)
            {
                sum -= exp.Value;
            }

            Balance = sum;
            return Balance;
        }

        public void OrganizeIncomesByDate()
        {            
            Incomes.Sort((x, y) => DateTime.Compare(x.Date, y.Date));                       
        }

        public void OrganizeIncomesByValue()
        {
            Incomes.Sort((x, y) => DCompare(x.Value, y.Value));
        }

        public void OrganizeExpensesByDate()
        {
           Expenses.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
        }

        public void OrganizeExpensesByValue()
        {
            Expenses.Sort((x, y) => DCompare(x.Value, y.Value));
        } */

        int DCompare(decimal x, decimal y)
        {
            if (x - y > 0) return 1;
            else if (x - y < 0) return -1;
            else return 0;
        }
    }

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
        
        public Drawer(string name, decimal maximum, DateTime date)
        {
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

        public void AddExpense()
        {

        }

    }
}
