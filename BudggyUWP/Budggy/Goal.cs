using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Budggy
{
    public class Goal
    {
        public string Name;
        public string Description;

        public decimal Value;

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
            }
        }
        public decimal GoalVal;
        public int Priority;

        public Goal()
        {
            Value = 0;
            Percentage = 0;
            GoalVal = 0;
            Priority = 0;
        }

        
    }
}
