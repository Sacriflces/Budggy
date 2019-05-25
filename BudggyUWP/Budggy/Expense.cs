﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budggy
{
    public class Expense : Transaction
    {
        public string Drawer;
        public Expense() : base()
        {

        }
        public Expense(decimal value, string destr, string bin, DateTime date) : base(value, destr, bin, date)
        {
            Drawer = null;
        }

        public override bool IsIncome()
        {
            return false;
        }


    }
}
