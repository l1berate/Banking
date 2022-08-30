// See https://aka.ms/new-console-template for more information

using Banking;
using System;
using System.IO;
using System.Diagnostics;

//initialize db object for everyone to use
Database db = new Database();

#region Menus

void mainMenu()
{
    ConsoleMenu welcomeMenu = new ConsoleMenu("Welcome to Old School Banking!",
        new string[] {"Login as Administrator", "Login as Customer", "Exit"},
        "Press Up/Down keys and then Enter to make a selection");
    int result = welcomeMenu.ShowMenu();

    switch (result)
    {
        case 0:
            adminLoginMenu();
            break;
        case 1:
            customerLoginMenu();
            break;
        case 2:
            exitScreen();
            break;
    }

}

void adminLoginMenu()
{
    ConsoleMenu adminUsernamePrompt = new ConsoleMenu("Admin Login - Username",
        new string[] { "Please enter your administrator username below." },
        "Username: ",
        ConsoleColor.Black,
        ConsoleColor.Green);
    string username = adminUsernamePrompt.ShowPrompt();

    ConsoleMenu adminPasswordPrompt = new ConsoleMenu("Admin Login - Password",
        new string[] { "Please enter your administrator password below." },
        "Password: ",
        ConsoleColor.Black,
        ConsoleColor.DarkGreen);
    string password = adminPasswordPrompt.ShowPrompt();
    try
    {
        if (!db.checkAdminLogin(username, password))
        {
            ConsoleMenu wrongCreds = new ConsoleMenu("Admin Login - Invalid Credentials",
            new string[] { "The credentials you have entered are incorrect. Please press any key to exit." },
            "",
            ConsoleColor.Black,
            ConsoleColor.Red);
            wrongCreds.ShowInfo();
            mainMenu();
        }
        adminMenu();
    }
    catch (Exception ex)
    {
        dbError(ex);
    }
    

}

void customerLoginMenu()
{
    ConsoleMenu custUsernamePrompt = new ConsoleMenu("Customer Login - Username",
        new string[] { "Please enter your customer username below." },
        "Username: ",
        ConsoleColor.Black,
        ConsoleColor.Green);
    string username = custUsernamePrompt.ShowPrompt();

    ConsoleMenu custPasswordPrompt = new ConsoleMenu("Customer Login - Password",
        new string[] { "Please enter your customer password below." },
        "Password: ",
        ConsoleColor.Black,
        ConsoleColor.DarkGreen);
    string password = custPasswordPrompt.ShowPrompt();

    try
    {
        if (!db.checkLogin(username, password))
        {
            ConsoleMenu wrongCreds = new ConsoleMenu("Customer Login - Invalid Credentials",
            new string[] { "The credentials you have entered are incorrect. Please press any key to exit." },
            "",
            ConsoleColor.Black,
            ConsoleColor.Red);
            wrongCreds.ShowInfo();
            mainMenu();
        }
        customerMenu();
    }
    catch (Exception ex)
    {
        dbError(ex);
    }
    
}

void adminMenu()
{
    bool continueAdminMenu = false;
    do
    {
        continueAdminMenu = false;
        ConsoleMenu adminMenu = new ConsoleMenu("Admin Menu",
        new string[] { "Create a new account",
                        "Delete an account",
                        "Edit account details",
                        "Transfer funds between accounts",
                        "Search accounts",
                        "List accounts",
                        "Summary of all accounts",
                        "Exit"},
        "Press Up/Down keys and then Enter to make a selection",
        ConsoleColor.White,
        ConsoleColor.Black);
        int choice = adminMenu.ShowMenu();

        switch (choice)
        {
            case 0:
                // create account
                /* createAccount(string accName, string userName, string passWord, 
                string ssn, string email, string phoneno, string[] accType, double[] accBalance,
                bool isAdmin = false)*/

                ConsoleMenu askName = new ConsoleMenu("Enter New Account's Information",
                new string[] { "Please enter in the full name of the account holder." },
                "Full Name: ",
                ConsoleColor.White,
                ConsoleColor.Green);
                string accName = askName.ShowPrompt();

                ConsoleMenu askUsername = new ConsoleMenu("Enter New Account's Information",
                new string[] { "Please enter in the username of the account holder." },
                "Username: ",
                ConsoleColor.White,
                ConsoleColor.Green);
                string userName = askUsername.ShowPrompt();

                ConsoleMenu askPassword = new ConsoleMenu("Enter New Account's Information",
                new string[] { "Please enter in the password of the account holder." },
                "Password: ",
                ConsoleColor.White,
                ConsoleColor.Green);
                string passWord = askPassword.ShowPrompt();

                ConsoleMenu askSSN = new ConsoleMenu("Enter New Account's Information",
                new string[] { "Please enter in the social security number of the account holder." },
                "SSN: ",
                ConsoleColor.White,
                ConsoleColor.Green);
                string ssn = askSSN.ShowPrompt();

                ConsoleMenu askEmail = new ConsoleMenu("Enter New Account's Information",
                new string[] { "Please enter in the email of the account holder." },
                "Email: ",
                ConsoleColor.White,
                ConsoleColor.Green);
                string email = askEmail.ShowPrompt();

                ConsoleMenu askPhone = new ConsoleMenu("Enter New Account's Information",
                new string[] { "Please enter in the phone number of the account holder." },
                "Phone Number: ",
                ConsoleColor.White,
                ConsoleColor.Green);
                string phoneno = askPhone.ShowPrompt();

                ConsoleMenu accComboMenu = new ConsoleMenu("Choose which Accounts to Open",
                    new string[] {"Checkings only",                 //case 0
                              "Checkings & Savings",            //case 1
                              "Checkings & Loan",               //case 2
                              "Checkings, Savings, & Loan" },   //case 3
                    "Press Up/Down keys and then Enter to choose; Every account must have a checkings.",
                    ConsoleColor.White,
                    ConsoleColor.Green);
                int accCombo = accComboMenu.ShowMenu();

                string[] accType = new string[0];
                switch (accCombo)
                {
                    case 0:
                        accType = accType.Concat(new string[] { "Checkings" }).ToArray();
                        break;
                    case 1:
                        accType = accType.Concat(new string[] { "Checkings", "Savings" }).ToArray();
                        break;
                    case 2:
                        accType = accType.Concat(new string[] { "Checkings", "Loan" }).ToArray();
                        break;
                    case 3:
                        accType = accType.Concat(new string[] { "Checkings", "Savings", "Loan" }).ToArray();
                        break;
                }

                double[] accBalance = new double[0];
                ConsoleMenu askBalance;
                foreach (string acct in accType)
                {
                    askBalance = new ConsoleMenu($"Enter {acct} Initial Balance",
                        new string[] { $"Please enter in the starting balance of the {acct}." },
                        "Balance: $",
                        ConsoleColor.White,
                        ConsoleColor.Green);
                    double balance = Convert.ToDouble(askBalance.ShowPrompt());
                    accBalance = accBalance.Concat(new double[] { balance }).ToArray();
                }

                ConsoleMenu askAdminMenu = new ConsoleMenu("Choose Account Privilege Level",
                    new string[] {"Customer Privileges",                 //case 0
                              "Administrator Privileges" },          //case 1
                    "Press Up/Down keys and then Enter to make a selection",
                    ConsoleColor.White,
                    ConsoleColor.Green);
                int isAdmin = askAdminMenu.ShowMenu();

                try
                {
                    // call db.createAccount here and catch any sql related errors.
                    db.createAccount(accName, userName, passWord, ssn, email, phoneno,
                        accType, accBalance, (isAdmin == 1) ? true : false);

                    string plural = (accType.Length == 1) ? " has been" : "s have been";
                    ConsoleMenu accCreated = new ConsoleMenu("Account Created",
                        new string[] { $"{accName}'s account{plural} successfully created." },
                        "Press any key to exit.",
                        ConsoleColor.DarkGreen,
                        ConsoleColor.White);
                    accCreated.ShowInfo();
                    continueAdminMenu = true;
                }
                catch (Exception ex)
                {
                    dbError(ex);
                }


                break;
            case 1:
                // delete account
                // get user number of account to delete
                ConsoleMenu askUsrNo = new ConsoleMenu("Delete an Account",
                new string[] { "Please enter in the User Number of the account holder.",
                "You can use search to find this if needed."},
                "User Number: ",
                ConsoleColor.Red,
                ConsoleColor.Black);
                int usrNo = Convert.ToInt32(askUsrNo.ShowPrompt());

                try
                {
                    db.deleteAccount(usrNo);

                    ConsoleMenu usrDeleted = new ConsoleMenu("Account Deleted",
                        new string[] { $"User Number {usrNo} has been successfully deleted." },
                        "Press any key to exit.",
                        ConsoleColor.White,
                        ConsoleColor.DarkRed);
                    usrDeleted.ShowInfo();
                    continueAdminMenu = true;
                }
                catch (Exception ex)
                {
                    dbError(ex);
                }

                break;
            case 2:
                // edit account
                // get user number of account to edit
                ConsoleMenu askEditNo = new ConsoleMenu("Edit an Account",
                new string[] { "Please enter in the User Number of the account holder.",
                "You can use search to find this if needed."},
                "User Number: ",
                ConsoleColor.White,
                ConsoleColor.Magenta);
                int usrNoEdit = Convert.ToInt32(askEditNo.ShowPrompt());

                // get the user's info so we can have it prefilled in below.
                string userInfo = "";
                try
                {
                    userInfo = db.getUserAccountInfo(usrNoEdit);
                }
                catch (Exception ex)
                {
                    dbError(ex);
                }
                string[] userInfoArray = userInfo.Split('|');


                // Show user each entry and have them either hit enter to leave as is
                // or change it.
                ConsoleMenu askNameE = new ConsoleMenu("Edit Account Information",
                new string[] { "Please change the full name or press enter to leave as is." },
                "Full Name: ",
                ConsoleColor.White,
                ConsoleColor.Magenta);
                string accNameE = askNameE.ShowPrompt(userInfoArray[1]);

                ConsoleMenu askUsernameE = new ConsoleMenu("Edit Account Information",
                new string[] { "Please change the username or press enter to leave as is." },
                "Username: ",
                ConsoleColor.White,
                ConsoleColor.Magenta);
                string accUsernameE = askUsernameE.ShowPrompt(userInfoArray[2]);

                ConsoleMenu askPasswordE = new ConsoleMenu("Edit Account Information",
                new string[] { "Please change the password or press enter to leave as is." },
                "Password: ",
                ConsoleColor.White,
                ConsoleColor.Magenta);
                string accPasswordE = askPasswordE.ShowPrompt(userInfoArray[3]);

                ConsoleMenu askSSNE = new ConsoleMenu("Edit Account Information",
                new string[] { "Please change the SSN or press enter to leave as is." },
                "SSN: ",
                ConsoleColor.White,
                ConsoleColor.Magenta);
                string accSSNE = askSSNE.ShowPrompt(userInfoArray[4]);

                ConsoleMenu askEmailE = new ConsoleMenu("Edit Account Information",
                new string[] { "Please change the email or press enter to leave as is." },
                "Email: ",
                ConsoleColor.White,
                ConsoleColor.Magenta);
                string accEmailE = askEmailE.ShowPrompt(userInfoArray[5]);

                ConsoleMenu askPhoneE = new ConsoleMenu("Edit Account Information",
                new string[] { "Please change the phone number or press enter to leave as is." },
                "Phone Number: ",
                ConsoleColor.White,
                ConsoleColor.Magenta);
                string accPhoneE = askPhoneE.ShowPrompt(userInfoArray[6]);

                ConsoleMenu askAdminE = new ConsoleMenu("Edit Account Information",
                new string[] { "Please change the privilege or press enter to leave as is.",
                "Type in '1' for Administrator privileges or a '0' for Customer privileges."},
                "Admin: ",
                ConsoleColor.White,
                ConsoleColor.Magenta);
                string accAdminE = askAdminE.ShowPrompt(userInfoArray[7]);

                // try to update the database with user's changes
                try
                {
                    db.editAccount(usrNoEdit, accNameE, accUsernameE, accPasswordE,
                        accSSNE, accEmailE, accPhoneE, Convert.ToInt32(accAdminE));

                    // success message to user
                    ConsoleMenu usrEdited = new ConsoleMenu("Account Edited",
                        new string[] { $"User Number {usrNoEdit} has been successfully edited." },
                        "Press any key to exit.",
                        ConsoleColor.DarkMagenta,
                        ConsoleColor.White);
                    usrEdited.ShowInfo();
                    continueAdminMenu = true;

                }
                catch (Exception ex)
                {
                    dbError(ex);
                }

                break;
            case 3:
                // transfer funds
                // ask for source accNumber
                ConsoleMenu askTransSource = new ConsoleMenu("Transfer Funds",
                new string[] { "Please enter the Account Number of the source account for the transfer." },
                "Source Account Number: ",
                ConsoleColor.White,
                ConsoleColor.DarkBlue);
                int source = Convert.ToInt32(askTransSource.ShowPrompt());

                // ask for destination accNumber
                ConsoleMenu askTransDest = new ConsoleMenu("Transfer Funds",
                new string[] { "Please enter the Account Number of the destination account for the transfer." },
                "Destination Account Number: ",
                ConsoleColor.White,
                ConsoleColor.DarkBlue);
                int destination = Convert.ToInt32(askTransDest.ShowPrompt());

                // ask for the amount of the transfer
                ConsoleMenu askAmount = new ConsoleMenu("Enter Transfer Amount",
                    new string[] { "Please enter in the amount to transfer." },
                    "Amount: $",
                    ConsoleColor.White,
                    ConsoleColor.DarkBlue);
                double amount = Convert.ToDouble(askAmount.ShowPrompt());

                try
                { 
                    db.transferFunds(source, destination, amount);
                    ConsoleMenu goodTransfer = new ConsoleMenu("Transfer Completed",
                        new string[] { $"${Convert.ToString(amount)} has successfully been transfered to {destination}." },
                        "Press any key to exit.",
                        ConsoleColor.DarkBlue,
                        ConsoleColor.White);
                    goodTransfer.ShowInfo();
                    continueAdminMenu = true;
                }
                catch (Exception ex)
                {
                    dbError(ex);
                }
                break;
            case 4:
                // search accounts
                ConsoleMenu searchMenu = new ConsoleMenu("Search Accounts",
                    new string[] { "User Number",
                                   "Name",
                                   "Username",
                                   "SSN",
                                   "Email",
                                   "Phone Number",
                                   "Priveleges Level"},
                    "Please choose what you would like to search by and then press enter.",
                    ConsoleColor.Gray,
                    ConsoleColor.DarkMagenta);
                int searchChoice = searchMenu.ShowMenu();

                ConsoleMenu searchStringMenu = new ConsoleMenu("Search Value",
                    new string[] { "Please enter your search value." },
                    "?: ",
                    ConsoleColor.Gray,
                    ConsoleColor.DarkMagenta);
                string searchString = searchStringMenu.ShowPrompt();

                try
                {
                    string[] accountsFound = db.searchAccounts(searchString, searchChoice);
                    ConsoleMenu searchResults = new ConsoleMenu("Search Results",
                        accountsFound,
                        "Press Up/Down to look at other pages or any other key to exit.",
                        ConsoleColor.DarkMagenta,
                        ConsoleColor.Gray, 4); 
                    searchResults.ShowInfo(4);
                    continueAdminMenu = true;
                }
                catch (Exception ex)
                {
                    dbError(ex);
                }

                break;
            case 5:
                // list accounts
                try
                {
                    string[] accountsFound = db.searchAccounts();
                    ConsoleMenu searchResults = new ConsoleMenu("Search Results",
                        accountsFound,
                        "Press Up/Down to look at other pages or any other key to exit.",
                        ConsoleColor.Cyan,
                        ConsoleColor.Black, 4);
                    searchResults.ShowInfo(4);
                    continueAdminMenu = true;
                }
                catch (Exception ex)
                {
                    dbError(ex);
                }

                break;
            case 6:
                // summarize accounts
                // total accounts: XX total balances: XX
                // Checkings: XX accounts | Savings: XX accounts | Loan: XX accounts


                break;
            case 7:
                mainMenu();
                break;
        }
    } while (continueAdminMenu);
    
}

void customerMenu()
{
    ConsoleMenu custMenu = new ConsoleMenu("Customer Menu",
        new string[] {"View details",               //case 0
                      "Make a withdrawal",          //case 1
                      "Deposit Funds",              //case 2
                      "Transfer money",             //case 3
                      "Change Password",            //case 4
                      "View Last 5 Transactions",   //case 5
                      "Exit"},                      //case 6
        "Press Up/Down keys and then Enter to make a selection",
        ConsoleColor.White,
        ConsoleColor.DarkBlue);
    int choice = custMenu.ShowMenu();

    switch (choice)
    {
        case 0:

            break;
        case 1:

            break;
        case 2:

            break;
        case 3:

            break;
        case 4:

            break;
        case 5:
            
            break;
        case 6:
            mainMenu();
            break;
    }


}

void exitScreen()
{
    ConsoleMenu wrongCreds = new ConsoleMenu("Thank you for using Old School Banking!",
        new string[] { "We hope you have a wonderful day! Press any key to exit." },
        "",
        ConsoleColor.Black,
        ConsoleColor.Gray);
    wrongCreds.ShowInfo();
}

void dbError(Exception e)
{
    ConsoleMenu dbError = new ConsoleMenu("Database Error",
            new string[] { "We're sorry, it seems we've encountered some issue with the database.", 
                "This issue has already been logged and we'll do our best to resolve this as quicly as possible!", 
                "We appreciate your patience and continued patronage." },
            "Error Logged - Press any key to exit.",
            ConsoleColor.Black,
            ConsoleColor.DarkYellow);
    // get date time and output it with e.Message to the error.log
    // summarize try catch statements and this new dbError function
    File.AppendAllText("error.log", String.Concat(DateTime.Now.Date, " | ", DateTime.Now.TimeOfDay, " --- ", e.Message, "\n"));
    Debug.WriteLine(e.ToString());
    dbError.ShowInfo();
    mainMenu();
}

#endregion


if (Console.WindowWidth < 50 && Console.WindowHeight < 50)
{
    Console.WriteLine("Error: Console is too small, please expand the console window and try again.");
    Environment.Exit(0);
}

mainMenu();