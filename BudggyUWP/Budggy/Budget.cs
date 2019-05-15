using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budggy
{
    /* TAGS:
     * FIX IT***
     * 
     * 
     * 
     * Need to change the implementations of the delete functions. I can pass the object instead of its description only for incomes though. I don't want to be able
     * to delete the savings bin
     */
    public class Budget
    {

        public ObservableCollection<Bin> Bins = new ObservableCollection<Bin>()  {
        /*    new Bin("Savings", "", .3m),
            new Bin("Entertainment", "Going out money and gaming money, and whatever else I need to make this description longer", .5m),
            new Bin("Gas", "", .1m),
            new Bin("Food", "", .05m),
            new Bin("Presents", "Money for Presents lol", .05m), */
    };
        public ObservableCollection<Income> Incs = new ObservableCollection<Income>() {
        /*    new Income(2500.00m, "Money.. I wonder what happens if this description... isn't actually short and takes up A TON of space. You know what I mean?", "Savings", DateTime.Now),
            new Income(143.72m, "Money", "Savings", DateTime.Now),
            new Income(28.00m, "Money", "Savings", DateTime.Now),
            new Income(55.53m, "Money", "Savings", DateTime.Now),
            new Income(200m, "Money", "Savings", DateTime.Now),
            new Income(50.21m, "Money", "Savings", DateTime.Now),
            new Income(77.73m, "Money", "Savings", DateTime.Now),
            new Income(192m, "Money", "Savings", DateTime.Now),
            new Income(10.10m, "Money", "Savings", DateTime.Now),
            new Income(172.46m, "Money", "Savings", DateTime.Now),
            new Income(60.18m, "Money", "Savings", DateTime.Now),  */
        };
        public ObservableCollection<Expense> Exps = new ObservableCollection<Expense>()  {
          /*  new Expense(3m, "Money", "Entertainment", DateTime.Now),
            new Expense(14.18m, "Money", "Entertainment", DateTime.Now),
            new Expense(29.37m, "Money", "Gas", DateTime.Now),
            new Expense(8.47m, "Money", "Food", DateTime.Now),
            new Expense(5.04m, "Money", "Presents", DateTime.Now),
            new Expense(6.55m, "Money", "Food", DateTime.Now),
            new Expense(30.05m, "Money", "Food", DateTime.Now),
            new Expense(2.25m, "Money", "Food", DateTime.Now),
            new Expense(28.40m, "Money", "Gas", DateTime.Now),
            new Expense(13.99m, "Amazon", "Entertainment", DateTime.Now),
            new Expense(49.43m, "Amazon", "Presents", DateTime.Now), */
        };
        public ObservableCollection<MonthBudget> MonthlyBudgets = new ObservableCollection<MonthBudget>() {
            
        };      
        public decimal DefaultMonthlyBudget { get; set; }
        decimal Balance { get; set; }
      

        public Budget()
        {
            DefaultMonthlyBudget = 2500;
            CreateMonthlyBudget();
            CalcMonthBudgetAll();
            CalcMonthBudgetInc();
            CalcBinBalance();
            
        }
        // Adds a bin to the Bins collection.  
        public int AddBin(string name, string description, decimal percentage, decimal goalBalance = 2500, decimal minimumBalance = 0, decimal multiplier = 1)
        {
            decimal currentSplit = 0;
            decimal diff;

            //return something difficult for each case? Also returns -1 if the percentage is too large.
            if (percentage > 100) return -1;            

            //checks for a name match among the already created bins.
            foreach (Bin bin in Bins)
            {
                if (string.Compare(name.ToLower(), bin.Name.ToLower()) == 0)
                    return 0;
                currentSplit += bin.Percentage;
            }

            percentage = (percentage < 1) ? percentage * 100 : percentage;

            //correcting percentage if the new bin's percentage increases the split to above 100%
            if(currentSplit + percentage > 100)
            {
                diff = 100 - currentSplit + percentage;
                
                foreach(Bin bin in Bins)
                {
                    if(diff < bin.Percentage)
                    {
                        bin.Percentage -= diff;
                        diff = 0;
                        break;
                    } else
                    {
                        diff -= bin.Percentage;
                        bin.Percentage = 0;
                    }
                }
            }

            Bins.Add(new Bin(name, description, percentage, minimumBalance, goalBalance, multiplier));
            return 1;
        }     

        //Sets all Bins' balances to zero.
        internal void BinBalanceToZero()
        {            
            foreach (Bin bin in Bins)
            {
                bin.Balance = 0;
            }
        }

        //Calculates the Bin balance based on the incomes and expenses.
        public void CalcBinBalance()
        {
            int index;
            BinBalanceToZero();
            
            foreach(Income inc in Incs)
            {                               
                    index = Bins.IndexOf(Bins.Where(x => String.Compare(x.Name, inc.Bin) == 0).FirstOrDefault());                        
                    if (index != -1) Bins[index].Balance += inc.Value;              
            }

            foreach (Expense exp in Exps)
            {

                index = Bins.IndexOf(Bins.Where(x => String.Compare(x.Name, exp.Bin) == 0).FirstOrDefault());
                    if (index != -1) Bins[index].Balance -= exp.Value;
                
            }
        }

        // Method to calculate the balance based on the Balance within the bins 
        public decimal TotalBalance()
        {
            decimal balance = 0;

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
       
        // Delete a specific bin and transfer its balance to the Savings Bin
        public void DeleteBin(string bin)
        {
            int index = Bins.IndexOf(Bins.Where(x => string.Compare(x.Name, bin) == 0).FirstOrDefault());

            // Cannot delete the Savings Bin
            if (index == 0) return;
            decimal balance = Bins[index].GetBalance();

            //Changes the bin property of each income and expense associated with the bin to Savings
            foreach(Income inc in Incs) 
            {
                if (string.Compare(inc.Bin, bin) == 0)
                    inc.Bin = Bins[0].Name;
            }

            foreach (Expense exp in Exps)
            {
                if (string.Compare(exp.Bin, bin) == 0)
                    exp.Bin = Bins[0].Name;
            }

            //transfers all funds or debts from the deleted bin to savings and deletes from the budget's list
            TransferFunds(Bins[0].Name, Bins[index].Name, balance, DateTime.Today);
            Bins.RemoveAt(index);
        }

        //transfer funds from one bin to another (from bin2 to bin1)  
        public int TransferFunds(string bin1, string bin2, decimal amount, DateTime date)
        {
            //finds the indexes of the bins.
            int index1 = Bins.IndexOf(Bins.Where(x => string.Compare(x.Name, bin1) == 0).FirstOrDefault());
            int index2 = Bins.IndexOf(Bins.Where(x => string.Compare(x.Name, bin2) == 0).FirstOrDefault());

            //if the amount is too large return 0
            if (amount > Bins[index2].GetBalance())
                return 0;

            //creates the transfer.
            Incs.Add(new Income(amount, "[Transfer from] " + Bins[index2].Name, Bins[index1].Name, date));
            Exps.Add(new Expense(amount, "[Transfer to] " + Bins[index1].Name, Bins[index2].Name, date));

            CalcBinBalance();
            return 1;

        }

        //Adds an income to the incs list either splitting it into the bins or into one list.
        public int AddIncome(decimal value, string destr, DateTime date, string mode)
        {
            int index;        
            if(mode == "Split")
            {
                //checks the percentage. if it is greater than 100 then it returns.
                decimal percentage = 0;
                foreach (Bin bin in Bins)
                {
                    percentage += bin.Percentage;
                }
                

                if (percentage > 100)
                {
                    return -1;
                }
                else
                {                    
                    //splits the income across the bins based on their percentage.
                    foreach (Bin bin in Bins)
                    {
                        bin.AddIncome(new Income(value * bin.Percentage / 100, destr, bin.Name, date));
                        
                    }
                   // CalcBinBalance();
                   // CalcMonthBudgetInc();
                    OrganizeIncomesByDate();
                    Incs.Add(new Income(value, destr, "Split", date));
                    return 1;
                }

            }
            else //income goes into a specific bin
            {
                index = Bins.IndexOf(Bins.Where(x => string.Compare(x.Name, mode) == 0).FirstOrDefault());
                Income newInc = new Income(value, destr, mode, date);
                if (index != -1)
                {
                    Incs.Add(newInc);
                    Bins[index].AddIncome(newInc);
                   // CalcBinBalance();
                    OrganizeIncomesByDate();
                   // CalcMonthBudgetInc();
                    return 1;
                }
                else
                    Incs.Add(new Income(value, destr, null, date));

               // CalcBinBalance();
                OrganizeIncomesByDate();
               // CalcMonthBudgetInc();
                return index;
            }
          

        }
        
        //Deletes an income from incs list.
        public void DeleteIncome(decimal value, string destr, myDateTime date, string bin)
        {
            //finds the income object that needs to be deleted.
            int index = Incs.IndexOf(Incs.Where(x => string.Compare(x.Description, destr) == 0 && value == x.Value
            && myDateTime.Compare(x.Date, date) == 0 && string.Compare(x.Bin, bin) == 0).FirstOrDefault());

            int binIndex;

            if(bin == "Split")
            {
                //remove the income from each bin
                int incIndex;
                foreach  (Bin BIN in Bins)
                {
                    incIndex = BIN.Incomes.IndexOf(BIN.Incomes.Where(x => string.Compare(x.Description, destr) == 0 && value == x.Value
                     && myDateTime.Compare(x.Date, date) == 0 && string.Compare(x.Bin, BIN.Name) == 0).FirstOrDefault());

                    if(incIndex != -1)
                    {
                        BIN.RemoveIncome(incIndex);

                    }
                }

            } else
            {
                //find and remove the income from the bin
                binIndex = Bins.IndexOf(Bins.Where(x => string.Compare(x.Name, bin) == 0).FirstOrDefault());
                int incIndex = Bins[binIndex].Incomes.IndexOf(Bins[binIndex].Incomes.Where(x => string.Compare(x.Description, destr) == 0 && value == x.Value
            && myDateTime.Compare(x.Date, date) == 0 && string.Compare(x.Bin, bin) == 0).FirstOrDefault());

                Bins[binIndex].RemoveIncome(incIndex);   
            }

            //finds the MonthBudget that is associated with the income object and removes it from its list.
            foreach (MonthBudget bud in MonthlyBudgets)
            {
                if(Incs[index].Date.Month == bud.Date.Month && Incs[index].Date.Year == bud.Date.Year)
                {
                    bud.RemoveIncome(Incs[index]);
                    break;
                }
            }
            
            Incs.RemoveAt(index);
            OrganizeIncomesByDate();
            CalcBinBalance();
            CalcMonthBudgetAll();
        }
        //maybe add a split functionality later on as well

        public void AddExpense(decimal value, string destr, DateTime date, string bin, string drawer = null)
        {
            Expense exp; 
            int index = Bins.IndexOf(Bins.Where(x => string.Compare(x.Name, bin) == 0).FirstOrDefault());
            if (index != -1)
            {
                exp = new Expense(value, destr, Bins[index].Name, date);
            }
            else
                exp = new Expense(value, destr, null, date);

            exp.Drawer = drawer;
            Exps.Add(exp);
            Bins[index].AddDrawerExpense(exp);

            CalcBinBalance();
           // AddMonthBudgetExpense(value, destr, bin, date);
            OrganizeExpensesByDate();
            CalcMonthBudgetExp();
        }

        public void DeleteExpense(decimal value, string destr, myDateTime date, string bin)
        {
            int index = Exps.IndexOf(Exps.Where(x => string.Compare(x.Description, destr) == 0 && value == x.Value
            && myDateTime.Compare(x.Date, date) == 0 && string.Compare(x.Bin, bin) == 0).FirstOrDefault());
            int Binindex = Bins.IndexOf(Bins.Where(x => string.Compare(x.Name, bin) == 0).FirstOrDefault());
            foreach (MonthBudget bud in MonthlyBudgets)
            {
                if (Exps[index].Date.Month == bud.Date.Month && Exps[index].Date.Year == bud.Date.Year)
                {
                    bud.RemoveExpense(Exps[index]);
                    break;
                }
            }

            Bins[Binindex].RemoveDrawerExpense(Exps[index]);
            Exps.RemoveAt(index);

            OrganizeExpensesByDate();
            CalcBinBalance();
            CalcMonthBudgetAll();

        }

        //Sorting Methods date and value
        public void OrganizeIncomesByDate()
        {
            IEnumerable<Income> incomes = Incs.OrderByDescending(x => x.Date);
            int count = Incs.Count;

            foreach (Income inc in incomes)
            {
                Incs.Add(inc);
            }

            for (int i = 0; i < count; i++)
            {
                Incs.RemoveAt(0);
            }
            // Incs.Sort((x, y) => DateTime.Compare(x.Date, y.Date)); FIX IT***
        }

        public void OrganizeIncomesByValue()
        {
            IEnumerable<Income> incomes = Incs.OrderBy(x => x.Value);
            int count = Incs.Count;

            foreach (Income inc in incomes)
            {
                Incs.Add(inc);
            }

            for (int i = 0; i < count; i++)
            {
                Incs.RemoveAt(0);
            }
            //  Incs.Sort((x, y) => DCompare(x.Value, y.Value)); FIX IT***
        }

        public void OrganizeExpensesByDate()
        {
            
            IEnumerable<Expense> expenses = Exps.OrderByDescending(x => x.Date);
            int count = Exps.Count;

            foreach (Expense exp in expenses)
            {
                Exps.Add(exp);
            }

            for (int i = 0; i < count; i++)
            {
                Exps.RemoveAt(0);
            }
            // Exps.Sort((x, y) => DateTime.Compare(x.Date, y.Date)); FIX IT***
        }

        public void OrganizeExpensesByValue()
        {
            IEnumerable<Expense> expenses = Exps.OrderBy(x => x.Value);
            int count = Exps.Count;

            foreach (Expense exp in expenses)
            {
                Exps.Add(exp);
            }

            for (int i = 0; i < count; i++)
            {
                Exps.RemoveAt(0);
            }

            //  Exps.Sort((x, y) => DCompare(x.Value, y.Value)); FIX IT***

        }

        int DCompare(decimal x, decimal y)
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
                if (MonthlyBudgets[i].Date.Month == DateTime.Now.Month && MonthlyBudgets[i].Date.Year == DateTime.Now.Year)
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
            foreach(MonthBudget bud in MonthlyBudgets)
            {
                bud.Value = bud.BudgetVal;
            }
            CalcMonthBudgetExp();
            CalcMonthBudgetInc();
        }

        public void CalcMonthBudgetExp()
        {
            foreach (MonthBudget bud in MonthlyBudgets)
            {
                bud.ExpAmount = 0;
                bud.Value = bud.BudgetVal;
                foreach(Expense exp in Exps)
                {
                    if(exp.Date.Month == bud.Date.Month && exp.Date.Year == bud.Date.Year)
                    {
                        if (!(exp.Description.Contains("[Transfer to]")))
                        {
                            bud.SubtractExpense(exp);
                        }
                    }
                }      
               
            }
        }

        public void CalcMonthBudgetInc()
        {
            foreach (MonthBudget bud in MonthlyBudgets)
            {
                bud.IncAmount = 0;
                foreach (Income inc in Incs)
                {
                    if (inc.Date.Month == bud.Date.Month && inc.Date.Year == bud.Date.Year)
                    {
                        if (!(inc.Description.Contains("[Transfer from]")))
                        {
                            bud.AddIncome(inc);
                        }
                    }
                }
            }
        }
        /*
        public void AddMonthBudgetExpense(decimal value, string destr, string bin, DateTime date)
        {
            int index = Bins.IndexOf(Bins.Where(x => string.Compare(x.Name, bin) == 0).FirstOrDefault());
           if (index != -1)
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
            
        } */
        //Don't need this. I'll already know what the index is
        public void CreateBinDrawer(string bin, string name, decimal maximum)
        {
            int index = Bins.IndexOf(Bins.Where(x => string.Compare(x.Name, bin) == 0).FirstOrDefault());
            if (index != -1)
            {
                Bins[index].CreateDrawer(name, maximum);
            }

        }

        public void RemoveBinDrawer(string bin, string name)
        {
             int index = Bins.IndexOf(Bins.Where(x => string.Compare(x.Name, bin) == 0).FirstOrDefault());
            if (index != -1)
            {
                Bins[index].RemoveDrawer(name);
            }
        }

    }

   /* public class BudgetViewModel
    {
        private Budget budget = new Budget();
        public Budget Budggy { get
            {
                return budget;
            } }
    } */
}
