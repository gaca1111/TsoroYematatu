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

        public int Move_to
        {
            get
            {
                return move_to;
            }

            set
            {
                move_to = value;
            }
        }

        public int Result_weight
        {
            get
            {
                return result_weight;
            }

            set
            {
                result_weight = value;
            }
        }

        public Move(int _move_to) {

            Move_to = _move_to;
        }

        public Move(int _move_to, int _move_from) {

            Move_to = _move_to;
            move_from = _move_from;
        }
    }
    public class Game_Logic {

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

            for (int i = 0; i < first_phase_counter; i++) {

                Find_Moves_First_Phase();

                Evaluate_Moves_First_Phase();

                board.Print_Board();

                Change_Turn();
            }

            Second_Phase();
        }

        private void Find_Moves_First_Phase() {

            possible_moves.Clear();

            for (int i = 0; i < board.empty_field.Count; i++) {

                possible_moves.Add(new Move((int)board.empty_field[i]));
            }
        }

        private void Evaluate_Moves_First_Phase() {

            for (int i = 0; i < possible_moves.Count; i++) {
   
                possible_moves[i] = board.Move_To((Move)possible_moves[i], whose_turn, true);
            }

            int highest_value = -1000;
            ArrayList best_option = new ArrayList();
            Move blank;

            for (int i = 0; i < possible_moves.Count; i++) {

                blank = (Move)possible_moves[i];

                if (blank.Result_weight >= highest_value) {

                    if (blank.Result_weight == highest_value) {

                        highest_value = blank.Result_weight;
                        best_option.Add(blank);
                    }
                    else {

                        highest_value = blank.Result_weight;
                        best_option.Clear();
                        best_option.Add(blank);
                    }     
                }
            }

            Random rnd = new Random();
            int index = rnd.Next(0, best_option.Count);    
            blank = (Move)best_option[index];

            if (blank.Move_to == 0 || blank.Move_to == 1 || blank.Move_to == 2) {

                blank.Move_to = 0;
                board.Move_To(blank, whose_turn, false);
                board.empty_field.Remove(blank.Move_to);

                blank.Move_to = 1;
                board.Move_To(blank, whose_turn, false);
                board.empty_field.Remove(blank.Move_to);

                blank.Move_to = 2;
                board.Move_To(blank, whose_turn, false);
                board.empty_field.Remove(blank.Move_to);

            }
            else {

                board.Move_To(blank, whose_turn, false);
                board.empty_field.Remove(blank.Move_to);
            }                
        }

        private void Second_Phase() {

            game_phase = Game_Phase.Second;

            Console.WriteLine("second");
            Console.ReadLine();
        }

        private void Change_Turn() {

            if (whose_turn == Pawn.Black) {

                whose_turn = Pawn.White;
                return;
            }

            if(whose_turn == Pawn.White) {

                whose_turn = Pawn.Black;
                return;
            }
        }
    }
}

