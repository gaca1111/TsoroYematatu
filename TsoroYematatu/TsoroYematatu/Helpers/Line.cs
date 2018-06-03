using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TsoroYematatu.Enums;

namespace TsoroYematatu.Helpers
{
    public class Line
    {
        public int Base_line_weight { get; set; }
        public Pawn[] Line_state { get; set; }
        private static int line_state_size = 3;

        public Line(Pawn firstPawn, Pawn secondPawn, Pawn thirdPawn)
        {
            Line_state = new Pawn[line_state_size];
            Line_state[0] = firstPawn;
            Line_state[1] = secondPawn;
            Line_state[2] = thirdPawn;
        }
    }
}