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

       

        void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Drawer> Drawers = new ObservableCollection<Drawer>();
        public ObservableCollection<Income> Incomes = new ObservableCollection<Income>();
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
                _balance = value;
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


        public decimal MinimumBalance { get; set; }

        private decimal _goalBalance;
        public decimal GoalBalance {
            get
            {
                return _goalBalance;
            }
            set
            {
                _goalBalance = value;
                OnPropertyChange("GoalBlanace");
            }
        }

        internal decimal Multiplier { get; set; }

        public Bin()
        {

        }

        public Bin(string name, string description, decimal percentage, decimal minimumBalance, decimal goalBalance, decimal multiplier)
        {
            Name = name;
            Description = description;
            Percentage = percentage;
            MinimumBalance = minimumBalance;
            GoalBalance = goalBalance;
            Multiplier = multiplier;
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
            if(Drawers.IndexOf(Drawers.Where(x => string.Compare(x.Name, name) == 0).FirstOrDefault()) != -1)
                return;

            Drawer item = new Drawer(name, maximum, DateTime.Now, Name);
            Drawers.Add(item);
        }

        public void RemoveDrawer(string name)
        {
            int index = Drawers.IndexOf(Drawers.Where(x => string.Compare(x.Name, name) == 0).FirstOrDefault());
            if(index != -1)
            {
                Drawers.RemoveAt(index);
            }
        }

        public void AddDrawerExpense(Expense exp)
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

        public void RemoveDrawerExpense(Expense exp)
        {
            Balance += exp.Value;
            int index = Drawers.IndexOf(Drawers.Where(x => string.Compare(x.Name, exp.Drawer) == 0).FirstOrDefault());
            if (index != -1)
            {
                Drawers[index].RemoveExpense(exp);
            }
        }

        public void AddIncome(Income inc)
        {
            decimal adjustedValue = AddtoGoals(inc.Value);
            Balance += adjustedValue;
            inc.Percentage = Percentage;
            Incomes.Add(inc);
            
        }

        public void RemoveIncome(int index)
        {
            Balance -= Incomes[index].Value;
            Incomes.RemoveAt(index);

            if(Balance < 0)
            {
                PullfromGoals();
            }
        }

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

        private decimal AddtoGoals(decimal val)
        {
            decimal adjVal = val;
            decimal percentage = 0;
            decimal splitAmount = 0;

            //1. check if the percentages add up to less than or equal to 100%
            foreach (Goal goal in Goals)
            {
                percentage += goal.Percentage;
            }

            if (percentage > 1) return adjVal;
            //2. then remove part of the adjVal and insert into the goals
            //need to test this to see if adjVal -= splitAmount works outside of the if and else.
            foreach (Goal goal in Goals)
            {
                splitAmount = adjVal * (goal.Percentage);
                if(goal.Value + splitAmount >= goal.GoalVal)
                {
                    splitAmount = (goal.GoalVal - goal.Value);
                    goal.Value = goal.GoalVal;
                //    adjVal -= splitAmount;
                } else
                {
                    goal.Value += splitAmount;
               //     adjVal -= splitAmount;
                }
                adjVal -= splitAmount;
            }

            return adjVal;
        }

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

        //need a function to change the priority of goals correctly
    }

}
