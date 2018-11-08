using BattleShip.BLL.GameLogic;
using BattleShip.BLL.Requests;
using BattleShip.BLL.Responses;
using BattleShip.BLL.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip.UI
{
    public class BoardSetup
    {

        public static void PlaceShips(Board GameBoard)
        {
            int piecesize = 1;
            for (ShipType piece = ShipType.Destroyer; piece <= ShipType.Carrier; piece++)
            {
                if (piece == ShipType.Cruiser)
                {
                    piecesize = 3;
                }
                else
                {
                    piecesize++; 
                }
                Console.Write($"Select a spot for the {piece} ({piecesize} spaces): ");

                Coordinate shipCor = Input.GetCoordinate();
                ShipDirection shipdr = Input.GetShipDirection();
                PlaceShipRequest request = new PlaceShipRequest();
                request.Coordinate = shipCor;
                request.Direction = shipdr;
                request.ShipType = piece;
                ShipPlacement verify = GameBoard.PlaceShip(request);
                switch (verify)
                {
                    case ShipPlacement.NotEnoughSpace:
                        Console.WriteLine("Ship placement failed.  Not enough space");
                        piece--;
                        piecesize--;
                        break;
                    case ShipPlacement.Overlap:
                        Console.WriteLine("Ship placement failed.  Overlaps another ship");
                        piece--;
                        piecesize--;
                        break;
                }


            }
            Console.WriteLine("Board set complete.  Press any key to continue");
            Console.ReadKey();
        }
    }
}
