using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Draughts.Domain
{
    /// <summary>
    /// Implementa las interfaces utilizadas por los observadores.
    /// </summary>
    public interface Subject
    {
        /// <summary>
        /// Espera las notificaciones.
        /// </summary>
        /// <param name="obs">Observador.</param>
        void registerInterest(Observer obs);
    }

    /// <summary>
    /// Implementa los métodos para notificar los cambios.
    /// </summary>
    public interface Observer
    {
        /// <summary>
        /// Notifica la casilla cambiada.
        /// </summary>
        /// <param name="row">Fila.</param>
        /// <param name="column">Columna.</param>
        /// <param name="state">Estado.</param>
        void notify(int row, int column, int state);
        /// <summary>
        /// Notifica el cambio de turno.
        /// </summary>
        /// <param name="turn">Turno.</param>
        void notify(int turn);
    }
}
