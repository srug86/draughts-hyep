using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Draughts.Presentation;

namespace Draughts.Domain
{
    public class GameAdmin : Subject
    {
        public List<Observer> observers = new List<Observer>();
        private int row;
        private int column;
        private int state;

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

        public int getTable(int row, int column)
        {
            return this.table[row, column];
        }
        public void setTable(int row, int column, int state)
        {
            this.table[row, column] = state;
            this.row = row;
            this.column = column;
            this.state = state;
            tableValuesHaveChanged();
        }

        private int turn;
        public int Turn
        {
            get { return turn; }
            set { turn = value; }
        }

        private Boolean finish;
        private Boolean selected;

        public GameAdmin(InitWin init, Player pl1, Player pl2)
        {
            this.pl1 = pl1;
            this.pl2 = pl2;
            this.turn = 1;
            this.finish = false;
            this.selected = false;
            GameWin game = new GameWin(init, this, this);
            game.Show();
            initTable();
        }

        // implementación de la interfaz
        public void registerInterest(Observer obs)
        {
            // Agrega un objeto Observer a la colección
            observers.Add(obs);
        }

        // método que recorre la colección de Observers llamando a los notify() de cada Observer
        private void switchBox()
        {
            for (int i = 0; i < observers.Count; i++)
            {
                Observer obs = (Observer)observers[i];
                obs.notify(this.row, this.column, this.state);
            }
        }

        private void tableValuesHaveChanged()
        {
            //Ejecutamos el método creado anteriormente mediante el delegado para notificar a 
            //todos los observadores del cambio sucedido en el comboBox
            DelegateOfTheBox oSwitchBox = new DelegateOfTheBox();
            DelegateOfTheBox.BoxDelegate oBoxDelegate = new DelegateOfTheBox.BoxDelegate(switchBox);
            oSwitchBox.switchBox += oBoxDelegate;
            oSwitchBox.changeContentsBox = state;

            oSwitchBox.switchBox -= oBoxDelegate;
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
        }

        public void selectedBox(int row, int column)
        {
            if (!finish)
            {
                if (!selected)
                {
                    if (calculateOptions(row, column))
                    {
                        selected = true;
                    }
                }
                else
                {
                    selected = false;
                    int result = doPlay(row, column);
                    if (result > -1)   // Jugada válida
                    {
                        if (result != 0)    // Partida finalizada
                        {
                            //Console.WriteLine("Partida finalizada...");
                            //if (result > 0) Console.WriteLine("Ha ganado el jugador " + result);
                            //else Console.WriteLine("Ha habido un empate");
                            this.finish = true;
                        }
                        else
                        {
                            //Console.WriteLine("Jugada REALIZADA.\nCambio de turno...");
                            // Cambio de turno
                            if (this.Turn == 1) this.Turn = 2;
                            else this.Turn = 1;
                        }
                    }
                    else
                    {
                        resetOptions();
                    }
                }
            }
        }

        public int doPlay(int row, int column)
        {
            int result = -1;
            if (this.getTable(row, column) == this.turn * 11111)   // Movimiento de una ficha
            {
                if (row != 0 && row != 7) this.setTable(row, column, this.turn * 1);
                else this.setTable(row, column, this.turn * 11);  // La ficha ha coronado
                result = 0;
            }
            else if (this.getTable(row, column) == this.turn * 111111) // Movimiento de la reina
            {
                this.setTable(row, column, this.turn * 11);
                result = 0;
            }
            if (result == 0)    // Después de un movimiento se eliminan las otras opciones
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        if (this.getTable(i, j) > this.turn * 100)
                            this.setTable(i, j, 0);
            return result;
        }

        public Boolean calculateOptions(int row, int column)
        {
            if (this.getTable(row, column) == this.turn)   // Ficha normal
            {
                this.setTable(row, column, this.turn * 111);
                chipOptions(row, column, false);
                return true;
            }
            else if (this.getTable(row, column) == this.turn * 11)   // Reina
            {
                this.setTable(row, column, this.turn * 1111);
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
                    if (this.getTable(row + 1, column - 1) == 0 && !eating)
                        this.setTable(row + 1, column - 1, this.turn * 11111);
                    else if (this.getTable(row + 1, column - 1) == 2 * 1 ||
                        this.getTable(row + 1, column - 1) == 2 * 11)
                    {
                        if (column - 2 >= 0 && row + 2 < 8)
                        {
                            if (this.getTable(row + 2, column - 2) == 0)
                            {
                                this.setTable(row + 2, column - 2, this.turn * 11111);
                                if (this.getTable(row + 1, column - 1) == 2 * 1)
                                    this.setTable(row + 1, column - 1, 2 * 111);
                                else if (this.getTable(row + 1, column - 1) == 2 * 11)
                                    this.setTable(row + 1, column - 1, 2 * 1111);
                                chipOptions(row + 2, column - 2, true);
                            }
                        }
                    }
                }
                if (column + 1 < 8 && row + 1 < 8)
                {
                    if (this.getTable(row + 1, column + 1) == 0 && !eating)
                        this.setTable(row + 1, column + 1, this.turn * 11111);
                    else if (this.getTable(row + 1, column + 1) == 2 * 1 ||
                        this.getTable(row + 1, column + 1) == 2 * 11)
                    {
                        if (column + 2 < 8 && row + 2 < 8)
                        {
                            if (this.getTable(row + 2, column + 2) == 0)
                            {
                                this.setTable(row + 2, column + 2, this.turn * 11111);
                                if (this.getTable(row + 1, column + 1) == 2 * 1)
                                    this.setTable(row + 1, column + 1, 2 * 111);
                                else if (this.getTable(row + 1, column + 1) == 2 * 11)
                                    this.setTable(row + 1, column + 1, 2 * 1111);
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
                    if (this.getTable(row - 1, column - 1) == 0 && !eating)
                        this.setTable(row - 1, column - 1, this.turn * 11111);
                    else if (this.getTable(row - 1, column - 1) == 1 ||
                        this.getTable(row - 1, column - 1) == 11)
                    {
                        if (column - 2 >= 0 && row - 2 >= 0)
                        {
                            if (this.getTable(row - 2, column - 2) == 0)
                            {
                                this.setTable(row - 2, column - 2, this.turn * 11111);
                                if (this.getTable(row - 1, column - 1) == 1)
                                    this.setTable(row - 1, column - 1, 111);
                                else if (this.getTable(row - 1, column - 1) == 11)
                                    this.setTable(row - 1, column - 1, 1111);
                                chipOptions(row - 2, column - 2, true);
                            }
                        }
                    }
                }
                if (column + 1 < 8 && row - 1 >= 0)
                {
                    if (this.getTable(row - 1, column + 1) == 0 && !eating)
                        this.setTable(row - 1, column + 1, this.turn * 11111);
                    else if (this.getTable(row - 1, column + 1) == 1 ||
                        this.getTable(row - 1, column + 1) == 11)
                    {
                        if (column + 2 < 8 && row - 2 >= 0)
                        {
                            if (this.getTable(row - 2, column + 2) == 0)
                            {
                                this.setTable(row - 2, column + 2, this.turn * 11111);
                                if (this.getTable(row - 1, column + 1) == 1)
                                    this.setTable(row - 1, column + 1, 111);
                                else if (this.getTable(row - 1, column + 1) == 11)
                                    this.setTable(row - 1, column + 1, 1111);
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
                                if (this.getTable(row + i, column + i) == 0)
                                    this.setTable(row + i, column + i, this.turn * 111111);
                            }
                            else stop = true;
                            break;
                        case 1:
                            if (row + i < 8 && column - i >= 0)
                            {
                                if (this.getTable(row + i, column - i) == 0)
                                    this.setTable(row + i, column - i, this.turn * 111111);
                            }
                            else stop = true;
                            break;
                        case 2:
                            if (row - i >= 0 && column + i < 8)
                            {
                                if (this.getTable(row - i, column + i) == 0)
                                    this.setTable(row - i, column + i, this.turn * 111111);
                            }
                            else stop = true;
                            break;
                        case 3:
                            if (row - i >= 0 && column - i >= 0)
                            {
                                if (this.getTable(row - i, column - i) == 0)
                                    this.setTable(row - i, column - i, this.turn * 111111);
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
                    if (this.getTable(i, j) > 10000) this.setTable(i, j, 0);
                    else if (this.getTable(i, j) == 111) this.setTable(i, j, 1);
                    else if (this.getTable(i, j) == 1111) this.setTable(i, j, 11);
                    else if (this.getTable(i, j) == 2 * 111) this.setTable(i, j, 2);
                    else if (this.getTable(i, j) == 2 * 1111) this.setTable(i, j, 22);
                }
        }
    }
}