using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Draughts.Presentation;
using System.Threading;
using System.Collections;
using Draughts.Communications;

namespace Draughts.Domain
{
    /// <summary>
    /// Implementa la lógica de juego de una partida de Damas.
    /// </summary>
    public class GameActions : Subject
    {
        /// <summary>
        /// Lista de observadores para ver los cambios en la interfaz.
        /// </summary>
        public List<Observer> observers = new List<Observer>();
        /// <summary>
        /// Atributos de fila, columna y estado de la casilla.
        /// </summary>
        private int row, column, state;
        /// <summary>
        /// Fila y columna seleccionada.
        /// </summary>
        private int rowSelected, columnSelected;

        /// <summary>
        /// Jugadores de la partida.
        /// </summary>
        private Player pl1, pl2;
        /// <summary>
        /// Devuelve o modifica el jugador 1.
        /// </summary>
        /// <value>
        /// Player.
        /// </value>
        public Player Pl1
        {
            get { return pl1; }
            set { pl1 = value; }
        }

        /// <summary>
        /// Devuelve o modifica el jugador 2.
        /// </summary>
        /// <value>
        /// Player.
        /// </value>
        public Player Pl2
        {
            get { return pl2; }
            set { pl2 = value; }
        }

        /// <summary>
        /// Tablero de juego.
        /// </summary>
        private int[,] table;

        /// <summary>
        /// Devuelve el contenido de una casilla.
        /// </summary>
        /// <param name="row">Fila.</param>
        /// <param name="column">Columna.</param>
        /// <returns>Contenido de la casilla.</returns>
        public int getTable(int row, int column)
        {
            return this.table[row, column];
        }
        /// <summary>
        /// Modifica el contenido de una casilla.
        /// </summary>
        /// <param name="row">Fila.</param>
        /// <param name="column">Columna.</param>
        /// <param name="state">Estado.</param>
        public void setTable(int row, int column, int state)
        {
            this.table[row, column] = state;
            this.row = row;
            this.column = column;
            this.state = state;
            if (!this.cpuTime) tableValuesHaveChanged();
        }

        /// <summary>
        /// Turno de juego.
        /// </summary>
        private int turn;

        /// <summary>
        /// Devuelve el turno.
        /// </summary>
        /// <returns>Turno.</returns>
        public int getTurn()
        {
            return this.turn;
        }
        /// <summary>
        /// Modifica el turno de juego.
        /// </summary>
        /// <param name="t">Turno.</param>
        public void setTurn(int t)
        {
            this.turn = t;
            turnHasChanged();
        }

        /// <summary>
        /// Final de la partida.
        /// </summary>
        private Boolean finish;
        /// <summary>
        /// Casilla seleccionada.
        /// </summary>
        private Boolean selected;
        /// <summary>
        /// Movimiento de una ficha.
        /// </summary>
        private Boolean move;
        /// <summary>
        /// Juegas contra la CPU.
        /// </summary>
        private Boolean cpuTime;
        /// <summary>
        /// Jugar en red.
        /// </summary>
        private Boolean netGame;

        /// <summary>
        /// Devuelve o modifica el valor de netGame.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [net game]; otherwise, <c>false</c>.
        /// </value>
        public Boolean NetGame
        {
            get { return netGame; }
            set { netGame = value; }
        }
        /// <summary>
        /// Saber si eres el enemigo o no.
        /// </summary>
        private Boolean enemyMode;

        /// <summary>
        /// Devuelve o modifica el valor de enemyMode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enemy mode]; otherwise, <c>false</c>.
        /// </value>
        public Boolean EnemyMode
        {
            get { return enemyMode; }
            set { enemyMode = value; }
        }
        /// <summary>
        /// Profundidad de procesamiento de la CPU.
        /// </summary>
        const int cpuDepth = 1;
        /// <summary>
        /// Instancia de tipo GameAdmin.
        /// </summary>
        private GameAdmin gAdmin = GameAdmin.Instance;
        /// <summary>
        /// Instancia de tipo NetMode.
        /// </summary>
        private NetMode net = NetMode.Instance;
        /// <summary>
        /// Constructor de la clase <see cref="GameActions"/>.
        /// </summary>
        /// <param name="init">InitWin.</param>
        /// <param name="pl1">Player1.</param>
        /// <param name="pl2">Player2.</param>
        public GameActions(InitWin init, Player pl1, Player pl2)
        {
            this.pl1 = pl1;
            this.pl2 = pl2;
            this.turn = 1;
            this.finish = false;
            this.selected = false;
            this.move = false;
            this.cpuTime = false;
            this.netGame = false;
            this.enemyMode = false;
            GameWin game = new GameWin(init, this, this);
            game.Show();
            initTable();
        }

        // método que inicializa el contenido de las casillas del tablero
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

        /* 
         * --- MÉTODOS DEL OBSERVADOR ---
         */
        // implementación de la interfaz
        public void registerInterest(Observer obs)
        {
            // Agrega un objeto Observer a la colección
            observers.Add(obs);
        }

        // método que recorre la colección de Observers para avisar de un cambio en una casilla
        private void switchBox()
        {
            for (int i = 0; i < observers.Count; i++)
            {
                Observer obs = (Observer)observers[i];
                obs.notify(this.row, this.column, this.state);
            }
        }

        // método que recorre la colección de Observers para avisar de un cambio de turno
        private void switchTurn()
        {
            for (int i = 0; i < observers.Count; i++)
            {
                Observer obs = (Observer)observers[i];
                obs.notify(this.turn);
            }
        }

        // método que crea el delegado del observador de las casillas del tablero
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

        // método que crea el delegado del observador de los cambios de turno
        private void turnHasChanged()
        {
            // Se ejecuta el método creado anteriormente mediante el delegado para notificar a 
            // todos los observadores del cambio sucedido en el comboBox
            DelegateOfTheTurn oChangeTurn = new DelegateOfTheTurn();
            DelegateOfTheTurn.TurnDelegate oTurnDelegate = new DelegateOfTheTurn.TurnDelegate(switchTurn);
            oChangeTurn.switchTurn += oTurnDelegate;
            oChangeTurn.changeContentsTurn = state;

            oChangeTurn.switchTurn -= oTurnDelegate;
        }

        /*
         * --- MÉTODOS DE LA LÓGICA DE JUEGO ---
         */
        public void selectedBox(int row, int column, Boolean thisUser)
        {
            if (!this.finish)
            {
                if (this.NetGame)
                {
                    if ((thisUser && this.turn == gAdmin.PlayerNumber) ||
                        (!thisUser))
                    {
                        if (!selected) selecting(row, column);
                        else moving(row, column, thisUser);
                    }
                }
                else
                {
                    if (!selected) selecting(row, column);
                    else moving(row, column, thisUser);
                }
            }
        }

        private void selecting(int row, int column)
        {
            if (calculateOptions(row, column))  // Se muestran las opciones para esa pieza
            {
                this.rowSelected = row;
                this.columnSelected = column;
                selected = true;
            }
        }

        private void moving(int row, int column, Boolean thisUser)
        {
            int result = doPlay(row, column);
            selected = false;
            if (result > -1)   // Jugada válida
            {
                // Si procede, se envía la jugada al jugador remoto.
                if (this.NetGame)
                {
                    if (thisUser && gAdmin.PlayerNumber == this.turn)
                    {
                        string coordinates = "#&" + this.rowSelected + "&" + this.columnSelected + "&" + row + "&" + column;
                        net.sendMsg(coordinates);
                    }
                }
                if (result != 0)    // Partida finalizada
                {
                    this.finish = true;
                    this.setTurn(result * 11); // 11 Gana jug1, 22 Gana jug2, 33 Empate
                    if (this.NetGame)
                    {
                        if (result == 33) gAdmin.Pl.Draws++;
                        else if (result == gAdmin.PlayerNumber) gAdmin.Pl.Wins++;
                        else gAdmin.Pl.Loses++;
                        gAdmin.updatePlayer();
                    }
                }
                else
                {
                    if (this.getTurn() == 1)
                    {
                        this.setTurn(2);
                        if (this.pl2.Avatar == "cpu.png")   // Si el jug2 es la CPU
                        {
                            this.cpuTime = true;
                            this.turnOfTheCPU();
                        }
                    }
                    else if (this.getTurn() == 2) this.setTurn(1);
                }
            }
            else resetOptions();
        }

        // método que resetea las opciones para una ficha
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

        // método que realiza el movimiento de una ficha
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

        // método que comprueba si hay ganador
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
            if ((rP > 0 || rQ > 0) && wP == 0 && wQ == 0) result = 1;   // Ganan rojas
            else if ((wP > 0 || wQ > 0) && rP == 0 && rQ == 0) result = 2;  // Ganan blancas
            else if (rP == 0 && wP == 0 && rQ == 1 && wQ == 1) result = 3;  // Tablas
            else if (strangled()) result = this.turn; // Si el contrario está ahogado pierde
            return result;
        }

        private Boolean strangled()
        {
            this.turn = anotherPlayer(this.turn);
            // Se calculan todas las opciones posibles
            ArrayList movements = new ArrayList();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (this.getTable(i, j) == this.turn * 1 || this.getTable(i, j) == this.turn * 11)
                    {
                        calculateOptions(i, j);
                        for (int k = 0; k < 8; k++)
                            for (int l = 0; l < 8; l++)
                            {
                                if (this.getTable(k, l) == this.turn * 11111 ||
                                    this.getTable(k, l) == this.turn * 111111)
                                    movements.Add(new Movement(i, j, k, l));
                            }
                        resetOptions();
                    }
                }
            this.turn = anotherPlayer(this.turn);
            if (movements.Count == 0)
                return true;
            else return false;
        }

        // método que calcula las opciones de movimiento de una ficha
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

        // método que define los movimientos posibles de una ficha normal
        private void chipOptions(int row, int column, Boolean eating)
        {
            if (this.turn == 1) // Opciones para las fichas del jug1
            {
                if (column - 1 >= 0 && row + 1 < 8)
                {
                    if (this.getTable(row + 1, column - 1) == 0 && !eating)
                    {
                        this.setTable(row + 1, column - 1, this.turn * 11111);
                    }
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
            else if (this.turn == 2)    // Opciones para las fichas del jug2
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

        // método que define los movimientos posibles de una ficha reina
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

        // método que calcula la trayectoria que sigue una pieza desde una casilla origen hasta una casilla destino
        private Boolean movingPiece(int rowSrc, int columnSrc, int rowDst, int columnDst, int dir)
        {
            for (int i = dir; i <= 4 && !move; i++)
            {
                switch (i)
                {
                    case 0:
                        if (rowSrc + 1 < 8 && columnSrc - 1 >= 0)
                        {
                            switch (this.getTable(rowSrc + 1, columnSrc - 1))
                            {
                                case 0:
                                    break;
                                case 1:
                                case 11:
                                case 2:
                                case 22:
                                    if (rowSrc + 1 == rowDst && columnSrc - 1 == columnDst)
                                    {
                                        this.move = true;
                                        confirmMove(); // confirmar jugada
                                    }
                                    break;
                                case 111:
                                    this.table[rowSrc + 1, columnSrc - 1] = 6;
                                    if (!movingPiece(rowSrc + 1, columnSrc - 1, rowDst, columnDst, 0)) i = 5;
                                    break;
                                case 1111:
                                    this.table[rowSrc + 1, columnSrc - 1] = 7;
                                    if (!movingPiece(rowSrc + 1, columnSrc - 1, rowDst, columnDst, 0)) i = 5;
                                    break;
                                case 222:
                                    this.table[rowSrc + 1, columnSrc - 1] = 8;
                                    if (!movingPiece(rowSrc + 1, columnSrc - 1, rowDst, columnDst, 0)) i = 5;
                                    break;
                                case 2222:
                                    this.table[rowSrc + 1, columnSrc - 1] = 9;
                                    if (!movingPiece(rowSrc + 1, columnSrc - 1, rowDst, columnDst, 0)) i = 5;
                                    break;
                                case 11111:
                                case 111111:
                                case 22222:
                                case 222222:
                                    this.table[rowSrc + 1, columnSrc - 1] = 5;
                                    if (!movingPiece(rowSrc + 1, columnSrc - 1, rowDst, columnDst, 0)) i = 5;
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
                                case 1:
                                case 11:
                                case 2:
                                case 22:
                                    if (rowSrc + 1 == rowDst && columnSrc + 1 == columnDst)
                                    {
                                        this.move = true;
                                        confirmMove(); // confirmar jugada
                                    }
                                    break;
                                case 111:
                                    this.table[rowSrc + 1, columnSrc + 1] = 6;
                                    if (!movingPiece(rowSrc + 1, columnSrc + 1, rowDst, columnDst, 0)) i = 5;
                                    break;
                                case 1111:
                                    this.table[rowSrc + 1, columnSrc + 1] = 7;
                                    if (!movingPiece(rowSrc + 1, columnSrc + 1, rowDst, columnDst, 0)) i = 5;
                                    break;
                                case 222:
                                    this.table[rowSrc + 1, columnSrc + 1] = 8;
                                    if (!movingPiece(rowSrc + 1, columnSrc + 1, rowDst, columnDst, 0)) i = 5;
                                    break;
                                case 2222:
                                    this.table[rowSrc + 1, columnSrc + 1] = 9;
                                    if (!movingPiece(rowSrc + 1, columnSrc + 1, rowDst, columnDst, 0)) i = 5;
                                    break;
                                case 11111:
                                case 111111:
                                case 22222:
                                case 222222:
                                    this.table[rowSrc + 1, columnSrc + 1] = 5;
                                    if (!movingPiece(rowSrc + 1, columnSrc + 1, rowDst, columnDst, 0)) i = 5;
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
                                case 1:
                                case 11:
                                case 2:
                                case 22:
                                    if (rowSrc - 1 == rowDst && columnSrc - 1 == columnDst)
                                    {
                                        this.move = true;
                                        confirmMove(); // confirmar jugada
                                    }
                                    break;
                                case 111:
                                    this.table[rowSrc - 1, columnSrc - 1] = 6;
                                    if (!movingPiece(rowSrc - 1, columnSrc - 1, rowDst, columnDst, 0)) i = 5;
                                    break;
                                case 1111:
                                    this.table[rowSrc - 1, columnSrc - 1] = 7;
                                    if (!movingPiece(rowSrc - 1, columnSrc - 1, rowDst, columnDst, 0)) i = 5;
                                    break;
                                case 222:
                                    this.table[rowSrc - 1, columnSrc - 1] = 8;
                                    if (!movingPiece(rowSrc - 1, columnSrc - 1, rowDst, columnDst, 0)) i = 5;
                                    break;
                                case 2222:
                                    this.table[rowSrc - 1, columnSrc - 1] = 9;
                                    if (!movingPiece(rowSrc - 1, columnSrc - 1, rowDst, columnDst, 0)) i = 5;
                                    break;
                                case 11111:
                                case 111111:
                                case 22222:
                                case 222222:
                                    this.table[rowSrc - 1, columnSrc - 1] = 5;
                                    if (!movingPiece(rowSrc - 1, columnSrc - 1, rowDst, columnDst, 0)) i = 5;
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
                                case 1:
                                case 11:
                                case 2:
                                case 22:
                                    if (rowSrc - 1 == rowDst && columnSrc + 1 == columnDst)
                                    {
                                        this.move = true;
                                        confirmMove(); // confirmar jugada
                                    }
                                    break;
                                case 111:
                                    this.table[rowSrc - 1, columnSrc + 1] = 6;
                                    if (!movingPiece(rowSrc - 1, columnSrc + 1, rowDst, columnDst, 0)) i = 5;
                                    break;
                                case 1111:
                                    this.table[rowSrc - 1, columnSrc + 1] = 7;
                                    if (!movingPiece(rowSrc - 1, columnSrc + 1, rowDst, columnDst, 0)) i = 5;
                                    break;
                                case 222:
                                    this.table[rowSrc - 1, columnSrc + 1] = 8;
                                    if (!movingPiece(rowSrc - 1, columnSrc + 1, rowDst, columnDst, 0)) i = 5;
                                    break;
                                case 2222:
                                    this.table[rowSrc - 1, columnSrc + 1] = 9;
                                    if (!movingPiece(rowSrc - 1, columnSrc + 1, rowDst, columnDst, 0)) i = 5;
                                    break;
                                case 11111:
                                case 111111:
                                case 22222:
                                case 222222:
                                    this.table[rowSrc - 1, columnSrc + 1] = 5;
                                    if (!movingPiece(rowSrc - 1, columnSrc + 1, rowDst, columnDst, 0)) i = 5;
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

        // método que confirma el movimiento de una pieza
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

        // método que anula el movimiento de una pieza
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

        // método que calcula la trayectoria de una ficha reina mientras come
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

        // método que confirma el movimiento de una reina
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

        // método que anula el movimiento de una reina
        private void resetQueenOptions()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (this.getTable(i, j) == 5)
                        this.table[i, j] = 0;
                }
        }

        /*
         * --- MÉTODOS DE LA IA DEL JUGADOR CPU ---
         */
        // método que genera cada posible jugada de la CPU
        private void cpuSelectedBox(int row, int column)
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
                }
            }
        }

        // método que gestiona el turno de juego del jugador CPU
        private void turnOfTheCPU()
        {
            // Se calculan todas las opciones posibles
            ArrayList movements = new ArrayList();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (this.getTable(i, j) == this.turn * 1 || this.getTable(i, j) == this.turn * 11)
                    {
                        calculateOptions(i, j);
                        for (int k = 0; k < 8; k++)
                            for (int l = 0; l < 8; l++)
                            {
                                if (this.getTable(k, l) == this.turn * 11111 ||
                                    this.getTable(k, l) == this.turn * 111111)
                                    movements.Add(new Movement(i, j, k, l));
                            }
                        resetOptions();
                    }
                }
            // Se guarda la situación actual del tablero
            int [,] oldTable = saveTable();
            // Y se calcula el valor de cada movimiento
            int alpha = -9999;
            int beta = 9999;
            int bestValue = alpha;
            if (movements.Count == 0)
            {
                this.finish = true;
                this.setTurn(11);   // Si CPU no puede mover, gana jug1
            }
            else
            {
                Movement bestMove = (Movement)movements[0];
                foreach (Movement movement in movements)
                {
                    loadTable(oldTable);    // Se carga la tabla original
                    cpuSelectedBox(movement.SrcRow, movement.SrcColumn);
                    cpuSelectedBox(movement.DstRow, movement.DstColumn);
                    int value = alphaBetaSearch(oldTable, saveTable(), anotherPlayer(this.turn), 1, alpha, beta);
                    this.turn = 2;
                    movement.Value = value;
                    if (value > bestValue)
                    {
                        bestValue = movement.Value;
                        bestMove = movement;
                        alpha = bestValue;
                    }
                }
                // Si todos los movimientos tienen valor 0, el movimiento es aleatorio
                Boolean equals = true;
                foreach (Movement movement in movements)
                    if (bestMove.Value != 0) equals = false;
                if (equals)
                {
                    Random random = new Random();
                    bestMove = (Movement)movements[random.Next(0, movements.Count)];
                }
                loadTable(oldTable);
                // Se realiza el movimiento
                this.cpuTime = false;
                selectedBox(bestMove.SrcRow, bestMove.SrcColumn, true);
                selectedBox(bestMove.DstRow, bestMove.DstColumn, true);
            }
        }

        // método de búsqueda minimax con poda alfa-beta
        private int alphaBetaSearch(int [ , ] oldTable, int [ , ] newTable, int player, int depth, int alpha, int beta)
        {
            int result = checkTable();
            if (result != 0)
            {
                if (result == this.turn)
                    return 9999;
                if (result == anotherPlayer(this.turn))
                    return -9999;
                if (result == 3)
                    return 0;
            }
            if (depth >= cpuDepth) return utility(oldTable, newTable, this.turn);
            loadTable(newTable);
            this.turn = player;
            ArrayList movements = new ArrayList();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (this.getTable(i, j) == player * 1 || this.getTable(i, j) == player * 11)
                    {
                        calculateOptions(i, j);
                        for (int k = 0; k < 8; k++)
                            for (int l = 0; l < 8; l++)
                            {
                                if (this.getTable(k, l) == player * 11111 ||
                                    this.getTable(k, l) == player * 111111)
                                    movements.Add(new Movement(i, j, k, l));
                            }
                        resetOptions();
                    }
                }
            if (movements.Count == 0) return 9999;
            // Se calcula el valor de cada movimiento            
            foreach (Movement movement in movements)
            {
                loadTable(newTable);    // Se carga la tabla original
                cpuSelectedBox(movement.SrcRow, movement.SrcColumn);
                cpuSelectedBox(movement.DstRow, movement.DstColumn);
                int value = alphaBetaSearch(newTable, saveTable(), anotherPlayer(player), depth + 1, alpha, beta);
                if ((depth % 2) == 0)   // es capa par, osea MAX
                {
                    alpha = max(alpha, value);
                    if (alpha >= beta) return alpha;
                }
                else
                {
                    beta = min(beta, value);
                    if (beta <= alpha) return beta;
                }
            }
            if ((depth % 2) == 0) return alpha; // es capa par, osea MAX
            else return beta;   // es capa impar, osea MIN
        }

        // método que devuelve el valor de la variable mayor
        private int max(int v1, int v2)
        {
            if (v1 > v2) return v1;
            else return v2;
        }

        // método que devuelve el valor de la variable menor
        private int min(int v1, int v2)
        {
            if (v1 < v2) return v1;
            else return v2;
        }

        // método que define la utilidad de una jugada
        private int utility(int [ , ] oldTable, int [ , ] newTable, int player)
        {
            int oldOwnChips = 0, oldOwnQueens = 0, oldOtherChips = 0, oldOtherQueens = 0;
            int newOwnChips = 0, newOwnQueens = 0, newOtherChips = 0, newOtherQueens = 0;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    switch (oldTable[i, j])
                    {
                        case 1:
                            if (player == 1) oldOwnChips++;
                            else oldOtherChips++;
                            break;
                        case 2:
                            if (player == 2) oldOwnChips++;
                            else oldOtherChips++;
                            break;
                        case 11:
                            if (player == 1) oldOwnQueens++;
                            else oldOtherQueens++;
                            break;
                        case 22:
                            if (player == 2) oldOwnQueens++;
                            else oldOtherQueens++;
                            break;
                        default: break;
                    }
                    switch (newTable[i , j])
                    {
                        case 1:
                            if (player == 1) newOwnChips++;
                            else newOtherChips++;
                            break;
                        case 2:
                            if (player == 2) newOwnChips++;
                            else newOtherChips++;
                            break;
                        case 11:
                            if (player == 1) newOwnQueens++;
                            else newOtherQueens++;
                            break;
                        case 22:
                            if (player == 2) newOwnQueens++;
                            else newOtherQueens++;
                            break;
                        default: break;
                    }
                }
            return (oldOtherChips - newOtherChips) * 200 + (oldOtherQueens - newOtherQueens) * 300
                - (oldOwnChips - newOwnChips) * 200 - (oldOwnQueens - newOwnQueens) * 300;
        }

        // método que devuelve el valor del tablero en un momento determinado
        private int [ , ] saveTable()
        {
            int [ , ] table = new int[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    table[i, j] = this.getTable(i, j);
            return table;
        }

        // método que carga en el tablero el valor de un tablero auxiliar
        private void loadTable(int [ , ] table)
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    this.setTable(i, j, table[i, j]);
        }

        // método que devuelve el jugador oponente a uno dado
        private int anotherPlayer(int player)
        {
            if (player == 1) return 2;
            else return 1;
        }
    }
}