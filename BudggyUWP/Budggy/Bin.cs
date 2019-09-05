using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

       //add Year / Month fields, so that I can switch which list CurrDrawers return when it is called. I think i can also remove drawers from CurrDrawers as well. we'll see 

        void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));            
        }
        //have a huge list of drawers... and just return a list with drawers from a specific date to the UI
        public List<DrawerContainer> AllDrawers = new List<DrawerContainer>();
        public ObservableCollection<Drawer> CurrDrawers {
            get
            {
                int index = AllDrawers.IndexOf(AllDrawers.Where(x => x.Month == Month && x.Year == Year).FirstOrDefault());

                if (index == -1)
                {
                    if (AllDrawers.Count > 0) RefreshDrawers();
                    AllDrawers.Add(new DrawerContainer(Year, Month));
                    index = AllDrawers.Count - 1;
                } 
                return AllDrawers[index].Drawers;
            }

        }     //could set this up how I did the four orientation images lol 
        //LIST OF OBSERVABLECOLLECTION DRAWERS just add the drawers to this list based on their month / year and create a new one if it is different 
        //{
        //    get
        //    {
        //        List<Drawer> temp = AllDrawers.Where(x => x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year).ToList();
        //        return new ObservableCollection<Drawer>(temp.AsEnumerable());
        //    }
        //}   
      //  public ObservableCollection<Income> Incomes = new ObservableCollection<Income>();
        public ObservableCollection<Goal> Goals = new ObservableCollection<Goal>();




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

        private decimal _balance;
        public decimal Balance {
            get { return _balance; }
            set
            {
                
                _balance = Math.Round(value, 2, MidpointRounding.AwayFromZero);
                OnPropertyChange("Balance");
                if(value > 0.00m)
                BalanceString = _balance.ToString("C");
                else
                {
                    BalanceString = $"-${(-1 * value).ToString("G")}";
                }
            }
        }

        private string _balanceString;
        public string BalanceString
        {
            get { return _balanceString; }
            set
            {
                _balanceString = value;
                OnPropertyChange("BalanceString");
            }
        }

        private decimal _percentage;
        public decimal Percentage {
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



        

        internal int ID { get; set; }
        public int Month = DateTime.Now.Month;
        public int Year = DateTime.Now.Year;
        public Bin()
        {

        }

        public Bin(string name, string description, decimal percentage, int id)
        {
            Name = name;
            Description = description;
            Percentage = percentage;
            ID = id;
            Balance = 0;
        }

        public Bin(string name, string description, decimal percentage)
        {
            Name = name;
            Description = description;
            Percentage = percentage;
            Balance = 0;
        }
        
        public decimal GetBalance()
        {
            return Balance;
        }
     

        int DCompare(decimal x, decimal y)
        {
            if (x - y > 0) return 1;
            else if (x - y < 0) return -1;
            else return 0;
        }

        public void CreateDrawer(string name = null, decimal maximum = 100m)
        {
            int index;
            DrawerContainer.DCCompare dC = new DrawerContainer.DCCompare();
            index = AllDrawers.IndexOf(AllDrawers.Where(x => x.Year == DateTime.Now.Year && x.Month == DateTime.Now.Month).FirstOrDefault());
            if(index == -1)
            { //Organize by Date, latest date at the bottom
                AllDrawers.Add(new DrawerContainer(DateTime.Now.Year, DateTime.Now.Month));
                AllDrawers.Sort(dC);
                index = AllDrawers.Count - 1;
            }

            AllDrawers[index].CreateDrawer(Name, ID, name, maximum);
            //if(Drawers.IndexOf(Drawers.Where(x => string.Compare(x.Name, name) == 0 && x.Month == DateTime.Now.Month
            //&& x.Year == DateTime.Now.Year).FirstOrDefault()) != -1)
             //   return;
             

           // Drawer item = new Drawer(name, maximum, DateTime.Now, Name)
           // {
            //    ID = IDGenerator.RandIDGen(10000, GetSortedDrawerIDs())
            //};

           // Drawers.Add(item);
        }

        private int[] GetSortedDrawerIDs()
        {
            SortedSet<int> drawerIds = new SortedSet<int>();
            List<Drawer> currentDrawers = CurrDrawers.ToList().Where(x=> x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year).ToList();
            
            int[] drawerIDarr = new int[currentDrawers.Count];

            for (int i = 0; i < currentDrawers.Count; ++i)
            {
                drawerIds.Add(currentDrawers[i].ID);
            }

            SortedSet<int>.Enumerator enumerator = drawerIds.GetEnumerator();

            for (int i = 0; i < currentDrawers.Count; ++i)
            {
                drawerIDarr[i] = enumerator.Current;
                enumerator.MoveNext();
            }

            return drawerIDarr;
        }
        //Fix for Drawer Change
        public void RemoveDrawer(string name, int year, int month)
        {
            int index = CurrDrawers.IndexOf(CurrDrawers.Where(x => string.Compare(x.Name, name) == 0
            && x.Year == year && x.Month == month).FirstOrDefault());
            if(index != -1)
            {
                CurrDrawers.RemoveAt(index);
            }
        }

        public void CreateGoal(string name = null, decimal goalVal = 100m)
        {
            int iD;
            if (Goals.IndexOf(Goals.Where(x => string.Compare(x.Name, name) == 0).FirstOrDefault()) != -1)
                return;
            iD = IDGenerator.RandIDGen(10000, GetSortedGoalIDs());
            Goal item = new Goal()
            {
                Name = name,
                GoalVal = goalVal,
                Priority = Goals.Count,
                ID = iD,
                BinID = ID
            };
            
            Goals.Add(item);
        }

        public int[] GetSortedGoalIDs()
        {
            SortedSet<int> goalIds = new SortedSet<int>();
            int[] goalIDarr = new int[Goals.Count];

            for (int i = 0; i < Goals.Count; ++i)
            {
                goalIds.Add(Goals[i].ID);
            }

            SortedSet<int>.Enumerator enumerator = goalIds.GetEnumerator();

            for (int i = 0; i < Goals.Count; ++i)
            {
                goalIDarr[i] = enumerator.Current;
                enumerator.MoveNext();
            }

            return goalIDarr;
        }

        public void RemoveGoal(string name)
        {
            int index = Goals.IndexOf(Goals.Where(x => string.Compare(x.Name, name) == 0).FirstOrDefault());
            if (index != -1)
            {
                Balance += Goals[index].Value;
                Goals.RemoveAt(index);
            }
        }

        /*    public void AddDrawerExpense(Expense exp)
            {
                Balance -= exp.Value;
                int index = Drawers.IndexOf(Drawers.Where(x => string.Compare(x.Name, exp.Drawer) == 0).FirstOrDefault());
                if (index != -1)
                {
                    Drawers[index].AddExpense(exp);
                }
                if(Balance < 0)
                {
                    PullfromGoals();
                }
            }

            public void AddGoalExpense(Expense exp)
            {
                int index = Goals.IndexOf(Goals.Where(x => string.Compare(x.Name, exp.Drawer) == 0).FirstOrDefault());
                if (index != -1)
                {
                    Goals[index].AddExpense(exp);
                } else
                {
                    Balance -= exp.Value;
                }
            }

            public void RemoveDrawerExpense(Expense exp)
            {
                Balance += exp.Value;
                int index = Drawers.IndexOf(Drawers.Where(x => string.Compare(x.Name, exp.Drawer) == 0).FirstOrDefault());
                if (index != -1)
                {
                    Drawers[index].RemoveExpense(exp);
                }
            }

            public void RemoveGoalExpense(Expense exp)
            {

                int index = Goals.IndexOf(Goals.Where(x => string.Compare(x.Name, exp.Drawer) == 0).FirstOrDefault());
                if (index != -1)
                {
                   Goals[index].RemoveExpense(exp);
                } else
                {
                    Balance += exp.Value;
                }
            } */
        //Fix for Drawer Change
        public string AddTransaction(Transaction transaction)
        {
            decimal value = 0;
            if (transaction.IncomeSplit) value = Math.Round(transaction.Value * _percentage, 2);
            else value = transaction.Value;
            
            if (value > 0m)
            {              
                decimal totalAmount = 0;
                StringBuilder str = new StringBuilder();
                Tuple<int[], decimal[], decimal[]> goalAndPerc = AddToGoals(value);
                str.Append("_" + ID + "-" + _percentage + "(");

                for (int i = 0; i < Goals.Count; ++i)
                {
                    if (goalAndPerc.Item2[i] != 0)
                    {
                        str.Append("|" + goalAndPerc.Item1[i] + "-" + goalAndPerc.Item2[i]);                        
                        totalAmount += goalAndPerc.Item3[i];
                    }
                }
                str.Append(")");
                Balance += value - totalAmount;
                return str.ToString();
            } else
            {
                if (transaction.DrawerExp) {
                    Balance += transaction.Value;
                    int index = CurrDrawers.IndexOf(CurrDrawers.Where(x => String.Compare(x.Name, transaction.DrawerGoal) == 0
                                && x.Month == transaction.Date.Month && x.Year == transaction.Date.Year).FirstOrDefault());
                    if (index != -1)
                    {
                        CurrDrawers[index].AddExpense(transaction);
                        transaction.DrawerGoalID = CurrDrawers[index].ID;
                    }                  
                } else {
                    int index = Goals.IndexOf(Goals.Where(x => String.Compare(x.Name, transaction.DrawerGoal) == 0).FirstOrDefault());
                    if (index != -1)
                    {
                        Goals[index].AddExpense(transaction);
                        transaction.DrawerGoalID = Goals[index].ID;
                    }
                    else
                    {
                        Balance += transaction.Value;
                    }
                }

            }

            return "_" + ID + "-" + Percentage + "()";       
        }
        //Fix for Drawer Change
        public void RemoveTransaction(Transaction transaction, string splitString)
        {
            if (transaction.Value > 0m)
            {
                char[] separators = { '(', ')' };
                splitString = splitString.Replace(")", "");
                string[] binGoalStrs = splitString.Split(separators);
                string[] temp = binGoalStrs[0].Split('-');
                decimal value = Math.Round(transaction.Value * Convert.ToDecimal(temp[1]));
                decimal BinAmount = value;
                int goalID;
                decimal goalPerc;
                int index;
                decimal valTemp;

                if(String.Compare("", binGoalStrs[1]) != 0)
                {
                    binGoalStrs[1] = binGoalStrs[1].Remove(0, 1);
                    string[] goalStrs = binGoalStrs[1].Split('|');
                    for (int i = 0; i < goalStrs.Count(); ++i)
                    {
                        temp = goalStrs[i].Split('-');
                        goalID = Convert.ToInt32(temp[0]);
                        goalPerc = Convert.ToDecimal(temp[1]);
                        valTemp = Math.Round(value * goalPerc, 2);
                        index = Goals.IndexOf(Goals.Where(x => x.ID == goalID).FirstOrDefault());
                        Goals[index].Value -= valTemp;
                        BinAmount -= valTemp;
                    }
                }                
                Balance -= BinAmount;
            }
            else
            {
                if (transaction.DrawerExp)
                {
                    Balance -= transaction.Value;
                    int index = CurrDrawers.IndexOf(CurrDrawers.Where(x => x.ID == transaction.DrawerGoalID
                                && x.Month == transaction.Date.Month && x.Year == transaction.Date.Year).FirstOrDefault());
                    if (index != -1)
                    {
                        CurrDrawers[index].RemoveExpense(transaction);
                    }
                }
                else
                {
                    int index = Goals.IndexOf(Goals.Where(x => x.ID == transaction.DrawerGoalID).FirstOrDefault());
                    if(index != -1)
                    {
                        Goals[index].RemoveExpense(transaction);
                    } else
                    {
                        Balance -= transaction.Value;
                    }
                }
            }
        }

            /*  public void AddIncome(Income inc)
              {

                  decimal adjustedValue = AddtoGoals(inc.Value);
                  Balance += adjustedValue;
                  //inc.Percentage = Percentage;
                  //Incomes.Add(inc);

              } */

     /*       public void RemoveIncome(int index)
        {
            Balance -= Incomes[index].Value;
            Incomes.RemoveAt(index);

            if(Balance < 0)
            {
                PullfromGoals();
            }
        } */

        //going to need to test all goal stuff when I begin implementing the ui
        private void PullfromGoals()
        {
            //1. find the goal with the least priority should be organized that way

            //2. take as much as needed from the goal to set the balance to zero
            RecurPull(0);
            //3. check if the balance is still below zero, and if it is go to the goal with the next lowest priority and repeat. probably recursive,
            //so need another function.
        }

        private void RecurPull(int index)
        {
            //stop cases
            if (Balance == 0 || index == Goals.Count) return;
            decimal diff = Goals[index].Value + Balance;

            if(diff >= 0)
            {
                Goals[index].Value += Balance;
                Balance = 0;                
            } else
            {
                Balance += Goals[index].Value;
                Goals[index].Value = 0;
            }

            //I believe the lowest priority will be first
            RecurPull(++index);
            
        }
        private Tuple<int[], decimal[], decimal[]> AddToGoals(decimal val)
        {
            Tuple<int[], decimal[], decimal[]> goalAndPerc = Tuple.Create(new int[Goals.Count], new decimal[Goals.Count], new decimal[Goals.Count]);
            decimal percentage = 0;            
            decimal splitAmount = 0;
            int goalIndex = 0;

            //1. check if the percentages add up to less than or equal to 100%
            foreach (Goal goal in Goals)
            {
                percentage += goal.Percentage;
                goalAndPerc.Item1[goalIndex] = goal.ID;
                goalAndPerc.Item2[goalIndex] = goal.Percentage;
                goalAndPerc.Item3[goalIndex] = Math.Round(val * goal.Percentage, 2);
                splitAmount += goalAndPerc.Item3[goalIndex++];                
            }

            if (percentage > 1)
            {
                for (int i = 0; i < Goals.Count; ++i) {
                    goalAndPerc.Item2[i] = 0;
                }
                return goalAndPerc;
            }
            
            goalIndex = 0;
            //2. then remove part of the adjVal and insert into the goals
            //need to test this to see if adjVal -= splitAmount works outside of the if and else.
            //if (splitAmount != val * percentage)
            //{
            //    goalAndPerc.Item3[goalIndex] += ((val * percentage) - splitAmount);
            //}

            foreach (Goal goal in Goals)
            {

                if (goal.Value + goalAndPerc.Item3[goalIndex] >= goal.GoalVal)
                {
                    goalAndPerc.Item3[goalIndex] = (goal.GoalVal - goal.Value);
                    
                    goalAndPerc.Item2[goalIndex] = goalAndPerc.Item3[goalIndex] / val;
                    goal.Value = goal.GoalVal;
                }
                else
                {
                    goal.Value += goalAndPerc.Item3[goalIndex];
                }
                ++goalIndex;
            }

            return goalAndPerc;
        }

     /*   private decimal AddtoGoals(decimal val)
        {
            decimal adjVal = val;
            decimal percentage = 0;
            decimal splitAmount = 0;     
            decimal[] IncVal = new decimal[Goals.Count];
            int goalIndex = 0;

            //1. check if the percentages add up to less than or equal to 100%
            foreach (Goal goal in Goals)
            {
                percentage += goal.Percentage;
                IncVal[goalIndex] = Math.Round(val * goal.Percentage, 2);
                splitAmount += IncVal[goalIndex++];
            }

            if (percentage > 1) return adjVal;
            goalIndex = 0;
            //2. then remove part of the adjVal and insert into the goals
            //need to test this to see if adjVal -= splitAmount works outside of the if and else.
            if(splitAmount != val * percentage)
            {
                IncVal[goalIndex] += ((val * percentage) - splitAmount);
            }
            foreach (Goal goal in Goals)
            {
                
                if(goal.Value + IncVal[goalIndex] >= goal.GoalVal)
                {
                    IncVal[goalIndex] = (goal.GoalVal - goal.Value);
                    goal.Value = goal.GoalVal;               
                } else
                {
                    goal.Value += IncVal[goalIndex];        
                }
                adjVal -= IncVal[goalIndex++];
            }

            return adjVal;
        } */

        private void AddGoal()
        {
            Goal goal = new Goal()
            {
                Priority = Goals.Count,
            };

            Goals.Add(goal);
            GoalsByPriority();
            //organize based on priority
        }

        private void GoalsByPriority()
        {
            Goals.OrderByDescending(x => x.Priority);
        }

        private void RemoveGoal(Goal goal)
        {
            int index = Goals.IndexOf(Goals.Where(x => String.Compare(x.Name, goal.Name) == 0
            && String.Compare(x.Description, goal.Description) == 0).FirstOrDefault());
            int priority = Goals.Count;

            Balance += goal.Value;
            Goals.RemoveAt(index);

            //redo the priorities... assuming lowest priority (higher number) is first
            foreach (Goal gol in Goals)
            {
                gol.Priority = priority--;
            }
        }

        public void RefreshDrawers()
        {
            foreach (Drawer drawer in AllDrawers[AllDrawers.Count -1].Drawers)
            {
                if (drawer.Month != DateTime.Now.Month || drawer.Year != DateTime.Now.Year)
                {
                   CreateDrawer(drawer.Refresh());
                }                
            }
        }

        public void CreateDrawer(Drawer drawer)
        {
            int index;
            DrawerContainer.DCCompare dC = new DrawerContainer.DCCompare();
            index = AllDrawers.IndexOf(AllDrawers.Where(x => x.Year == DateTime.Now.Year && x.Month == DateTime.Now.Month).FirstOrDefault());
            if (index == -1)
            {
                //Organize by Date, latest date at the bottom
                AllDrawers.Add(new DrawerContainer(DateTime.Now.Year, DateTime.Now.Month));
                AllDrawers.Sort(dC);
                index = AllDrawers.Count - 1;
            }

            AllDrawers[index].AddDrawer(drawer);
            //if(Drawers.IndexOf(Drawers.Where(x => string.Compare(x.Name, name) == 0 && x.Month == DateTime.Now.Month
            //&& x.Year == DateTime.Now.Year).FirstOrDefault()) != -1)
            //   return;


            // Drawer item = new Drawer(name, maximum, DateTime.Now, Name)
            // {
            //    ID = IDGenerator.RandIDGen(10000, GetSortedDrawerIDs())
            //};

            // Drawers.Add(item);
        }

        //need a function to change the priority of goals correctly
    }

    public class DrawerContainer
    {
        public int Year;
        public int Month;
        public ObservableCollection<Drawer> Drawers = new ObservableCollection<Drawer>();

        public DrawerContainer(int year, int month)
        {
            Year = year;
            Month = month;
        }

        public void CreateDrawer(string binName, int binID, string name = null, decimal maximum = 100m)
        {
            if (Drawers.IndexOf(Drawers.Where(x => string.Compare(x.Name, name) == 0).FirstOrDefault()) != -1)
                return;

            Drawer item = new Drawer(name, maximum, new DateTime(Year, Month, 1), binName)
            {
                ID = IDGenerator.RandIDGen(10000, GetSortedDrawerIDs()),
                BinID = binID
            };

            Drawers.Add(item);
        }

        public void AddDrawer(Drawer drawer)
        {
            if (drawer.Year != Year || drawer.Month != Month) return;
            Drawers.Add(drawer);
        }

        private int[] GetSortedDrawerIDs()
        {
            SortedSet<int> drawerIds = new SortedSet<int>();
            List<Drawer> currentDrawers = Drawers.ToList();

            int[] drawerIDarr = new int[currentDrawers.Count];

            for (int i = 0; i < currentDrawers.Count; ++i)
            {
                drawerIds.Add(currentDrawers[i].ID);
            }

            SortedSet<int>.Enumerator enumerator = drawerIds.GetEnumerator();

            for (int i = 0; i < currentDrawers.Count; ++i)
            {
                drawerIDarr[i] = enumerator.Current;
                enumerator.MoveNext();
            }

            return drawerIDarr;
        }

        public class DCCompare : IComparer<DrawerContainer>
        {
            public int Compare(DrawerContainer x, DrawerContainer y)
            {
                if (x.Year > y.Year)
                    return 1;
                else if (x.Year < y.Year)
                    return -1;
                else
                {
                    if (x.Month > y.Month)
                        return 1;
                    else if (x.Month < y.Month)
                        return -1;
                    else
                        return 0;
                }
            }
        }
    }


}

