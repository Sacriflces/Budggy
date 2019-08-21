using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Budggy
{
    public class Goal : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChange("Description");
            }
        }

        private decimal _value;
        public decimal Value
        {
            get { return _value; }
            set
            {
                _value = Math.Round(value, 2, MidpointRounding.AwayFromZero);
                OnPropertyChange("Value");
            }
        }

        private decimal _percentage;
        public decimal Percentage
        {
            get { return _percentage; }
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

        private decimal _goalVal;
        public decimal GoalVal
        {
            get { return _goalVal; }
            set
            {
                _goalVal = value;
                OnPropertyChange("GoalVal");
            }
        }

        private int _priority;
        public int Priority
        {
            get { return _priority; }
            set
            {
                _priority = value;
                OnPropertyChange("Priority");
            }
        }

        internal int ID { get; set; }
        internal int BinID { get; set; }
        public bool ReduceAfterExp = false;
        public Goal()
        {
            Value = 0;
            Percentage = 0;
            GoalVal = 0;
            Priority = 0;
        }

        public void AddExpense(Transaction transaction)
        {
            if (ReduceAfterExp) GoalVal += transaction.Value;
            Value += transaction.Value;
        }

        public void RemoveExpense(Transaction transaction)
        {
            if (ReduceAfterExp) GoalVal -= transaction.Value;
            Value -= transaction.Value;
        }
        
    }
}
