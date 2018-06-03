using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using TsoroYematatu.Enums;
using TsoroYematatu.Helpers;
using TsoroYematatu.Models;

namespace TsoroYematatu.BLL
{

    public class Game_Logic
    {
        private int first_phase_counter = 6;
        private Pawn whose_turn;
        private bool wictory = true;
        private Pawn winner = Pawn.Empty;
        private Pawn player_pawn = Pawn.Empty;
        private Game_Mode game_mode;
        private Board board;
        private ArrayList possible_moves;

        public GameBoard Start_Game(Game_Mode _game_mode, Pawn _player_pawn, GameBoard gameBoard, int fieldNumber)
        {
            game_mode = _game_mode;
            whose_turn = gameBoard.turn == 1 ? Pawn.White : Pawn.Black;
            player_pawn = gameBoard.playerPawn == 0 ? _player_pawn :
                gameBoard.playerPawn == 1 ? Pawn.White : Pawn.Black;
            board = new Board(gameBoard);
            possible_moves = new ArrayList();
            switch (game_mode)
            {
                case Game_Mode.PlayervsAI:
                    if (gameBoard.stones[0] == 0 && gameBoard.stones[1] == 0) gameBoard.playerPawn = _player_pawn == Pawn.White ? 1 : 2;
                    if (gameBoard.winner == 0)
                    {
                        gameBoard = First_Phase(fieldNumber, gameBoard);
                        gameBoard = First_Phase(fieldNumber, gameBoard);
                        gameBoard.stones[gameBoard.turn - 1]++;
                        gameBoard.stones[gameBoard.turn]++;
                    }
                    else return gameBoard;
                    break;
                case Game_Mode.PlayervsPlayer:
                    gameBoard = PlayerPhase(fieldNumber, gameBoard);
                    break;
                case Game_Mode.AIvsAI:
                    while (gameBoard.winner == 0)
                    {
                        gameBoard = gameBoard.stones[gameBoard.turn - 1] < 3 ? First_Phase(fieldNumber, gameBoard) : Second_Phase(gameBoard);
                    }

                    break;
            }

            return gameBoard;
        }

        public GameBoard PlayerPhase(int fieldNumber, GameBoard gameBoard)
        {
            if (gameBoard.stones[gameBoard.turn - 1] < 3)
            {
                var blank = new Move(fieldNumber + 2);
                whose_turn = gameBoard.turn == 1 ? Pawn.White : Pawn.Black;

                if (blank.Move_to == 2)
                {
                    board.Move_To(blank, whose_turn, false);
                    board.empty_field.Remove(0);
                    board.empty_field.Remove(1);
                    board.empty_field.Remove(2);
                }
                else
                {
                    board.Move_To(blank, whose_turn, false);
                    board.empty_field.Remove(blank.Move_to);
                }

                winner = board.Check_Lines_Victory(gameBoard);
                if (winner != Pawn.Empty)
                {
                    gameBoard.winner = winner == Pawn.White ? 1 : 2;
                }
            }
            else
            {
                var iteratorForEmpty = 0;
                for (; iteratorForEmpty < 7; iteratorForEmpty++)
                {
                    if (gameBoard.fields[iteratorForEmpty] == 0) break;
                }
                var to = iteratorForEmpty + 2;
                var from = fieldNumber + 2;
                var blank = new Move(to, from);
                gameBoard.fields[from - 2] = 0;

                if (blank.Move_to == 0 || blank.Move_to == 1 || blank.Move_to == 2)
                {
                    board.Move_To_From(blank, whose_turn, false);
                    board.empty_field.Remove(0);
                    board.empty_field.Remove(1);
                    board.empty_field.Remove(2);
                    gameBoard.fields[0] = gameBoard.turn;
                }
                else
                {
                    board.Move_To_From(blank, whose_turn, false);
                    board.empty_field.Remove(blank.Move_to);
                    gameBoard.fields[to - 2] = gameBoard.turn;
                }

                if (blank.Move_from == 0 || blank.Move_from == 1 || blank.Move_from == 2)
                {
                    board.empty_field.Add(0);
                    board.empty_field.Add(1);
                    board.empty_field.Add(2);
                }
                else
                    board.empty_field.Add(blank.Move_from);
                winner = board.Check_Lines_Victory(gameBoard);

                if (winner != Pawn.Empty)
                    gameBoard.winner = winner == Pawn.White ? 1 : 2;

            }
            Change_Turn();
            gameBoard.ChangeTurn();
            return gameBoard;
        }

        private GameBoard First_Phase(int fieldNumber, GameBoard gameBoard)
        {
            if (whose_turn == player_pawn)
            {
                return PlayerPhase(fieldNumber, gameBoard);
            }

            System.Threading.Thread.Sleep(1000);
            if (gameBoard.stones[1] < 3)
            {
                Find_Moves_First_Phase(gameBoard);
                Evaluate_Moves_First_Phase(gameBoard);

                board.Print_Board();
                winner = board.Check_Lines_Victory(gameBoard);

                if (winner != Pawn.Empty)
                    gameBoard.winner = winner == Pawn.White ? 1 : 2;

                board.Write_Stale_Last_Board();
                Change_Turn();
                gameBoard.ChangeTurn();

                return gameBoard;
            }

            return Second_Phase(gameBoard);
        }

        private void Find_Moves_First_Phase(GameBoard gameBoard)
        {
            possible_moves.Clear();
            for (var i = 0; i < board.empty_field.Count; i++)
                if ((int)board.empty_field[i] > 1 && gameBoard.fields[(int) board.empty_field[i] - 2] == 0)
                    possible_moves.Add(new Move((int) board.empty_field[i]));
        }

        private void Evaluate_Moves_First_Phase(GameBoard gameBoard)
        {
            for (int i = 0; i < possible_moves.Count; i++)
            {
                possible_moves[i] = board.Move_To((Move)possible_moves[i], whose_turn, true);
            }

            Move blank;

            blank = whose_turn == Pawn.Black ? Find_Best_Move_Black() : Find_Best_Move_White();

            if (blank.Move_to == 0 || blank.Move_to == 1 || blank.Move_to == 2)
            {
                board.Move_To(blank, whose_turn, false);
                gameBoard.fields[0] = whose_turn == Pawn.White ? 1 : 2;
                board.empty_field.Remove(0);
                board.empty_field.Remove(1);
                board.empty_field.Remove(2);
            }
            else
            {
                board.Move_To(blank, whose_turn, false);
                board.empty_field.Remove(blank.Move_to);
                gameBoard.fields[blank.Move_to - 2] = whose_turn == Pawn.White ? 1 : 2;
            }
        }

        private GameBoard Second_Phase(GameBoard gameBoard)
        {
            System.Threading.Thread.Sleep(1000);

            Find_Moves_Second_Phase(gameBoard);
            Evaluate_Moves_Second_Phase(gameBoard);
            
            winner = board.Check_Lines_Victory(gameBoard);

            if (winner != Pawn.Empty)
                gameBoard.winner = winner == Pawn.White ? 1 : 2;

            board.Write_Stale_Last_Board();
            Change_Turn();
            gameBoard.ChangeTurn();

            board.Write_Base_Last_Board(winner);
            return gameBoard;
        }

        private void Find_Moves_Second_Phase(GameBoard gameBoard)
        {
            possible_moves.Clear();
            
            board.SetBoardState(gameBoard);

            for (var t = 0; t < board.empty_field.Count; t++)
            {
                if ((int)board.empty_field[t] > 1 && gameBoard.fields[(int) board.empty_field[t] - 2] == 0)
                {
                    int place = (int) board.empty_field[t];
                    int counter = place;
                    int x = 0;
                    int y = 0;

                    for (int i = 0; i < board.Get_Board_Size(); i++)
                    for (int j = 0; j < board.Get_Board_Size(); j++)
                    {
                        if (counter == 0)
                        {
                            x = j;
                            y = i;
                        }

                        counter--;
                    }

                    // r

                    if (board.Check_Board_State(whose_turn, 1, 0, place + 1, x, y))
                        possible_moves.Add(new Move(place, place + 1));

                    if (board.Check_Board_State(whose_turn, 2, 0, place + 2, x, y))
                        possible_moves.Add(new Move(place, place + 2));

                    // l

                    if (board.Check_Board_State(whose_turn, -1, 0, place - 1, x, y))
                        possible_moves.Add(new Move(place, place - 1));

                    if (board.Check_Board_State(whose_turn, -2, 0, place - 2, x, y))
                        possible_moves.Add(new Move(place, place - 2));

                    // d

                    if (board.Check_Board_State(whose_turn, 0, 1, place + 3, x, y))
                        possible_moves.Add(new Move(place, place + 3));

                    if (board.Check_Board_State(whose_turn, 0, 2, place + 6, x, y))
                        possible_moves.Add(new Move(place, place + 6));

                    // u

                    if (board.Check_Board_State(whose_turn, 0, -1, place - 3, x, y))
                        possible_moves.Add(new Move(place, place - 3));

                    if (board.Check_Board_State(whose_turn, 0, -2, place - 6, x, y))
                        possible_moves.Add(new Move(place, place - 6));
                }
            }
        }

        private void Evaluate_Moves_Second_Phase(GameBoard gameBoard)
        {
            for (int i = 0; i < possible_moves.Count; i++)
                possible_moves[i] = board.Move_To_From((Move) possible_moves[i], whose_turn, true);

            Move blank;

            blank = whose_turn == Pawn.Black ? Find_Best_Move_Black() : Find_Best_Move_White();

            if (blank.Move_to == 0 || blank.Move_to == 1 || blank.Move_to == 2)
            {
                board.Move_To_From(blank, whose_turn, false);
                gameBoard.fields[0] = whose_turn == Pawn.White ? 1 : 2;
                gameBoard.fields[blank.Move_from - 2] = 0;
                board.empty_field.Remove(0);
                board.empty_field.Remove(1);
                board.empty_field.Remove(2);
            }
            else
            {
                if (blank.Move_from == 0 || blank.Move_from == 1 || blank.Move_from == 2) gameBoard.fields[0] = 0;
                else gameBoard.fields[blank.Move_from - 2] = 0;
                board.Move_To_From(blank, whose_turn, false);
                gameBoard.fields[blank.Move_to - 2] = whose_turn == Pawn.White ? 1 : 2;
                board.empty_field.Remove(blank.Move_to);
            }

            if (blank.Move_from == 0 || blank.Move_from == 1 || blank.Move_from == 2)
            {
                board.empty_field.Add(0);
                board.empty_field.Add(1);
                board.empty_field.Add(2);
            }
            else
                board.empty_field.Add(blank.Move_from);
        }

        private Move Find_Best_Move_Black()
        {
            int highest_value = -1000;
            ArrayList best_option = new ArrayList();
            Move blank;

            for (int i = 0; i < possible_moves.Count; i++)
            {
                blank = (Move)possible_moves[i];

                if (blank.Result_weight < highest_value) continue;
                if (blank.Result_weight == highest_value)
                {
                    highest_value = blank.Result_weight;
                    best_option.Add(blank);
                }
                else
                {
                    highest_value = blank.Result_weight;
                    best_option.Clear();
                    best_option.Add(blank);
                }
            }

            Random rnd = new Random();
            int index = rnd.Next(0, best_option.Count);
            blank = (Move)best_option[index];

            return blank;
        }

        private Move Find_Best_Move_White()
        {
            int highest_value = 1000;
            ArrayList best_option = new ArrayList();
            Move blank;

            for (int i = 0; i < possible_moves.Count; i++)
            {
                blank = (Move)possible_moves[i];

                if (blank.Result_weight > highest_value) continue;
                if (blank.Result_weight == highest_value)
                {
                    highest_value = blank.Result_weight;
                    best_option.Add(blank);
                }
                else
                {
                    highest_value = blank.Result_weight;
                    best_option.Clear();
                    best_option.Add(blank);
                }
            }

            Random rnd = new Random();
            int index = rnd.Next(0, best_option.Count);
            blank = (Move)best_option[index];

            return blank;
        }

        private void Change_Turn()
        {
            switch (whose_turn)
            {
                case Pawn.Black:
                    whose_turn = Pawn.White;
                    return;
                case Pawn.White:
                    whose_turn = Pawn.Black;
                    return;
            }
        }
    }
}