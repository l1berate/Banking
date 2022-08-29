// See https://aka.ms/new-console-template for more information

using Banking;
using System;
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

void adminMenu()
{
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

            break;
        case 7:
            mainMenu();
            break;
    }
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

#endregion


if (Console.WindowWidth < 50 && Console.WindowHeight < 50)
{
    Console.WriteLine("Error: Console is too small, please expand the console window and try again.");
    Environment.Exit(0);
}

mainMenu();