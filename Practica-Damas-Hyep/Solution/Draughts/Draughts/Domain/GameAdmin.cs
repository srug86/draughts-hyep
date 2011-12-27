using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Draughts.Domain
{
    class GameAdmin
    {
        private Player pl1, pl2;
        public Player Pl1
        {
            get { return pl1; }
            set { pl1 = value; }
        }

        public Player Pl2
        {
            get { return pl2; }
            set { pl2 = value; }
        }

        private int[,] table;
        public int[,] Table
        {
            get { return table; }
            set { table = value; }
        }

        int turn;
        public int Turn
        {
            get { return turn; }
            set { turn = value; }
        }

        public GameAdmin(Player pl1, Player pl2)
        {
            this.pl1 = pl1;
            this.pl2 = pl2;
            this.turn = 1;
            initTable();
        }

        private void initTable()
        {
            this.table = new int[8, 8] {
            {0, 1, 0, 1, 0, 1, 0, 1},
            {1, 0, 1, 0, 1, 0, 1, 0},
            {0, 1, 0, 1, 0, 1, 0, 1},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {2, 0, 2, 0, 2, 0, 2, 0},
            {0, 2, 0, 2, 0, 2, 0, 2},
            {2, 0, 2, 0, 2, 0, 2, 0} };
            /*            this.table = new int[8, 8] {
                        {0, 1, 0, 1, 0, 1, 0, 1},
                        {1, 0, 1, 0, 1, 0, 1, 0},
                        {0, 1, 0, 0, 0, 1, 0, 1},
                        {0, 0, 0, 0, 0, 0, 0, 0},
                        {0, 0, 0, 0, 0, 0, 0, 0},
                        {0, 0, 0, 0, 0, 0, 2, 0},
                        {0, 0, 0, 1, 0, 0, 0, 2},
                        {2, 0, 2, 0, 0, 0, 2, 0} }; */
        }

        public void printTable()
        {
            for (int i = 7; i >= 0; i--)
            {
                Console.WriteLine("");
                for (int j = 0; j < 8; j++)
                {
                    if (j == 0) Console.Write(i + 1 + " |\t");
                    Console.Write(this.table[i, j] + "\t");
                }
                Console.WriteLine("");
                if (i > 0) Console.Write("  |");
            }
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("  |\t1\t2\t3\t4\t5\t6\t7\t8\n");
        }

        public int doPlay(int row, int column)
        {
            int result = -1;
            if (this.table[row, column] == this.turn * 11111)   // Movimiento de una ficha
            {
                if (row != 0 && row != 7) this.table[row, column] = this.turn * 1;
                else this.table[row, column] = this.turn * 11;  // La ficha ha coronado
                result = 0;
            }
            else if (this.table[row, column] == this.turn * 111111) // Movimiento de la reina
            {
                this.table[row, column] = this.turn * 11;
                result = 0;
            }
            if (result == 0)    // Después de un movimiento se eliminan las otras opciones
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        if (this.table[i, j] > this.turn * 100)
                            this.table[i, j] = 0;
            return result;
        }

        public Boolean calculateOptions(int row, int column)
        {
            if (this.table[row, column] == this.turn)   // Ficha normal
            {
                this.table[row, column] = this.turn * 111;
                chipOptions(row, column, false);
                return true;
            }
            else if (this.table[row, column] == this.turn * 11)   // Reina
            {
                this.table[row, column] = this.turn * 1111;
                queenOptions(row, column);
                return true;
            }
            else return false;
        }

        private void chipOptions(int row, int column, Boolean eating)
        {
            if (this.turn == 1)
            {
                if (column - 1 >= 0 && row + 1 < 8)
                {
                    if (this.table[row + 1, column - 1] == 0 && !eating)
                        this.table[row + 1, column - 1] = this.turn * 11111;
                    else if (this.table[row + 1, column - 1] == 2 * 1 ||
                        this.table[row + 1, column - 1] == 2 * 11)
                    {
                        if (column - 2 >= 0 && row + 2 < 8)
                        {
                            if (this.table[row + 2, column - 2] == 0)
                            {
                                this.table[row + 2, column - 2] = this.turn * 11111;
                                if (this.table[row + 1, column - 1] == 2 * 1)
                                    this.table[row + 1, column - 1] = 2 * 111;
                                else if (this.table[row + 1, column - 1] == 2 * 11)
                                    this.table[row + 1, column - 1] = 2 * 1111;
                                chipOptions(row + 2, column - 2, true);
                            }
                        }
                    }
                }
                if (column + 1 < 8 && row + 1 < 8)
                {
                    if (this.table[row + 1, column + 1] == 0 && !eating)
                        this.table[row + 1, column + 1] = this.turn * 11111;
                    else if (this.table[row + 1, column + 1] == 2 * 1 ||
                        this.table[row + 1, column + 1] == 2 * 11)
                    {
                        if (column + 2 < 8 && row + 2 < 8)
                        {
                            if (this.table[row + 2, column + 2] == 0)
                            {
                                this.table[row + 2, column + 2] = this.turn * 11111;
                                if (this.table[row + 1, column + 1] == 2 * 1)
                                    this.table[row + 1, column + 1] = 2 * 111;
                                else if (this.table[row + 1, column + 1] == 2 * 11)
                                    this.table[row + 1, column + 1] = 2 * 1111;
                                chipOptions(row + 2, column + 2, true);
                            }
                        }
                    }
                }
            }
            else if (this.turn == 2)
            {
                if (column - 1 >= 0 && row - 1 >= 0)
                {
                    if (this.table[row - 1, column - 1] == 0 && !eating)
                        this.table[row - 1, column - 1] = this.turn * 11111;
                    else if (this.table[row - 1, column - 1] == 1 ||
                        this.table[row - 1, column - 1] == 11)
                    {
                        if (column - 2 >= 0 && row - 2 >= 0)
                        {
                            if (this.table[row - 2, column - 2] == 0)
                            {
                                this.table[row - 2, column - 2] = this.turn * 11111;
                                if (this.table[row - 1, column - 1] == 1)
                                    this.table[row - 1, column - 1] = 111;
                                else if (this.table[row - 1, column - 1] == 11)
                                    this.table[row - 1, column - 1] = 1111;
                                chipOptions(row - 2, column - 2, true);
                            }
                        }
                    }
                }
                if (column + 1 < 8 && row - 1 >= 0)
                {
                    if (this.table[row - 1, column + 1] == 0 && !eating)
                        this.table[row - 1, column + 1] = this.turn * 11111;
                    else if (this.table[row - 1, column + 1] == 1 ||
                        this.table[row - 1, column + 1] == 11)
                    {
                        if (column + 2 < 8 && row - 2 >= 0)
                        {
                            if (this.table[row - 2, column + 2] == 0)
                            {
                                this.table[row - 2, column + 2] = this.turn * 11111;
                                if (this.table[row - 1, column + 1] == 1)
                                    this.table[row - 1, column + 1] = 111;
                                else if (this.table[row - 1, column + 1] == 11)
                                    this.table[row - 1, column + 1] = 1111;
                                chipOptions(row - 2, column + 2, true);
                            }
                        }
                    }
                }
            }
        }

        private void queenOptions(int row, int column)
        {
            for (int dir = 0; dir < 4; dir++)
            {
                Boolean stop = false;
                for (int i = 0; i < 8 && !stop; i++)
                {
                    switch (dir)
                    {
                        case 0:
                            if (row + i < 8 && column + i < 8)
                            {
                                if (this.table[row + i, column + i] == 0)
                                    this.table[row + i, column + i] = this.turn * 111111;
                            }
                            else stop = true;
                            break;
                        case 1:
                            if (row + i < 8 && column - i >= 0)
                            {
                                if (this.table[row + i, column - i] == 0)
                                    this.table[row + i, column - i] = this.turn * 111111;
                            }
                            else stop = true;
                            break;
                        case 2:
                            if (row - i >= 0 && column + i < 8)
                            {
                                if (this.table[row - i, column + i] == 0)
                                    this.table[row - i, column + i] = this.turn * 111111;
                            }
                            else stop = true;
                            break;
                        case 3:
                            if (row - i >= 0 && column - i >= 0)
                            {
                                if (this.table[row - i, column - i] == 0)
                                    this.table[row - i, column - i] = this.turn * 111111;
                            }
                            else stop = true;
                            break;
                    }
                }
            }
        }

        public void resetOptions()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (this.table[i, j] > 10000) this.table[i, j] = 0;
                    else if (this.table[i, j] == 111) this.table[i, j] = 1;
                    else if (this.table[i, j] == 1111) this.table[i, j] = 11;
                    else if (this.table[i, j] == 2 * 111) this.table[i, j] = 2;
                    else if (this.table[i, j] == 2 * 1111) this.table[i, j] = 22;
                }
        }
    }
}
