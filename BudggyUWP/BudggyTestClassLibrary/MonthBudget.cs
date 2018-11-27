using System;
using System.Collections.Generic;
using System.Text;

namespace BudggyTestClassLibrary
{
   public class MonthBudget
    {
        internal double BudgetVal { get; set; }
        public double Value { get; set; }
        internal DateTime Month { get; set; }

        internal MonthBudget(double value, int month, int year)
        {
            Value = value;
            BudgetVal = value;
            Month = new DateTime(year, month, 1);
        }

        internal void SubtractExpense(Expense exp)
        {
            if(exp.Date.Month == Month.Month && exp.Date.Year == Month.Year)
            {
                Value -= exp.Value;
            }
        }

        internal void NewBudget(double newVal)
        {
            double diff = BudgetVal - Value;
            BudgetVal = newVal;
            Value = newVal - diff;
        }
    }
}
