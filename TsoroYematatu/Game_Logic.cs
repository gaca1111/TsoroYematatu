using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsoroYematatu {

    public enum Game_Mode { AIvsAI, PlayervsAI, PlayervsPlayer };

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

        public int Move_from
        {
            get
            {
                return move_from;
            }

            set
            {
                move_from = value;
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
            Move_from = _move_from;
        }
    }

    public class Game_Logic {

        private int first_phase_counter = 6;
        private Pawn whose_turn;
        private bool wictory = true;
        private Pawn winner = Pawn.Empty;
        private Pawn player_pawn = Pawn.Empty;
        private Game_Mode game_mode;

        private Board board;
        private ArrayList possible_moves;

        public void Start_Game(Game_Mode _game_mode, Pawn _player_pawn) {

            game_mode = _game_mode;
            whose_turn = Pawn.White;

            if (game_mode == Game_Mode.PlayervsAI) {

                player_pawn = _player_pawn;
            }

            if (game_mode == Game_Mode.PlayervsPlayer) {

                player_pawn = whose_turn;
            }

            board = new Board();
            possible_moves = new ArrayList();

            First_Phase();
        }

        private void First_Phase() {
        
            for (int i = 0; i < first_phase_counter; i++) {

                Console.WriteLine("------ " + whose_turn);

                if (whose_turn == player_pawn) {

                    Console.WriteLine("Move to");

                    Move blank = new Move(Int32.Parse(Console.ReadLine()));

                    if (blank.Move_to == 0 || blank.Move_to == 1 || blank.Move_to == 2) {

                        board.Move_To(blank, whose_turn, false);
                        board.empty_field.Remove(0);
                        board.empty_field.Remove(1);
                        board.empty_field.Remove(2);
                    }
                    else {

                        board.Move_To(blank, whose_turn, false);
                        board.empty_field.Remove(blank.Move_to);
                    }
                }
                else {

                    Find_Moves_First_Phase();

                    Evaluate_Moves_First_Phase();
                }

                board.Print_Board();

                winner = board.Check_Lines_Victory();
            
                if (winner != Pawn.Empty) {

                    wictory = false;
                    break;
                }

                board.Write_Stale_Last_Board();

                Change_Turn();

                if (game_mode == Game_Mode.PlayervsPlayer) {

                    player_pawn = whose_turn;
                }

                Console.ReadLine();
            }

            if (wictory == false) {

                Console.WriteLine("Winner - " + winner);

                board.Write_Base_Last_Board(winner);

            }
            else {

                Second_Phase();
            }          
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
  
            Move blank;

            if (whose_turn == Pawn.Black) {

                blank = Find_Best_Move_Black();
            }
            else {

                blank = Find_Best_Move_White();
            }
                   
            if (blank.Move_to == 0 || blank.Move_to == 1 || blank.Move_to == 2) {

                board.Move_To(blank, whose_turn, false);
                board.empty_field.Remove(0);
                board.empty_field.Remove(1);
                board.empty_field.Remove(2);
            }
            else {

                board.Move_To(blank, whose_turn, false);
                board.empty_field.Remove(blank.Move_to);
            }                
        }

        private void Second_Phase() {
  
            while (wictory) {

                Console.WriteLine("------ " + whose_turn);

                if (whose_turn == player_pawn) {

                    Console.WriteLine("Move to from");

                    int to = Int32.Parse(Console.ReadLine());
                    int from = Int32.Parse(Console.ReadLine());

                    Move blank = new Move(to, from);

                    if (blank.Move_to == 0 || blank.Move_to == 1 || blank.Move_to == 2) {

                        board.Move_To_From(blank, whose_turn, false);
                        board.empty_field.Remove(0);
                        board.empty_field.Remove(1);
                        board.empty_field.Remove(2);

                    }
                    else {

                        board.Move_To_From(blank, whose_turn, false);
                        board.empty_field.Remove(blank.Move_to);
                    }

                    if (blank.Move_from == 0 || blank.Move_from == 1 || blank.Move_from == 2) {

                        board.empty_field.Add(0);
                        board.empty_field.Add(1);
                        board.empty_field.Add(2);
                    }
                    else {

                        board.empty_field.Add(blank.Move_from);
                    }
                }
                else {

                    Find_Moves_Second_Phase();

                    for (int i = 0; i < possible_moves.Count; i++) {

                        Move test = (Move)possible_moves[i];
                    }

                    Evaluate_Moves_Second_Phase();
                }
               
                board.Print_Board();
             
                winner = board.Check_Lines_Victory();
        
                if (winner != Pawn.Empty) {

                    wictory = false;
                }

                board.Write_Stale_Last_Board();

                Change_Turn();

                if (game_mode == Game_Mode.PlayervsPlayer) {

                    player_pawn = whose_turn;
                }

                Console.ReadLine();
            }

            Console.WriteLine("Winner - " + winner);

            board.Write_Base_Last_Board(winner);
        }

        private void Find_Moves_Second_Phase() {

            possible_moves.Clear();

            for (int t = 0; t < board.empty_field.Count; t++) {

                int place = (int)board.empty_field[t];
                int counter = place;

                int x = 0;
                int y = 0;

                for (int i = 0; i < board.Get_Board_Size(); i++) {

                    for (int j = 0; j < board.Get_Board_Size(); j++) {

                        if (counter == 0) {

                            x = j;
                            y = i;
                        }

                        counter--;
                    }
                }

                // r

                if (board.Check_Board_State(whose_turn, 1, 0, place + 1, x, y)) {

                    possible_moves.Add(new Move(place, place + 1));
                }

                if (board.Check_Board_State(whose_turn, 2, 0, place + 2, x, y)) {

                    possible_moves.Add(new Move(place, place + 2));
                }

                // l

                if (board.Check_Board_State(whose_turn, -1, 0, place - 1, x, y)) {

                    possible_moves.Add(new Move(place, place - 1));
                }

                if (board.Check_Board_State(whose_turn, -2, 0, place - 2, x, y)) {

                    possible_moves.Add(new Move(place, place - 2));
                }

                // d

                if (board.Check_Board_State(whose_turn, 0, 1, place + 3, x, y)) {

                    possible_moves.Add(new Move(place, place + 3));
                }

                if (board.Check_Board_State(whose_turn, 0, 2, place + 6, x, y)) {

                    possible_moves.Add(new Move(place, place + 6));
                }

                // u

                if (board.Check_Board_State(whose_turn, 0, -1, place - 3, x, y)) {

                    possible_moves.Add(new Move(place, place - 3));
                }

                if (board.Check_Board_State(whose_turn, 0, -2, place - 6, x, y)) {

                    possible_moves.Add(new Move(place, place - 6));
                }
            }
        }

        private void Evaluate_Moves_Second_Phase() {

            for (int i = 0; i < possible_moves.Count; i++) {

                possible_moves[i] = board.Move_To_From((Move)possible_moves[i], whose_turn, true);
            }

            Move blank;

            if (whose_turn == Pawn.Black) {

                blank = Find_Best_Move_Black();
            }
            else {

                blank = Find_Best_Move_White();
            }

            if (blank.Move_to == 0 || blank.Move_to == 1 || blank.Move_to == 2) {

                board.Move_To_From(blank, whose_turn, false);
                board.empty_field.Remove(0); 
                board.empty_field.Remove(1); 
                board.empty_field.Remove(2);

            }
            else {

                board.Move_To_From(blank, whose_turn, false);
                board.empty_field.Remove(blank.Move_to);
            }

            if (blank.Move_from == 0 || blank.Move_from == 1 || blank.Move_from == 2) {

                board.empty_field.Add(0);
                board.empty_field.Add(1);
                board.empty_field.Add(2);
            }
            else {

                board.empty_field.Add(blank.Move_from);
            }     
        }

        private Move Find_Best_Move_Black() {

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

            return blank;
        }

        private Move Find_Best_Move_White() {

            int highest_value = 1000;
            ArrayList best_option = new ArrayList();
            Move blank;
     
            for (int i = 0; i < possible_moves.Count; i++) {

                blank = (Move)possible_moves[i];           

                if (blank.Result_weight <= highest_value) {

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

            return blank;
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