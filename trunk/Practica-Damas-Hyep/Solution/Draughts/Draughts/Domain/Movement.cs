using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Draughts.Domain
{
    /// <summary>
    /// Define el tipo abstracto de datos 'Movement'.
    /// </summary>
    class Movement
    {
        /// <summary>
        /// Fila origen.
        /// </summary>
        private int srcRow;

        /// <summary>
        /// Devuelve o modifica la fila origen.
        /// </summary>
        /// <value>
        /// Fila.
        /// </value>
        public int SrcRow
        {
            get { return srcRow; }
            set { srcRow = value; }
        }
        /// <summary>
        /// Columna origen.
        /// </summary>
        private int srcColumn;

        /// <summary>
        /// Devuelve o modifica la columna origen.
        /// </summary>
        /// <value>
        /// Columna.
        /// </value>
        public int SrcColumn
        {
            get { return srcColumn; }
            set { srcColumn = value; }
        }
        /// <summary>
        /// Fila destino.
        /// </summary>
        private int dstRow;

        /// <summary>
        /// Devuelve o modifica la fila destino.
        /// </summary>
        /// <value>
        /// Fila.
        /// </value>
        public int DstRow
        {
            get { return dstRow; }
            set { dstRow = value; }
        }
        /// <summary>
        /// Columna destino.
        /// </summary>
        private int dstColumn;

        /// <summary>
        /// Devuelve o modifica la columna destino.
        /// </summary>
        /// <value>
        /// Columna.
        /// </value>
        public int DstColumn
        {
            get { return dstColumn; }
            set { dstColumn = value; }
        }
        /// <summary>
        /// Utilidad de si se puede realizar esa jugada.
        /// </summary>
        private int value;

        /// <summary>
        /// Devuelve o modifica la utilidad.
        /// </summary>
        /// <value>
        /// Utilidad.
        /// </value>
        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Constructor de la clase <see cref="Movement"/>.
        /// </summary>
        /// <param name="srcRow">Fila origen.</param>
        /// <param name="srcColumn">Columna origen.</param>
        /// <param name="dstRow">Fila destino.</param>
        /// <param name="dstColumn">Columna destino.</param>
        public Movement(int srcRow, int srcColumn, int dstRow, int dstColumn)
        {
            this.srcRow = srcRow;
            this.srcColumn = srcColumn;
            this.dstRow = dstRow;
            this.dstColumn = dstColumn;
            this.value = 0;
        }
    }
}
