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

            Add_To_Line_Base_Array(line_base_array.Length, Pawn.Empty, Pawn.Empty, Pawn.Empty, 1);
            Add_To_Line_Base_Array(line_base_array.Length, Pawn.White, Pawn.White, Pawn.White, 1);
            Add_To_Line_Base_Array(line_base_array.Length, Pawn.Black, Pawn.Black, Pawn.Black, 1);

            // >1 czarny 2 puste<

            Add_To_Line_Base_Array(line_base_array.Length, Pawn.Black, Pawn.Empty, Pawn.Empty, 1);
            Add_To_Line_Base_Array(line_base_array.Length, Pawn.Empty, Pawn.Black, Pawn.Empty, 1);
            Add_To_Line_Base_Array(line_base_array.Length, Pawn.Empty, Pawn.Empty, Pawn.Black, 1);

            // >1 bialy 2 puste<

            Add_To_Line_Base_Array(line_base_array.Length, Pawn.White, Pawn.Empty, Pawn.Empty, 1);
            Add_To_Line_Base_Array(line_base_array.Length, Pawn.Empty, Pawn.White, Pawn.Empty, 1);
            Add_To_Line_Base_Array(line_base_array.Length, Pawn.Empty, Pawn.Empty, Pawn.White, 1);

            // >2 czarne 1 pusty<

            Add_To_Line_Base_Array(line_base_array.Length, Pawn.Black, Pawn.Black, Pawn.Empty, 1);
            Add_To_Line_Base_Array(line_base_array.Length, Pawn.Empty, Pawn.Black, Pawn.Black, 1);
            Add_To_Line_Base_Array(line_base_array.Length, Pawn.Black, Pawn.Empty, Pawn.Black, 1);

            // >2 biale 1 pusty<

            Add_To_Line_Base_Array(line_base_array.Length, Pawn.White, Pawn.White, Pawn.Empty, 1);
            Add_To_Line_Base_Array(line_base_array.Length, Pawn.Empty, Pawn.White, Pawn.White, 1);
            Add_To_Line_Base_Array(line_base_array.Length, Pawn.White, Pawn.Empty, Pawn.White, 1);

            // >czarny bialy pusty<

            Add_To_Line_Base_Array(line_base_array.Length, Pawn.Black, Pawn.White, Pawn.Empty, 1);
            Add_To_Line_Base_Array(line_base_array.Length, Pawn.White, Pawn.Black, Pawn.Empty, 1);
            Add_To_Line_Base_Array(line_base_array.Length, Pawn.Empty, Pawn.Black, Pawn.White, 1);
            Add_To_Line_Base_Array(line_base_array.Length, Pawn.Empty, Pawn.White, Pawn.Black, 1);
            Add_To_Line_Base_Array(line_base_array.Length, Pawn.Black, Pawn.Empty, Pawn.White, 1);
            Add_To_Line_Base_Array(line_base_array.Length, Pawn.White, Pawn.Empty, Pawn.Black, 1);

            // >2 czarne 1 bialy<

            Add_To_Line_Base_Array(line_base_array.Length, Pawn.Black, Pawn.Black, Pawn.White, 1);
            Add_To_Line_Base_Array(line_base_array.Length, Pawn.White, Pawn.Black, Pawn.Black, 1);
            Add_To_Line_Base_Array(line_base_array.Length, Pawn.Black, Pawn.White, Pawn.Black, 1);

            // >2 biale 1 czarny<

            Add_To_Line_Base_Array(line_base_array.Length, Pawn.White, Pawn.White, Pawn.Black, 1);
            Add_To_Line_Base_Array(line_base_array.Length, Pawn.Black, Pawn.White, Pawn.White, 1);
            Add_To_Line_Base_Array(line_base_array.Length, Pawn.White, Pawn.Black, Pawn.White, 1);
        }

        private void Add_To_Line_Base_Array(int position, Pawn first_pawn, Pawn second_pawn, Pawn third_pawn, int base_weight) {

            line_base_array[position] = new Line(first_pawn, second_pawn, third_pawn);
            line_base_array[position].Base_line_weight = base_weight;
        }
    }
}
   
