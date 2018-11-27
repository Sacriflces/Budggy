using System;
using BudggyTestClassLibrary;


namespace BudggyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Budget budget = new Budget();

            budget.AddBin("Food", "Everything related to food", 0);
            budget.AddBin("Entertainment", "self-explanatory. games, concerts, etc.", .50);
            budget.AddBin("Projects", "fund my mad scientist ideas", .15);
            budget.AddBin("Gas", "fund my ability to be mobile", .20);
            budget.SavingsPercentage(.15);
            budget.CreateMonthlyBudget();

            budget.AddIncome(5000, "Paycheck", DateTime.Now, "Split");
            budget.AddIncome(250, "Grocery Money", DateTime.Now, "Food");

            budget.AddExpense(20.48, "Grocery", new DateTime(2018, 11, 13), "Food");
            budget.AddExpense(33.89, "Gas", new DateTime(2018, 11, 13), "Gas");
            budget.AddExpense(42.14, "Gym Membership", new DateTime(2018, 11, 13), "Entertainment");
            budget.AddExpense(4.93, "Krispy Kreme", new DateTime(2018, 11, 6), "Food");
            budget.AddExpense(31.63, "Gas", new DateTime(2018, 11, 5), "Gas");
            budget.AddExpense(6.87, "Bojangles", new DateTime(2018, 11, 2), "Food");
            budget.AddExpense(36.12, "Gas", new DateTime(2018, 11, 1), "Gas");

            budget.AddIncome(176.45, "Paycheck", new DateTime(2018, 11, 21), "Split");
            budget.AddIncome(10.10, "Tip!", new DateTime(2018, 11, 9), "Split");
            budget.AddIncome(22.00, "Paycheck", new DateTime(2018, 11, 1), "Split");
            budget.AddIncome(48.18, "Paycheck", new DateTime(2018, 11, 1), "Split");
            budget.AddIncome(34.25, "Paycheck", new DateTime(2018, 10, 25), "Split");
            budget.AddIncome(21.87, "Paycheck", new DateTime(2018, 10, 25), "Split");
            budget.AddIncome(171.32, "Paycheck", new DateTime(2018, 11, 6), "Split");
            budget.AddIncome(220.00, "Paycheck", DateTime.Today, "Split");

            budget.TransferFunds(3, -1, 500, DateTime.Now); 

            foreach  (Bin bin in budget.Bins)
            {
                Console.WriteLine("{1}: {0:C2}", bin.CalcBalance(), bin.Name);
            }
            Console.WriteLine("{1}: {0:C2}", budget.Savings.CalcBalance(), "Savings");
            Console.WriteLine("Total Balance : {0:C2}", budget.TotalBalance());
            Console.WriteLine("Monthly budget : {0:C2}", budget.MonthlyBudgets[0].Value);
            Console.ReadLine();
            //budget.AddExpense(17.75, "food", new DateTime(2025, 8, 18), "Food");
            

            

           /* 
            Bin bin = new Bin();
            bin.AddExpense();
            bin.AddExpense();
            bin.AddExpense();
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
