using System;
using System.Collections.Generic;
using System.Text;

namespace BudggyTestClassLibrary
{
    class Savings : Bin
    {
        public Savings(string name, string description, double percentage) : base(name, description, percentage)
        {
        }

        public Savings(string name, string description, double percentage, double minimumBalance, double goalBalance, double multiplier) : base(name, description, percentage, minimumBalance, goalBalance, multiplier)
        {
        }
    }
}
