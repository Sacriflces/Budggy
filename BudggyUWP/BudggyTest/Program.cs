using System;
using Budggy;


namespace BudggyTest
{
    class Program
    {
        static void Main(string[] args)
        {

            Bin bin = new Bin();
            bin.addExpense(17.55, "food", DateTime.Now);
            bin.addIncome(176.45, "Paycheck", DateTime.Today);
            
            
            Console.WriteLine("{0:C} {1:C}", bin.getIValue(0), bin.getEValue(0));
            Console.ReadLine();
            string myName = "Jonathan Service";
            char initial = 'J';
            initial = myName[1];
            
        }
    }
}
