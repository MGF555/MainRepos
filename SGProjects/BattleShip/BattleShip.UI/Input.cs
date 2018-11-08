using BattleShip.BLL.Requests;
using BattleShip.BLL.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip.UI
{
    static class Input
    {
        public static ShipDirection GetShipDirection()
        {
            ShipDirection shipDir = ShipDirection.Down;


            bool hold = true;
            while (hold)
            {
                Console.Write("Select a direction - up down left right: ");
                string direction = Console.ReadLine().ToLower();



                if (direction == "up")
                {
                    shipDir = ShipDirection.Up;
                    hold = false;
                }
                else if (direction == "down")
                {
                    shipDir = ShipDirection.Down;
                    hold = false;
                }
                else if (direction == "left")
                {
                    shipDir = ShipDirection.Left;
                    hold = false;
                }
                else if (direction == "right")
                {
                    shipDir = ShipDirection.Right;
                    hold = false;
                }
                else
                {
                    Console.WriteLine("Invalid input");
                }
            }
            return shipDir;
        }

        internal static Coordinate GetCoordinate()
        {

            while (true)
            {
                string input = Console.ReadLine().ToLower();
                int first = 0;
                int second = 0;
                int x;

                if (input.Length != 0)
                {
                    first = input[0] - 'a' + 1;
                }

                if (input.Length == 3)
                {
                    if (int.TryParse(input.Substring(1, 2), out x))
                    {
                        second = int.Parse(input.Substring(1, 2));
                    }
                }
                else if(input.Length == 2)
                {
                    if(int.TryParse(input.Substring(1, 1), out x))
                    {
                        second = int.Parse(input.Substring(1, 1));
                    }
                }
                Coordinate cord = new Coordinate(first, second);

                if (first < 1 || first > 10 || second < 1 || second > 10 || first == 0)
                {
                    Console.WriteLine("Invalid input.  Enter coordinates in a LetterNumber format.  A1 - J10");
                }
                else
                {
                    return cord;
                }
            }
        }

    }
}
                    
                

            
    

