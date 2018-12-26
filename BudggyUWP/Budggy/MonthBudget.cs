using System;
using System.Collections.Generic;
using System.Text;

namespace Budggy
{
   public class MonthBudget
    {
        internal double BudgetVal { get; set; }
        public double Value { get; set; }
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

        string MonthConvert(int num)
        {
            switch (num){
                case 0:
                    return "January";
                case 1:
                    return "February";
                case 2:
                    return "March";
                case 3:
                    return "April";
                case 4:
                    return "May";
                case 5:
                    return "June";
                case 6:
                    return "July";
                case 7:
                    return "August";
                case 8:
                    return "Sepetember";
                case 9:
                    return "October";
                case 10:
                    return "November";
                case 11:
                    return "December";
                default:
                    return "Really bro???";
            }
        }
    }
}
