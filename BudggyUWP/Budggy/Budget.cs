﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Budggy 
{


/* TAGS:
 * FIX IT***
 * 
 * 
 * 
 * Need to change the implementations of the delete functions. I can pass the object instead of its description only for incomes though. I don't want to be able
 * to delete the savings bin
 * Later on, when we add a transaction or bin, or w/e while there is no internet connection. Set the Id to -1 if it cannot access the database and add it to a update list
 * that will be saved locally. It will insert into the database once the internet connection is restored.
 */
public class Budget : INotifyPropertyChanged
    {

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

        public ObservableCollection<Bin> Bins = new ObservableCollection<Bin>()  {
          /* new Bin("Savings", "", .3m),
           new Bin("Entertainment", "Going out money and gaming money, and whatever else I need to make this description longer", .5m) { ID = 1 },
           new Bin("Gas", "", .1m){ ID = 2 },
           new Bin("Food", "", .05m){ ID = 3 },
           new Bin("Presents", "Money for Presents lol", .05m){ ID = 4 }, */
    };
    public ObservableCollection<Transaction> transactions = new ObservableCollection<Transaction>();

        //public ObservableCollection<Income> Incs = new ObservableCollection<Income>() {
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
       // };
       // public ObservableCollection<Expense> Exps = new ObservableCollection<Expense>()  {
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
       // };

    public ObservableCollection<RepeatTransaction> repeatedTrans = new ObservableCollection<RepeatTransaction>()
    {

    };

        /*list of repeated transactions with another list that contains their frequency in days. Check on startup and when a refresh button is pressed (just put it into 
         * a method). if the current date is greater than the repeated transaction date + its frequency, add a copy to either Incs or Exps based on what it is lol. Also 
         check for if multiple frequencies have occurred, so recursive call itself until the difference between the current transaction date and the current date is less than
         its frequency (in days).
         
             also the refresh will check if the budget has changed as well.
             
             geez so much stuff to do. Transaction class for the repeated transactions and maybe even combine Incs and Exps into one 
             also need to add goals to the bin where a certain % of the money entering a bin will put allocated to it. It'll also STOP receiving part of the income when its full.
             The goals will also have priorities associated with them, so the amount stored will be used up if the bin amount goes to 0.
             -Need to figure out how to use the money in the goals... for example I wanted x amount saved for presents. I use some, and then it begins to fill up again since it is not maxed
             - add a function to add an expense to the goal? (add the goal to the description)
             You know I could actually at a boolean that let me know when something has been edited, so I can add the changes to the database*/
             
        public ObservableCollection<MonthBudget> MonthlyBudgets = new ObservableCollection<MonthBudget>() {
            
        };      
        public decimal DefaultMonthlyBudget { get; set; }
        private decimal _balance;
        public decimal Balance { get { return _balance; }
            set {
                _balance = value;
                OnPropertyChanged("Balance");
            }
        }
      

        public Budget()
        {
            try
            {
                Bins = Account.SelectBins();
                AddGoalsToBins();
                AddDrawersToBins();
                MonthlyBudgets.Add(Account.SelectMonthBudget(DateTime.Now));
                if(MonthlyBudgets[MonthlyBudgets.Count - 1].Setup == true)
                {
                    MonthlyBudgets.RemoveAt(MonthlyBudgets.Count - 1);
                    CreateMonthlyBudget();
                }
                repeatedTrans = Account.SelectRepeatTransactions();
                transactions = Account.SelectTransactions(DateTime.Now);
            }
            catch
            {

            }
            
            DefaultMonthlyBudget = 0;
            Balance = TotalBalance();
         /*   if(MonthlyBudgets.Count == 0)
            {
                CreateMonthlyBudget();
            } */
         //   CreateMonthlyBudget();
          //  CalcBinBalance();
            
        }
        //Add Goals to Bins

        public void AddGoalsToBins()
        {
            var allGoals = Account.SelectGoals();
            foreach(Goal goal in allGoals)
            {
                Bins.First(x => x.ID == goal.BinID).Goals.Add(goal);
            }
        }

        //Add Drawers to Bins
        public void AddDrawersToBins()
        {
            var allDrawers = Account.SelectDrawersByMonth(DateTime.Now);
            if (allDrawers.Count == 0)
            {
                int Month = (DateTime.Now.Month == 1) ? 12 : DateTime.Now.Month - 1;
                int Year = (Month == 12) ? DateTime.Now.Year - 1 : DateTime.Now.Year;
                DateTime dateTime = new DateTime(Year, Month, 1);
                allDrawers = Account.SelectDrawersByMonth(dateTime);
            } 
            foreach (Drawer drawer in allDrawers) //Might add in a while loop for if the user hasn't used this for a few months lol.
                //The while loop would keep refreshing until it go to the current Month/Year combination
            {
                if(drawer.Month != DateTime.Now.Month || drawer.Year != DateTime.Now.Year)
                    Bins.First(x => x.ID == drawer.BinID).CreateDrawer(drawer.Refresh());
                else
                    Bins.First(x => x.ID == drawer.BinID).CreateDrawer(drawer);
            }
        }

        // Adds a bin to the Bins collection.  
        public int AddBin(string name, string description, decimal percentage)
        {
            int iD = 0;
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

            iD = IDGenerator.RandIDGen(10000, GetSortedBinIDs());
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
            
            Bins.Add(new Bin(name, description, percentage, iD));
            Bins[Bins.Count - 1].Setup = false;
            Account.InsertBin(Bins[Bins.Count - 1]);
            return 1;
        }     

        private int[] GetSortedBinIDs()
        {
            SortedSet<int> binIds = new SortedSet<int>();            
            int[] binIDarr = new int[Bins.Count];
            
            for(int i = 0; i < Bins.Count; ++i)
            {
                binIds.Add(Bins[i].ID);
            }

            SortedSet<int>.Enumerator enumerator = binIds.GetEnumerator();

            for(int i = 0; i < Bins.Count; ++i)
            {
                binIDarr[i] = enumerator.Current;
                enumerator.MoveNext();
            }

            return binIDarr;
        }

        //Sets all Bins' balances to zero.
     /*   internal void BinBalanceToZero()
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
        } */

        // Method to calculate the balance based on the Balance within the bins 
        public decimal TotalBalance()
        {
            decimal balance = 0;

            foreach(Bin bin in Bins)
            {
                balance += bin.Balance;
                foreach (Goal goal in bin.Goals)
                {
                    balance += goal.Value;
                }
            }

            return balance;
        }
       
        // Delete a specific bin and transfer its balance to the Savings Bin
        public void DeleteBin(string bin)
        {
            int index = Bins.IndexOf(Bins.Where(x => string.Compare(x.Name, bin) == 0).FirstOrDefault());

            // Cannot delete the Savings Bin
            if (index == 0) return;
            decimal balance = Bins[index].GetBalance();

            //Changes the bin property of each income and expense associated with the bin to Savings
            foreach(Transaction transaction in transactions) 
            {
                if (string.Compare(transaction.Bin, bin) == 0)
                    transaction.Bin = Bins[0].Name;
                else if (transaction.IncomeSplit)
                {
                    if(transaction.IncomeString.Contains(Bins[index].ID.ToString() + "-"))
                    transaction.RemoveBin(Bins[0].ID, Bins[index].ID);
                }
                transaction.DrawerGoal = null;
            }
            //transfers all funds or debts from the deleted bin to savings and deletes from the budget's list
            TransferFunds(Bins[0].Name, Bins[index].Name, balance, DateTime.Today);
            Account.DeleteBin(Bins[index]);
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
            transactions.Add(new Transaction(amount, "[Transfer from] " + Bins[index2].Name, Bins[index1].Name, Bins[index1].ID, date));
            transactions.Add(new Transaction(-amount, "[Transfer to] " + Bins[index1].Name, Bins[index2].Name, Bins[index2].ID, date));

          //  CalcBinBalance();
            return 1;

        }

        //Adds an income to the incs list either splitting it into the bins or into one list.
        public int AddIncome(decimal value, string destr, DateTime date, string mode, bool split)
        {
            int index = 1;
            Transaction newInc = new Transaction(value, destr, mode, -1, date)
            {
                BinID = -1,
                TransactionID = Account.GenerateTransactionID(date)
            };           
            
            string location = mode;
            Balance += value;
            

            if (split)
            {
                newInc.IncomeSplit = true;
                int binIndex = 0;
                //decimal splitVal = 0; 
                //decimal[] incValArr = new decimal[Bins.Count];
                //checks the percentage. if it is greater than 100 then it returns.
                decimal percentage = 0;
                foreach (Bin bin in Bins)
                {
                    percentage += bin.Percentage;
                    //also determine the amount that goes into each bin
                  //  incValArr[binIndex] = Math.Round((value * bin.Percentage / 100), 2);
                   // splitVal += incValArr[binIndex++];
                }
                

                if (percentage > 100)
                {
                    return -1;
                }
                else
                {
                    binIndex = 0;
                   // if(splitVal != value)
                    //{
                      //  incValArr[0] += (value - splitVal);
                    //}
                    //splits the income across the bins based on their percentage.
                    foreach (Bin bin in Bins)
                    {
                        newInc.IncomeString += bin.AddTransaction(newInc);
                        //bin.AddIncome(new Income(incValArr[binIndex++], destr, bin.Name, date));    
                    }

                    //remove the first '_'
                    newInc.IncomeString = newInc.IncomeString.Remove(0, 1); 
                }
                transactions.Add(newInc);                
                AddMonthBudgetInc(newInc);
                Account.InsertTransaction(newInc);
                newInc.Setup = false;
                OrganizeIncomesByDate();
                return 1;

            }
            else //income goes into a specific bin
            {
                index = Bins.IndexOf(Bins.Where(x => string.Compare(x.Name, mode) == 0).FirstOrDefault());               
                if (index != -1)
                {
                    newInc.BinID = Bins[index].ID;
                    Bins[index].AddTransaction(newInc);
                    transactions.Add(newInc);
                    AddMonthBudgetInc(newInc);
                    Account.InsertTransaction(newInc);
                    newInc.Setup = false;
                    OrganizeIncomesByDate();
                }              
                return index;
            }           
        }
        
        //Deletes an income from incs list.
        public void DeleteIncome(decimal value, string destr, DateTime date, string bin)
        {
            //finds the income object that needs to be deleted.
            int index = transactions.IndexOf(transactions.Where(x => string.Compare(x.Description, destr) == 0 && value == x.Value
            && DateTime.Compare(x.Date, date) == 0 && string.Compare(x.Bin, bin) == 0).FirstOrDefault());

            int binIndex;

            if(transactions[index].IncomeSplit)
            {
                string[] binStr = transactions[index].IncomeString.Split('_');
                int temp = int.MaxValue;
                foreach (string str in binStr)
                {
                    temp = Convert.ToInt32(str.Split('-')[0]);
                    binIndex = Bins.IndexOf(Bins.Where(x => x.ID == temp).FirstOrDefault());
                    Bins[binIndex].RemoveTransaction(transactions[index], str);
                }
                ////remove the income from each bin
                //int incIndex;
                //foreach  (Bin BIN in Bins)
                //{
                //    incIndex = BIN.Incomes.IndexOf(BIN.Incomes.Where(x => string.Compare(x.Description, destr) == 0 && (Math.Abs(value - x.Value / x.Percentage) < .5m)
                //     && DateTime.Compare(x.Date, date) == 0 && string.Compare(x.Bin, BIN.Name) == 0).FirstOrDefault());

                //    if(incIndex != -1)
                //    {
                //        BIN.RemoveIncome(incIndex);

                //    }
                //}

            } else
            {
                //find and remove the income from the bin
                binIndex = Bins.IndexOf(Bins.Where(x => x.ID == Convert.ToInt32(transactions[index].IncomeString.Split('-')[0].Remove(0,1))).FirstOrDefault());
                //int incIndex = Bins[binIndex].Incomes.IndexOf(Bins[binIndex].Incomes.Where(x => string.Compare(x.Description, destr) == 0 && value == x.Value
                //&& DateTime.Compare(x.Date, date) == 0 && string.Compare(x.Bin, bin) == 0).FirstOrDefault());

                Bins[binIndex].RemoveTransaction(transactions[index], transactions[index].IncomeString);

              //  if (incIndex != -1)
              //  {                    
              //      Bins[binIndex].RemoveIncome(incIndex);
             //   }
            }

            //finds the MonthBudget that is associated with the income object and removes it from its list.
        /*    foreach (MonthBudget bud in MonthlyBudgets)
            {
                if(Incs[index].Date.Month == bud.Date.Month && Incs[index].Date.Year == bud.Date.Year)
                {
                    bud.RemoveIncome(Incs[index]);
                    break;
                }
            } */
            DeleteMonthBudgetInc(transactions[index]);
            Balance -= transactions[index].Value;
            Account.DeleteTransaction(transactions[index]);
            transactions.RemoveAt(index);
            OrganizeIncomesByDate();
       //     CalcBinBalance();     
        }
        //maybe add a split functionality later on as well

        public void AddExpense(decimal value, string destr, DateTime date, string bin, bool drawerOrGoal, string drawer = null)
        {
            if (value > 0m) value *= -1;
            Transaction exp; 
            int index = Bins.IndexOf(Bins.Where(x => string.Compare(x.Name, bin) == 0).FirstOrDefault());
            if (index != -1)
            {
                exp = new Transaction(value, destr, Bins[index].Name, Bins[index].ID, date)
                {
                    TransactionID = Account.GenerateTransactionID(date)
                };
            }
            else
                return;
                //exp = new Expense(value, destr, null, date);

            exp.DrawerGoal = drawer;
            exp.DrawerExp = drawerOrGoal;
            
            Balance += exp.Value;
           // if (drawerOrGoal)
           // {
                Bins[index].AddTransaction(exp);
                //Bins[index].AddDrawerTransaction(exp);
            //} else
          //  {
           //     Bins[index].AddGoalExpense(exp);
          //  }
            
            AddMonthBudgetExp(exp);
            transactions.Add(exp);
            Account.InsertTransaction(exp);
            exp.Setup = false;
            //   CalcBinBalance(); 
            OrganizeExpensesByDate();
        }

        public void DeleteExpense(decimal value, string destr, DateTime date, string bin)
        {
            int index = transactions.IndexOf(transactions.Where(x => string.Compare(x.Description, destr) == 0 && value == x.Value
            && DateTime.Compare(x.Date, date) == 0 && string.Compare(x.Bin, bin) == 0).FirstOrDefault());
            int BinIndex = Bins.IndexOf(Bins.Where(x => string.Compare(x.Name, bin) == 0).FirstOrDefault());
            /*  foreach (MonthBudget bud in MonthlyBudgets)
              {
                  if (Exps[index].Date.Month == bud.Date.Month && Exps[index].Date.Year == bud.Date.Year)
                  {
                      bud.RemoveExpense(Exps[index]);
                      break;
                  }
              } */

            Bins[BinIndex].RemoveTransaction(transactions[index], transactions[index].IncomeString);
            //if (Exps[index].DrawerExp)
            //{
            //    Bins[Binindex].RemoveDrawerExpense(Exps[index]);
            //} else
            //{
            //    Bins[Binindex].RemoveGoalExpense(Exps[index]);
            //}
            
            DeleteMonthBudgetExp(transactions[index]);
            Balance -= transactions[index].Value;
            Account.DeleteTransaction(transactions[index]);
            transactions.RemoveAt(index);

            OrganizeExpensesByDate();
       //     CalcBinBalance();       
        }

        //Sorting Methods date and value
        public void OrganizeIncomesByDate()
        {
            IEnumerable<Transaction> incomes = transactions.OrderByDescending(x => x.Date);
            int count = transactions.Count;

            foreach (Transaction transaction in incomes)
            {
                transactions.Add(transaction);
            }

            for (int i = 0; i < count; i++)
            {
                transactions.RemoveAt(0);
            }
            // Incs.Sort((x, y) => DateTime.Compare(x.Date, y.Date)); FIX IT***
        }

        public void OrganizeIncomesByValue()
        {
            IEnumerable<Transaction> incomes = transactions.OrderBy(x => x.Value);
            int count = transactions.Count;

            foreach (Transaction transaction in incomes)
            {
                transactions.Add(transaction);
            }

            for (int i = 0; i < count; i++)
            {
                transactions.RemoveAt(0);
            }
            //  Incs.Sort((x, y) => DCompare(x.Value, y.Value)); FIX IT***
        }

        public void OrganizeExpensesByDate()
        {
            
            IEnumerable<Transaction> expenses = transactions.OrderByDescending(x => x.Date);
            
            int count = transactions.Count;

            foreach (Transaction transaction in expenses)
            {
                transactions.Add(transaction);
            }

            for (int i = 0; i < count; i++)
            {
                transactions.RemoveAt(0);
            }
            // Exps.Sort((x, y) => DateTime.Compare(x.Date, y.Date)); FIX IT***
        }

        public void OrganizeExpensesByValue()
        {
            IEnumerable<Transaction> expenses = transactions.OrderBy(x => x.Value);
            int count = transactions.Count;

            foreach (Transaction transaction in expenses)
            {
                transactions.Add(transaction);
            }

            for (int i = 0; i < count; i++)
            {
                transactions.RemoveAt(0);
            }

            //  Exps.Sort((x, y) => DCompare(x.Value, y.Value)); FIX IT***

        }
        
        public void OrganizeMonthBudgetsByDate()
        {
            IEnumerable<MonthBudget> monthBudgets = MonthlyBudgets.OrderByDescending(x => x.Date);

            int count = MonthlyBudgets.Count;

            foreach (MonthBudget monthBudget in monthBudgets)
            {
                MonthlyBudgets.Add(monthBudget);
            }

            for (int i = 0; i < count; i++)
            {
                MonthlyBudgets.RemoveAt(0);
            }
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
                CreateMonthlyBudget(DateTime.Now.Month, DateTime.Now.Year);                

                foreach (Bin bin in Bins)
                {
                    bin.RefreshDrawers();
                }
            }
        }

        public void CreateMonthlyBudget(int month, int year)
        {
            MonthBudget monthBudget = new MonthBudget(DefaultMonthlyBudget, month, year);
            MonthlyBudgets.Add(monthBudget);
            Account.InsertMonthlyBudget(monthBudget);
            monthBudget.Setup = false;
            CalcMonthBudget();
            OrganizeMonthBudgetsByDate();
            return;
        }

        public void CalcMonthBudget()
        {
            //finds the month budget for this month 
            decimal newDefault = 0;

            foreach (Bin bin in Bins)
            {
                foreach(Drawer drawer in bin.CurrDrawers)
                {
                  if(drawer.Month == DateTime.Now.Month && drawer.Year == DateTime.Now.Year)
                    newDefault += drawer.AvailAmount;
                }
            }


            for(int i = MonthlyBudgets.Count - 1; i >= 0; --i)
            {
                if(MonthlyBudgets[i].Date.Month == DateTime.Now.Month && MonthlyBudgets[i].Date.Year == DateTime.Now.Year)
                {
                    MonthlyBudgets[i].NewBudget(newDefault);
                    break;
                }
            }

            DefaultMonthlyBudget = newDefault;
        }

        public void AddMonthBudgetExp(Transaction exp)
        {
            //find the associated month budget and subtract it from its budget
            foreach (MonthBudget bud in MonthlyBudgets)
            {
                if(exp.Date.Month == bud.Date.Month && exp.Date.Year == bud.Date.Year)
                {
                    if(!(exp.Description.Contains("[Transfer to]")))
                    {
                        bud.AddTransaction(exp);
                        return;
                    }
                }
            }
            //if you can't find it.. create a new monthly budget and then call the function again
            CreateMonthlyBudget(exp.Date.Month, exp.Date.Year);
            AddMonthBudgetExp(exp);
        }

        public void DeleteMonthBudgetExp(Transaction exp)
        {
            //find the associated month budget and remove it from its budget
            foreach (MonthBudget bud in MonthlyBudgets)
            {
                if (exp.Date.Month == bud.Date.Month && exp.Date.Year == bud.Date.Year)
                {
                    if (!(exp.Description.Contains("[Transfer to]")))
                    {
                        bud.RemoveTransaction(exp);
                        return;
                    }
                }
            }
        }

        public void AddMonthBudgetInc(Transaction inc)
        {
            //find the associated month budget and add it to its budget
            foreach (MonthBudget bud in MonthlyBudgets)
            {
                if (inc.Date.Month == bud.Date.Month && inc.Date.Year == bud.Date.Year)
                {
                    if (!(inc.Description.Contains("[Transfer from]")))
                    {
                        bud.AddTransaction(inc);
                        return;
                    }
                }
            }
            //if you can't find it.. create a new monthly budget and then call the function again
            CreateMonthlyBudget(inc.Date.Month, inc.Date.Year);
            AddMonthBudgetInc(inc);
        }

        public void DeleteMonthBudgetInc(Transaction inc)
        {
            //find the associated month budget and remove it from its budget
            foreach (MonthBudget bud in MonthlyBudgets)
            {
                if (inc.Date.Month == bud.Date.Month && inc.Date.Year == bud.Date.Year)
                {
                    if (!(inc.Description.Contains("[Transfer from]")))
                    {
                        bud.RemoveTransaction(inc);
                        return;
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

            //recalculate the current MonthBudget.. budget
            CalcMonthBudget();

        }

        public void RemoveBinDrawer(string bin, string name)
        {
             int index = Bins.IndexOf(Bins.Where(x => string.Compare(x.Name, bin) == 0).FirstOrDefault());
            if (index != -1)
            {
                Bins[index].RemoveDrawer(name, DateTime.Now.Year, DateTime.Now.Month);
            }

            //recalculate the current MonthBudget.. budget
            CalcMonthBudget();
        }

        public void Refresh()
        {
            CheckAllRepeatTransac();
            CalcMonthBudget();
        }

        public void CheckAllRepeatTransac()
        {
            int index = 0;
            foreach  (RepeatTransaction transaction in repeatedTrans)
            {
                CheckRepeatTransac(transaction, index);
                ++index;
            }
        }

        //need to test this 
        public void CheckRepeatTransac(RepeatTransaction trans, int index)
        {
            DateTime transactionDate = new DateTime(trans.Date.Year, trans.Date.Month, trans.Date.Day);            
            if (trans.Monthly)
            {
                transactionDate = transactionDate.AddMonths(trans.Frequency);
            } else
            {
                transactionDate = transactionDate.AddDays(trans.Frequency);
            }
            
            
          
            if(DateTime.Compare(DateTime.Today, transactionDate) > -1)
            {
                if (repeatedTrans[index].Value > 0m)
                {
                    AddIncome(trans.Value, trans.Description, transactionDate, trans.Bin, trans.IncomeSplit);
                    repeatedTrans[index].Date = transactionDate;
                       // = new Income(trans.Transaction.Value, trans.Transaction.Description, trans.Transaction.Bin, transactionDate);
                } else
                {
                    AddExpense(trans.Value, trans.Description, transactionDate, trans.Bin, trans.DrawerExp, trans.DrawerGoal);
                    repeatedTrans[index].Date = transactionDate; //new Expense
                    //{
                    //    Value = exp.Value,
                    //    Description = exp.Description,
                    //    Date = transactionDate,//new myDateTime(transactionDate),
                    //    Bin = exp.Bin,
                    //    DrawerExp = exp.GetDrawerExp(),
                    //    Drawer = exp.GetDrawer(),
                    //};
                }
                
                //repeatedTrans[index].Transaction.Date = new myDateTime(transactionDate);
                CheckRepeatTransac(trans, index);
            }          
        }

        public void AddRepeatTransaction(Transaction trans, int frequency = 1, bool monthly = true)
        {
            int index = repeatedTrans.IndexOf(repeatedTrans.Where(x => (string.Compare(x.Bin, trans.Bin) == 0) 
            && (string.Compare(x.Description, trans.Description) == 0) && 
            (x.Value == trans.Value)).FirstOrDefault());

            RepeatTransaction[] rTransArr = repeatedTrans.OrderBy(x => x.TransactionID).ToArray();
            int[] sortedIDArr = new int[rTransArr.Length];
            
            for(int i = 0; i < rTransArr.Length; ++i)
            {
                sortedIDArr[i] = rTransArr[i].TransactionID;
            }

            if (index == -1)
            {
                RepeatTransaction repeat = new RepeatTransaction(trans, frequency, monthly)
                {
                    TransactionID = IDGenerator.RandIDGen(10000, sortedIDArr)
                };       
                repeatedTrans.Add(repeat);
                Account.InsertRepeatTransaction(repeat);
                repeat.Setup = false;
            }
        }

        public void RemoveRepeatTransaction(RepeatTransaction repeatTrans)
        {
            int index = repeatedTrans.IndexOf(repeatedTrans.Where(x => x.TransactionID == repeatTrans.TransactionID).FirstOrDefault());

            //string.Compare(x.Bin, repeatTrans.Bin) == 0 &&
            //string.Compare(x.Description, repeatTrans.Description) == 0 &&
            //x.Frequency == repeatTrans.Frequency &&
            //x.Monthly == repeatTrans.Monthly &&
            //x.Value == repeatTrans.Value
            if (index != -1)
            {
                Account.DeleteRepeatedTransaction(repeatedTrans[index]);
                repeatedTrans.RemoveAt(index);
                
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
