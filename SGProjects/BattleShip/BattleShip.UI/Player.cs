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
    public class Player
    {
        public string Name { get; set; }

        public Board GameBoard { get; set; }

        public bool Victory { get; set; }

        public void Attack(Player attacker)
        {
            
            bool turn = true;
            while (turn)
            {
                Console.WriteLine($"{attacker.Name}, your turn");
                Output.BoardOutput(this);

                turn = true;
                Coordinate atckCord = Input.GetCoordinate();
                FireShotResponse verify = GameBoard.FireShot(atckCord);

                Output.AttackMessage(this, verify);

                if (verify.ShotStatus == ShotStatus.Victory)
                {
                    attacker.Victory = true;
                }

                if (verify.ShotStatus != ShotStatus.Duplicate && verify.ShotStatus != ShotStatus.Invalid)
                {
                    turn = false;
                }


                Console.ReadLine();
                Console.Clear();
            
            }
        }
    }
}
