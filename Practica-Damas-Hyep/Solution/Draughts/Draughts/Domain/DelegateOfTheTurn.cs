using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Draughts.Domain
{
    /// <summary>
    /// Delegado que informa de que ha cambiado el turno de juego.
    /// </summary>
    class DelegateOfTheTurn
    {
        public delegate void TurnDelegate();
        public event TurnDelegate switchTurn;

        public object changeContentsTurn
        {
            set
            {
                switchTurn();
            }
        }
    }
}
