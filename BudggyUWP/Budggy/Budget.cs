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
     */
    public class Budget
    {

        public ObservableCollection<Bin> Bins = new ObservableCollection<Bin>()  {
            new Bin("Savings", "", .3),
            new Bin("Entertainment", "Going out money and gaming money", .5),
            new Bin("Gas", "", .1),
            new Bin("Food", "", .05),
        };
        public ObservableCollection<Income> Incs = new ObservableCollection<Income>() {
            new Income(2500.00, "Money.. I wonder what happens if this description... isn't actually short and takes up A TON of space. You know what I mean?", "Savings", DateTime.Now),
            new Income(143.72, "Money", "Savings", DateTime.Now),
            new Income(28.00, "Money", "Savings", DateTime.Now),
            new Income(55.53, "Money", "Savings", DateTime.Now),
            new Income(200, "Money", "Savings", DateTime.Now),
            new Income(50.21, "Money", "Savings", DateTime.Now),
            new Income(77.73, "Money", "Savings", DateTime.Now),
            new Income(192, "Money", "Savings", DateTime.Now),
            new Income(10.10, "Money", "Savings", DateTime.Now),
            new Income(172.46, "Money", "Savings", DateTime.Now),
            new Income(60.18, "Money", "Savings", DateTime.Now),
        };
        public ObservableCollection<Expense> Exps = new ObservableCollection<Expense>()  {
            new Expense(3, "Money", "Entertainment", DateTime.Now),
            new Expense(14.18, "Money", "Entertainment", DateTime.Now),
            new Expense(29.37, "Money", "Gas", DateTime.Now),
            new Expense(8.47, "Money", "Food", DateTime.Now),
            new Expense(5.04, "Money", "Presents", DateTime.Now),
            new Expense(6.55, "Money", "Food", DateTime.Now),
            new Expense(30.05, "Money", "Food", DateTime.Now),
            new Expense(2.25, "Money", "Food", DateTime.Now),
            new Expense(28.40, "Money", "Gas", DateTime.Now),
            new Expense(13.99, "Amazon", "Entertainment", DateTime.Now),
            new Expense(49.43, "Amazon", "Presents", DateTime.Now),
        };
        public ObservableCollection<MonthBudget> MonthlyBudgets = new ObservableCollection<MonthBudget>() {
            
        };
       /* public List<Bin> Bins = new List<Bin>() {
         new Bin("Entertainment", "Going out money and gaming money", .5),
        };
        List<Income> Incs = new List<Income>()
        {
            new Income(2500, "Money", "Savings", DateTime.Now),
        };
        List<Expense> Exps = new List<Expense>()
        {
            new Expense(15, "Money", "Savings", DateTime.Now),
        };
        public List<MonthBudget> MonthlyBudgets = new List<MonthBudget>() {
            new MonthBudget(2500, 12, 2018),
        };*/
        public double DefaultMonthlyBudget { get; set; }
        //string Name { get; set; }
        double Balance { get; set; }
        //public Savings Savings = new Savings("Savings", "", .15);
/* think about putting all the incs and exps in the budget class, instead of the Bins and just have a string that states what bin its apart of 
  I'm liking this idea. it'll be easier to use MVVM did it*/

        public Budget()
        {
            DefaultMonthlyBudget = 2500;
            CreateMonthlyBudget();
            CalcMonthBudgetAll();
            CalcMonthBudgetInc();
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
       
        // Delete a bin and transfer funds into another bin... (maybe savings by default? which mean I'd have to make that bin type)
        public void DeleteBin(string bin)
        {
            int index = Bins.IndexOf(Bins.Where(x => string.Compare(x.Name, bin) == 0).FirstOrDefault());
            // Bins.FindIndex(x => string.Compare(x.Name, bin) == 0); FIX IT***
            if (index == 0) return;
            double balance = Bins[index].GetBalance();

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

        //transfer funds from one bin to another (from index2 to index1) ** change it to be based on strings 
        public int TransferFunds(string bin1, string bin2, double amount, DateTime date)
        {
            int index1 = Bins.IndexOf(Bins.Where(x => string.Compare(x.Name, bin1) == 0).FirstOrDefault());
            int index2 = Bins.IndexOf(Bins.Where(x => string.Compare(x.Name, bin2) == 0).FirstOrDefault()); 
           // if (bin1 != Savings.Name && bin2 != Savings.Name)
            //{
                if (amount > Bins[index2].GetBalance())
                    return 0;
                Incs.Add(new Income(amount, "[Transfer from] " + Bins[index2].Name, Bins[index1].Name, date));
                Exps.Add(new Expense(amount, "[Transfer to] " + Bins[index1].Name, Bins[index2].Name, date));
                
           /* } else if(index1 == -1)
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
            } */

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
                

                if (percentage > 1)
                {
                    return 0;
                }
                else
                {                    
                    
                    foreach (Bin bin in Bins)
                    {
                        Incs.Add(new Income(value * bin.Percentage, destr, bin.Name, date));
                    }
                    CalcBinBalance();
                    CalcMonthBudgetInc();
                    OrganizeIncomesByDate();
                    return 1;
                }

            }
            else
            {
                index = Bins.IndexOf(Bins.Where(x => string.Compare(x.Name, mode) == 0).FirstOrDefault());

                if (index != -1)
                {
                    Incs.Add(new Income(value, destr, mode, date));
                    CalcBinBalance();
                    OrganizeIncomesByDate();
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
            int index = Incs.IndexOf(Incs.Where(x => string.Compare(x.Description, destr) == 0 && value == x.Value
            && DateTime.Compare(x.Date, date) == 0 && string.Compare(x.Bin, bin) == 0).FirstOrDefault()); //Incs.FindIndex(x => string.Compare(destr, x.Description) == 0 && value == x.Value 
            //&& DateTime.Compare(x.Date, date) == 0 && string.Compare(x.Bin, bin) == 0); 

            Incs.RemoveAt(index);
            OrganizeIncomesByDate();
        }
        //maybe add a split functionality later on as well

        public void AddExpense(double value, string destr, DateTime date, string bin)
        {
            int index = Bins.IndexOf(Bins.Where(x => string.Compare(x.Name, bin) == 0).FirstOrDefault());
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
            int index = Exps.IndexOf(Exps.Where(x => string.Compare(x.Description, destr) == 0 && value == x.Value
            && DateTime.Compare(x.Date, date) == 0 && string.Compare(x.Bin, bin) == 0).FirstOrDefault());// Exps.FindIndex(x => string.Compare(destr, x.Description) == 0 && value == x.Value
            //&& DateTime.Compare(x.Date, date) == 0 && string.Compare(x.Bin, bin) == 0); FIX IT***

            Exps.RemoveAt(index);
            OrganizeExpensesByDate();
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

        public void CalcMonthBudgetInc()
        {
            foreach (MonthBudget bud in MonthlyBudgets)
            {
                foreach (Income inc in Incs)
                {
                    if (inc.Date.Month == bud.Month.Month && inc.Date.Year == bud.Month.Year)
                    {
                        if (!(inc.Description.Contains("[Transfer from]")))
                        {
                            bud.AddIncome(inc);
                        }
                    }
                }
            }
        }

        public void AddMonthBudgetExpense(double value, string destr, string bin, DateTime date)
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
            
        }
        
    }

    public class BudgetViewModel
    {
        private Budget budget = new Budget();
        public Budget Budggy { get
            {
                return budget;
            } }
    }
}
