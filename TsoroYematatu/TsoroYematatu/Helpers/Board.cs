using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TsoroYematatu.Enums;
using TsoroYematatu.Models;

namespace TsoroYematatu.Helpers
{
    public class Board
    {

        private static int[,] board_field_weight = new int[3, 3] { { 5, 5, 5 }, { 3, 8, 3 }, { 0, 1, 0 } };
        private static int board_size = 3;
        private static string txt_base_name = "Board_Base.txt";
        private static string txt_stale_name = "Stalemate_Base.txt";

        private Line_Base line_base;

        public ArrayList empty_field;

        private Pawn[,] board_state = new Pawn[board_size, board_size];
        private Pawn[,] experimental_board_state = new Pawn[board_size, board_size];
        private Pawn[,] base_board_state = new Pawn[board_size, board_size];
        private Pawn[,] last_board_state = new Pawn[board_size, board_size];

        public int Get_Board_Size()
        {
            return board_size;
        }

        public Board(GameBoard gameBoard)
        {
            Setup_Board();
            Setup_Empty_Field(gameBoard);
            line_base = new Line_Base();
            line_base.Setup_Line_Base();
        }

        private void Setup_Board()
        {
            for (int i = 0; i < board_size; i++)
                for (int j = 0; j < board_size; j++)
                {
                    board_state[i, j] = Pawn.Empty;
                    experimental_board_state[i, j] = Pawn.Empty;
                    last_board_state[i, j] = Pawn.Empty;
                    base_board_state[i, j] = Pawn.Empty;
                }
        }

        public void SetBoardState(GameBoard gameBoard)
        {
            for (var i = 0; i < 3; i++)
            for (var j = 0; j < 3; j++)
            {
                switch (i)
                {
                    case 0 when gameBoard.fields[0] != 0:
                        board_state[i, j] = gameBoard.fields[0] == 1 ? Pawn.White : Pawn.Black;
                        experimental_board_state[i, j] = gameBoard.fields[0] == 1 ? Pawn.White : Pawn.Black;
                        last_board_state[i, j] = gameBoard.fields[0] == 1 ? Pawn.White : Pawn.Black;
                        break;
                    case 0 when gameBoard.fields[0] == 0:
                        board_state[i, j] = Pawn.Empty;
                        experimental_board_state[i, j] = Pawn.Empty;
                        last_board_state[i, j] = Pawn.Empty;
                        break;
                    case 1 when gameBoard.fields[j] != 0:
                        board_state[i, j] = gameBoard.fields[j + 1] == 1 ? Pawn.White : Pawn.Black;
                        experimental_board_state[i, j] = gameBoard.fields[j + 1] == 1 ? Pawn.White : Pawn.Black;
                        last_board_state[i, j] = gameBoard.fields[j + 1] == 1 ? Pawn.White : Pawn.Black;
                        break;
                    case 1 when gameBoard.fields[j] == 0:
                        board_state[i, j] = Pawn.Empty;
                        experimental_board_state[i, j] = Pawn.Empty;
                        last_board_state[i, j] = Pawn.Empty;
                        break;
                    case 2 when gameBoard.fields[j + 2] != 0:
                        board_state[i, j] = gameBoard.fields[j + 4] == 1 ? Pawn.White : Pawn.Black;
                        experimental_board_state[i, j] = gameBoard.fields[j + 4] == 1 ? Pawn.White : Pawn.Black;
                        last_board_state[i, j] = gameBoard.fields[j + 4] == 1 ? Pawn.White : Pawn.Black;
                        break;
                    case 2 when gameBoard.fields[j + 2] == 0:
                        board_state[i, j] = Pawn.Empty;
                        experimental_board_state[i, j] = Pawn.Empty;
                        last_board_state[i, j] = Pawn.Empty;
                        break;
                }
            }
            Find_Board(txt_base_name);
        }

        private void Setup_Empty_Field(GameBoard gameBoard)
        {
            empty_field = new ArrayList();
            for (var i = 0; i < board_size * board_size; i++)
            {
                if (i < 3 && gameBoard.fields[0] == 0)
                    empty_field.Add(i);
                if (i > 2 && gameBoard.fields[i - 2] == 0)
                    empty_field.Add(i);
            }
        }
        public void Print_Board()
        {
            for (int i = 0; i < board_size; i++)
            {
                for (int j = 0; j < board_size; j++)
                    switch (board_state[i, j])
                    {
                        case Pawn.White:
                            Console.Write("O ");
                            break;
                        case Pawn.Black:
                            Console.Write("X ");
                            break;
                        case Pawn.Empty:
                            Console.Write(". ");
                            break;
                    }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void Print_Last_Board()
        {
            for (int i = 0; i < board_size; i++)
            {
                for (int j = 0; j < board_size; j++)
                    switch (last_board_state[i, j])
                    {
                        case Pawn.White:
                            Console.Write("O ");
                            break;
                        case Pawn.Black:
                            Console.Write("X ");
                            break;
                        case Pawn.Empty:
                            Console.Write(". ");
                            break;
                    }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void Print_Exp_Board()
        {
            for (int i = 0; i < board_size; i++)
            {
                for (int j = 0; j < board_size; j++)
                    switch (experimental_board_state[i, j])
                    {
                        case Pawn.White:
                            Console.Write("O ");
                            break;
                        case Pawn.Black:
                            Console.Write("X ");
                            break;
                        case Pawn.Empty:
                            Console.Write(". ");
                            break;
                    }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public Move Move_To(Move move, Pawn pawn_type, bool experimental)
        {
            if (!experimental) Update_Last_Board_State();

            var field_weight = Get_Board_Field_Weight(move.Move_to);

            if (pawn_type == Pawn.White) field_weight = field_weight * -1;
 
            if (move.Move_to == 0 || move.Move_to == 1 || move.Move_to == 2)
            {
                Set_Board_State(0, pawn_type, experimental);
                Set_Board_State(1, pawn_type, experimental);
                Set_Board_State(2, pawn_type, experimental);
            }
            else
                Set_Board_State(move.Move_to, pawn_type, experimental);

            var stale = Find_Board(txt_stale_name);

            if (pawn_type == Pawn.Black) stale = stale * -1;
            
            move.Result_weight = Check_Lines(experimental) + field_weight + Find_Board(txt_base_name) + stale;

            Console.WriteLine("plansza " + Find_Board(txt_base_name) + " " + stale + " " + move.Result_weight);

            Print_Exp_Board();
            Clear_Experimental_Board_State();

            return move;
        }

        public Move Move_To_From(Move move, Pawn pawn_type, bool experimental)
        {
            if (!experimental)
                Update_Last_Board_State();

            int field_weight = Get_Board_Field_Weight(move.Move_to);

            if (pawn_type == Pawn.White)
                field_weight = field_weight * -1;

            if (move.Move_to == 0 || move.Move_to == 1 || move.Move_to == 2)
            {
                Set_Board_State(0, pawn_type, experimental);
                Set_Board_State(1, pawn_type, experimental);
                Set_Board_State(2, pawn_type, experimental);
            }
            else
                Set_Board_State(move.Move_to, pawn_type, experimental);

            if (move.Move_from == 0 || move.Move_from == 1 || move.Move_from == 2)
            {
                Set_Board_State(0, Pawn.Empty, experimental);
                Set_Board_State(1, Pawn.Empty, experimental);
                Set_Board_State(2, Pawn.Empty, experimental);
            }
            else
                Set_Board_State(move.Move_from, Pawn.Empty, experimental);

            int stale = Find_Board(txt_stale_name);

            if (pawn_type == Pawn.Black)
                stale = stale * -1;

            move.Result_weight = Check_Lines(experimental) + field_weight + Find_Board(txt_base_name) + stale;

            Console.WriteLine("plansza " + Find_Board(txt_base_name) + " " + stale + " " + move.Result_weight);

            Print_Exp_Board();
            Clear_Experimental_Board_State();

            return move;
        }

        private int Check_Lines(bool experimental)
        {
            return Check_Left_Line(experimental) + Check_Interior_Line(experimental) + Check_Right_Line(experimental) + Check_Middle_Line(experimental) + Check_Lower_Line(experimental);
        }

        public Pawn Check_Lines_Victory(GameBoard gameBoard)
        {
            if (gameBoard.fields[0] == gameBoard.fields[1] && gameBoard.fields[1] == gameBoard.fields[4] && gameBoard.fields[0] != 0)
                return gameBoard.fields[0] == 1 ? Pawn.White : Pawn.Black;
            
            if (gameBoard.fields[0] == gameBoard.fields[2] && gameBoard.fields[2] == gameBoard.fields[5] && gameBoard.fields[0] != 0)
                return gameBoard.fields[0] == 1 ? Pawn.White : Pawn.Black;

            if (gameBoard.fields[0] == gameBoard.fields[3] && gameBoard.fields[3] == gameBoard.fields[6] && gameBoard.fields[0] != 0)
                return gameBoard.fields[0] == 1 ? Pawn.White : Pawn.Black;

            if (gameBoard.fields[1] == gameBoard.fields[2] && gameBoard.fields[2] == gameBoard.fields[3] && gameBoard.fields[1] != 0)
                return gameBoard.fields[1] == 1 ? Pawn.White : Pawn.Black;

            if (gameBoard.fields[4] == gameBoard.fields[5] && gameBoard.fields[5] == gameBoard.fields[6] && gameBoard.fields[4] != 0)
                return gameBoard.fields[4] == 1 ? Pawn.White : Pawn.Black;

            return Pawn.Empty;
        }

        private int Check_Left_Line(bool experimental)
        {
            Line line;

            line = experimental 
                ? new Line(experimental_board_state[0, 0], experimental_board_state[1, 0], experimental_board_state[2, 0]) 
                : new Line(board_state[0, 0], board_state[1, 0], board_state[2, 0]);

            return line_base.Get_Line_Weight(line);
        }

        private int Check_Interior_Line(bool experimental)
        {
            Line line;

            line = experimental 
                ? new Line(experimental_board_state[0, 1], experimental_board_state[1, 1], experimental_board_state[2, 1]) 
                : new Line(board_state[0, 1], board_state[1, 1], board_state[2, 1]);

            return line_base.Get_Line_Weight(line);
        }

        private int Check_Right_Line(bool experimental)
        {
            Line line;

            line = experimental 
                ? new Line(experimental_board_state[0, 2], experimental_board_state[1, 2], experimental_board_state[2, 2]) 
                : new Line(board_state[0, 2], board_state[1, 2], board_state[2, 2]);

            return line_base.Get_Line_Weight(line);
        }

        private int Check_Middle_Line(bool experimental)
        {
            Line line;

            line = experimental 
                ? new Line(experimental_board_state[1, 0], experimental_board_state[1, 1], experimental_board_state[1, 2]) 
                : new Line(board_state[1, 0], board_state[1, 1], board_state[1, 2]);

            return line_base.Get_Line_Weight(line);
        }

        private int Check_Lower_Line(bool experimental)
        {
            Line line;

            line = experimental 
                ? new Line(experimental_board_state[2, 0], experimental_board_state[2, 1], experimental_board_state[2, 2]) 
                : new Line(board_state[2, 0], board_state[2, 1], board_state[2, 2]);

            return line_base.Get_Line_Weight(line);
        }

        private void Set_Board_State(int place, Pawn pawn_type, bool experimental)
        {

            for (int i = 0; i < board_size; i++)
                for (int j = 0; j < board_size; j++)
                {
                    if (place == 0)
                    {

                        if (experimental)
                        {

                            experimental_board_state[i, j] = pawn_type;
                        }
                        else
                        {

                            board_state[i, j] = pawn_type;
                        }
                    }
                    place--;
                }
        }

        private void Clear_Experimental_Board_State()
        {
            for (int i = 0; i < board_size; i++)
                for (int j = 0; j < board_size; j++)
                    experimental_board_state[i, j] = board_state[i, j];
        }

        private void Update_Last_Board_State()
        {
            for (var i = 0; i < board_size; i++)
                for (var j = 0; j < board_size; j++)
                    last_board_state[i, j] = board_state[i, j];
        }

        public bool Check_Board_State(Pawn pawn_type, int jump_left_right, int jump_up_down, int place, int x, int y)
        {
            if (x + jump_left_right < 0 || x + jump_left_right >= board_size)
                return false;
            
            if (y + jump_up_down < 0 || y + jump_up_down >= board_size)
                return false;
            
            for (int i = 0; i < board_size; i++)
                for (int j = 0; j < board_size; j++)
                {
                    if (place == 0)
                        return board_state[i, j] == pawn_type;

                    place--;
                }
            return false;
        }

        private int Get_Board_Field_Weight(int place)
        {
            for (var i = 0; i < board_size; i++)
                for (var j = 0; j < board_size; j++)
                {
                    if (place == 0)
                    {
                        return board_field_weight[i, j];
                    }
                    place--;
                }
            return -1000;
        }

        private bool Check_Last_Experimental()
        {
            for (var i = 0; i < board_size; i++)
                for (var j = 0; j < board_size; j++)
                    if (last_board_state[i, j] != experimental_board_state[i, j])
                        return false;
            return true;
        }

        private bool Check_Base_Experimental()
        {
            for (var i = 0; i < board_size; i++)
                for (var j = 0; j < board_size; j++)
                    if (base_board_state[i, j] != experimental_board_state[i, j])
                        return false;
            return true;
        }

        private int Find_Board(string txt_name)
        {
            var exists = false;
            var line_counter = 0;
            var x = 0;
            var y = 0;
            var counter = 0;
            var place = 0;
            var weight = 0;
            if (!File.Exists(txt_name)) File.Create(txt_name);

            var path = Path.GetFullPath(txt_name);
            foreach (var line in File.ReadLines(txt_name))
            {
                if (exists)
                {
                    weight = Int32.Parse(line);
                    break;
                }

                place = counter;
                if (counter != 9)
                {
                    for (int i = 0; i < board_size; i++)
                        for (int j = 0; j < board_size; j++)
                        {
                            if (place == 0)
                            {
                                x = i;
                                y = j;
                            }
                            place--;
                        }

                    switch (line)
                    {
                        case "White":
                            base_board_state[x, y] = Pawn.White;
                            break;
                        case "Black":
                            base_board_state[x, y] = Pawn.Black;
                            break;
                        case "Empty":
                            base_board_state[x, y] = Pawn.Empty;
                            break;
                    }
                }

                line_counter++;
                counter++;

                switch (counter)
                {
                    case 9:
                        if (Check_Base_Experimental())
                            exists = true;
                        break;
                    case 10:
                        counter = 0;
                        break;
                }
            }

            return exists ? weight : 0;
        }

        public void Write_Base_Last_Board(Pawn pawn_type)
        {

            bool exists = false;
            int line_counter = 0;
            int x = 0;
            int y = 0;
            int counter = 0;
            int place = 0;
            int weight = 0;

            foreach (string line in File.ReadLines(txt_base_name))
            {
                if (exists)
                {
                    weight = Int32.Parse(line);
                    break;
                }

                place = counter;

                if (counter != 9)
                {
                    for (int i = 0; i < board_size; i++)
                        for (int j = 0; j < board_size; j++)
                        {
                            if (place == 0)
                            {
                                x = i;
                                y = j;
                            }
                            place--;
                        }
                    
                    switch (line)
                    {
                        case "White":
                            experimental_board_state[x, y] = Pawn.White;
                            break;
                        case "Black":
                            experimental_board_state[x, y] = Pawn.Black;
                            break;
                        case "Empty":
                            experimental_board_state[x, y] = Pawn.Empty;
                            break;
                    }
                }

                line_counter++;
                counter++;

                switch (counter)
                {
                    case 9:
                        if (Check_Last_Experimental())
                            exists = true;
                        break;
                    case 10:
                        counter = 0;
                        break;
                }
            }

            if (exists)
            {
                if (pawn_type == Pawn.White)
                    weight--;
                else
                    weight++;
                
                Line_Changer(weight.ToString(), txt_base_name, line_counter);
            }
            else
            {
                using (StreamWriter w = File.AppendText(txt_base_name))
                {
                    Console.WriteLine("new win");
                    for (int i = 0; i < board_size; i++)
                        for (int j = 0; j < board_size; j++)
                            w.WriteLine(last_board_state[i, j]);
                        
                    if (pawn_type == Pawn.White)
                        w.WriteLine(-1);
                    else
                        w.WriteLine(1);
                }
            }
        }

        public void Write_Stale_Last_Board()
        {

            bool exists = false;
            int line_counter = 0;
            int x = 0;
            int y = 0;
            int counter = 0;
            int place = 0;
            int weight = 0;

            foreach (string line in File.ReadLines(txt_stale_name))
            {
                if (exists)
                {
                    weight = Int32.Parse(line);
                    break;
                }

                place = counter;

                if (counter != 9)
                {
                    for (int i = 0; i < board_size; i++)
                        for (int j = 0; j < board_size; j++)
                        {
                            if (place == 0)
                            {
                                x = i;
                                y = j;
                            }
                            place--;
                        }

                    switch (line)
                    {
                        case "White":
                            experimental_board_state[x, y] = Pawn.White;
                            break;
                        case "Black":
                            experimental_board_state[x, y] = Pawn.Black;
                            break;
                        case "Empty":
                            experimental_board_state[x, y] = Pawn.Empty;
                            break;
                    }
                }

                line_counter++;
                counter++;

                switch (counter)
                {
                    case 9:
                        if (Check_Last_Experimental())
                            exists = true;
                            break;
                    case 10:
                        counter = 0;
                        break;
                }
            }

            if (exists)
            {
                weight++;
                Line_Changer(weight.ToString(), txt_stale_name, line_counter);
            }
            else
            {
                using (StreamWriter w = File.AppendText(txt_stale_name))
                {
                    Console.WriteLine("new stale");

                    for (int i = 0; i < board_size; i++)
                        for (int j = 0; j < board_size; j++)
                            w.WriteLine(last_board_state[i, j]);
                    
                    w.WriteLine(1);
                }
            }

            Clear_Experimental_Board_State();
        }

        static void Line_Changer(string newText, string fileName, int line_to_edit)
        {
            Console.WriteLine("old " + newText);

            var path = Path.GetFullPath(fileName);



            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[line_to_edit] = newText;
            File.WriteAllLines(fileName, arrLine);
        }
    }
}