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
        /// <summary>
        /// Delegado para saber si ha cambiado el turno.
        /// </summary>
        public delegate void TurnDelegate();
        /// <summary>
        /// Evento de cambio de turno.
        /// </summary>
        public event TurnDelegate switchTurn;

        /// <summary>
        /// Modifica el cambio de turno.
        /// </summary>
        /// <value>
        /// Turno cambiado.
        /// </value>
        public object changeContentsTurn
        {
            set
            {
                switchTurn();
            }
        }
    }
}
