using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsoroYematatu {

    public enum Pawn { Black, White, Empty };

    public class Line {

        private static int line_state_size = 3;

        private int base_line_weight;

        private Pawn[] line_state;

        public Line(Pawn first_pawn, Pawn second_pawn, Pawn third_pawn) {

            Line_state = new Pawn[line_state_size];

            Line_state[0] = first_pawn;
            Line_state[1] = second_pawn;
            Line_state[2] = third_pawn;
        }

        public int Base_line_weight
        {
            get
            {
                return base_line_weight;
            }

            set
            {
                base_line_weight = value;
            }
        }

        public Pawn[] Line_state
        {
            get
            {
                return line_state;
            }

            set
            {
                line_state = value;
            }
        }
    }

    public class Board {

        private static int[,] board_field_weight = new int[3, 3] {{5,5,5},{3,8,3},{0,1,0}};
        private static int board_size = 3;

        private Line_Base line_base; 
        
        public ArrayList empty_field;

        private Pawn[,] board_state = new Pawn[board_size, board_size];
        private Pawn[,] experimental_board_state = new Pawn[board_size, board_size];

        public Board() {

            Setup_Board();
            Setup_Empty_Field();
            line_base = new Line_Base();
            line_base.Setup_Line_Base();
        }

        private void Setup_Board() {

            for (int i = 0; i < board_size; i++) {

                for (int j = 0; j < board_size; j++) {

                    board_state[i, j] = Pawn.Empty;
                    experimental_board_state[i, j] = Pawn.Empty;

                }
            }       
        }

        private void Setup_Empty_Field() {

            empty_field = new ArrayList();

            for (int i = 0; i < board_size*board_size; i++) {

                empty_field.Add(i);
            }
        }

        public void Print_Board() {

            for (int i = 0; i < board_size; i++) {

                for (int j = 0; j < board_size; j++) {

                    Console.Write(experimental_board_state[i, j] + "");                   
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public Move Move_To(Move move, Pawn pawn_type, bool experimental) {

            int field_weight = Get_Board_Field_Weight(move.Move_to);

            Set_Board_State(move.Move_to, pawn_type, experimental);

            move.Result_weight = Check_Lines(experimental) + field_weight;

            for (int i = 0; i < board_size; i++) {

                for (int j = 0; j < board_size; j++) {

                    experimental_board_state[i, j] = board_state[i, j];
                }
            }

            return move;
        }

        private int Check_Lines(bool experimental) {

            return Check_Left_Line(experimental) + Check_Interior_Line(experimental) + Check_Right_Line(experimental) + Check_Middle_Line(experimental)+ Check_Lower_Line(experimental);
        }

        private int Check_Left_Line(bool experimental) {

            Line line;

            if (experimental) {

                line = new Line(experimental_board_state[0, 0], experimental_board_state[1, 0], experimental_board_state[2, 0]);
            }
            else {

                line = new Line(board_state[0, 0], board_state[1, 0], board_state[2, 0]);
            }

            return line_base.Get_Line_Weight(line);
        }

        private int Check_Interior_Line(bool experimental) {

            Line line;

            if (experimental) {

                line = new Line(experimental_board_state[0, 1], experimental_board_state[1, 1], experimental_board_state[2, 1]);
            }
            else {

                line = new Line(board_state[0, 1], board_state[1, 1], board_state[2, 1]);
            }

            return line_base.Get_Line_Weight(line);
        }

        private int Check_Right_Line(bool experimental) {

            Line line;

            if (experimental) {

                line = new Line(experimental_board_state[0, 2], experimental_board_state[1, 2], experimental_board_state[2, 2]);
            }
            else {

                line = new Line(board_state[0, 2], board_state[1, 2], board_state[2, 2]);
            }

            return line_base.Get_Line_Weight(line);
        }

        private int Check_Middle_Line(bool experimental) {

            Line line;

            if (experimental) {

                line = new Line(experimental_board_state[1, 0], experimental_board_state[1, 1], experimental_board_state[1, 2]);
            }
            else {

                line = new Line(board_state[1, 0], board_state[1, 1], board_state[1, 2]);
            }

            return line_base.Get_Line_Weight(line);
        }

        private int Check_Lower_Line(bool experimental) {

            Line line;

            if (experimental) {

                line = new Line(experimental_board_state[2, 0], experimental_board_state[2, 1], experimental_board_state[2, 2]);
            }
            else {

                line = new Line(board_state[2, 0], board_state[2, 1], board_state[2, 2]);
            }

            return line_base.Get_Line_Weight(line);
        }

        private void Set_Board_State(int place, Pawn pawn_type, bool experimental) {

            for (int i = 0; i < board_size; i++) {

                for (int j = 0; j < board_size; j++) {

                    if (place == 0) {

                        if (experimental) {

                            experimental_board_state[i, j] = pawn_type;    
                        }
                        else {
 
                            board_state[i, j] = pawn_type;       
                        }
                    }

                    place--;             
                }
            }
        }

        private int Get_Board_Field_Weight(int place) {

            for (int i = 0; i < board_size; i++) {

                for (int j = 0; j < board_size; j++) {

                    if (place == 0) {

                        return board_field_weight[i, j];
                    }

                    place--;
                }
            }

            return -1000;
        }
    }
}
