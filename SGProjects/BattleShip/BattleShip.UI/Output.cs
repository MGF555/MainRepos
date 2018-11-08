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
    public static class Output
    {

        public static void BoardOutput(Player currentPlayer)
        {
            char letter = 'A';
            Console.WriteLine("  1 2 3 4 5 6 7 8 9 10");
            for (int x = 1; x <= 10; x++)
            {
                Console.Write(letter++);
                for (int y = 1; y <= 10; y++)
                {
                    Coordinate cord = new Coordinate(x, y);
                    ShotHistory cordState = currentPlayer.GameBoard.CheckCoordinate(cord);
                    if (cordState == ShotHistory.Unknown)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(" +");
                        Console.ResetColor();
                    }
                    else if (cordState == ShotHistory.Hit)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" H");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(" M");
                        Console.ResetColor();
                    }

                }
                Console.Write("\n");
            }
            Console.Write("Enter a coordinate to attack: ");
        }


        internal static void AttackMessage(Player player, FireShotResponse verify)
        {


            switch (verify.ShotStatus)
            {
                case (ShotStatus.Duplicate):
                    Console.WriteLine("Invalid input.  Already attacked specified coordinate");
                    break;
                case (ShotStatus.Invalid):
                    Console.WriteLine("Invalid input.  Attack must remain on the board");
                    break;
                case (ShotStatus.Hit):
                    Console.WriteLine("Enemy ship hit!");
                    break;
                case (ShotStatus.HitAndSunk):
                    Console.WriteLine($"Enemy {verify.ShipImpacted} hit and sunk!");
                    break;
                case (ShotStatus.Miss):
                    Console.WriteLine("Attack missed!");
                    break;
                case (ShotStatus.Victory):
                    Console.WriteLine($"The final ship has been sunk!");
                    break;
            }
        }
    }
}
