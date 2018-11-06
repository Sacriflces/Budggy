using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budggy
{
   public class Bin
    {
        List<Expense> Expenses = new List<Expense>();
        List<Income> Incomes = new List<Income>();
        string Description { get; set; }
        string Name { get; set; }
        double Balance { get; set; }
        double Percentage { get; set; }

        public void addExpense(double value, string destr, DateTime date)
        {
            Expense expense = new Expense(value, destr, date);
            Expenses.Add(expense);
        }

        public void addIncome(double value, string destr, DateTime date)
        {
            Income income = new Income(value, destr, date);
            Incomes.Add(income);
        }

        public double getEValue(int index)
        {
            return Expenses[index].Value;
        }

        public double getIValue(int index)
        {
            return Incomes[index].Value;
        }

    }
}
