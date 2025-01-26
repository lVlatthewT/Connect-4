using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//////////////////////////////////////////////////////////////////////////
///Program: connect 4.                                                  //
///Author: Matthew Tattersall                                           //
///Date: 11/11/2024                                                     //
///Description: Play games of connect 4 against a friend or computer.   //
//////////////////////////////////////////////////////////////////////////
namespace Connect4
{
    internal class Connect4
    {
        const int ROWS = 6;
        const int COLUMNS = 7;
        static string[,] grid = new string[ROWS, COLUMNS];
        static string EMPTY = "+";
        static string CurrentPlayer = "X", OpponentPlayer = "O";
        static string Name;
        static int TotalPlayers;

        //player 1 

        // player 1 counter colour dark red
        static ConsoleColor BackgroundPlayer1Colour1 = ConsoleColor.DarkRed;
        static ConsoleColor ForegroundPlayer1Colour1 = ConsoleColor.DarkRed;



        // player 2 and computer counter colour dark yellow.
        static ConsoleColor BackgroundPlayer2Colour = ConsoleColor.DarkYellow;
        static ConsoleColor ForegroundPlayer2Colour = ConsoleColor.DarkYellow;

        static void Main(string[] args)
        {
            EnterName();
            Menu();
            while (true)
            {
                NewGame();
                


                while (true)
                {

                    if (TotalPlayers == 1)
                    {
                        //player 1
                        PlaceCounter();
                        GenerateGrid();
                        if (WinnerCheck())
                        {
                            Console.ReadKey();
                            break;
                        }

                        // computer
                        ComputerPlace();
                        GenerateGrid();
                        if (WinnerCheck())
                        {
                            Console.WriteLine("Computer wins!");
                            Console.ReadKey();
                            break;
                        }

                    }

                    else if (TotalPlayers == 2)
                    {
                        //player 1
                        PlaceCounter();
                        GenerateGrid();
                        
                        CurrentPlayer = "X";
                        if (WinnerCheck())
                        {

                            Console.ReadKey();
                            break;
                        }

                        //player 2
                        PlaceCounter();
                        GenerateGrid();
                        CurrentPlayer = "O";
                        if (WinnerCheck())
                        {

                            Console.ReadKey();
                            break;
                        }
                    }
                    else // perform player counter input validation. ensuring players cannot enter out of bound numbers.
                    {
                        Console.WriteLine("Invalid option. please try again.");
                        Console.ReadKey();
                        break;
                    }


                }



            }


        }




        static void EnterName() // This is a method, which allows a user to enter their names to help personalise the game towards them. in addition, this method is used to provide input validation.
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\tConnect 4");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\n Please enter your name: ");
            Name = Console.ReadLine();


            // checks if inputted name does not equal 1 or exceeds 35 characters long and will display an error message if either conditions are true and will recalled the entername method.
            if (Name.Length == 1)
            {
                Console.WriteLine("\nYour name must be more than 1 character long, Please try again.\n");
                EnterName();
            }
            else if (Name.Length >35)
            {
                Console.WriteLine("\nYour name must be less than 35 characters long, Please try again.\n");
                EnterName();
            }
            else if (Name.Length == 0) //This checks whether the user has left the name input field blank, if condition is true than an error message is displayed and the entername method is recalled.
            {
                Console.WriteLine("\nYou must enter your name to continue, Please try again. \n");
                EnterName();

            }


        }

        // A method of the game's main menu, which will allow end-users to choose their specific option and is called upon in the program's main method.
        static void Menu()
        {

            string MainMenuChoice;


            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine($"\n\tWelcome {Name} to connect 4!\n ");
            Console.WriteLine("-------------------------------------------");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Please select an option to progress.");
            Console.WriteLine("Press A to start the game.");
            Console.WriteLine("Press B for input controls and instructions.");
            Console.WriteLine("Press C to exit the program.");

            Console.Write("\nWhich option would you like to choose: ");
            MainMenuChoice = Console.ReadLine().ToUpper();
            // This switch case statement will allow end-users to select their intended menu option such as: start game or instructions.

            switch (MainMenuChoice)
            {
                case "A":
                    NewGame();
                    break;

                case "B":
                    Instructions();
                    break;

                case "C":
                    Exit();
                    break;

                default:
                    Console.WriteLine("Invalid option, please choose an option from the list above.");
                    Thread.Sleep(1500);
                    Menu();
                    break;

            }
        }

        

        static void NewGame()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Please choose 1 or 2 players: ");
            TotalPlayers = int.Parse(Console.ReadLine());
            ClearGrid();
            CurrentPlayer = "X";

        }



        static void ClearGrid()
        {
            for (int j = 0; j < COLUMNS; j++)
                for (int i = 0; i < ROWS; i++)
                    grid[i, j] = EMPTY;

        }
        static void GenerateGrid() // generates and displays the game grid.
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write("");


            for (int j = 0; j < COLUMNS; j++)
                Console.Write($" {j} ");
            Console.WriteLine();


            for (int i = 0; i < ROWS; i++)
            {


                for (int j = 0; j < COLUMNS; j++)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;


                    if (grid[i, j] == "X") // alters the foreground and background colour of player 1
                    {
                        Console.BackgroundColor = BackgroundPlayer1Colour1;
                        Console.ForegroundColor = ForegroundPlayer1Colour1;
                    }

                    else if (grid[i, j] == "O") // alters the foreground and background colour of player 2.
                    {
                        Console.BackgroundColor = BackgroundPlayer2Colour;
                        Console.ForegroundColor = ForegroundPlayer2Colour;
                    }
                    Console.Write($" {grid[i, j]} ");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            Console.ResetColor();


        }



        static void PlaceCounter()
        {

            GenerateGrid();
            CounterDrop();
        }
        static void CounterDrop()
        {
            string CounterPlace;
            Console.Write("Please choose a column to drop a counter: ");
            CounterPlace = Console.ReadLine();
            if (!int.TryParse(CounterPlace, out int column) || column < 0 || column >= COLUMNS)
            {
                Console.WriteLine("Invalid move. Please try again.");
                Console.ReadKey();
                return; // Go back to the start of the loop
            }
            // Ensures counters are dropped down to the last available space within a column.
            int DropCounter = -1;
            for (int row = ROWS - 1; row >= 0; row--)
            {
                if (grid[row, column] == EMPTY)
                {
                    DropCounter = row;
                    break;
                }

                else
                {
                    if (DropCounter != -1)
                    {
                        break;
                    }
                }
            }
            // A check to determine whether the current column is full.
            if (DropCounter == -1)
            {
                Console.WriteLine("This column is full. please choose another."); // Column full message is displayed when the program determines, if the current column is full.
                Console.ReadKey();
                return;
            }
            grid[DropCounter, column] = CurrentPlayer;

        }


        static void ComputerPlace()
        {
            // The computer can choose really quickly, to let the player have a chance to view the
            // board before the computer has a go we add a delay
            Console.WriteLine("Computer's turn, thinking...");
            Thread.Sleep(1500);
            // choose a random spot (not very intelligent!)
            Random random = new Random();

            while (true)
            {
                int i = random.Next(ROWS);
                int j = random.Next(COLUMNS);
                if (grid[i, j] == EMPTY)
                {
                    grid[i, j] = OpponentPlayer;  // set to this player piece
                    break;                      // exit while loop
                }

            }
        }



        static void Back() // This is a method for a back button, which will take the user back to the main menu. I have incorporated this into a method, as repeating the same code can become redudant throughout the program.
        {
            string BackButton;
            Console.Write("\nPress B to return to main menu: ");
            BackButton = Console.ReadLine().ToUpper();
            switch (BackButton)
            {
                case "B":
                    Menu();
                    break;

                default:
                    Console.WriteLine("Invalid option,");
                    break;
            }
            Console.WriteLine("  ");
        }






        static void Exit() //This is the exit method, which will terminate the program after 2 seconds.
        {
            Console.WriteLine($"Thank you for playing. Goodbye {Name}...");
            Thread.Sleep(2000);
            Environment.Exit(0);
        }




        static void Instructions() // Displays the instructions sub-menu on-screen.
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nInstructions: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nwin: The first player to receive 4 counters in a row either horizontally, diagonally or vertically.");
            Console.WriteLine("\nDraw: If all players have used their 21 moves each with neither player having 4 counters in a row.");
            Back();
        }

        static bool WinnerCheck() // Checks against the different winning methods
        {
            for (int j = 0; j < COLUMNS; j++) //Loops through columns and rows to check if a win condition is met.
                for (int i = 0; i < ROWS; i++)
                {
                    if (VerticalCheck(i, j))
                        return true;
                    else if (HorizontalCheck(i, j))
                        return true;
                    else if (DiagonalLeftCheck(i, j))
                        return true;
                    else if (DiagonalRightCheck(i, j))
                        return true;
                }

            return false; //Continues to loop, if no win conditions are met.

        }


        // This is the win conditions section, which checks for horizontal, vertical and diagonal win conditions.
        static bool VerticalCheck(int i, int j)
        {
            if (grid[i, j] == EMPTY) return false;
            int CounterCount = 1;
            string currentCounter = grid[i, j];
            j++;

            while ((j < COLUMNS) && (CounterCount < 4) && (grid[i, j] == currentCounter))
            {
                CounterCount++;
                j++;
            }
            if (CounterCount == 4)
            {
                GenerateGrid();
                Console.WriteLine($"Player {currentCounter} is the winner.");
                Thread.Sleep(1500);
                ClearGrid();
                Console.Clear();
                Menu();
                return true;
            }
            return false;
        }

        static bool HorizontalCheck(int i, int j)
        {
            if (grid[i, j] == EMPTY) return false;
            int CounterCount = 1;
            string currentCounter = grid[i, j];
            i++;

            while ((i < ROWS) && (CounterCount < 4) && (grid[i, j] == currentCounter))
            {
                CounterCount++;
                i++;
            }

            if (CounterCount == 4)
            {
                GenerateGrid();
                Console.WriteLine($"Player {currentCounter} is the winner.");
                Thread.Sleep(1500);
                ClearGrid();
                Console.Clear();
                Menu();
                return true;
            }
            return false;
        }

        static bool DiagonalRightCheck(int i, int j)
        {
            if (grid[i, j] == EMPTY) return false;
            int CounterCount = 1;
            string currentCounter = grid[i, j];
            i++;
            j++;

            while ((i < ROWS) && (j < COLUMNS) && (CounterCount < 4) && (grid[i, j] == currentCounter))
            {
                CounterCount++;
                i++;
                j++;
            }
            if (CounterCount == 4)
            {
                GenerateGrid();
                Console.WriteLine($"Player {currentCounter} is the winner.");
                Thread.Sleep(1500);
                ClearGrid();
                Console.Clear();
                Menu();
                return true;
            }
            return false;
        }

        static bool DiagonalLeftCheck(int i, int j)
        {
            if (grid[i, j] == EMPTY) return false;
            int CounterCount = 1;
            string currentCounter = grid[i, j];
            i--;
            j++;

            while ((i >= 0) && (j < COLUMNS) && (CounterCount < 4) && (grid[i, j] == currentCounter))
            {
                CounterCount++;
                i--;
                j++;
            }

            if ((CounterCount == 4))
            {
                GenerateGrid();
                Console.WriteLine($"Player {currentCounter} is the winner.");
                Thread.Sleep(1500);
                ClearGrid();
                Console.Clear();
                Menu();
                return true;

            }
            return false;
        }

       
    }
}

