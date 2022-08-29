using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Security.AccessControl;
using System.Diagnostics;

namespace Banking
{
    internal class Database
    {
        SqlConnection sqlCon = new SqlConnection(
                "server=DESKTOP-A1JL32J;database=bankingDB;integrated security=true");

        
        // check login creds
        public bool checkLogin(string usrName, string pWord)
        {
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand(
                "select count(*) from USERS where accUsername=@usrName and accPassword=@pWord", 
                sqlCon);
            cmd.Parameters.AddWithValue("@usrName", usrName);
            cmd.Parameters.AddWithValue("@pWord", pWord);
            return (Convert.ToInt32(cmd.ExecuteScalar()) == 1) ? true : false;
        }

        // check admin login creds
        public bool checkAdminLogin(string usrName, string pWord)
        {
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand(
                "select count(*) from USERS where accUsername=@usrName and accPassword=@pWord and isAdmin=1",
                sqlCon);
            cmd.Parameters.AddWithValue("@usrName", usrName);
            cmd.Parameters.AddWithValue("@pWord", pWord);
            return (Convert.ToInt32(cmd.ExecuteScalar()) == 1) ? true : false;
        }


        // create account
        public bool createAccount(string accName, string userName, string passWord, 
            string ssn, string email, string phoneno, string accType, double accBalance,
            bool isAdmin=false)
        {
            if (!isAdmin)
            {
                // 3 insert into commands to generate new account
                // use try/catch blocks and return false if account creation fails
                // easy way to pass error message to consolemenu?
                SqlCommand cmd = new SqlCommand(
                    "", 
                    sqlCon);
            }
            else
            {
                // same as above but admin bit is set to true (1)

            }
            return false;
        }

        // delete account
        public bool deleteAccount(int usrNo)
        {

            return false;
        }

        // edit account
        public bool editAccount(int usrNo, string accName, string userName, string passWord,
            string ssn, string email, string phoneno, string accType, double accBalance,
            bool isAdmin = false)
        {

            return false;
        }

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
