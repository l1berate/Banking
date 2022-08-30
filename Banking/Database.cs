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
using System.Data.Common;

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
                reader.Close();
            }
            else
            {
                reader.Close();
                return "";
            }
            sqlCon.Close();
            return returnMe;
        }

        // transfer funds
        public void transferFunds(int source, int destination, double amount)
        {
            // create transaction and update balances
            sqlCon.Open();

            // query source account to get user number and balance
            SqlCommand cmd = new SqlCommand(
                "select userNumber, accBalance from ACCOUNTS where accNumber=@source",
                sqlCon);
            cmd.Parameters.AddWithValue("@source", source);
            SqlDataReader reader = cmd.ExecuteReader();
            int usrNoS = 0;
            double oldBalanceS = 0.0;
            if (reader.HasRows)
            {
                reader.Read();
                usrNoS = reader.GetInt32(0);
                oldBalanceS = (double)reader.GetDecimal(1);
            }
            reader.Close();

            // query destination account to get user number and balance
            SqlCommand cmd1 = new SqlCommand(
                "select userNumber, accBalance from ACCOUNTS where accNumber=@destination",
                sqlCon);
            cmd1.Parameters.AddWithValue("@destination", destination);
            SqlDataReader reader1 = cmd1.ExecuteReader();
            int usrNoD = 0;
            double oldBalanceD = 0.0;
            
            if (reader1.HasRows)
            {
                reader1.Read();
                usrNoD = reader1.GetInt32(0);
                oldBalanceD = (double)reader1.GetDecimal(1);
            }
            reader1.Close();
            
            // add the transaction to the source account
            SqlCommand cmd2 = new SqlCommand(
                "insert into TRANSACTIONS (accNumber, userNumber, transAmount, transDescription) values (@source, @usrNoS, @newBalance, @transD)",
                    sqlCon);
            cmd2.Parameters.AddWithValue("@source", source);
            cmd2.Parameters.AddWithValue("@usrNoS", usrNoS);
            cmd2.Parameters.AddWithValue("@newBalance", Math.Round(-1 * amount, 2));
            cmd2.Parameters.AddWithValue("@transD", $"Transfer to {destination}");
            cmd2.ExecuteScalar();

            // add the transaction to the destination
            SqlCommand cmd3 = new SqlCommand(
                "insert into TRANSACTIONS (accNumber, userNumber, transAmount, transDescription) values (@destination, @usrNoD, @newBalance, @transD)",
                    sqlCon);
            cmd3.Parameters.AddWithValue("@destination", destination);
            cmd3.Parameters.AddWithValue("@usrNoD", usrNoD);
            cmd3.Parameters.AddWithValue("@newBalance", Math.Round(amount, 2));
            cmd3.Parameters.AddWithValue("@transD", $"Transfer from {source}");
            cmd3.ExecuteScalar();

            //update source balance in ACCOUNTS
            SqlCommand cmd4 = new SqlCommand(
                "update ACCOUNTS set accBalance=@newBalance where accNumber=@source",
                sqlCon);
            cmd4.Parameters.AddWithValue("@newBalance", Math.Round(oldBalanceS - amount, 2));
            cmd4.Parameters.AddWithValue("@source", source);
            cmd4.ExecuteNonQuery();

            //update destination balance in ACCOUNTS
            SqlCommand cmd5 = new SqlCommand(
                "update ACCOUNTS set accBalance=@newBalance where accNumber=@destination",
                sqlCon);
            cmd5.Parameters.AddWithValue("@newBalance", Math.Round(oldBalanceD + amount, 2));
            cmd5.Parameters.AddWithValue("@destination", destination);
            cmd5.ExecuteNonQuery();

            sqlCon.Close();
        }

        // search accounts
        public string[] searchAccounts(string searchMe="", int column=0)
        {
            // search USERS where columns[column] like @searchMe
            sqlCon.Open();
            string[] columns = new string[] { 
                "userNumber",
                "accName",
                "accUsername",
                "accSSN",
                "accEmail",
                "accPhone",
                "isAdmin"};
            SqlCommand cmd;
            if (searchMe == "")
            {
                cmd = new SqlCommand(
                $"select * from USERS",
                sqlCon);
            }
            else
            {
                cmd = new SqlCommand(
                $"select * from USERS where {columns[column]} like '%{searchMe}%'",
                sqlCon);
            }
            
            //cmd.Parameters.AddWithValue("@searchMe", searchMe);
            SqlDataReader reader = cmd.ExecuteReader();
            string[] results = new string[] { "User # |   Account Name    |   Username   |    SSN    |         Email         |Phone Number|Admin",
                                              "-------|-------------------|--------------|-----------|-----------------------|------------|-----"};
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //accType = accType.Concat(new string[] { "Checkings" }).ToArray();
                    string admin = reader.GetBoolean(7) ? "true" : "false";
                    string[] resultsArray = 
                        new string[] { $"{reader.GetInt32(0)}",
                                       $"{reader.GetString(1)}",
                                       $"{reader.GetString(2)}",
                                       $"{reader.GetString(4)}",
                                       $"{reader.GetString(5)}",
                                       $"{reader.GetString(6)}",
                                       $"{admin}" };
                    string resultString = "";
                    int i = 0;
                    foreach (string res in results[0].Split("|"))
                    {
                        if (res.Length < resultsArray[i].Length)
                        {
                            resultString += String.Concat(resultsArray[i].Substring(0,res.Length-2), "..");
                            if (i < resultsArray.Length-1)
                            {
                                resultString += "|";
                            }
                        }
                        if (res.Length == resultsArray[i].Length)
                        {
                            resultString += resultsArray[i];
                            if (i < resultsArray.Length - 1)
                            {
                                resultString += "|";
                            }
                        }
                        if (res.Length > resultsArray[i].Length)
                        {
                            resultString += resultsArray[i];
                            resultString += String.Concat(Enumerable.Repeat(" ", res.Length - resultsArray[i].Length));
                            if (i < resultsArray.Length - 1)
                            {
                                resultString += "|";
                            }
                        }
                        i++;
                    }
                    results = results.Concat(new string[] { 
                        resultString }).ToArray();
                }
            }
            reader.Close();
            sqlCon.Close();
            return results.Length > 2 ? results : new string[] { "No accounts found." };
        }


        // summarize accounts
        public string summarizeAccounts()
        {
            //return back the total statements we need to get displayed
            sqlCon.Open();
            // test changes

            sqlCon.Close();
            return "";
        }

        // view account
        public string viewAccount(int usrNo)
        {
            // yet another wrapper for getUserAccountInfo?? Should be easy at
            // least hopefully, if even needed at all
            sqlCon.Open();


            sqlCon.Close();

            return "";
        }


        // withdraw
        public void withdraw(int sourceAccNo, double transAmount)
        {
            // sql updates to make a transaction, then update the ACCOUNTS table balance
            sqlCon.Open();


            sqlCon.Close();

        }


        // deposit
        public void deposit(int destinationAccNo, double transAmount)
        {
            // same as withdraw just positive instead of negative
            sqlCon.Open();


            sqlCon.Close();

        }

        // change password
        public void changePassword(int usrNo, string newPassword)
        {
            // a simple update of the password
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand(
                "update USERS set accPassword=@newPassword where userNumber=@usrNo", 
                sqlCon);
            cmd.Parameters.AddWithValue("@newPassword", newPassword);
            cmd.Parameters.AddWithValue("@usrNo", usrNo);
            cmd.ExecuteNonQuery();

            sqlCon.Close();
        }

        // view account last 5 transactions



    }
}
