using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsoroYematatu {

    public enum Pawn { Black, White, Empty };

    public class Line {

        private int base_line_weight;

        private Pawn[] line_state;
    }


    public class Board {

        private int[,] board_field_weight = new int[3, 3] {{1,1,1},{1,1,1},{1,1,1}};


        private Pawn[,] board_state = new Pawn[3, 3];

    }
}
