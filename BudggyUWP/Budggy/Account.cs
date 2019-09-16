using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace Budggy
{
    static class Account
    {
        static public string Username = "Jmarcus2004";
        static public string Password = "";
        static public string Email = "";
        static private string ConnectionStr = @"Data Source=75.182.88.195, 1434; Initial Catalog=Budggy; User ID=sa; Password=Stormer1;"; // Connect " +
        //"Timeout=30; Encrypt=False; TrustServerCertificate=False; ApplicationIntent=ReadWrite; MultiSubnetFailover=False";

        //"Server=75.182.88.195, 1434;Database=Budggy;User Id=sa; Password=Stormer1;"; 

        static public ObservableCollection<Bin> SelectBins()
        {
            string getBinsQuery = "SELECT * FROM [Bins] WHERE [User] =" + "'" + Username + "'";
            var Bins = new ObservableCollection<Bin>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using(SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandText = getBinsQuery;
                            using(SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Bin Bin = new Bin()
                                    {
                                        ID = reader.GetInt32(0),
                                        Name = reader.GetString(1),
                                        Description = reader.GetString(2),
                                        Balance = reader.GetDecimal(3),
                                        Percentage = reader.GetDecimal(4),
                                    };
                                    Bin.Setup = false;
                                    Bins.Add(Bin);
                                }
                            }
                        }
                    }
            }
            }
            catch
            {
               
            }
            
            return Bins;
        }

        static public void UpdateBin(Bin bin)
        {
            try
            {
                using(SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.CommandText = "stpUpdateBin";

                            //add parameters
                            command.Parameters.Add(new SqlParameter("@ID", bin.ID));
                            command.Parameters.Add(new SqlParameter("@Name", bin.Name));
                            command.Parameters.Add(new SqlParameter("@Description", bin.Description));
                            command.Parameters.Add(new SqlParameter("@Balance", bin.Balance));
                            command.Parameters.Add(new SqlParameter("@Percentage", bin.Percentage));
                            command.Parameters.Add(new SqlParameter("@User", Username));

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch
            {

            }
        }

        static public void DeleteBin(Bin bin)
        {           
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.CommandText = "stpDeleteBin";

                            //add parameters
                            command.Parameters.Add(new SqlParameter("@ID", bin.ID));                            
                            command.Parameters.Add(new SqlParameter("@User", Username));

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch
            {

            }
        }

        static public void InsertBin(Bin bin)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.CommandText = "stpInsertBin";

                            //add parameters
                            command.Parameters.Add(new SqlParameter("@ID", bin.ID));
                            command.Parameters.Add(new SqlParameter("@Name", bin.Name));
                            command.Parameters.Add(new SqlParameter("@Description", bin.Description));
                            command.Parameters.Add(new SqlParameter("@Balance", bin.Balance));
                            command.Parameters.Add(new SqlParameter("@Percentage", bin.Percentage));
                            command.Parameters.Add(new SqlParameter("@User", Username));

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch
            {

            }
        }

        static public ObservableCollection<Transaction> SelectTransactions(DateTime date)
        {
            var transactions = new ObservableCollection<Transaction>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.CommandText = "stpSelect100TransactionsFromDate";

                            //add parameters
                            command.Parameters.Add(new SqlParameter("@Date", date));                            
                            command.Parameters.Add(new SqlParameter("@User", Username));

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Transaction transaction = new Transaction()
                                    {
                                        Value = reader.GetDecimal(0),
                                        Description = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                        Date = reader.GetDateTime(2),
                                        IncomeString = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                        DrawerGoal = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                        DrawerGoalID = reader.GetInt32(5),
                                        IncomeSplit = reader.GetBoolean(6),
                                        DrawerExp = reader.GetBoolean(7),
                                        Bin = reader.IsDBNull(8) ? "" : reader.GetString(8),
                                        BinID = reader.GetInt32(10),
                                        TransactionID = reader.GetInt32(11)
                                    };
                                    transaction.Setup = false;
                                    transactions.Add(transaction);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            return transactions;
        }

        static public int GenerateTransactionID(DateTime date)
        {
            SortedSet<int> transIDs = new SortedSet<int>();
            string getTransactionQuery = "Select [TransactionID] FROM [Transactions] WHERE YEAR([Date]) = " + date.Year
                + " AND MONTH([Date]) = " + date.Month + " AND DAY([Date]) = " + date.Day + " AND [User] ='" + Username + "'";
            try
            {
                using(SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if(conn.State == System.Data.ConnectionState.Open)
                    {
                        using(SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandText = getTransactionQuery;

                            using(SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    transIDs.Add(reader.GetInt32(0));
                                }
                            }
                        }
                    }
                }
                int[] transIDArr = new int[transIDs.Count];
                SortedSet<int>.Enumerator enumerator = transIDs.GetEnumerator();

                for(int i = 0; i < transIDs.Count; ++i)
                {
                    transIDArr[i] = enumerator.Current;
                    enumerator.MoveNext();
                }

                return IDGenerator.RandIDGen(10000, transIDArr);
            }
            catch
            {
                return -1;
            }
        }
        static public void InsertTransaction(Transaction trans)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.CommandText = "stpInsertTransaction";

                            //add parameters
                            command.Parameters.Add(new SqlParameter("@Value", trans.Value));
                            command.Parameters.Add(new SqlParameter("@Description", trans.Description));
                            command.Parameters.Add(new SqlParameter("@Date", trans.Date));
                            command.Parameters.Add(new SqlParameter("@BinSplitString", trans.IncomeString));
                            command.Parameters.Add(new SqlParameter("@DrawerGoal", trans.DrawerGoal));
                            command.Parameters.Add(new SqlParameter("@DrawerGoalID", trans.DrawerGoalID));
                            command.Parameters.Add(new SqlParameter("@IncomeSplit", trans.IncomeSplit));
                            command.Parameters.Add(new SqlParameter("@DrawerExpense", trans.DrawerExp));
                            command.Parameters.Add(new SqlParameter("Bin", trans.Bin));
                            command.Parameters.Add(new SqlParameter("@User", Username));
                            command.Parameters.Add(new SqlParameter("@BinID", trans.BinID));
                            command.Parameters.Add(new SqlParameter("@TransactionID", trans.TransactionID));

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch
            {

            }
        }

        static public void UpdateTransaction(Transaction trans)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.CommandText = "stpUpdateTransaction";

                            //add parameters
                            command.Parameters.Add(new SqlParameter("@Value", trans.Value));
                            command.Parameters.Add(new SqlParameter("@Description", trans.Description));
                            command.Parameters.Add(new SqlParameter("@Date", trans.Date));
                            command.Parameters.Add(new SqlParameter("@BinSplitString", trans.IncomeString));
                            command.Parameters.Add(new SqlParameter("@DrawerGoal", trans.DrawerGoal));
                            command.Parameters.Add(new SqlParameter("@DrawerGoalID", trans.DrawerGoalID));
                            command.Parameters.Add(new SqlParameter("@IncomeSplit", trans.IncomeSplit));
                            command.Parameters.Add(new SqlParameter("@DrawerExpense", trans.DrawerExp));
                            command.Parameters.Add(new SqlParameter("@Bin", trans.Bin));
                            command.Parameters.Add(new SqlParameter("@User", Username));
                            command.Parameters.Add(new SqlParameter("@BinID", trans.BinID));
                            command.Parameters.Add(new SqlParameter("@TransactionID", trans.TransactionID));

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch
            {

            }
        }

        static public void DeleteTransaction(Transaction trans)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.CommandText = "stpDeleteTransaction";

                            //add parameters
                            command.Parameters.Add(new SqlParameter("@Date", trans.Date));
                            command.Parameters.Add(new SqlParameter("@User", Username));
                            command.Parameters.Add(new SqlParameter("@BinID", trans.BinID));
                            command.Parameters.Add(new SqlParameter("@TransactionID", trans.TransactionID));

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch
            {

            }
        }

        static public ObservableCollection<RepeatTransaction> SelectRepeatTransactions()
        {
            var repeatTransList = new ObservableCollection<RepeatTransaction>();
            string commandStr = "SELECT * FROM [RepeatedTransactions] WHERE [User] = '" + Username + "'";

            try
            {
                using(SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if(conn.State == System.Data.ConnectionState.Open)
                    {
                        using(SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandText = commandStr;
                            
                            using(SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    RepeatTransaction temp = new RepeatTransaction()
                                    {
                                        Value = reader.GetDecimal(0),
                                        Description = reader.GetString(1),
                                        Date = reader.GetDateTime(2),
                                        IncomeString = reader.GetString(3),
                                        DrawerGoal = reader.GetString(4),
                                        DrawerGoalID = reader.GetInt32(5),
                                        IncomeSplit = reader.GetBoolean(6),
                                        DrawerExp = reader.GetBoolean(7),
                                        Bin = reader.GetString(8),
                                        Frequency = reader.GetInt32(10),
                                        Monthly = reader.GetBoolean(11),
                                        BinID = reader.GetInt32(12),
                                        TransactionID = reader.GetInt32(13)
                                    };
                                    temp.Setup = false;
                                    repeatTransList.Add(temp);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            return repeatTransList;
        }

        static public void InsertRepeatTransaction(RepeatTransaction trans)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.CommandText = "stpInsertRepeatedTransaction";

                            //add parameters
                            command.Parameters.Add(new SqlParameter("@Value", trans.Value));
                            command.Parameters.Add(new SqlParameter("@Description", trans.Description));
                            command.Parameters.Add(new SqlParameter("@Date", trans.Date));
                            command.Parameters.Add(new SqlParameter("@BinSplitString", trans.IncomeString));
                            command.Parameters.Add(new SqlParameter("@DrawerGoal", trans.DrawerGoal));
                            command.Parameters.Add(new SqlParameter("@DrawerGoalID", trans.DrawerGoalID));
                            command.Parameters.Add(new SqlParameter("@IncomeSplit", trans.IncomeSplit));
                            command.Parameters.Add(new SqlParameter("@DrawerExpense", trans.DrawerExp));
                            command.Parameters.Add(new SqlParameter("@Bin", trans.Bin));
                            command.Parameters.Add(new SqlParameter("@User", Username));
                            command.Parameters.Add(new SqlParameter("@Frequency", trans.Frequency));
                            command.Parameters.Add(new SqlParameter("@Monthly", trans.Monthly));
                            command.Parameters.Add(new SqlParameter("@BinID", trans.BinID));
                            command.Parameters.Add(new SqlParameter("@TransactionID", trans.TransactionID));

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch
            {

            }
        }

        static public void UpdateRepeatTransaction(RepeatTransaction trans)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.CommandText = "stpUpdateRepeatedTransaction";

                            //add parameters
                            command.Parameters.Add(new SqlParameter("@Value", trans.Value));
                            command.Parameters.Add(new SqlParameter("@Description", trans.Description));
                            command.Parameters.Add(new SqlParameter("@Date", trans.Date));
                            command.Parameters.Add(new SqlParameter("@BinSplitString", trans.IncomeString));
                            command.Parameters.Add(new SqlParameter("@DrawerGoal", trans.DrawerGoal));
                            command.Parameters.Add(new SqlParameter("@DrawerGoalID", trans.DrawerGoalID));
                            command.Parameters.Add(new SqlParameter("@IncomeSplit", trans.IncomeSplit));
                            command.Parameters.Add(new SqlParameter("@DrawerExpense", trans.DrawerExp));
                            command.Parameters.Add(new SqlParameter("@Bin", trans.Bin));
                            command.Parameters.Add(new SqlParameter("@User", Username));
                            command.Parameters.Add(new SqlParameter("@Frequency", trans.Frequency));
                            command.Parameters.Add(new SqlParameter("@Monthly", trans.Monthly));
                            command.Parameters.Add(new SqlParameter("@BinID", trans.BinID));
                            command.Parameters.Add(new SqlParameter("@TransactionID", trans.TransactionID));

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch
            {

            }
        }

        static public void DeleteRepeatedTransaction(RepeatTransaction trans)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.CommandText = "stpDeleteRepeatedTransaction";

                            //add parameters
                            command.Parameters.Add(new SqlParameter("@User", Username));
                            command.Parameters.Add(new SqlParameter("@TransactionID", trans.TransactionID));

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch
            {

            }
        }

        static public MonthBudget SelectMonthBudget(DateTime date)
        {
            string getMonthBudgetQuery = "SELECT * FROM [MonthlyBudgets] WHERE [User] = '" + Username + "' AND MONTH([Date]) = " + date.Month
                + " AND YEAR([Date]) = " + date.Year;
            MonthBudget monthBudget = new MonthBudget();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandText = getMonthBudgetQuery;

                            using(SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    
                                    monthBudget.BudgetVal = reader.GetDecimal(0);
                                    monthBudget.Value = reader.GetDecimal(1);
                                    monthBudget.IncAmount = reader.GetDecimal(2);
                                    monthBudget.ExpAmount = reader.GetDecimal(3);
                                    monthBudget.Date = reader.GetDateTime(4);
                                    monthBudget.Setup = false;
                                }
                            }

                            
                        }
                    }
                }
            }
            catch
            {

            }

            return monthBudget;
        }
        static public void InsertMonthlyBudget(MonthBudget monthBudget)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.CommandText = "stpInsertMonthlyBudget";

                            //add parameters
                            command.Parameters.Add(new SqlParameter("@BudgetValue", monthBudget.BudgetVal));
                            command.Parameters.Add(new SqlParameter("@RemainingBudget", monthBudget.Value));
                            command.Parameters.Add(new SqlParameter("@IncomeAmount", monthBudget.IncAmount));
                            command.Parameters.Add(new SqlParameter("@ExpenseAmount", monthBudget.ExpAmount));
                            command.Parameters.Add(new SqlParameter("@Date", monthBudget.Date));
                            command.Parameters.Add(new SqlParameter("@User", Username));

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch
            {

            }
        }
        static public void UpdateMonthBudget(MonthBudget monthBudget)
        {           
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.CommandText = "stpUpdateMonthlyBudget";

                            //add parameters
                            command.Parameters.Add(new SqlParameter("@BudgetValue", monthBudget.BudgetVal));
                            command.Parameters.Add(new SqlParameter("@RemainingBudget", monthBudget.Value));
                            command.Parameters.Add(new SqlParameter("@IncomeAmount", monthBudget.IncAmount));
                            command.Parameters.Add(new SqlParameter("@ExpenseAmount", monthBudget.ExpAmount));
                            command.Parameters.Add(new SqlParameter("@Date", monthBudget.Date));
                            command.Parameters.Add(new SqlParameter("@User", Username));

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch
            {

            }
        }

        static public void DeleteMonthBudget(MonthBudget monthBudget)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.CommandText = "stpDeleteMonthBudget";

                            //add parameters
                            command.Parameters.Add(new SqlParameter("@Date", monthBudget.Date));
                            command.Parameters.Add(new SqlParameter("@User", Username));

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch
            {

            }
        }

        static public List<Drawer> SelectDrawersByMonth(DateTime date)
        {
            var drawers = new List<Drawer>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.CommandText = "stpSelectDrawersByMonth";

                            command.Parameters.Add(new SqlParameter("@Month", date.Month));
                            command.Parameters.Add(new SqlParameter("@Year", date.Year));
                            command.Parameters.Add(new SqlParameter("@User", Username));

                            using(SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Drawer temp = new Drawer()
                                    {
                                        ID = reader.GetInt32(0),
                                        Name = reader.GetString(1),
                                        Month = reader.GetInt32(2),
                                        Year = reader.GetInt32(3),
                                        MonthlyAmount = reader.GetDecimal(4),
                                        AvailAmount = reader.GetDecimal(5),
                                        CurrentSpent = reader.GetDecimal(6),
                                        rollOver = reader.GetBoolean(7),
                                        BinName = reader.GetString(8),
                                        BinID = reader.GetInt32(10)
                                    };
                                    temp.Setup = false;
                                    drawers.Add(temp);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            return drawers;
        }

        static public void InsertDrawer(Drawer drawer)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.CommandText = "stpInsertDrawer";

                            command.Parameters.Add(new SqlParameter("@ID", drawer.ID));
                            command.Parameters.Add(new SqlParameter("@Name", drawer.Name));
                            command.Parameters.Add(new SqlParameter("@Month", drawer.Month));
                            command.Parameters.Add(new SqlParameter("@Year", drawer.Year));
                            command.Parameters.Add(new SqlParameter("@MonthlyAmount", drawer.MonthlyAmount));
                            command.Parameters.Add(new SqlParameter("@AvailAmount", drawer.AvailAmount));
                            command.Parameters.Add(new SqlParameter("@Spent", drawer.CurrentSpent));
                            command.Parameters.Add(new SqlParameter("@RollOver", drawer.rollOver));
                            command.Parameters.Add(new SqlParameter("@Bin", drawer.BinName));
                            command.Parameters.Add(new SqlParameter("@User", Username));
                            command.Parameters.Add(new SqlParameter("@BinID", drawer.BinID));

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch
            {

            }
        }

        static public void UpdateDrawer(Drawer drawer)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.CommandText = "stpUpdateDrawer";

                            command.Parameters.Add(new SqlParameter("@ID", drawer.ID));
                            command.Parameters.Add(new SqlParameter("@Name", drawer.Name));
                            command.Parameters.Add(new SqlParameter("@Month", drawer.Month));
                            command.Parameters.Add(new SqlParameter("@Year", drawer.Year));
                            command.Parameters.Add(new SqlParameter("@MonthlyAmount", drawer.MonthlyAmount));
                            command.Parameters.Add(new SqlParameter("@AvailAmount", drawer.AvailAmount));
                            command.Parameters.Add(new SqlParameter("@Spent", drawer.CurrentSpent));
                            command.Parameters.Add(new SqlParameter("@RollOver", drawer.rollOver));
                            command.Parameters.Add(new SqlParameter("@Bin", drawer.BinName));                            
                            command.Parameters.Add(new SqlParameter("@User", Username));
                            command.Parameters.Add(new SqlParameter("@BinID", drawer.BinID));

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch
            {

            }
        }

        static public void DeleteDrawer(Drawer drawer)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.CommandText = "stpDeleteDrawer";

                            command.Parameters.Add(new SqlParameter("@ID", drawer.ID));
                            command.Parameters.Add(new SqlParameter("@Month", drawer.Month));
                            command.Parameters.Add(new SqlParameter("@Year", drawer.Year));
                            command.Parameters.Add(new SqlParameter("@Bin", drawer.BinName));
                            command.Parameters.Add(new SqlParameter("@User", Username));
                            command.Parameters.Add(new SqlParameter("@BinID", drawer.BinID));

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch
            {

            }
        }
        static public List<Goal> SelectGoals()
        {
            var goals = new List<Goal>();
            string SelectGoalsQuery = "SELECT * FROM [Goals] WHERE [User] = '" + Username + "'"; 
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandText = SelectGoalsQuery;

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Goal temp = new Goal()
                                    {
                                        ID = reader.GetInt32(0),
                                        Name = reader.GetString(1),
                                        Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                        Value = reader.GetDecimal(3),
                                        Percentage = reader.GetDecimal(4),
                                        GoalVal = reader.GetDecimal(5),
                                        Priority = reader.GetInt32(6),
                                        ReduceAfterExp = reader.GetBoolean(7),
                                        BinName = reader.GetString(8),
                                        BinID = reader.GetInt32(10)     
                                    };
                                    temp.Setup = false;
                                    goals.Add(temp);
                                }
                                
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            return goals;
        }
        static public void InsertGoal(Goal goal)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.CommandText = "stpInsertGoal";

                            //add parameters
                            command.Parameters.Add(new SqlParameter("@ID", goal.ID));
                            command.Parameters.Add(new SqlParameter("@Name", goal.Name));
                            command.Parameters.Add(new SqlParameter("@Description", goal.Description));
                            command.Parameters.Add(new SqlParameter("@Value", goal.Value));
                            command.Parameters.Add(new SqlParameter("@Percentage", goal.Percentage));
                            command.Parameters.Add(new SqlParameter("@GoalValue", goal.GoalVal));
                            command.Parameters.Add(new SqlParameter("@Priority", goal.Priority));
                            command.Parameters.Add(new SqlParameter("@ReduceAfterExpense", goal.ReduceAfterExp));
                            command.Parameters.Add(new SqlParameter("@Bin", goal.BinName));
                            command.Parameters.Add(new SqlParameter("@User", Username));
                            command.Parameters.Add(new SqlParameter("@BinID", goal.BinID));

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch
            {

            }
        }
        static public void UpdateGoal(Goal goal)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.CommandText = "stpUpdateGoal";

                            //add parameters
                            command.Parameters.Add(new SqlParameter("@ID", goal.ID));
                            command.Parameters.Add(new SqlParameter("@Name", goal.Name));
                            command.Parameters.Add(new SqlParameter("@Description", goal.Description));
                            command.Parameters.Add(new SqlParameter("@Value", goal.Value));
                            command.Parameters.Add(new SqlParameter("@Percentage", goal.Percentage));
                            command.Parameters.Add(new SqlParameter("@GoalValue", goal.GoalVal));
                            command.Parameters.Add(new SqlParameter("@Priority", goal.Priority));
                            command.Parameters.Add(new SqlParameter("@ReduceAfterExpense", goal.ReduceAfterExp));
                            command.Parameters.Add(new SqlParameter("@Bin", goal.BinName));
                            command.Parameters.Add(new SqlParameter("@User", Username));
                            command.Parameters.Add(new SqlParameter("@BinID", goal.BinID));

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch
            {

            }
        }

        static public void DeleteGoal(Goal goal)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.CommandText = "stpDeleteGoal";

                            //add parameters
                            command.Parameters.Add(new SqlParameter("@ID", goal.ID));
                            command.Parameters.Add(new SqlParameter("@User", Username));
                            command.Parameters.Add(new SqlParameter("@BinID", goal.BinID));

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch
            {

            }
        }
        /*  Need Select, Update, and Delete functions 


                

                


 */
    }
}
