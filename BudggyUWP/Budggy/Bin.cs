﻿using System;
using System.Collections.Generic;
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

        // internal List<Expense> Expenses = new List<Expense>();
        //internal List<Income> Incomes = new List<Income>();

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

        private double _balance;
        public double Balance {
            get { return _balance; }
            set
            {
                _balance = value;
                OnPropertyChange("Balance");
            }
        }

        private double _percentage;
        public double Percentage {
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


        public double MinimumBalance { get; set; }

        private double _goalBalance;
        public double GoalBalance {
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

        internal double Multiplier { get; set; }

        public Bin()
        {

        }

        public Bin(string name, string description, double percentage, double minimumBalance, double goalBalance, double multiplier)
        {
            Name = name;
            Description = description;
            Percentage = percentage;
            MinimumBalance = minimumBalance;
            GoalBalance = goalBalance;
            Multiplier = multiplier;
            Balance = 0;
        }

        public Bin(string name, string description, double percentage)
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
        
        public double GetBalance()
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

        int DCompare(double x, double y)
        {
            if (x - y > 0) return 1;
            else if (x - y < 0) return -1;
            else return 0;
        }
    }
}
