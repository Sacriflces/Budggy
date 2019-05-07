using System;
using System.Collections.Generic;
using System.Text;

namespace Budggy
{
    public class Savings : Bin
    {
        public Savings(string name, string description, decimal percentage) : base(name, description, percentage)
        {
            Balance = 0;
        }

        public Savings(string name, string description, decimal percentage, decimal minimumBalance, decimal goalBalance, decimal multiplier) : base(name, description, percentage, minimumBalance, goalBalance, multiplier)
        {
            Balance = 0;
        }
    }
}
