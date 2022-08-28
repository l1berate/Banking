using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Security.AccessControl;

namespace Banking
{
    internal class Database
    {
        SqlConnection sqlConnection = new SqlConnection(
                "server=DESKTOP-A1JL32J;database=bankingDB;integrated security=true");

        // create account
        private bool createAccount(string accName, string userName, string passWord, 
            string ssn, string email, string phoneno, string accType, double accBalance,
            bool isAdmin=false)
        {
            if (!isAdmin)
            {
                // 3 insert into commands to generate new account
                // use try/catch blocks and return false if account creation fails
                // easy way to pass error message to consolemenu?
                SqlCommand cmd = new SqlCommand("", sqlConnection);
            }
            else
            {
                // same as above but admin bit is set to true (1)

            }
            return true;
        }

        // delete account


        // edit account


        // transfer funds


        // search accounts


        // accounts list


        // summarize accounts


        // view account


        // withdraw


        // deposit


        // change password


        // view account last 5 transactions



    }
}
