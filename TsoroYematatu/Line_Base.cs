using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsoroYematatu {
    public class Line_Base {

        private static int line_base_array_size = 27;

        Line[] line_base_array;

        public void Setup_Line_Base() {

            line_base_array = new Line[line_base_array_size];

            // >3 takie same<

            Add_To_Line_Base_Array(0, Pawn.Empty, Pawn.Empty, Pawn.Empty, 0);
            Add_To_Line_Base_Array(1, Pawn.White, Pawn.White, Pawn.White, -100);
            Add_To_Line_Base_Array(2, Pawn.Black, Pawn.Black, Pawn.Black, 100);

            // >1 czarny 2 puste<

            Add_To_Line_Base_Array(3, Pawn.Black, Pawn.Empty, Pawn.Empty, 20);
            Add_To_Line_Base_Array(4, Pawn.Empty, Pawn.Black, Pawn.Empty, 20);
            Add_To_Line_Base_Array(5, Pawn.Empty, Pawn.Empty, Pawn.Black, 20);

            // >1 bialy 2 puste<

            Add_To_Line_Base_Array(6, Pawn.White, Pawn.Empty, Pawn.Empty, -20);
            Add_To_Line_Base_Array(7, Pawn.Empty, Pawn.White, Pawn.Empty, -20);
            Add_To_Line_Base_Array(8, Pawn.Empty, Pawn.Empty, Pawn.White, -20);

            // >2 czarne 1 pusty<

            Add_To_Line_Base_Array(9, Pawn.Black, Pawn.Black, Pawn.Empty, 12);
            Add_To_Line_Base_Array(10, Pawn.Empty, Pawn.Black, Pawn.Black, 12);
            Add_To_Line_Base_Array(11, Pawn.Black, Pawn.Empty, Pawn.Black, 12);

            // >2 biale 1 pusty<

            Add_To_Line_Base_Array(12, Pawn.White, Pawn.White, Pawn.Empty, -12);
            Add_To_Line_Base_Array(13, Pawn.Empty, Pawn.White, Pawn.White, -12);
            Add_To_Line_Base_Array(14, Pawn.White, Pawn.Empty, Pawn.White, -12);

            // >czarny bialy pusty<

            Add_To_Line_Base_Array(15, Pawn.Black, Pawn.White, Pawn.Empty, 0);
            Add_To_Line_Base_Array(16, Pawn.White, Pawn.Black, Pawn.Empty, 0);
            Add_To_Line_Base_Array(17, Pawn.Empty, Pawn.Black, Pawn.White, 0);
            Add_To_Line_Base_Array(18, Pawn.Empty, Pawn.White, Pawn.Black, 0);
            Add_To_Line_Base_Array(19, Pawn.Black, Pawn.Empty, Pawn.White, 0);
            Add_To_Line_Base_Array(20, Pawn.White, Pawn.Empty, Pawn.Black, 0);

            // >2 czarne 1 bialy<

            Add_To_Line_Base_Array(21, Pawn.Black, Pawn.Black, Pawn.White, 7);
            Add_To_Line_Base_Array(22, Pawn.White, Pawn.Black, Pawn.Black, 7);
            Add_To_Line_Base_Array(23, Pawn.Black, Pawn.White, Pawn.Black, 6);

            // >2 biale 1 czarny<

            Add_To_Line_Base_Array(24, Pawn.White, Pawn.White, Pawn.Black, -7);
            Add_To_Line_Base_Array(25, Pawn.Black, Pawn.White, Pawn.White, -7);
            Add_To_Line_Base_Array(26, Pawn.White, Pawn.Black, Pawn.White, -6);
        }

        private void Add_To_Line_Base_Array(int position, Pawn first_pawn, Pawn second_pawn, Pawn third_pawn, int base_weight) {

            line_base_array[position] = new Line(first_pawn, second_pawn, third_pawn);
            line_base_array[position].Base_line_weight = base_weight;
        }

        public int Get_Line_Weight(Line line) {

            for (int i = 0; i < line_base_array.Length; i++) {

                if (line.Line_state[0] == line_base_array[i].Line_state[0] && line.Line_state[1] == line_base_array[i].Line_state[1] && line.Line_state[2] == line_base_array[i].Line_state[2]) {

                    return line_base_array[i].Base_line_weight;
                }
            }

            return -1000;
        }
    }
}
   
