using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budggy
{
    internal class Budget
    {
        List<Bin> Bins { get; set; }
        string Name { get; set; }
        double Balance { get; set; }
    }
}
