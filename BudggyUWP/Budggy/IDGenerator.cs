using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budggy
{
    static class IDGenerator
    {
        public static int RandIDGen(int n, int[] exclude)
        {
            Random r = new Random();
            int result = r.Next(n - exclude.Length);

            for (int i = 0; i < exclude.Length; ++i) {
                if (result < exclude[i]) return result;
                ++result;
            }
            return result;
        }
    }
}
