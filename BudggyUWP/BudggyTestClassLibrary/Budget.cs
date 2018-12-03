using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudggyTestClassLibrary
{
    public class Budget
    {
        public List<Bin> Bins = new List<Bin>();
        List<Income> Incs = new List<Income>();
        List<Expense> Exps = new List<Expense>();       
        public List<MonthBudget> MonthlyBudgets = new List<MonthBudget>();
        public double DefaultMonthlyBudget { get; set; }
        string Name { get; set; }
        double Balance { get; set; }
        public Savings Savings = new Savings("Savings", "", .15);
/* think about putting all the incs and exps in the budget class, instead of the Bins and just have a string that states what bin its apart of 
  I'm liking this idea. it'll be easier to use MVVM did it*/

        public Budget()
        {
            DefaultMonthlyBudget = 2500;
        }
        // Methods to add bins
        //check if multiplier is between 1 and 0
        public void AddBin(string name, string description, double percentage, double minimumBalance, double goalBalance, double multiplier)
        {
            Bins.Add(new Bin(name, description, percentage, minimumBalance, goalBalance, multiplier));
        }

        public void AddBin(string name, string description, double percentage)
        {

            Bins.Add(new Bin(name, description, percentage));
        }
        internal void BinBalanceToZero()
        {
            Savings.Balance = 0;
            foreach (Bin bin in Bins)
            {
                bin.Balance = 0;
            }
        }
        public void CalcBinBalance()
        {
            int index;
            BinBalanceToZero();
            
            foreach(Income inc in Incs)
            {
                if(String.Compare(inc.Bin, Savings.Name) != 0)
                {
                    index = Bins.FindIndex(x => String.Compare(x.Name, inc.Bin) == 0);
                    if (index != -1) Bins[index].Balance += inc.Value;
                } else
                {
                    Savings.Balance += inc.Value;
                }
               
            }

            foreach (Expense exp in Exps)
            {
                if (String.Compare(exp.Bin, Savings.Name) != 0)
                {
                    index = Bins.FindIndex(x => String.Compare(x.Name, exp.Bin) == 0);
                    if (index != -1) Bins[index].Balance -= exp.Value;
                } else
                {
                    Savings.Balance -= exp.Value;
                    
                }
            }
        }

        // Method to calculate the balance based on the Balance within the bins 
        public double TotalBalance()
        {
            double balance = 0;

            foreach(Income inc in Incs)
            {
                balance += inc.Value;
            }

            foreach (Expense exp in Exps)
            {
                balance -= exp.Value;
            }
            Balance = balance;
            return Balance;
        }

        //Method to change percentage on the Savings balance
        public void SavingsPercentage(double value)
        {
            Savings.Percentage = value;
        }

        // Delete a bin and transfer funds into another bin... (maybe savings by default? which mean I'd have to make that bin type)
        public void DeleteBin(string bin)
        {
            int index = Bins.FindIndex(x => string.Compare(x.Name, bin) == 0);

            double balance = Bins[index].GetBalance();

            //Changes the bin property of each income and expense associated with the bin to null
            foreach(Income inc in Incs) 
            {
                if (string.Compare(inc.Bin, bin) == 0)
                    inc.Bin = null;
            }

            foreach (Expense exp in Exps)
            {
                if (string.Compare(exp.Bin, bin) == 0)
                    exp.Bin = null;
            }

            //transfers all funds or debts from the deleted bin to savings and deletes from the budget's list
            TransferFunds(Savings.Name, Bins[index].Name, balance, DateTime.Today);
            Bins.RemoveAt(index);
        }

        //transfer funds from one bin to another (from index2 to index1) ** change it to be based on strings 
        public int TransferFunds(string bin1, string bin2, double amount, DateTime date)
        {
            int index1 = Bins.FindIndex(x => string.Compare(x.Name, bin1) == 0);
            int index2 = Bins.FindIndex(x => string.Compare(x.Name, bin2) == 0);
            if(bin1 != Savings.Name && bin2 != Savings.Name)
            {
                if (amount > Bins[index2].GetBalance())
                    return 0;
                Incs.Add(new Income(amount, "[Transfer from] " + Bins[index2].Name, Bins[index1].Name, date));
                Exps.Add(new Expense(amount, "[Transfer to] " + Bins[index1].Name, Bins[index2].Name, date));
                
            } else if(index1 == -1)
            {
                if (amount > Bins[index2].GetBalance())
                    return 0;
                Incs.Add(new Income(amount, "[Transfer from] " + Bins[index2].Name, Savings.Name, date));
                Exps.Add(new Expense(amount, "[Transfer to] " + Savings.Name, Bins[index2].Name, date));               
               
            } else if(index2 == -1)
            {
                if (amount > Savings.GetBalance())
                    return 0;
                Incs.Add(new Income(amount, "[Transfer from] " + Savings.Name, Bins[index1].Name, date));
                Exps.Add(new Expense(amount, "[Transfer to] " + Bins[index1].Name, Savings.Name, date));               
            }

            CalcBinBalance();
            return 1;

        }

        //split income into each bin based on their percentage preset
        //On another note. should we have the budget contain the percentage preset? It might be easier to manage and check
        public int AddIncome(double value, string destr, DateTime date, string mode)
        {
            int index;
            if(mode == "Split")
            {
                double percentage = 0;
                foreach (Bin bin in Bins)
                {
                    percentage += bin.Percentage;
                }
                percentage += Savings.Percentage;

                if (percentage > 1)
                {
                    return 0;
                }
                else
                {
                    Incs.Add(new Income(value * Savings.Percentage, destr, Savings.Name, date));
                    
                    foreach (Bin bin in Bins)
                    {
                        Incs.Add(new Income(value * bin.Percentage, destr, bin.Name, date));
                    }
                    CalcBinBalance();
                    OrganizeIncomesByDate();
                    return 1;
                }

            }
            else
            {
                index = Bins.FindIndex(x => string.Compare(x.Name, mode) == 0);

                if (index != -1)
                {
                    Incs.Add(new Income(value, destr, mode, date));
                    CalcBinBalance();
                    return 1;
                }
                else
                    Incs.Add(new Income(value, destr, null, date));

                CalcBinBalance();
                OrganizeIncomesByDate();
                return index;
            }
          

        }
        
        public void DeleteIncome(double value, string destr, DateTime date, string bin)
        {   
            int index = Incs.FindIndex(x => string.Compare(destr, x.Description) == 0 && value == x.Value 
            && DateTime.Compare(x.Date, date) == 0 && string.Compare(x.Bin, bin) == 0);

            Incs.RemoveAt(index);
            OrganizeIncomesByDate();
        }
        //maybe add a split functionality later on as well

        public void AddExpense(double value, string destr, DateTime date, string bin)
        {
            int index = Bins.FindIndex(x => string.Compare(x.Name, bin) == 0);
            if (index != -1)
            {
                Exps.Add(new Expense(value, destr, Bins[index].Name, date));
            }
            else
                Exps.Add(new Expense(value, destr, null, date));

            CalcBinBalance();
            AddMonthBudgetExpense(value, destr, bin, date);
            OrganizeExpensesByDate();
        }

        public void DeleteExpense(double value, string destr, DateTime date, string bin)
        {
            int index = Exps.FindIndex(x => string.Compare(destr, x.Description) == 0 && value == x.Value
            && DateTime.Compare(x.Date, date) == 0 && string.Compare(x.Bin, bin) == 0);

            Exps.RemoveAt(index);
            OrganizeExpensesByDate();
        }

        //Sorting Methods date and value
        public void OrganizeIncomesByDate()
        {
            Incs.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
        }

        public void OrganizeIncomesByValue()
        {
            Incs.Sort((x, y) => DCompare(x.Value, y.Value));
        }

        public void OrganizeExpensesByDate()
        {
            Exps.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
        }

        public void OrganizeExpensesByValue()
        {
            Exps.Sort((x, y) => DCompare(x.Value, y.Value));
        }

        int DCompare(double x, double y)
        {
            if (x - y > 0) return 1;
            else if (x - y < 0) return -1;
            else return 0;
        }

        //Need to actually have monthly budget maybe monthly budget class with a DateTime Month. could add to the bins as well
        //maybe need a monthly budget variable... that'll set the value
        public void CreateMonthlyBudget()
        {
            int found = 0;

            for(int i = 0; i<MonthlyBudgets.Count; i++)
            {
                if (MonthlyBudgets[i].Month.Month == DateTime.Now.Month && MonthlyBudgets[i].Month.Year == DateTime.Now.Year)
                {
                    found = 1;
                    break;
                }
            }

            if(found == 0)
            {
                MonthBudget budget = new MonthBudget(DefaultMonthlyBudget, DateTime.Now.Month, DateTime.Now.Year);
                MonthlyBudgets.Add(budget);
            }
        }
        // calculates all of the monthly budgets wheww.... 
        // checks all the expenses in bins and the budget controller. It also checks if a transfer occurred, so there won't be extra substractions
        public void CalcMonthBudgetAll()
        {
            foreach (MonthBudget bud in MonthlyBudgets)
            {
                bud.Value = bud.BudgetVal;
                foreach(Expense exp in Exps)
                {
                    if(exp.Date.Month == bud.Month.Month && exp.Date.Year == bud.Month.Year)
                    {
                        if (!(exp.Description.Contains("[Transfer to]")))
                        {
                            bud.SubtractExpense(exp);
                        }
                    }
                }                
            }
        }

        public void AddMonthBudgetExpense(double value, string destr, string bin, DateTime date)
        {
           int index = Bins.FindIndex(x => string.Compare(x.Name, bin) == 0);
           if(index != -1)
            {
                foreach (MonthBudget budget in MonthlyBudgets)
                {
                    budget.SubtractExpense(new Expense(value, destr, bin, date));
                }
            } else
            {
                foreach (MonthBudget budget in MonthlyBudgets)
                {
                    budget.SubtractExpense(new Expense(value, destr, null, date));
                }
            }
            
        }
        
    }
}
