using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsoroYematatu {

    public enum Pawn { Black, White, Empty };

    public class Line {

        private static int line_state_size = 3;

        private int base_line_weight;

        private Pawn[] line_state;

        public Line(Pawn first_pawn, Pawn second_pawn, Pawn third_pawn) {

            Line_state = new Pawn[line_state_size];

            Line_state[0] = first_pawn;
            Line_state[1] = second_pawn;
            Line_state[2] = third_pawn;
        }

        public int Base_line_weight
        {
            get
            {
                return base_line_weight;
            }

            set
            {
                base_line_weight = value;
            }
        }

        public Pawn[] Line_state
        {
            get
            {
                return line_state;
            }

            set
            {
                line_state = value;
            }
        }
    }

    public class Board {

        private static int[,] board_field_weight = new int[3, 3] {{5,5,5},{3,8,3},{0,1,0}};
        private static int board_size = 3;
        private static string txt_name = "Board_Base.txt";

        private Line_Base line_base; 
        
        public ArrayList empty_field;

        private Pawn[,] board_state = new Pawn[board_size, board_size];
        private Pawn[,] experimental_board_state = new Pawn[board_size, board_size];
        private Pawn[,] base_board_state = new Pawn[board_size, board_size];
        private Pawn[,] last_board_state = new Pawn[board_size, board_size];

        public int Get_Board_Size() {

            return board_size;
        }


        public Board() {

            Setup_Board();
            Setup_Empty_Field();
            line_base = new Line_Base();
            line_base.Setup_Line_Base();
        }

        private void Setup_Board() {

            for (int i = 0; i < board_size; i++) {

                for (int j = 0; j < board_size; j++) {

                    board_state[i, j] = Pawn.Empty;
                    experimental_board_state[i, j] = Pawn.Empty;
                    last_board_state[i, j] = Pawn.Empty;
                    base_board_state[i, j] = Pawn.Empty;
                }
            }       
        }

        private void Setup_Empty_Field() {

            empty_field = new ArrayList();

            for (int i = 0; i < board_size*board_size; i++) {

                empty_field.Add(i);
            }
        }

        public void Print_Board() {

            for (int i = 0; i < board_size; i++) {

                for (int j = 0; j < board_size; j++) {

                    if (board_state[i, j] == Pawn.White) {

                        Console.Write("O ");
                    }

                    if (board_state[i, j] == Pawn.Black) {

                        Console.Write("X ");
                    }

                    if (board_state[i, j] == Pawn.Empty) {

                        Console.Write(". ");
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public void Print_Last_Board() {

            for (int i = 0; i < board_size; i++) {

                for (int j = 0; j < board_size; j++) {

                    if (last_board_state[i, j] == Pawn.White) {

                        Console.Write("O ");
                    }

                    if (last_board_state[i, j] == Pawn.Black) {

                        Console.Write("X ");
                    }

                    if (last_board_state[i, j] == Pawn.Empty) {

                        Console.Write(". ");
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public void Print_Exp_Board() {

            for (int i = 0; i < board_size; i++) {

                for (int j = 0; j < board_size; j++) {

                    if (experimental_board_state[i, j] == Pawn.White) {

                        Console.Write("O ");
                    }

                    if (experimental_board_state[i, j] == Pawn.Black) {

                        Console.Write("X ");
                    }

                    if (experimental_board_state[i, j] == Pawn.Empty) {

                        Console.Write(". ");
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public Move Move_To(Move move, Pawn pawn_type, bool experimental) {

            if (!experimental) {

                Update_Last_Board_State();
            }

            int field_weight = Get_Board_Field_Weight(move.Move_to);

            if (pawn_type == Pawn.White) {

                field_weight = field_weight * -1;
            }

            if (move.Move_to == 0 || move.Move_to == 1 || move.Move_to == 2) {

                Set_Board_State(0, pawn_type, experimental);
                Set_Board_State(1, pawn_type, experimental);
                Set_Board_State(2, pawn_type, experimental);

            }
            else {

                Set_Board_State(move.Move_to, pawn_type, experimental);
            }

            move.Result_weight = Check_Lines(experimental) + field_weight + Find_Board();

            Console.WriteLine("plansza " + Find_Board() +" "  + move.Result_weight);

            Print_Exp_Board();

            Clear_Experimental_Board_State();

            return move;
        }

        public Move Move_To_From(Move move, Pawn pawn_type, bool experimental) {

            if (!experimental) {

                Update_Last_Board_State();
            }

            int field_weight = Get_Board_Field_Weight(move.Move_to);

            if (pawn_type == Pawn.White) {

                field_weight = field_weight * -1;
            }

            if (move.Move_to == 0 || move.Move_to == 1 || move.Move_to == 2) {

                Set_Board_State(0, pawn_type, experimental);
                Set_Board_State(1, pawn_type, experimental);
                Set_Board_State(2, pawn_type, experimental);

            }
            else {

                Set_Board_State(move.Move_to, pawn_type, experimental);
            }

            if (move.Move_from == 0 || move.Move_from == 1 || move.Move_from == 2) {

                Set_Board_State(0, Pawn.Empty, experimental);
                Set_Board_State(1, Pawn.Empty, experimental);
                Set_Board_State(2, Pawn.Empty, experimental);

            }
            else {

                Set_Board_State(move.Move_from, Pawn.Empty, experimental);
            }

            move.Result_weight = Check_Lines(experimental) + field_weight + Find_Board();

            Console.WriteLine("plansza " + Find_Board() + " " + move.Result_weight);

            Print_Exp_Board();

            Clear_Experimental_Board_State();

            return move;
        }

        private int Check_Lines(bool experimental) {

            return Check_Left_Line(experimental) + Check_Interior_Line(experimental) + Check_Right_Line(experimental) + Check_Middle_Line(experimental)+ Check_Lower_Line(experimental);
        }

        public Pawn Check_Lines_Victory() {

            int result1;
            int result2;
            int result3;
            int result4;
            int result5;

            result1 = Check_Left_Line(false);
            result2 = Check_Right_Line(false);
            result3 = Check_Interior_Line(false);
            result4 = Check_Middle_Line(false);
            result5 = Check_Lower_Line(false);

            if (result1 == 100 || result2 == 100 || result3 == 100 || result4 == 100 || result5 == 100) {

                return Pawn.Black;
            }

            if (result1 == -100 || result2 == -100 || result3 == -100 || result4 == -100 || result5 == -100) {

                return Pawn.White;
            }

            return Pawn.Empty;
        }

        private int Check_Left_Line(bool experimental) {

            Line line;

            if (experimental) {

                line = new Line(experimental_board_state[0, 0], experimental_board_state[1, 0], experimental_board_state[2, 0]);
            }
            else {

                line = new Line(board_state[0, 0], board_state[1, 0], board_state[2, 0]);
            }

            return line_base.Get_Line_Weight(line);
        }

        private int Check_Interior_Line(bool experimental) {

            Line line;

            if (experimental) {

                line = new Line(experimental_board_state[0, 1], experimental_board_state[1, 1], experimental_board_state[2, 1]);
            }
            else {

                line = new Line(board_state[0, 1], board_state[1, 1], board_state[2, 1]);
            }

            return line_base.Get_Line_Weight(line);
        }

        private int Check_Right_Line(bool experimental) {

            Line line;

            if (experimental) {

                line = new Line(experimental_board_state[0, 2], experimental_board_state[1, 2], experimental_board_state[2, 2]);
            }
            else {

                line = new Line(board_state[0, 2], board_state[1, 2], board_state[2, 2]);
            }

            return line_base.Get_Line_Weight(line);
        }

        private int Check_Middle_Line(bool experimental) {

            Line line;

            if (experimental) {

                line = new Line(experimental_board_state[1, 0], experimental_board_state[1, 1], experimental_board_state[1, 2]);
            }
            else {

                line = new Line(board_state[1, 0], board_state[1, 1], board_state[1, 2]);
            }

            return line_base.Get_Line_Weight(line);
        }

        private int Check_Lower_Line(bool experimental) {

            Line line;

            if (experimental) {

                line = new Line(experimental_board_state[2, 0], experimental_board_state[2, 1], experimental_board_state[2, 2]);
            }
            else {

                line = new Line(board_state[2, 0], board_state[2, 1], board_state[2, 2]);
            }

            return line_base.Get_Line_Weight(line);
        }

        private void Set_Board_State(int place, Pawn pawn_type, bool experimental) {

            for (int i = 0; i < board_size; i++) {

                for (int j = 0; j < board_size; j++) {

                    if (place == 0) {

                        if (experimental) {

                            experimental_board_state[i, j] = pawn_type;    
                        }
                        else {
 
                            board_state[i, j] = pawn_type;       
                        }
                    }

                    place--;             
                }
            }
        }

        private void Clear_Experimental_Board_State() {

            for (int i = 0; i < board_size; i++) {

                for (int j = 0; j < board_size; j++) {

                    experimental_board_state[i, j] = board_state[i, j];
                }
            }
        }

        private void Update_Last_Board_State() {

            for (int i = 0; i < board_size; i++) {

                for (int j = 0; j < board_size; j++) {

                    last_board_state[i, j] = board_state[i, j];
                }
            }
        }

        public bool Check_Board_State(Pawn pawn_type, int jump_left_right, int jump_up_down, int place, int x, int y) {

            if (x + jump_left_right < 0 || x + jump_left_right >= board_size) {

                return false;
            }

            if (y + jump_up_down < 0 || y + jump_up_down >= board_size) {

                return false;
            }

            for (int i = 0; i < board_size; i++) {

                for (int j = 0; j < board_size; j++) {

                    if (place == 0) {

                        if (board_state[i, j] == pawn_type) {
                        
                            return true;
                        }
                        else {

                            return false;
                        }         
                    }

                    place--;
                }
            }

            return false;
        }

        private int Get_Board_Field_Weight(int place) {

            for (int i = 0; i < board_size; i++) {

                for (int j = 0; j < board_size; j++) {

                    if (place == 0) {

                        return board_field_weight[i, j];
                    }

                    place--;
                }
            }

            return -1000;
        }

        private bool Check_Last_Experimental() {


            for (int i = 0; i < board_size; i++) {

                for (int j = 0; j < board_size; j++) {

                    if (last_board_state[i, j] != experimental_board_state[i, j]) {

                        return false;
                    }
                }
            }

            return true;
        }

        private bool Check_Base_Experimental() {

            for (int i = 0; i < board_size; i++) {

                for (int j = 0; j < board_size; j++) {

                    if (base_board_state[i, j] != experimental_board_state[i, j]) {

                        return false;
                    }
                }
            }

            return true;
        }

        private int Find_Board() {

            bool exists = false;

            int line_counter = 0;

            int x = 0;
            int y = 0;
            int counter = 0;
            int place = 0;

            int weight = 0;

            foreach (string line in File.ReadLines(txt_name)) {

                if (exists) {

                    weight = Int32.Parse(line);
                    break;
                }

                place = counter;

                if (counter != 9) {

                    for (int i = 0; i < board_size; i++) {

                        for (int j = 0; j < board_size; j++) {

                            if (place == 0) {

                                x = i;
                                y = j;
                            }

                            place--;
                        }
                    }

                    if (line == "White") {

                        base_board_state[x, y] = Pawn.White;
                    }

                    if (line == "Black") {

                        base_board_state[x, y] = Pawn.Black;
                    }

                    if (line == "Empty") {

                        base_board_state[x, y] = Pawn.Empty;
                    }
                }

                line_counter++;
                counter++;

                if (counter == 9) {

                    if (Check_Base_Experimental()) {

                        exists = true;
                    }
                }

                if (counter == 10) {

                    counter = 0;
                }
            }

            if (exists) {

                return weight;
            }

            return 0;
        }

        public void Write_Last_Board(Pawn pawn_type) {

            bool exists = false;
            
            int line_counter = 0;

            int x = 0;
            int y = 0;
            int counter = 0;
            int place = 0;

            int weight = 0;

            foreach (string line in File.ReadLines(txt_name)) {

                if (exists) {

                    weight = Int32.Parse(line);
                    break;
                }

                place = counter;

                if (counter != 9) {

                    for (int i = 0; i < board_size; i++) {

                        for (int j = 0; j < board_size; j++) {

                            if (place == 0) {

                                x = i;
                                y = j;
                            }

                            place--;
                        }
                    }

                    if (line == "White") {

                        experimental_board_state[x, y] = Pawn.White;
                    }

                    if (line == "Black") {

                        experimental_board_state[x, y] = Pawn.Black;
                    }

                    if (line == "Empty") {

                        experimental_board_state[x, y] = Pawn.Empty;
                    }
                }
                          
                line_counter++;
                counter++;
          
                if (counter == 9) {
                               
                    if (Check_Last_Experimental()) {

                        exists = true;
                    }
                }

                if (counter == 10) {

                    counter = 0;
                }
            }

            if (exists) {
            
                if (pawn_type == Pawn.White) {

                    weight--;
                }
                else {

                    weight++; ;
                }

                Line_Changer(weight.ToString(), txt_name, line_counter);
            }
            else {

                using (StreamWriter w = File.AppendText(txt_name)) {

                    Console.WriteLine("new win");

                    for (int i = 0; i < board_size; i++) {

                        for (int j = 0; j < board_size; j++) {

                            w.WriteLine(last_board_state[i, j]);
                        }
                    }

                    if (pawn_type == Pawn.White) {

                        w.WriteLine(-1);
                    }
                    else {

                        w.WriteLine(1);
                    }
                }
            }
        }

        static void Line_Changer(string newText, string fileName, int line_to_edit) {

            Console.WriteLine("old win " + newText);

            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[line_to_edit] = newText;
            File.WriteAllLines(fileName, arrLine);
        }
    }
}
