using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsoroYematatu {
    class Program {
        static void Main(string[] args) {


            Game_Logic g = new Game_Logic();

            g.Start_Game(Game_Mode.PlayervsAI, Pawn.White);
        }
    }
}
