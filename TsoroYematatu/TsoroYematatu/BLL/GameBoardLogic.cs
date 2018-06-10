using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TsoroYematatu.Enums;
using TsoroYematatu.Models;

namespace TsoroYematatu.BLL
{
    public class GameBoardLogic
    {
        private readonly Game_Logic gameLogic = new Game_Logic();

        public GameBoard FieldClick(int gameType, int fieldNumber, GameBoard gameBoard)
        {
            var pawn = gameBoard.turn == 1 ? Pawn.White : Pawn.Black;
            switch (gameType)
            {
                case 1:
                    gameLogic.Start_Game(Game_Mode.PlayervsPlayer, pawn, gameBoard, fieldNumber);
                    break;
                case 2:
                    gameLogic.Start_Game(Game_Mode.PlayervsAI, pawn, gameBoard, fieldNumber);
                    break;
                case 3:
                    gameLogic.Start_Game(Game_Mode.AIvsAI, pawn, gameBoard, fieldNumber);
                    break;
            }

            return gameBoard;
        }
    }
}