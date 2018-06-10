using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.WebPages;

namespace TsoroYematatu.Models
{
    public class GameBoard
    {
        public int[] fields = new int [7];
        public int[] stones = new int[2];
        public int turn;
        public int winner;
        public int playerPawn;
        public int wrongPawnClicked;

        public GameBoard()
        {
            winner = 0;
            turn = 1;
            for (var i = 0; i < 7; i++)
                fields[i] = 0;
            for (var i = 0; i < 2; i++)
                stones[i] = 0;
        }

        public void ChangeTurn()
        {
            turn = turn == 2 ? 1 : 2;
        }
    }
}