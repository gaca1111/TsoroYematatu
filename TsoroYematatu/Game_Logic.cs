using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsoroYematatu {

    public enum Game_Phase { First, Second };

    public class Move {

        private int result_weight;

        private int move_to;
        private int move_from;

        public Move(int _move_to)  {

            move_to = _move_to;
        }

        public Move(int _move_to, int _move_from)  {

            move_to = _move_to;
            move_from = _move_from;
        }


        class Game_Logic {

            private Game_Phase game_phase;
            private int first_phase_counter = 6;
            private Pawn whose_turn;

            private Board board;
            private ArrayList possible_moves;

            public void Start_Game() {

                game_phase = Game_Phase.First;
                whose_turn = Pawn.White;

                board = new Board();
                possible_moves = new ArrayList();

                First_Phase();
            }

            private void First_Phase() {

                Find_Moves_First_Phase();





                Change_Turn();
       
                first_phase_counter--;

                if (first_phase_counter == 0) {

                    Second_Phase();
                }

            }

            private void Find_Moves_First_Phase() {

                for (int i = 0; i < board.empty_field.Count; i++) {

                    possible_moves.Add(new Move(i));
                }
            }

            private void Evaluate_Moves_First_Phase() {

                for (int i = 0; i < possible_moves.Count; i++) {

                    possible_moves[i] = board.Move_To((Move)possible_moves[i], whose_turn, true);
                }
            }



            private void Second_Phase() {


            }


            private void Change_Turn() {

                if (whose_turn == Pawn.Black) {

                    whose_turn = Pawn.White;
                }
                else {

                    whose_turn = Pawn.Black;
                }
            }
        }
    }
}
