using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Security.AccessControl;
using System.Diagnostics;
using System.Security.Principal;

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
            bool returnValue = (Convert.ToInt32(cmd.ExecuteScalar()) == 1) ? true : false;
            sqlCon.Close();
            return returnValue;
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
            bool returnValue = (Convert.ToInt32(cmd.ExecuteScalar()) == 1) ? true : false;
            sqlCon.Close();
            return returnValue;
        }


        // create account
        public void createAccount(string accName, string userName, string passWord, 
            string ssn, string email, string phoneno, string[] accType, double[] accBalance,
            bool isAdmin=false)
        {
            sqlCon.Open();
            
            // 3 insert into commands to generate new account
            SqlCommand cmd = new SqlCommand(
                "insert into USERS (accName, accUsername, accPassword, accSSN, accEmail, accPhone, isAdmin) values (@accName, @userName, @passWord, @ssn, @email, @phoneno, @isAdmin)", 
                sqlCon);
            cmd.Parameters.AddWithValue("@accName", accName);
            cmd.Parameters.AddWithValue("@userName", userName);
            cmd.Parameters.AddWithValue("@passWord", passWord);
            cmd.Parameters.AddWithValue("@ssn", ssn);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@phoneno", phoneno);
            cmd.Parameters.AddWithValue("@isAdmin", (isAdmin) ? 1 : 0);
            cmd.ExecuteNonQuery();

            // get user number of newly created account for ACCOUNTS table
            SqlCommand cmd1 = new SqlCommand(
                "select userNumber from USERS where accUsername=@userName and accPassword=@passWord",
                sqlCon);
            cmd1.Parameters.AddWithValue("@userName", userName);
            cmd1.Parameters.AddWithValue("@passWord", passWord);
            int usrNo = Convert.ToInt32(cmd1.ExecuteScalar());

            // insert info for the ACCOUNTS table now and grab the account numbers
            
            foreach (string acct in accType)
            {
                // create entry in ACCOUNTS table
                SqlCommand cmd2 = new SqlCommand(
                    "insert into ACCOUNTS (userNumber, accType, accBalance) values (@usrNo, @acct, @accBal)",
                    sqlCon);
                cmd2.Parameters.AddWithValue("@usrNo", usrNo);
                cmd2.Parameters.AddWithValue("@acct", acct);
                cmd2.Parameters.AddWithValue("@accBal", accBalance[Array.FindIndex(accType, m => m == acct)]);
                cmd2.ExecuteScalar();

                // get account number of newly created account for TRANSACTIONS table
                SqlCommand cmd3 = new SqlCommand(
                    "select accNumber from ACCOUNTS where userNumber=@usrNo and accType=@acct",
                    sqlCon);
                cmd3.Parameters.AddWithValue("@usrNo", usrNo);
                cmd3.Parameters.AddWithValue("@acct", acct);
                int accNo = Convert.ToInt32(cmd3.ExecuteScalar());

                // add entry for 'Initial Deposit' in TRANSACTIONS
                SqlCommand cmd4 = new SqlCommand(
                    "insert into TRANSACTIONS (accNumber, userNumber, transAmount, transDescription) values (@accNo, @usrNo, @accBal, @transD)",
                    sqlCon);
                cmd4.Parameters.AddWithValue("@accNo", accNo);
                cmd4.Parameters.AddWithValue("@usrNo", usrNo);
                cmd4.Parameters.AddWithValue("@accBal", accBalance[Array.FindIndex(accType, m => m == acct)]);
                cmd4.Parameters.AddWithValue("@transD", (acct == "Loan") ? "Loan Approved" : "Initial Deposit");
                cmd4.ExecuteScalar();
            }
            sqlCon.Close();
        }

        // delete account
        public void deleteAccount(int usrNo)
        {
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand(
                    "delete from TRANSACTIONS where userNumber=@usrNo",
                    sqlCon);
            cmd.Parameters.AddWithValue("@usrNo", usrNo);
            cmd.ExecuteNonQuery();

            SqlCommand cmd1 = new SqlCommand(
                    "delete from ACCOUNTS where userNumber=@usrNo",
                    sqlCon);
            cmd1.Parameters.AddWithValue("@usrNo", usrNo);
            cmd1.ExecuteNonQuery();

            SqlCommand cmd2 = new SqlCommand(
                    "delete from USERS where userNumber=@usrNo",
                    sqlCon);
            cmd2.Parameters.AddWithValue("@usrNo", usrNo);
            cmd2.ExecuteNonQuery();

            sqlCon.Close();
        }

        // edit account
        public bool editAccount(int usrNo, string accName, string userName, string passWord,
            string ssn, string email, string phoneno, int isAdmin)
        {
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand(
                "update USERS set accName=@accName, accUsername=@userName, accPassword=@passWord, accSSN=@ssn, accEmail=@email, accPhone=@phoneno, isAdmin=@isAdmin where userNumber=@usrNo",
                sqlCon);
            cmd.Parameters.AddWithValue("@accName", accName);
            cmd.Parameters.AddWithValue("@userName", userName);
            cmd.Parameters.AddWithValue("@passWord", passWord);
            cmd.Parameters.AddWithValue("@ssn", ssn);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@phoneno", phoneno);
            cmd.Parameters.AddWithValue("@isAdmin", isAdmin);
            cmd.Parameters.AddWithValue("@usrNo", usrNo);
            cmd.ExecuteNonQuery();
            
            sqlCon.Close();
            return false;
        }

        public string getUserAccountInfo(int usrNo)
        {
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand(
                    "select * from USERS where userNumber=@usrNo",
                    sqlCon);
            cmd.Parameters.AddWithValue("@usrNo", usrNo);
            SqlDataReader reader = cmd.ExecuteReader();
            string returnMe = "";
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string bitValue = (reader.GetBoolean(7)) ? "1" : "0";
                    returnMe += reader.GetInt32(0) + "|" + reader.GetString(1) +
                        "|" + reader.GetString(2) + "|" + reader.GetString(3) + "|" +
                        reader.GetString(4) + "|" + reader.GetString(5) + "|" +
                        reader.GetString(6) + "|" + bitValue;
                }
            }
            else
            {
                return "";
            }
            sqlCon.Close();
            return returnMe;
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
