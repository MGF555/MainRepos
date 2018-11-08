using BattleShip.BLL.GameLogic;
using BattleShip.BLL.Requests;
using BattleShip.BLL.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip.UI
{
    class Workflow
    {
        public void PlayGame()
        {
            string name = null;
            int count = 1;
            Console.Clear();
            Console.WriteLine("Welcome to Battleship");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.Clear();

            Player p1 = new Player();
            Player p2 = new Player();


            while (count <= 2)
            {
                Console.Write($"Player {count}, enter your name: ");
                name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Please do not leave this blank");
                }
                else if (count == 1)
                {
                    p1.Name = name;
                    count++;
                }
                else if (count == 2 && p1.Name == name)
                {
                    Console.WriteLine($"Please enter a name other than {name}, taken by player 1");
                }
                else
                {
                    p2.Name = name;
                    count++;
                }
            }
            Console.WriteLine($"Player 1: {p1.Name}");
            Console.WriteLine($"Player 2: {p2.Name}");
            Console.ReadLine();
            Console.Clear();
            bool quit = false;
            
            do{
                p1.GameBoard = new Board();

                p2.GameBoard = new Board();

                Console.Clear();

                Console.WriteLine($"{p1.Name}, set up your board");

                BoardSetup.PlaceShips(p1.GameBoard);

                Console.Clear();

                Console.WriteLine($"{p2.Name}, set up your board");

                BoardSetup.PlaceShips(p2.GameBoard);

                Console.Clear();



                p1.Victory = false;
                p2.Victory = false;

                Random firstMove = new Random();
                int playerTurn = firstMove.Next(0, 2);
                if (playerTurn == 1)
                {
                    Console.WriteLine($"{p1.Name} will be going first");
                }
                else
                {
                    Console.WriteLine($"{p2.Name} will be going first");
                }

                Console.ReadKey();
                Console.Clear();

                while (!p1.Victory && !p2.Victory)
                {
                    if (playerTurn == 0)
                    {
                        p1.Attack(p2);
                        playerTurn = 1;
                    }
                    else
                    {
                        p2.Attack(p1);
                        playerTurn = 0;
                    }
                }
                if(p1.Victory == true)
                {
                    Console.WriteLine($"{p1.Name} is the victor!");
                }
                else
                {
                    Console.WriteLine($"{p2.Name} is the victor!");
                }
                while (true)
                {
                    Console.Write("Play again? Yes or no: ");
                    string again = Console.ReadLine().ToLower();
                    if (again == "yes")
                    {
                        Console.WriteLine("Boards will be reset.  Press any key to continue");
                        Console.ReadKey();
                        break;
                    }
                    else if (again == "no")
                    {
                        Console.WriteLine("Exiting game.  Press any key to continue");
                        quit = true;
                        Console.ReadKey();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input");
                    }
                }
            } while (!quit);
        }



    }
}