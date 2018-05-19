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

            line_state = new Pawn[line_state_size];

            line_state[0] = first_pawn;
            line_state[1] = second_pawn;
            line_state[2] = third_pawn;
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
    }

    public class Board {

        private static int[,] board_field_weight = new int[3, 3] {{5,5,5},{3,8,3},{0,1,0}};
        private static int board_size = 3;
        
        public ArrayList empty_field;

        private Pawn[,] board_state = new Pawn[board_size, board_size];
        private Pawn[,] experimental_board_state;


        public Board() {

            Setup_Board();
            Setup_Empty_Field();
        }

        private void Setup_Board() {

            for (int i = 0; i < board_size; i++) {

                for (int j = 0; j < board_size; j++) {

                    board_state[i, j] = Pawn.Empty;
                }
            }

            experimental_board_state = board_state;
        }

        private void Setup_Empty_Field() {

            for (int i = 0; i < board_size*board_size; i++) {

                empty_field.Add(i);
            }
        }

        public Move Move_To(Move move, Pawn pawn_colour, bool experimental) {

            

            return move;
        }
    }
}
