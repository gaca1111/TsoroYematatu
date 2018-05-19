using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsoroYematatu {

    public enum Game_Phase {First, Second};

    public class Move_First_Phase {

        private int move_to;
    }

    public class Move_Second_Phase{

        private int move_to;
        private int move_from;
    }


    class Game_Logic {

        Game_Phase game_phase;
        Pawn whose_turn;

        Board board;
        ArrayList possible_moves;

        public void Start_Game() {

            game_phase = Game_Phase.First;
            whose_turn = Pawn.White;

            board = new Board();
            possible_moves = new ArrayList();

            First_Phase();
        }

        private void First_Phase() {

        }

        private void Find_Moves_First_Phase() {


        }
    }
}
