using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TsoroYematatu.Helpers
{
    public class Move
    {
        public int Move_to { get; set; }
        public int Move_from { get; set; }
        public int Result_weight { get; set; }

        public Move(int _move_to)
        {
            Move_to = _move_to;
        }

        public Move(int _move_to, int _move_from)
        {
            Move_to = _move_to;
            Move_from = _move_from;
        }
    }
}