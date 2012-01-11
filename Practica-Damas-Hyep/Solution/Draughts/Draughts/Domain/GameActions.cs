using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Draughts.Presentation;

namespace Draughts.Domain
{
    public class GameActions : Subject
    {
        public List<Observer> observers = new List<Observer>();
        private int row, column, state;
        private int rowSelected, columnSelected;

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

        public int getTurn()
        {
            return this.turn;
        }
        public void setTurn(int t)
        {
            this.turn = t;
            turnHasChanged();
        }

        private Boolean finish;
        private Boolean selected;
        private Boolean move;

        public GameActions(InitWin init, Player pl1, Player pl2)
        {
            this.pl1 = pl1;
            this.pl2 = pl2;
            this.turn = 1;
            this.finish = false;
            this.selected = false;
            this.move = false;
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

        private void switchTurn()
        {
            for (int i = 0; i < observers.Count; i++)
            {
                Observer obs = (Observer)observers[i];
                obs.notify(this.turn);
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

        private void turnHasChanged()
        {
            //Ejecutamos el método creado anteriormente mediante el delegado para notificar a 
            //todos los observadores del cambio sucedido en el comboBox
            DelegateOfTheTurn oChangeTurn = new DelegateOfTheTurn();
            DelegateOfTheTurn.TurnDelegate oTurnDelegate = new DelegateOfTheTurn.TurnDelegate(switchTurn);
            oChangeTurn.switchTurn += oTurnDelegate;
            oChangeTurn.changeContentsTurn = state;

            oChangeTurn.switchTurn -= oTurnDelegate;
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
                        this.rowSelected = row;
                        this.columnSelected = column;
                        selected = true;
                    }
                }
                else
                {
                    int result = doPlay(row, column);
                    selected = false;
                    if (result > -1)   // Jugada válida
                    {
                        if (result != 0)    // Partida finalizada
                        {
                            this.finish = true;
                            this.setTurn(result * 11); // 11 Gana jug1, 22 Gana jug2, 33 Empate
                        }
                        else
                        {
                            if (this.getTurn() == 1) this.setTurn(2);
                            else this.setTurn(1);
                        }
                    }
                    else resetOptions();
                }
            }
        }

        public int doPlay(int row, int column)
        {
            int result = -1;
            if (this.getTable(row, column) == this.turn * 11111)   // Movimiento de una ficha
            {
                this.setTable(this.rowSelected, this.columnSelected, 0);
                if (row != 0 && row != 7) this.setTable(row, column, this.turn * 1);
                else this.setTable(row, column, this.turn * 11);  // La ficha ha coronado
                this.move = false;
                for (int i = 0; i < 4; i++)
                    if (movingPiece(this.rowSelected, this.columnSelected, row, column, i)) break;
                result = 0;
            }
            else if (this.getTable(row, column) == this.turn * 111111) // Movimiento de la reina
            {
                this.setTable(this.rowSelected, this.columnSelected, 0);
                this.setTable(row, column, this.turn * 11);
                this.move = false;
                for (int i = 0; i < 4; i++)
                    if (movingPiece(this.rowSelected, this.columnSelected, row, column, i)) break;
                result = 0;
            }
            if (result == 0)    // Después de un movimiento se eliminan las otras opciones
            {
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                    {
                        if (this.getTable(i, j) >= 11111) this.setTable(i, j, 0);
                        else if (this.getTable(i, j) == 111) this.setTable(i, j, 1);
                        else if (this.getTable(i, j) == 1111) this.setTable(i, j, 11);
                        else if (this.getTable(i, j) == 222) this.setTable(i, j, 2);
                        else if (this.getTable(i, j) == 2222) this.setTable(i, j, 22);
                    }
                result = checkTable();
            }
            return result;
        }

        private int checkTable()
        {
            int result = 0;
            int rP = 0, rQ = 0, wP = 0, wQ = 0;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    switch (this.getTable(i, j))
                    {
                        case 1: rP++; break;
                        case 2: wP++; break;
                        case 11: rQ++; break;
                        case 22: wQ++; break;
                        default: break;
                    }
            if ((rP > 0 || rQ > 0) && wP == 0 && wQ == 0) result = 1;
            else if ((wP > 0 || wQ > 0) && rP == 0 && rQ == 0) result = 2;
            else if (rP == 0 && wP == 0 && rQ == 1 && wQ == 1) result = 3;
            return result;
        }

        private Boolean calculateOptions(int row, int column)
        {
            if (this.getTable(row, column) == this.turn)   // Ficha normal
            {
                this.setTable(row, column, this.turn * 111);
                chipOptions(row, column, false);
                return true;
            }
            else if (this.getTable(row, column) == this.turn * 11)   // Reina
            {
                int enemy = 0;
                if (this.turn == 1) enemy = 2; else enemy = 1;
                this.setTable(row, column, this.turn * 1111);
                queenOptions(row, column, enemy, false, -1);
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

        private void queenOptions(int row, int column, int enemy, Boolean eating, int src)
        {
            for (int dir = 0; dir < 4; dir++)
            {
                Boolean stop = false;
                for (int i = 0; i < 8 && !stop; i++)
                {
                    switch (dir)
                    {
                        case 0:
                            if (row + i < 8 && column + i < 8 && src != dir)
                            {
                                if (this.getTable(row + i, column + i) == 0)
                                    this.setTable(row + i, column + i, this.turn * 111111);
                                else if (this.getTable(row + i, column + i) == this.turn ||
                                    this.getTable(row + i, column + i) == this.turn * 11)
                                    stop = true;
                                else if (this.getTable(row + i, column + i) == enemy * 1 ||
                                    this.getTable(row + i, column + i) == enemy * 11)
                                {
                                    if (row + i + 1 < 8 && column + i + 1 < 8)
                                    {
                                        if (this.getTable(row + i + 1, column + i + 1) == 0)
                                        {
                                            if (this.getTable(row + i, column + i) == enemy * 1 ||
                                                this.getTable(row + i, column + i) == enemy * 11)
                                            {
                                                if (this.getTable(row + i, column + i) == enemy * 1)
                                                    this.setTable(row + i, column + i, enemy * 111);
                                                else this.setTable(row + i, column + i, enemy * 1111);
                                                this.setTable(row + i + 1, column + i + 1, this.turn * 111111);
                                                stop = true;
                                                for (int j = 0; j < 4; j++)
                                                    if (j != 3) queenWhoEatsOptions(row + i + 1, column + i + 1, enemy, j);
                                            }
                                        }
                                        else stop = true;
                                    }
                                    else stop = true;
                                }
                            }
                            else stop = true;
                            break;
                        case 1:
                            if (row + i < 8 && column - i >= 0 && src != dir)
                            {
                                if (this.getTable(row + i, column - i) == 0)
                                    this.setTable(row + i, column - i, this.turn * 111111);
                                else if (this.getTable(row + i, column - i) == this.turn ||
                                    this.getTable(row + i, column - i) == this.turn * 11)
                                    stop = true;
                                else if (this.getTable(row + i, column - i) == enemy * 1 ||
                                    this.getTable(row + i, column - i) == enemy * 11)
                                {
                                    if (row + i + 1 < 8 && column - i - 1 >= 0)
                                    {
                                        if (this.getTable(row + i + 1, column - i - 1) == 0)
                                        {
                                            if (this.getTable(row + i, column - i) == enemy * 1 ||
                                                this.getTable(row + i, column - i) == enemy * 11)
                                            {
                                                if (this.getTable(row + i, column - i) == enemy * 1)
                                                    this.setTable(row + i, column - i, enemy * 111);
                                                else this.setTable(row + i, column - i, enemy * 1111);
                                                this.setTable(row + i + 1, column - i - 1, this.turn * 111111);
                                                stop = true;
                                                for (int j = 0; j < 4; j++)
                                                    if (j != 2) queenWhoEatsOptions(row + i + 1, column - i - 1, enemy, j);
                                            }
                                        }
                                        else stop = true;
                                    }
                                    else stop = true;
                                }
                            }
                            else stop = true;
                            break;
                        case 2:
                            if (row - i >= 0 && column + i < 8 && src != dir)
                            {
                                if (this.getTable(row - i, column + i) == 0)
                                    this.setTable(row - i, column + i, this.turn * 111111);
                                else if (this.getTable(row - i, column + i) == this.turn ||
                                    this.getTable(row - i, column + i) == this.turn * 11)
                                    stop = true;
                                else if (this.getTable(row - i, column + i) == enemy * 1 ||
                                    this.getTable(row - i, column + i) == enemy * 11)
                                {
                                    if (row - i - 1 >= 0 && column + i + 1 < 8)
                                    {
                                        if (this.getTable(row - i - 1, column + i + 1) == 0)
                                        {
                                            if (this.getTable(row - i, column + i) == enemy * 1 ||
                                                this.getTable(row - i, column + i) == enemy * 11)
                                            {
                                                if (this.getTable(row - i, column + i) == enemy * 1)
                                                    this.setTable(row - i, column + i, enemy * 111);
                                                else this.setTable(row - i, column + i, enemy * 1111);
                                                this.setTable(row - i - 1, column + i + 1, this.turn * 111111);
                                                stop = true;
                                                for (int j = 0; j < 4; j++)
                                                    if (j != 1) queenWhoEatsOptions(row - i - 1, column + i + 1, enemy, j);
                                            }
                                        }
                                        else stop = true;
                                    }
                                    else stop = true;
                                }
                            }
                            else stop = true;
                            break;
                        case 3:
                            if (row - i >= 0 && column - i >= 0 && src != dir)
                            {
                                if (this.getTable(row - i, column - i) == 0)
                                    this.setTable(row - i, column - i, this.turn * 111111);
                                else if (this.getTable(row - i, column - i) == this.turn ||
                                    this.getTable(row - i, column - i) == this.turn * 11)
                                    stop = true;
                                else if (this.getTable(row - i, column - i) == enemy * 1 ||
                                    this.getTable(row - i, column - i) == enemy * 11)
                                {
                                    if (row - i - 1 >= 0 && column - i - 1 >= 0)
                                    {
                                        if (this.getTable(row - i - 1, column - i - 1) == 0)
                                        {
                                            if (this.getTable(row - i, column - i) == enemy * 1 ||
                                                this.getTable(row - i, column - i) == enemy * 11)
                                            {
                                                if (this.getTable(row - i, column - i) == enemy * 1)
                                                    this.setTable(row - i, column - i, enemy * 111);
                                                else this.setTable(row - i, column - i, enemy * 1111);
                                                this.setTable(row - i - 1, column - i - 1, this.turn * 111111);
                                                stop = true;
                                                for (int j = 0; j < 4; j++)
                                                    if (j != 0) queenWhoEatsOptions(row - i - 1, column - i - 1, enemy, j);
                                            }
                                        }
                                        else stop = true;
                                    }
                                    else stop = true;
                                }
                            }
                            else stop = true;
                            break;
                    }
                }
            }
        }

        private Boolean movingPiece(int rowSrc, int columnSrc, int rowDst, int columnDst, int dir)
        {
            for (
                int i = dir; i <= 4 && !move; i++)
            {
                switch(i)
                {
                    case 0:
                        if (rowSrc + 1 < 8 && columnSrc - 1 >= 0)
                        {
                            switch (this.getTable(rowSrc + 1, columnSrc - 1))
                            {
                                case 0:
                                    break;
                                case 1: case 11: case 2: case 22:
                                    if (rowSrc + 1 == rowDst && columnSrc - 1 == columnDst)
                                    {
                                        this.move = true;
                                        confirmMove(); // confirmar jugada
                                    }
                                    break;
                                case 111:
                                    this.table[rowSrc + 1, columnSrc - 1] = 6;
                                    if (!movingPiece(rowSrc + 1, columnSrc - 1, rowDst, columnDst, 0)) i = 5;
                                    //movingPiece(rowSrc + 1, columnSrc - 1, rowDst, columnDst);
                                    break;
                                case 1111:
                                    this.table[rowSrc + 1, columnSrc - 1] = 7;
                                    if (!movingPiece(rowSrc + 1, columnSrc - 1, rowDst, columnDst, 0)) i = 5;
                                    //movingPiece(rowSrc + 1, columnSrc - 1, rowDst, columnDst);
                                    break;
                                case 222:
                                    this.table[rowSrc + 1, columnSrc - 1] = 8;
                                    if (!movingPiece(rowSrc + 1, columnSrc - 1, rowDst, columnDst, 0)) i = 5;
                                    //movingPiece(rowSrc + 1, columnSrc - 1, rowDst, columnDst);
                                    break;
                                case 2222:
                                    this.table[rowSrc + 1, columnSrc - 1] = 9;
                                    if (!movingPiece(rowSrc + 1, columnSrc - 1, rowDst, columnDst, 0)) i = 5;
                                    //movingPiece(rowSrc + 1, columnSrc - 1, rowDst, columnDst);
                                    break;
                                case 11111: case 111111: case 22222: case 222222:
                                    this.table[rowSrc + 1, columnSrc - 1] = 5;
                                    if (!movingPiece(rowSrc + 1, columnSrc - 1, rowDst, columnDst, 0)) i = 5;
                                    //movingPiece(rowSrc + 1, columnSrc - 1, rowDst, columnDst);
                                    break;
                                default: break;
                            }
                        }
                        break;
                    case 1:
                        if (rowSrc + 1 < 8 && columnSrc + 1 < 8)
                        {
                            switch (this.getTable(rowSrc + 1, columnSrc + 1))
                            {
                                case 0:
                                    break;
                                case 1: case 11: case 2: case 22:
                                    if (rowSrc + 1 == rowDst && columnSrc + 1 == columnDst)
                                    {
                                        this.move = true;
                                        confirmMove(); // confirmar jugada
                                    }
                                    break;
                                case 111:
                                    this.table[rowSrc + 1, columnSrc + 1] = 6;
                                    if (!movingPiece(rowSrc + 1, columnSrc + 1, rowDst, columnDst, 0)) i = 5;
                                    //movingPiece(rowSrc + 1, columnSrc + 1, rowDst, columnDst);
                                    break;
                                case 1111:
                                    this.table[rowSrc + 1, columnSrc + 1] = 7;
                                    if (!movingPiece(rowSrc + 1, columnSrc + 1, rowDst, columnDst, 0)) i = 5;
                                    //movingPiece(rowSrc + 1, columnSrc + 1, rowDst, columnDst);
                                    break;
                                case 222:
                                    this.table[rowSrc + 1, columnSrc + 1] = 8;
                                    if (!movingPiece(rowSrc + 1, columnSrc + 1, rowDst, columnDst, 0)) i = 5;
                                    //movingPiece(rowSrc + 1, columnSrc + 1, rowDst, columnDst);
                                    break;
                                case 2222:
                                    this.table[rowSrc + 1, columnSrc + 1] = 9;
                                    if (!movingPiece(rowSrc + 1, columnSrc + 1, rowDst, columnDst, 0)) i = 5;
                                    //movingPiece(rowSrc + 1, columnSrc + 1, rowDst, columnDst);
                                    break;
                                case 11111: case 111111: case 22222: case 222222:
                                    this.table[rowSrc + 1, columnSrc + 1] = 5;
                                    if (!movingPiece(rowSrc + 1, columnSrc + 1, rowDst, columnDst, 0)) i = 5;
                                    //movingPiece(rowSrc + 1, columnSrc + 1, rowDst, columnDst);
                                    break;
                                default: break;
                            }
                        }
                        break;
                    case 2:
                        if (rowSrc - 1 >= 0 && columnSrc - 1 >= 0)
                        {
                            
                            switch (this.getTable(rowSrc - 1, columnSrc - 1))
                            {
                                case 0:
                                    break;
                                case 1: case 11: case 2: case 22:
                                    if (rowSrc - 1 == rowDst && columnSrc - 1 == columnDst)
                                    {
                                        this.move = true;
                                        confirmMove(); // confirmar jugada
                                    }
                                    break;
                                case 111:
                                    this.table[rowSrc - 1, columnSrc - 1] = 6;
                                    if (!movingPiece(rowSrc - 1, columnSrc - 1, rowDst, columnDst, 0)) i = 5;
                                    //movingPiece(rowSrc - 1, columnSrc - 1, rowDst, columnDst);
                                    break;
                                case 1111:
                                    this.table[rowSrc - 1, columnSrc - 1] = 7;
                                    if (!movingPiece(rowSrc - 1, columnSrc - 1, rowDst, columnDst, 0)) i = 5;
                                    //movingPiece(rowSrc - 1, columnSrc - 1, rowDst, columnDst);
                                    break;
                                case 222:
                                    this.table[rowSrc - 1, columnSrc - 1] = 8;
                                    if (!movingPiece(rowSrc - 1, columnSrc - 1, rowDst, columnDst, 0)) i = 5;
                                    //movingPiece(rowSrc - 1, columnSrc - 1, rowDst, columnDst);
                                    break;
                                case 2222:
                                    this.table[rowSrc - 1, columnSrc - 1] = 9;
                                    if (!movingPiece(rowSrc - 1, columnSrc - 1, rowDst, columnDst, 0)) i = 5;
                                    //movingPiece(rowSrc - 1, columnSrc - 1, rowDst, columnDst);
                                    break;
                                case 11111: case 111111: case 22222: case 222222:
                                    this.table[rowSrc - 1, columnSrc - 1] = 5;
                                    if (!movingPiece(rowSrc - 1, columnSrc - 1, rowDst, columnDst, 0)) i = 5;
                                    //movingPiece(rowSrc - 1, columnSrc - 1, rowDst, columnDst);
                                    break;
                                default: break;
                            }
                        }
                        break;
                    case 3:
                        if (rowSrc - 1 >= 0 && columnSrc + 1 < 8)
                        {
                            switch (this.getTable(rowSrc - 1, columnSrc + 1))
                            {
                                case 0:
                                    break;
                                case 1: case 11: case 2: case 22:
                                    if (rowSrc - 1 == rowDst && columnSrc + 1 == columnDst)
                                    {
                                        this.move = true;
                                        confirmMove(); // confirmar jugada
                                    }
                                    break;
                                case 111:
                                    this.table[rowSrc - 1, columnSrc + 1] = 6;
                                    if (!movingPiece(rowSrc - 1, columnSrc + 1, rowDst, columnDst, 0)) i = 5;
                                    //movingPiece(rowSrc - 1, columnSrc + 1, rowDst, columnDst);
                                    break;
                                case 1111:
                                    this.table[rowSrc - 1, columnSrc + 1] = 7;
                                    if (!movingPiece(rowSrc - 1, columnSrc + 1, rowDst, columnDst, 0)) i = 5;
                                    //movingPiece(rowSrc - 1, columnSrc + 1, rowDst, columnDst);
                                    break;
                                case 222:
                                    this.table[rowSrc - 1, columnSrc + 1] = 8;
                                    if (!movingPiece(rowSrc - 1, columnSrc + 1, rowDst, columnDst, 0)) i = 5;
                                    //movingPiece(rowSrc - 1, columnSrc + 1, rowDst, columnDst);
                                    break;
                                case 2222:
                                    this.table[rowSrc - 1, columnSrc + 1] = 9;
                                    if (!movingPiece(rowSrc - 1, columnSrc + 1, rowDst, columnDst, 0)) i = 5;
                                    //movingPiece(rowSrc - 1, columnSrc + 1, rowDst, columnDst);
                                    break;
                                case 11111: case 111111: case 22222: case 222222:
                                    this.table[rowSrc - 1, columnSrc + 1] = 5;
                                    if (!movingPiece(rowSrc - 1, columnSrc + 1, rowDst, columnDst, 0)) i = 5;
                                    //movingPiece(rowSrc - 1, columnSrc + 1, rowDst, columnDst);
                                    break;
                                default: break;
                            }
                        }
                        break;
                    case 4:
                        resetMove(); // resetear tablero
                        break;
                    default:
                        break;
                }
            }
            return move;
        }

        private Boolean queenWhoEatsOptions(int row, int column, int enemy, int dir)
        {
            int j = 0;
            Boolean move = false;
            Boolean continue_ = true;
            while (continue_)
            {
                continue_ = false;
                switch (dir)
                {
                    case 0:
                        if (row + j + 1 < 8 && column + j + 1 < 8)
                        {
                            if (this.getTable(row + j + 1, column + j + 1) == 0)
                            {
                                this.table[row + j + 1, column + j + 1] = 5;
                                continue_ = true;
                                j++;
                            }
                            else if (this.getTable(row + j + 1, column + j + 1) == enemy * 1 ||
                                this.getTable(row + j + 1, column + j + 1) == enemy * 11)
                            {
                                if (row + j + 2 < 8 && column + j + 2 < 8)
                                {
                                    if (this.getTable(row + j + 2, column + j + 2) == 0)
                                    {
                                        this.table[row + j + 2, column + j + 2] = 5;
                                        if (this.getTable(row + j + 1, column + j + 1) == enemy * 1)
                                        {
                                            if (this.turn == 1) this.table[row + j + 1, column + j + 1] = 8;
                                            else this.table[row + j + 1, column + j + 1] = 6;
                                        }
                                        else
                                        {
                                            if (this.turn == 1) this.table[row + j + 1, column + j + 1] = 9;
                                            else this.table[row + j + 1, column + j + 1] = 7;
                                        }
                                        confirmQueenOptions();
                                        move = true;
                                        for (int i = 0; i < 4; i++)
                                            if (i != 3) queenWhoEatsOptions(row + j + 2, column + j + 2, enemy, i);
                                    }
                                }
                            }
                        }
                        break;
                    case 1:
                        if (row + j + 1 < 8 && column - j - 1 >= 0)
                        {
                            if (this.getTable(row + j + 1, column - j - 1) == 0)
                            {
                                this.table[row + j + 1, column - j - 1] = 5;
                                continue_ = true;
                                j++;
                            }
                            else if (this.getTable(row + j + 1, column - j - 1) == enemy * 1 ||
                                this.getTable(row + j + 1, column - j - 1) == enemy * 11)
                            {
                                if (row + j + 2 < 8 && column - j - 2 >= 0)
                                {
                                    if (this.getTable(row + j + 2, column - j - 2) == 0)
                                    {
                                        this.table[row + j + 2, column - j - 2] = 5;
                                        if (this.getTable(row + j + 1, column - j - 1) == enemy * 1)
                                        {
                                            if (this.turn == 1) this.table[row + j + 1, column - j - 1] = 8;
                                            else this.table[row + j + 1, column - j - 1] = 6;
                                        }
                                        else
                                        {
                                            if (this.turn == 1) this.table[row + j + 1, column - j - 1] = 9;
                                            else this.table[row + j + 1, column - j - 1] = 7;
                                        }
                                        confirmQueenOptions();
                                        move = true;
                                        for (int i = 0; i < 4; i++)
                                            if (i != 2) queenWhoEatsOptions(row + j + 2, column - j - 2, enemy, i);
                                    }
                                }
                            }
                        }
                        break;
                    case 2:
                        if (row - j - 1 >= 0 && column + j + 1 < 8)
                        {
                            if (this.getTable(row - j - 1, column + j + 1) == 0)
                            {
                                this.table[row - j - 1, column + j + 1] = 5;
                                continue_ = true;
                                j++;
                            }
                            else if (this.getTable(row - j - 1, column + j + 1) == enemy * 1 ||
                                this.getTable(row - j - 1, column + j + 1) == enemy * 11)
                            {
                                if (row - j - 2 >= 0 && column + j + 2 < 8)
                                {
                                    if (this.getTable(row - j - 2, column + j + 2) == 0)
                                    {
                                        this.table[row - j - 2, column + j + 2] = 5;
                                        if (this.getTable(row - j - 1, column + j + 1) == enemy * 1)
                                        {
                                            if (this.turn == 1) this.table[row - j - 1, column + j + 1] = 8;
                                            else this.table[row - j - 1, column + j + 1] = 6;
                                        }
                                        else
                                        {
                                            if (this.turn == 1) this.table[row - j - 1, column + j + 1] = 9;
                                            else this.table[row - j - 1, column + j + 1] = 7;
                                        }
                                        confirmQueenOptions();
                                        move = true;
                                        for (int i = 0; i < 4; i++)
                                            if (i != 1) queenWhoEatsOptions(row - j - 2, column + j + 2, enemy, i);
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        if (row - j - 1 >= 0 && column - j - 1 >= 0)
                        {
                            if (this.getTable(row - j - 1, column - j - 1) == 0)
                            {
                                this.table[row - j - 1, column - j - 1] = 5;
                                continue_ = true;
                                j++;
                            }
                            else if (this.getTable(row - j - 1, column - j - 1) == enemy * 1 ||
                                this.getTable(row - j - 1, column - j - 1) == enemy * 11)
                            {
                                if (row - j - 2 >= 0 && column - j - 2 >= 0)
                                {
                                    if (this.getTable(row - j - 2, column - j - 2) == 0)
                                    {
                                        this.table[row - j - 2, column - j - 2] = 5;
                                        if (this.getTable(row - j - 1, column - j - 1) == enemy * 1)
                                        {
                                            if (this.turn == 1) this.table[row - j - 1, column - j - 1] = 8;
                                            else this.table[row - j - 1, column - j - 1] = 6;
                                        }
                                        else
                                        {
                                            if (this.turn == 1) this.table[row - j - 1, column - j - 1] = 9;
                                            else this.table[row - j - 1, column - j - 1] = 7;
                                        }
                                        confirmQueenOptions();
                                        move = true;
                                        for (int i = 0; i < 4; i++)
                                            if (i != 0) queenWhoEatsOptions(row - j - 2, column - j - 2, enemy, i);
                                    }
                                }
                            }
                        }
                        break;
                    default: break;
                }
            }
            if (!move) resetQueenOptions();
            return move;
        }

        private void resetOptions()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (this.getTable(i, j) >= 11111) this.setTable(i, j, 0);
                    else if (this.getTable(i, j) == 111) this.setTable(i, j, 1);
                    else if (this.getTable(i, j) == 1111) this.setTable(i, j, 11);
                    else if (this.getTable(i, j) == 2 * 111) this.setTable(i, j, 2);
                    else if (this.getTable(i, j) == 2 * 1111) this.setTable(i, j, 22);
                }
        }

        private void confirmMove()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    int content = this.getTable(i, j);
                    if (content == 5 || content == 6 || content == 7 || content == 8 || content == 9)
                        this.setTable(i, j, 0);
                }
        }

        private void resetMove()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    switch (this.getTable(i, j))
                    {
                        case 5:
                            int piece = this.getTable(this.rowSelected, this.columnSelected);
                            if (piece == 111) this.table[i, j] = 11111;
                            else if (piece == 1111) this.table[i, j] = 111111;
                            else if (piece == 222) this.table[i, j] = 22222;
                            else if (piece == 2222) this.table[i, j] = 222222;
                            break;
                        case 6: this.table[i, j] = 111; break;
                        case 7: this.table[i, j] = 1111; break;
                        case 8: this.table[i, j] = 222; break;
                        case 9: this.table[i, j] = 2222; break;
                        default: break;
                    }
                }
        }

        private void confirmQueenOptions()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    switch (this.getTable(i, j))
                    {
                        case 5:
                            if (this.turn == 1) this.setTable(i, j, 111111);
                            else this.setTable(i, j, 222222);
                            break;
                        case 6: this.setTable(i, j, 111); break;
                        case 7: this.setTable(i, j, 1111); break;
                        case 8: this.setTable(i, j, 222); break;
                        case 9: this.setTable(i, j, 2222); break;
                        default: break;
                    }
                }
        }
        
        private void resetQueenOptions()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (this.getTable(i, j) == 5)
                        this.table[i, j] = 0;
                }
        }
    }
}