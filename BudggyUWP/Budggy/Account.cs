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
/*
        static public List<Transaction> SelectTransactions()
        {

        }

        static public List<RepeatTransaction> SelectRepeatTransactions()
        {

        }

        static public MonthBudget SelectMonthBudget(DateTime date)
        {

        }

        static public List<Drawer> SelectDrawersByMonth(int month)
        {

        }

        static public List<Goal> SelectGoals()
        {

        } */
    }
}
