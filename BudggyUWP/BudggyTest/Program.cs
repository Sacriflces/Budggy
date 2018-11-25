using System;
using BudggyTestClassLibrary;


namespace BudggyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Bin bin = new Bin();
            bin.AddExpense(17.75, "food", new DateTime(2025, 8, 18));
            bin.AddExpense(20.48, "Grocery", new DateTime(2018, 11, 13));
            bin.AddExpense(33.89, "Gas", new DateTime(2018, 11, 13));
            bin.AddExpense(42.14, "Gym Membership", new DateTime(2018, 11, 13));
            bin.AddExpense(4.93, "Krispy Kreme", new DateTime(2018, 11, 6));
            bin.AddExpense(31.63, "Gas", new DateTime(2018, 11, 5));
            bin.AddExpense(6.87, "Bojangles", new DateTime(2018, 11, 2));
            bin.AddExpense(36.12, "Gas", new DateTime(2018, 11, 1));

           
            

            bin.AddIncome(176.45, "Paycheck", new DateTime(2018, 11, 21));
            bin.AddIncome(10.10, "Tip!", new DateTime(2018, 11, 9));
            bin.AddIncome(22.00, "Paycheck", new DateTime(2018, 11, 1));
            bin.AddIncome(48.18, "Paycheck", new DateTime(2018, 11, 1));
            bin.AddIncome(34.25, "Paycheck", new DateTime(2018, 10, 25));
            bin.AddIncome(21.87, "Paycheck", new DateTime(2018, 10, 25));
            bin.AddIncome(171.32, "Paycheck", new DateTime(2018,11,6));
            bin.AddIncome(220.00, "Paycheck", DateTime.Today);

            Console.WriteLine("what is happening");
            

            Console.WriteLine("Incomes     Expenses");
            for(int i = 0; i < bin.ExpenseSize(); i++)
            {
                Console.WriteLine("{0:C}     {1:C}", bin.GetIValue(i), bin.GetEValue(i));
            }

            Console.WriteLine("Bin Balance {0:C}", bin.GetBalance());

            Console.ReadLine();
            /* Console.WriteLine("{0:C} {1:C} Balance {2:C}", bin.GetIValue(0), bin.GetEValue(0), bin.CalcBalance());

             string myName = "Jonathan Service";
             char initial = 'J';
             initial = myName[1]; */

        }
    }
}
