using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Draughts.Domain
{
    /// <summary>
    /// Delegado que informa de que ha cambiado el contenido de una casilla del tablero de juego.
    /// </summary>
    public class DelegateOfTheBox
    {
        /// <summary>
        /// Delegado para saber cuando una casilla ha cambiado.
        /// </summary>
        public delegate void BoxDelegate();
        /// <summary>
        /// Evento que ha cambiado una casilla.
        /// </summary>
        public event BoxDelegate switchBox;
        /// <summary>
        /// Modifica el cambio de contenido de la casilla.
        /// </summary>
        /// <value>
        /// Casilla modificada.
        /// </value>
        public object changeContentsBox
        {
            set
            {
                switchBox();
            }
        }
    }
}
