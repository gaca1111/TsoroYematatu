using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TsoroYematatu.Models
{
    public class GameBoardJson
    {
        public int turn { get; set; }
        public int fields0 { get; set; }
        public int fields1 { get; set; }
        public int fields2 { get; set; }
        public int fields3 { get; set; }
        public int fields4 { get; set; }
        public int fields5 { get; set; }
        public int fields6 { get; set; }
        public int stones0 { get; set; }
        public int stones1 { get; set; }
        public int fieldNumber { get; set; }
        public int gameType { get; set; }
    }
}