using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudggyTestClassLibrary
{
    public class Budget
    {
        List<Bin> Bins { get; set; }
        List<Income> Incs { get; set; }
        List<Expense> Exps { get; set; }

        string Name { get; set; }
        double Balance { get; set; }
        double MonthlyBudget { get; set; }
        Savings Savings { get; }

        // Methods to add bins
        public void AddBin(string name, string description, double percentage, double minimumBalance, double goalBalance, double multiplier)
        {
            Bins.Add(new Bin(name, description, percentage, minimumBalance, goalBalance, multiplier));
        }

        public void AddBin(string name, string description, double percentage)
        {
            Bins.Add(new Bin(name, description, percentage));
        }

        // Method to calculate the balance based on the Balance within the bins 
        public void TotalBalance()
        {
            double balance = 0;

            foreach(Bin bin in Bins)
            {
                balance += bin.GetBalance();
            }
            
            this.Balance = balance + Savings.GetBalance();
        }

        // Delete a bin and transfer funds into another bin... (maybe savings by default? which mean I'd have to make that bin type)
        public void DeleteBin(int index)
        {
            double balance = Bins[index].GetBalance();

            //adds incomes and expenses from the deleted bin to the Budget's list
            foreach(Income inc in Bins[index].Incomes) 
            {
                this.Incs.Add(inc);
            }

            foreach (Expense exp in Bins[index].Expenses)
            {
                this.Exps.Add(exp);
            }

            //transfers all funds or debts from the deleted bin to savings and deletes from the budget's list
            TransferFunds(-1, index, balance, DateTime.Today);
            Bins.RemoveAt(index);
        }

        //transfer funds from one bin to another (from index2 to index1)
        public int TransferFunds(int index1, int index2, double amount, DateTime date)
        {
            if (amount > Bins[index2].GetBalance())
                return 0;

            if(index1 != -1 && index2 != -1)
            {
                Bins[index1].AddIncome(amount, "Transfer from " + Bins[index2].Name, date);
                Bins[index2].AddExpense(amount, "Transfer to " + Bins[index1].Name, date);
            } else if(index1 == -1)
            {
                Savings.AddIncome(amount, "Transfer from " + Bins[index2].Name, date);
                Bins[index2].AddExpense(amount, "Transfer to Savings", date);
            } else if(index2 == -1)
            {
                Bins[index1].AddIncome(amount, "Transfer from Savings", date);
                Savings.AddExpense(amount, "Transfer to " + Bins[index1].Name, date);
            }

            
            return 1;

        }

        //split income into each bin based on their percentage preset
        //On another note. should we have the budget contain the percentage preset? It might be easier to manage and check
        public int AddIncome(double value, string destr, DateTime date, int mode)
        {
            if(mode == -1)
            {
                double percentage = 0;
                foreach (Bin bin in Bins)
                {
                    percentage += bin.Percentage;
                }

                if (percentage > 1)
                {
                    return 0;
                }
                else
                {
                    foreach (Bin bin in Bins)
                    {
                        bin.AddIncome(value * bin.Percentage, destr, date);
                    }
                    return 1;
                }

            }
            Bins[mode].AddIncome(value, destr, date);
            return 1;

        }

        //Need to actually have monthly budget maybe monthly budget class with a DateTime Month. could add to the bins as well
        //maybe need a monthly budget variable... that'll set the value
    }
}
