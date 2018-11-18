using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudggyTestClassLibrary
{
   public class Bin
    {
        internal List<Expense> Expenses = new List<Expense>();
        internal List<Income> Incomes = new List<Income>();
        string Description { get; set; }
        internal string Name { get; set; }
        double Balance { get; set; }
        double Percentage { get; set; }

        double MinimumBalance { get; set; }
        double GoalBalance { get; set; }
        double Multiplier { get; set; }

        public Bin(string name, string description, double percentage, double minimumBalance, double goalBalance, double multiplier)
        {
            Name = name;
            Description = description;
            Percentage = percentage;
            MinimumBalance = minimumBalance;
            GoalBalance = goalBalance;
            Multiplier = multiplier;
        }

        public Bin(string name, string description, double percentage)
        {
            Name = name;
            Description = description;
            Percentage = percentage;          
        }


        public void AddExpense(double value, string destr, DateTime date)
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

        public void AddIncome(double value, string destr, DateTime date)
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
        }
        
        public double GetBalance()
        {
            return Balance;
        }

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
        }

        int DCompare(double x, double y)
        {
            if (x - y > 0) return 1;
            else if (x - y < 0) return -1;
            else return 0;
        }
    }
}
