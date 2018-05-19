using System;
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
        

        private int[] empty_field;

        private Pawn[,] board_state = new Pawn[board_size, board_size];

        public Board() {

            Setup_empty_field();
        }

        private void Setup_empty_field() {

            empty_field = new int[board_size * board_size];

            for (int i = 0; i < empty_field.Length; i++) {

                empty_field[i] = i;
            }
        }
    }
}
