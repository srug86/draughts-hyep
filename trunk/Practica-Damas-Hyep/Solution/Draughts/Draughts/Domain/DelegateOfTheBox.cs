using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Draughts.Domain
{
    /// <summary>
    /// Delegado que informa de que ha cambiado el contenido de una casilla
    /// del tablero de juego.
    /// </summary>
    public class DelegateOfTheBox
    {
        public delegate void BoxDelegate();
        public event BoxDelegate switchBox;

        public object changeContentsBox
        {
            set
            {
                switchBox();
            }
        }
    }
}
